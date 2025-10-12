using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IDrinkService
{
    public IQueryable<Drink> GetAllDrinks();

    public Drink GetDrink(int id);

    public void CreateDrink(Drink drink);

    public void UpdateDrink(Drink drink);

    public void DeleteDrink(Drink drink);
}
