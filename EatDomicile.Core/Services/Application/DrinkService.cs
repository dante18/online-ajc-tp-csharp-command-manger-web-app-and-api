using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class DrinkService : IDrinkService
{
    private readonly ApplicationStoreContext _context;

    public DrinkService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Drink> GetAllDrinks()
    {
        return _context.Drinks.AsNoTracking();
    }

    public Drink GetDrink(int id)
    {
        return _context.Drinks.Where(d => d.Id == id).FirstOrDefault();
    }

    public void CreateDrink(Drink drink)
    {
        _context.Drinks.Add(drink);
        _context.SaveChanges();
    }

    public void UpdateDrink(Drink drink)
    {
        _context.Drinks.Update(drink);
        _context.SaveChanges();
    }

    public void DeleteDrink(Drink drink)
    {
        _context.Drinks.Remove(drink);
        _context.SaveChanges();
    }
}
