using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IPizzaService
{
    public IQueryable<Pizza> GetAllPizzas();

    public Pizza GetPizza(int id);

    public void CreatePizza(Pizza pizza);

    public void UpdatePizza(Pizza pizza);

    public void DeletePizza(Pizza pizza);
}
