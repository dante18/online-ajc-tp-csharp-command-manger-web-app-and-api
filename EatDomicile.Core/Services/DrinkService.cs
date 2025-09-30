using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Services;

public class DrinkService
{
    private readonly CommandStoreContext _context;

    public DrinkService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Drink> GetAllDrinks()
    {
        return _context.Drinks.ToList();
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
