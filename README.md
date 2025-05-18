# AxiteHR

**AxiteHR** is a modern, modular HR and business operations platform designed to streamline human resources management and support additional key functionalities such as invoicing and financial administration.

The project is subscription-based, the company buys a monthly subscription and can use the platform.

---

## 🧩 Overview

AxiteHR is a multi-functional, service-oriented web application built with scalability and flexibility in mind. The system supports:

- **Human Resources Management**  
  Employee files, contracts, invoices, absences, evaluations and more.

- **Invoicing**  
  Manage clients, generate and send invoices, and track payment statuses.

- **Modular Architecture**  
  Built as a set of microservices, allowing for easy extension and customization.

- **Multilingual Support**  
  The platform is fully localizable.

---

## 🔧 Technologies Used

- **.NET** – Backend services (Web APIs)
- **Angular** – Frontend (SPA)
- **Entity Framework** – Data access layers
- **Docker** – Containerized deployment
- **Redis / SQL Server** – Caching and persistent storage
- **SignalR** – Real-time features (e.g., notifications)
- **Ocelot** – API Gateway
- **Serilog** – Logging library
- **ELK Stack** – A powerful set of tools used for searching, analyzing, and visualizing log data in real time
- **MailKit** – Mail-client library
- **RabbitMQ** – Queuing and handling background processes
- **NUnit** – Unit and integration tests

---

## 🚀 Getting Started

### Run Locally

1. Clone the repository
2. Configure environment variables and local ssl certificates (see `README` in each project)
3. Run the solution:
```bash
docker-compose up --build
```
4. Frontend available at: `http://localhost:4200`  
API Gateway available at: `https://localhost:7777`

---

## 🌍 Localization

Localization is handled via `.resx` files and the shared `AxiteHR.GlobalizationResources` library.  
To add new languages or resources, follow the instructions in that project’s `README.md`.