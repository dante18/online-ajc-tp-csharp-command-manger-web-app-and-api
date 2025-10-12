using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class BurgerService : IBurgerService
{
    private readonly ApplicationStoreContext _context;

    public BurgerService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Burger> GetAllBurgers()
    {
        return _context.Burger.Include(p => p.Ingredients).AsNoTracking();
    }

    public Burger GetBurger(int id)
    {
        return _context.Burger.Include(p => p.Ingredients).Where(burger => burger.Id == id).FirstOrDefault();
    }

    public void CreateBurger(Burger burger)
    {
        _context.Burger.Add(burger);
        _context.SaveChanges();
    }

    public void UpdateBurger(Burger burger)
    {
        _context.Burger.Update(burger);
        _context.SaveChanges();
    }

    public void DeleteBurger(Burger burger)
    {
        _context.Burger.Remove(burger);
        _context.SaveChanges();
    }
}