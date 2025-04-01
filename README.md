# Process Monitor API

## 📌 Project Overview
Process Monitor API is a .NET 8-based system that monitors currently running processes on a machine, similar to the `top` utility on Linux. It provides real-time process data, supports SignalR-based notifications for high CPU usage, and offers an API for external clients to integrate their own interfaces.

## 📂 Project Structure
```
ProcessMonitor
│── ProcessMonitor.API          # API Layer (Controllers, DI, Startup Configurations)
│── ProcessMonitor.Application  # Business Logic (Services, Use Cases)
│── ProcessMonitor.Domain       # Entities, Models, DTOs
│── ProcessMonitor.Infrastructure # Infrastructure Layer (SignalR, Repositories, External Integrations)
│── ProcessMonitor.Presentation # DTOs for API Requests/Responses
│── README.md                   # Project Documentation
```

---

## 🚀 Getting Started

### 1️⃣ Prerequisites
- .NET 8 SDK
- Visual Studio or VS Code

### 2️⃣ Installation
```sh
# Clone the repository
git clone https://github.com/your-repo/ProcessMonitor.git
cd ProcessMonitor
```

### 3️⃣ Running the Application
```sh
# Navigate to the API project folder
cd ProcessMonitor.API

# Run the application
dotnet run
```
The API will be available at: `http://localhost:5000`

---

## 📡 API Endpoints

### 🔹 Process Management
| Method | Endpoint | Description |
|--------|---------|-------------|
| `GET` | `/api/process` | Get all running processes |
| `GET` | `/api/process/important` | Get important system processes |
| `GET` | `/api/process/{id}` | Get process details by ID |
| `POST` | `/api/process/start-monitoring` | Start monitoring high CPU usage |
| `POST` | `/api/process/stop-monitoring` | Stop monitoring |
| `GET` | `/api/process/monitoring-status` | Get monitoring status |

### 🔹 Real-time Notifications (SignalR)
Clients can connect to SignalR hub at:
```
/ws/notifications
```
- Receives notifications when any process exceeds 80% CPU usage.

---

## 🛠️ Technologies Used
- **.NET 8** - Core framework
- **ASP.NET Web API** - RESTful services
- **SignalR** - Real-time communication
- **Dependency Injection** - For service management
- **Minimal External Dependencies** - No 3rd party libraries used

---

## 📖 Documentation for Developers
To allow external developers to integrate additional clients, API documentation is auto-generated with Swagger:
```
http://localhost:5000/swagger
```
Developers can extend the system by implementing new UI clients using:
- Web-based clients (React, Angular, etc.)
- Mobile clients (Flutter, .NET MAUI, etc.)

---

## 🔗 Contributing
Feel free to fork and contribute! Open an issue or PR for improvements.

## 📜 License
MIT License. Free to use and modify.

