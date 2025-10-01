using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Services;

public sealed class IngredientService
{
    private readonly CommandStoreContext _context;

    public IngredientService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Ingredient> GetAllIngredients()
    {
        return _context.Ingredients.ToList();
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
        return this._context.Ingredients.Where(i => i.Id == id).FirstOrDefault();
    }

    public void CreateIngredient(Ingredient ingredient)
    {
        this._context.Ingredients.Add(ingredient);
        this._context.SaveChanges();
    }

    public void UpdateIngredient(Ingredient ingredient)
    {
        this._context.Ingredients.Update(ingredient);
        this._context.SaveChanges();
    }

    public void DeleteIngredient(Ingredient ingredient)
    {
        this._context.Ingredients.Remove(ingredient);
        this._context.SaveChanges();
    }
}