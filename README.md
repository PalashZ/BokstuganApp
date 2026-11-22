# BokstuganApp

## Projektbeskrivning
BokstuganApp är ett grupprojekt som går ut på att skapa ett digitalt bibliotek i C#. Programmet körs i terminalen och använder Spectre.Console för interaktiva menyer. Användare kan bläddra bland böcker, läsa recensioner och låna tillgängliga böcker. Programmet innehåller även funktioner för administration, statistik och hantering av tickets.

<img width="568" height="300" alt="Screenshot 2026-11-21 015837" src="https://github.com/user-attachments/assets/17715ffb-41eb-4f50-bacc-489b5f3eeebb" />
<img width="568" height="300" alt="Screenshot 2026-11-21 015918" src="https://github.com/user-attachments/assets/c6ac523b-e8f6-4990-9433-0d1cc7a12bb7" />
<img width="568" height="300" alt="Screenshot 2026-11-21 020007" src="https://github.com/user-attachments/assets/e7367675-5042-4979-bd0a-f81dd0b930f4" />

## Krav för att köra programmet
- .NET 10 SDK (`net10.0`)
- En terminal, till exempel PowerShell. Git Bash är valfritt.
- Visual Studio eller Visual Studio Code är valfritt.
- OBS! Lösenord behövs till rollen "Admin"
- Lösenord: nbi

### Alternativ 1: Visual Studio och Visual Studio Code
1. Klona eller ladda ner repot från GitHub.
2. Öppna projektmappen ”BokstuganApp”:
   - Visual Studio: Öppna filen BokstuganApp.slnx.
   - Visual Studio Code: Välj File > Open Folder och öppna projektmappen.
3. När projektmappen är öppen kan du titta på koden.
4. Kör programmet.
5. Välj ett av alternativen i programmet.

### Alternativ 2: Terminal
Öppna PowerShell i projektmappen där `BokstuganApp.csproj` ligger. Kör kommandona ett i taget:

```powershell
dotnet restore
dotnet build
dotnet run
```

När programmet startar väljer du ett alternativ i menyn.
