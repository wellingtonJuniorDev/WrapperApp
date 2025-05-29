#include "Repository.h"  
#include <fstream>  
#include <sstream> 
#include <unordered_map>  
#include <string>
#include <codecvt>
#include <locale>

static std::unordered_map<int, std::string> dataStore;  

void SaveToFile() {
	std::wofstream file("data.txt", std::ios::out);
	file.imbue(std::locale(std::locale::classic(), new std::codecvt_utf8<wchar_t>));

	for (const auto& pair : dataStore) {
		file << pair.first << L" " << std::wstring(pair.second.begin(), pair.second.end()) << L"\n";
	}
}

void LoadFromFile() {
	std::ifstream file("data.txt");
	if (!file) return;
	dataStore.clear();

	std::string line;
	while (std::getline(file, line)) {
		std::istringstream iss(line);
		int id;
		std::string name;

		if (iss >> id) {
			std::getline(iss >> std::ws, name); 
			dataStore[id] = name;
		}
	}
}

extern "C" {
	 int AddItem(const char* name) {
		LoadFromFile();
		static int idCounter = GetItemCount();
		dataStore[++idCounter] = name;
		SaveToFile();
		return idCounter;
	}

	const char* GetItem(int id) {
		static std::string buffer;
		LoadFromFile();

		auto it = dataStore.find(id);
		if (it != dataStore.end()) {
			buffer = it->second;
			return buffer.c_str();
		}
		return nullptr;
	}

	bool RemoveItem(int id) {
		LoadFromFile();
		auto it = dataStore.find(id);
		if (it != dataStore.end()) {
			dataStore.erase(it);
			SaveToFile();
			return true; // Success
		}
		return false; // Failure
	}

	int GetItemCount() {
		LoadFromFile();
		return static_cast<int>(dataStore.size());
	}

	void UpdateItem(int id, const char* name) {
		LoadFromFile();
		dataStore[id] = name;
		SaveToFile();
	}

	const char* ListItems() {
		static std::string buffer;
		LoadFromFile();

		buffer.clear();
		for (const auto& pair : dataStore) {
			buffer += std::to_string(pair.first) + ":" + pair.second + ";";
		}
		return buffer.c_str();
	}
}