using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using WrapperApp.Library.Adapters;


if (!bool.Parse(Environment.GetEnvironmentVariable("RUN_IPC_SERVER") ?? "false"))
{
    Console.WriteLine("Running console application...");
    TestWithConsole();
}
else
{
    Console.WriteLine("Running IPC server...");
    RunIpcServer();
}


void TestWithConsole()
{
    var option = 0;
    IPersistenceAdapter adapter = new PersistenceAdapter();

    do
    {
        Console.WriteLine("0 - Add item");
        Console.WriteLine("1 - Get item");
        Console.WriteLine("2 - Remove item");
        Console.WriteLine("3 - Get item count");
        Console.WriteLine("4 - Update item");
        Console.WriteLine("5 - List all items");
        Console.WriteLine("Other - Exit");

        Console.Write("\nSelect an option: ");

        if (int.TryParse(Console.ReadLine(), out int parsedOption))
        {
            option = parsedOption;
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
            continue;
        }

        switch (option)
        {
            case 0:
                Console.Write("Enter item name to add: ");
                var id = adapter.Add(Console.ReadLine());
                Console.WriteLine($"Item added with ID: {id}");
                break;
            case 1:
                Console.Write("Enter item ID to get: ");
                var idToGet = int.Parse(Console.ReadLine());
                var item = adapter.Get(idToGet);
                Console.WriteLine($"Item retrieved: {item}");
                break;
            case 2:
                Console.Write("Enter item ID to remove: ");
                var idToRemove = int.Parse(Console.ReadLine());
                var removed = adapter.Remove(idToRemove);
                Console.WriteLine($"Item removed: {removed}");
                break;
            case 3:
                Console.WriteLine("Getting item count...");
                Console.WriteLine($"Item count: {adapter.GetCount()}");
                break;
            case 4:
                Console.WriteLine("Enter item ID to update...");
                var idToUpdate = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter new name: ");
                var newName = Console.ReadLine();

                adapter.Update(idToUpdate, newName);
                Console.WriteLine("Item updated.");
                break;
            case 5:
                Console.WriteLine("Listing all items...");
                var items = adapter.ListAll();
                foreach (var i in items)
                {
                    Console.WriteLine(i);
                }
                break;

            default:
                break;
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    } while (option != 999);
}

void RunIpcServer()
{
    while (true)
    {
        using var server = new NamedPipeServerStream("WrapperApp.Business.Pipe", PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
        Console.WriteLine("Waiting client connection...");
        server.WaitForConnection();

        using var reader = new StreamReader(server, Encoding.UTF8);
        using var writer = new StreamWriter(server, new UTF8Encoding(false)) { AutoFlush = true };

        var requestJson = reader.ReadLine();
        var command = JsonSerializer.Deserialize<IPCRequest>(requestJson!, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Console.WriteLine("Client connected: {0} \nCommand: {1}", server.GetImpersonationUserName(), command);

        var response = HandleCommand(command!);
        var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        writer.WriteLine(responseJson);
    }
}

static object HandleCommand(IPCRequest cmd)
{
    IPersistenceAdapter adapter = new PersistenceAdapter();

    bool Update(){
        adapter.Update(cmd.Id, cmd.Name); 
        return true; 
    };

    return cmd.Action switch
    {
        IPCAction.ADD => adapter.Add(cmd.Name),
        IPCAction.GET => adapter.Get(cmd.Id),
        IPCAction.REMOVE => adapter.Remove(cmd.Id),
        IPCAction.UPDATE => Update(),
        IPCAction.COUNT => adapter.GetCount(),
        IPCAction.LIST => adapter.ListAll(),
        _ => new { error = "Unknown command" }
    };
 }

internal static class IPCAction
{
    internal const string ADD = "add";
    internal const string GET = "get";
    internal const string REMOVE = "remove";
    internal const string UPDATE = "update";
    internal const string COUNT = "count";
    internal const string LIST = "list";
}

record IPCRequest(string Action, int Id, string Name);