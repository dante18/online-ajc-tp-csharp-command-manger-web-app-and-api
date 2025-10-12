using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IIngredientService
{
    public IQueryable<Ingredient> GetAllIngredients();

    public List<Ingredient> GetAllIngredientsByBurger(int burgerId);

    public List<Ingredient> GetAllIngredientsByPizza(int pizzaId);

    public Ingredient GetIngredient(int id);

    public void CreateIngredient(Ingredient ingredient);

    public void UpdateIngredient(Ingredient ingredient);

    public void DeleteIngredient(Ingredient ingredient);
}