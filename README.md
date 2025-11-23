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

## Règles du jeu

Le joueur commence en cliquant sur Nouvelle aventure.

Le système génère un donjon contenant entre 4 et 6 salles maximum.

Chaque salle correspond à un événement différent :

Ennemi : combattre, fouiller ou fuir

Coffre : ouvrir ou ignorer

Piège : désamorcer ou fuir

Fontaine : boire (régénère)

Salle vide : explorer/continuer

Chaque action modifie :

les points de vie (PV)

le score

la position dans le donjon

# Fin de la partie

La partie se termine si :

le joueur atteint la dernière salle

les PV tombent à 0

le score devient négatif

la sortie est atteinte

# Sauvegarde

À la fin de l’aventure, la partie est enregistrée :

son score final

la date de la partie

Les sauvegardes sont visibles dans une page dédiée.

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

### 5️⃣ Comment lancer le projet
1 - Lancer le backend
API Auth + Base de données :
cd AuthenticationServices
dotnet run
cd GameServices
dotnet run
2 - Lancer le front Blazor
cd BlazorGame.Client
dotnet run


3 - Exécuter les tests 
cd Tests
dotnet test