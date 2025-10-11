using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IBurgerService
{
    public IQueryable<Burger> GetAllBurgers();

    public Burger GetBurger(int id);

    public void CreateBurger(Burger burger);

    public void UpdateBurger(Burger burger);

    public void DeleteBurger(Burger burger);
}