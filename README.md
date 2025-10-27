Groupe : 
- Priyank SOLANKI
- Younes KHOYA

# 🎮 Blazor Game Backend (.NET 8)

## 📦 Description

Ce projet est une architecture modulaire .NET 9 composée de plusieurs services :

| Module                     | Rôle                                                                                              |
|----------------------------|---------------------------------------------------------------------------------------------------|
| **AuthenticationServices** | Gestion des utilisateurs, rôles, authentification, accès à la base de données.                    |
| **GameServices**           | API principale du jeu (contrôleurs REST, logique métier, endpoints joueurs / ennemis / rooms).    |
| **SharedModels**           | Classes communes partagées entre les services (`Player`, `Ennemie`, `User`, `Room`, `Charatere`). |
| **BlazorGame.Client**      | Interface Web de l'application                                                                    |
| **Tests**                  | Tests unitaires (xUnit + EFCore InMemory) pour valider la logique et la persistance.              |

---

## ⚙️ Installation & Exécution

### 1️⃣ Prérequis
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- SQL Server (local pour l'instant)
- Visual Studio / Rider / VS Code

---

### 2️⃣ Cloner le projet
```bash
git clone https://github.com/PriyankSolanki/BlazorGameQuest1234.git
cd BlazorGame
```
### 3️⃣ Restorer les dépendances
```bash
dotnet restore
```

### 4️⃣ Modifier la chaîne de connexion

```
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=8889;Database=blazorgame;User=root;Password=root"
}
```

