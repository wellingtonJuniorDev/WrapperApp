# WrapperApp – Integration of C# (DLL) with Electron + React

This is a proof-of-concept project integrating:

- 🔧 Backend in C# (.NET)
- ⚙️ Native DLL in C++
- 🖥️ Desktop UI with Electron + React + Vite
- 🔌 Communication via Named Pipes (IPC) or directly via DLL using `electron-edge-js`

---

## 🧱 Project Structure
```text
WrapperApp/
├── WrapperApp.Client/ # Electron + React (with Vite) desktop app
├── WrapperApp.Console/ # .NET 8 app with IPC server
|── WrapperApp.Library/ # .NET standard DLL with core logic and exported DLL
├── WrapperApp.Persistence/ # C++ DLL with basic CRUD implementation
```
---

## ✅ Features

- Full CRUD for `Item (Id, Name)`
- Electron-to-.NET communication:
  - 📡 Named Pipes (IPC)
  - 📦 Direct DLL access via `electron-edge-js`
- Toggleable communication mode via UI checkbox
- Clean UI using TailwindCSS

---

## 🚀 How to Run

### 1. Build the C# Backend

- rebuild solution in the Visual Studio 2022
- run `WrapperApp.Console` to IPC server

>The C# build copies the DLLs to the `WrapperApp.Client\assets` folder

### 2. Run the Frontend (Electron + React)

```bash
cd WrapperApp.Client

# Install dependencies
npm install

# Build the complete application
npm rebuild

# Start the Electron app in development mode
npm start
```