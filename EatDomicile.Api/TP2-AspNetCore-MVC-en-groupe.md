# TP EatDomicile 2 - Architecture Web avec ASP.NET Core MVC

> **Objectif :** Transformer l'application console EatDomicile en une architecture web complète avec API et interface web en utilisant ASP.NET Core MVC et les bonnes pratiques d'architecture.

> **Organisation :** Travail à effecter en groupe.
Rappel des groupes :

| Groupe | Membres                        |
|--------|--------------------------------|
| 1      | Hakim, Théophile, Ibrahim      |
| 2      | Thomas, Josué, Abdessamad      |
| 3      | Alexandre, Pierre, Alain Roger |
| 4      | Anthony, Kévin, Eric           |
| 5      | Axel, Tom, Guillaume           |

## Contexte
Vous devez maintenant faire évoluer votre application EatDomicile (du TP précédent) vers une architecture web moderne. L'application console sera remplacée par une API Web, et une interface web sera ajoutée pour permettre aux utilisateurs de gérer les données via un navigateur.

## Prérequis
- Avoir réalisé le TP précédent (EatDomicile avec Entity Framework Core)
- Connaissance de base d'ASP.NET Core MVC
- Compréhension des principes SOLID et de l'injection de dépendances

## Architecture cible
Votre solution devra contenir les projets suivants :

```
EatDomicile.sln
├── EatDomicile.Core/          (Bibliothèque - réutilisée du TP1)
│   ├── Entities/
│   ├── Context/
│   ├── Services/
│   └── Migrations/
├── EatDomicile.Api/           (API Web - remplace l'application console)
│   ├── Controllers/
│   ├── Program.cs
│   └── Extensions/
├── EatDomicile.Web.Services/  (Bibliothèque - Services pour le Web)
│   ├── Services/
│   └── Extensions/
└── EatDomicile.Web/           (Application Web MVC)
    ├── Controllers/
    ├── Views/
    ├── Models/
    ├── wwwroot/
    └── Extensions/
```

## Instructions détaillées

### 1. Restructuration du projet Core (2 points)
- **Réorganisez** votre projet `EatDomicile.Core` existant pour suivre l'architecture ci-dessus
- **Créez des interfaces** pour vos services dans un dossier `Interfaces/`
- **Configurez** l'injection de dépendances pour vos services Core (directement dans Program.cs ou via des méthodes d'extension - voir bonus)
- **Assurez-vous** que ce projet ne contient aucune dépendance vers les couches supérieures

### 2. Création du projet API Web (4 points)
- **Créez** un nouveau projet `EatDomicile.Api` de type "ASP.NET Core Web API"
- **Implémentez** tous les contrôleurs API pour les entités du TP précédent :
  - `IngredientsController` ⭐
  - `DrinksController` ⭐
  - `BurgersController` ⭐
  - `PizzasController`
  - `PastasController`
  - `UsersController` ⭐
  - `AddressesController` ⭐
  - `OrdersController` ⭐
- **Implémentez** la gestion des erreurs avec des réponses HTTP appropriées
- **Configurez** l'injection de dépendances dans Program.cs

### 3. Projet de services Web (3 points)
- **Créez** le projet `EatDomicile.Web.Services` (bibliothèque)
- **Implémentez** des services pour communiquer avec l'API :
  - `IApiIngredientService` et son implémentation
  - `IApiDrinkService` et son implémentation
  - `IApiUserService` et son implémentation
  - `IApiOrderService` et son implémentation
- **Utilisez** `HttpClient` (simple) ou `IHttpClientFactory` (voir bonus) pour les appels HTTP
- **Gérez** les erreurs HTTP et les timeouts appropriés
- **Configurez** l'injection de dépendances pour ces services

### 4. Application Web MVC (5 points)
- **Créez** le projet `EatDomicile.Web` de type "ASP.NET Core Web App (MVC)"
- **Implémentez** les contrôleurs MVC avec vues complètes (CRUD) pour :
  - `IngredientsController` ⭐
  - `DrinksController` ⭐
  - `UsersController` ⭐
  - `OrdersController` ⭐ (avec gestion des relations)
- **Créez** les vues Razor correspondantes (Index, Create, Edit, Delete, Details)
- **Utilisez** des ViewModels appropriés pour l'affichage
- **Implémentez** la validation côté client et serveur
- **Ajoutez** des styles CSS et une navigation cohérente

### 5. Configuration et injection de dépendances (2 points)
- **Configurez** correctement tous les services dans `Program.cs` de chaque projet
- **Organisez** votre code de configuration de manière claire et lisible
- **Configurez** les chaînes de connexion et les paramètres d'environnement
- **Assurez-vous** que l'API et le Web peuvent fonctionner simultanément sur des ports différents

### 6. Gestion des commandes avancée (2 points)
- **Implémentez** dans l'API les endpoints pour les requêtes LINQ du TP précédent
- **Créez** une interface web pour visualiser :
  - Les statistiques des commandes
  - Les utilisateurs actifs
  - Les commandes végétariennes
  - Le calcul des calories par commande
- **Affichez** les données dans des tableaux HTML simples et lisibles

### 7. Tests et validation (2 points)
- **Testez** tous les endpoints API (avec Postman, curl, ou Swagger - voir aide ci-dessous)
- **Vérifiez** que toutes les pages web fonctionnent correctement
- **Validez** la communication entre l'API et le Web
- **Assurez-vous** que les données sont cohérentes entre les deux interfaces

---

## Points Bonus (jusqu'à +6 points)

