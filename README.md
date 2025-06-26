# OrdersProject

## 📌 Opis / Description

**PL:**  
OrdersProject to system do przetwarzania zamówień przesyłanych drogą mailową. Wykorzystuje architekturę Onion, wzorzec CQRS oraz technologię .NET 8 z Blazor Server. System automatycznie odbiera wiadomości e-mail (Gmail/IMAP), analizuje ich treść przy użyciu GPT-4 i zapisuje zamówienia do bazy danych MySQL.

**EN:**  
OrdersProject is a system for processing orders sent via email. It uses Onion architecture, CQRS pattern, and .NET 8 with Blazor Server. The system automatically receives email messages (Gmail/IMAP), parses their content using GPT-4, and stores orders in a MySQL database.

---

## ⚙️ Wymagania / Requirements

- .NET 8 SDK
- Docker + Docker Compose
- Konto Gmail z 2FA
- OpenAI API Key
- Karta płatnicza przypięta do konta OpenAI

---

## 🛠️ Konfiguracja pliku `.env` / `.env` file configuration

Utwórz plik `.env` w folderze głównym (obok `docker-compose.yml`) z poniższą zawartością:

```env
EMAIL_USERNAME=twoj_email@gmail.com
EMAIL_PASSWORD=haslo_aplikacji_gmail
OPENAI_API_KEY=klucz_openai
