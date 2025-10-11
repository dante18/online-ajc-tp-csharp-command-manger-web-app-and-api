using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class IngredientService : IIngredientService
{
    private readonly ApplicationStoreContext _context;

    public IngredientService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Ingredient> GetAllIngredients()
    {
        return _context.Ingredients.AsNoTracking();
    }

    public List<Ingredient> GetAllIngredientsByBurger(int burgerId)
    {
        return _context.Ingredients.ToList().Where(i => i.BurgerId == burgerId).ToList();
    }

    public List<Ingredient> GetAllIngredientsByPizza(int pizzaId)
    {
        return _context.Ingredients.ToList().Where(i => i.PizzaId == pizzaId).ToList();
    }

    public Ingredient GetIngredient(int id)
    {
        return _context.Ingredients.Where(i => i.Id == id).FirstOrDefault();
    }

    public void CreateIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        _context.SaveChanges();
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
        _context.SaveChanges();
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Remove(ingredient);
        _context.SaveChanges();
    }
}