# TP EatDomicile 2 - Architecture Web avec ASP.NET Core MVC

> **Objectif :** Transformer l'application console EatDomicile en une architecture web complète avec API et interface web en utilisant ASP.NET Core MVC et les bonnes pratiques d'architecture.

> **Organisation :** Travail à effecter en groupe.

## Contexte
Vous devez maintenant faire évoluer votre application EatDomicile (du [TP précédent](https://github.com/dante18/online-ajc-tp-csharp-command-manger-consol-app)) vers une architecture web moderne. L'application console sera remplacée par une API Web, et une interface web sera ajoutée pour permettre aux utilisateurs de gérer les données via un navigateur.

Ce projet permettra de mettre en pratique :
- L’utilisation de la programmation orientée objet (POO) en C#
- L’accès aux données avec Entity Framework (Code First) et une base de données locale
- L'utilisation .Net pour créer une application Web MVC et une API

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

## Configuration techniques
- **API** : https://localhost:7001, http://localhost:5001
- **Web** : https://localhost:7000, http://localhost:5000