### Bonus 1 : Méthodes d'extension pour l'injection de dépendances (+2 points)
- **Créez** des méthodes d'extension dans chaque projet :
  - `EatDomicile.Core/Extensions/ServiceCollectionExtensions.cs`
  - `EatDomicile.Api/Extensions/ServiceCollectionExtensions.cs`
  - `EatDomicile.Web.Services/Extensions/ServiceCollectionExtensions.cs`
  - `EatDomicile.Web/Extensions/ServiceCollectionExtensions.cs`
- **Implémentez** la configuration par environnement (Development, Production)
- **Utilisez** ces méthodes dans Program.cs pour un code plus propre et modulaire

### Bonus 2 : Utilisation d'IHttpClientFactory (+2 points)
- **Remplacez** les HttpClient simples par `IHttpClientFactory`
- **Configurez** des clients nommés ou typés pour les services Web

### Bonus 3 : Intégration d'ASP.NET Core Identity (+2 points)
- **Ajoutez** ASP.NET Core Identity au projet Web
- **Implémentez** l'authentification avec registration/login
- **Protégez** certaines pages avec `[Authorize]`
- **Créez** des rôles (Admin, User) avec des autorisations différentes
- **Personnalisez** les pages d'Identity selon le thème de l'application

---

## Barème de notation

**Total : 20 points + 6 points bonus**

### 1. Restructuration du projet Core (2 points)
- **1 pt** - Interfaces créées et architecture respectée
- **0,5 pt** - Méthodes d'extension pour l'injection de dépendances
- **0,5 pt** - Respect des principes SOLID

### 2. Projet API Web (4 points)
- **3 pts** - Tous les contrôleurs API implémentés et fonctionnels
- **0,5 pt** - Gestion des erreurs et réponses HTTP appropriées
- **0,5 pt** - Code propre et configuration DI correcte

### 3. Services Web avec IHttpClientFactory (3 points)
- **1,5 pts** - Services Web correctement implémentés
- **1 pt** - Configuration IHttpClientFactory avec clients nommés
- **0,5 pt** - Gestion des erreurs et timeouts
### 3. Services Web (3 points)
- **2 pts** - Services Web correctement implémentés avec HttpClient
- **0,5 pt** - Gestion des erreurs HTTP appropriée
- **0,5 pt** - Configuration et injection de dépendances correctesés
- **1 pt** - Interface utilisateur cohérente et navigation
- **0,5 pt** - Styles et présentation

### 5. Configuration et injection de dépendances (2 points)
- **1 pt** - Configuration correcte dans tous les projets
- **0,5 pt** - Organisation claire du code de configuration
- **0,5 pt** - Gestion des environnements et paramètres

### 6. Gestion des commandes avancée (2 points)
- **1 pt** - Endpoints API pour les requêtes LINQ
- **1 pt** - Interface web pour les statistiques et visualisations

### 7. Tests et validation (2 points)
- **0,5 pt** - Tests des endpoints API fonctionnels
- **0,5 pt** - Pages web opérationnelles
- **0,5 pt** - Communication API/Web validée
- **0,5 pt** - Cohérence des données

### Critères transversaux :
- **Qualité du code** : Architecture, nommage, commentaires
- **Respect des conventions** : ASP.NET Core MVC, REST API
- **Gestion des erreurs** : Validation, messages appropriés
- **Documentation** : README.md avec instructions d'installation et d'utilisation

### Pénalités :
- **-2 pts** : Code non compilable
- **-1 pt** : Architecture non respectée (dépendances circulaires, etc.)
- **-1 pt** : API et Web ne peuvent pas fonctionner simultanément
- **-0,5 pt** : Configuration incorrecte des services

---

## Livrables attendus

1. **Solution Visual Studio** complète avec tous les projets
2. **README.md** avec :
   - Instructions d'installation et de configuration
   - Ports utilisés pour l'API et le Web
   - Captures d'écran de l'interface web
   - Documentation de l'API (endpoints principaux)
3. **Scripts SQL** ou migrations pour la base de données
4. **Fichier de configuration** avec les chaînes de connexion d'exemple

## Conseils techniques

### Configuration des ports
- **API** : https://localhost:7001, http://localhost:5001
- **Web** : https://localhost:7000, http://localhost:5000

### Chaînes de connexion recommandées
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EatDomicile;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001"
  }
}
```
## Aide pour les tests d'API

### Option 1 : Utiliser Postman
- Téléchargez et installez [Postman](https://www.postman.com/)
- Créez une nouvelle collection pour votre API EatDomicile
- Testez chaque endpoint avec les méthodes HTTP appropriées (GET, POST, PUT, DELETE)

### Option 2 : Ajouter Swagger (optionnel)
Si vous souhaitez utiliser Swagger pour tester votre API :

**1. Ajoutez les packages NuGet dans EatDomicile.Api :**
```xml
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
```

**2. Configuration dans Program.cs :**
```csharp
// Ajoutez ces services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajoutez ce middleware (en mode Development uniquement)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

**3. Accédez à Swagger :**
- Lancez votre API
- Allez sur `https://localhost:7001/swagger` (ou le port de votre API)
- Testez vos endpoints directement depuis l'interface Swagger

### Option 3 : Tests avec curl
Exemples de commandes curl pour tester vos endpoints :
```bash
# GET tous les ingrédients
curl -X GET "https://localhost:7001/api/ingredients"

# POST un nouvel ingrédient
curl -X POST "https://localhost:7001/api/ingredients" \
     -H "Content-Type: application/json" \
     -d '{"name":"Tomate","kCal":18}'

# GET un ingrédient par ID
curl -X GET "https://localhost:7001/api/ingredients/1"
```

Bon développement ! 🍔🍕🍝