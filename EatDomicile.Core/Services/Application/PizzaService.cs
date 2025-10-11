using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class PizzaService : IPizzaService
{
    private readonly ApplicationStoreContext _context;

    public PizzaService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Pizza> GetAllPizzas()
    {
        return _context.Pizzas
            .Include(p => p.Ingredients)
            .Include(p => p.Doughs)
            .AsNoTracking();
    }

    public Pizza GetPizza(int id)
    {
        return _context.Pizzas
            .Include(p => p.Ingredients)
            .Include(p => p.Doughs)
            .Where(pizza => pizza.Id == id).FirstOrDefault();
    }

    public void CreatePizza(Pizza pizza)
    {
        _context.Pizzas.Add(pizza);
        _context.SaveChanges();
    }

    public void UpdatePizza(Pizza pizza)
    {
        _context.Pizzas.Update(pizza);
        _context.SaveChanges();
    }

    public void DeletePizza(Pizza pizza)
    {
        _context.Pizzas.Remove(pizza);
        _context.SaveChanges();
    }
}
