# OrdersProject

## 📌 Opis / Description

**PL:**  
OrdersProject to system do przetwarzania zamówień przesyłanych drogą mailową. Wykorzystuje architekturę Onion, wzorzec CQRS oraz technologię .NET 8 z Blazor Server. System automatycznie odbiera wiadomości e-mail (Gmail/IMAP), analizuje ich treść przy użyciu GPT-4 i zapisuje zamówienia do bazy danych MySQL.

**EN:**  
OrdersProject is a system for processing orders sent via email. It uses Onion architecture, CQRS pattern, and .NET 8 with Blazor Server. The system automatically receives email messages (Gmail/IMAP), parses their content using GPT-4, and stores orders in a MySQL database.

---
## 🌐 Dostępne adresy / Available URLs

http://localhost:5002	Frontend Blazor Server	
http://localhost:5000/swagger/index.html	Swagger UI (API REST)	
http://localhost:8081/index.php	phpMyAdmin (l: root, h: root)	

## ⚙️ Wymagania / Requirements

- .NET 8 SDK
- Docker + Docker Compose + Docker Desktop
- Konto Gmail z 2FA // Gmail account with 2FA
- OpenAI API Key
- Karta płatnicza podpięta do konta OpenAI // A payment method linked to the OpenAI account

---

## 🛠️ Konfiguracja pliku `.env` / `.env` file configuration

PL: Utwórz plik `.env` w folderze głównym (obok `docker-compose.yml`) z poniższą zawartością:
EN: Create a .env file in the root directory (next to docker-compose.yml) with the following content:

```env
EMAIL_USERNAME=twoj_email@gmail.com
EMAIL_PASSWORD=haslo_aplikacji_gmail
OPENAI_API_KEY=klucz_openai
```
