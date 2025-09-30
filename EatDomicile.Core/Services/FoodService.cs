using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Services;

public sealed class FoodService
{
    private readonly CommandStoreContext _context;

    public FoodService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Food> GetAllFoods()
    {
        return this._context.Food.ToList();
    }

    public Food GetFood(int id)
    {
        return this._context.Food.Where(f => f.Id == id).FirstOrDefault();
    }
}
