using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services;

public sealed class BurgerService
{
    private readonly CommandStoreContext _context;

    public BurgerService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Burger> GetAllBurgers()
    {
        return this._context.Burger.Include(p => p.Ingredients).ToList();
    }

    public Burger GetBurger(int id)
    {
        return this._context.Burger.Include(p => p.Ingredients).Where(burger => burger.Id == id).FirstOrDefault();
    }

    public void CreateBurger(Burger burger)
    {
        this._context.Burger.Add(burger);
        this._context.SaveChanges();
    }

    public void UpdateBurger(Burger burger)
    {
        this._context.Burger.Update(burger);
        this._context.SaveChanges();
    }

    public void DeleteBurger(Burger burger)
    {
        this._context.Burger.Remove(burger);
        this._context.SaveChanges();
    }
}