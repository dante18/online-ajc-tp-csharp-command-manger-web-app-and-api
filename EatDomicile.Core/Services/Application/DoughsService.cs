using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class DoughsService : IDoughsService
{
    private readonly ApplicationStoreContext _context;

    public DoughsService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Doughs> GetAllDoughs()
    {
        return _context.Doughs.AsNoTracking();
    }

    public Doughs GetDoughs(int id)
    {
        return _context.Doughs.FirstOrDefault(d => d.Id == id);
    }

    public void CreateDoughs(Doughs doughs)
    {
        _context.Doughs.Add(doughs);
        _context.SaveChanges();
    }

    public void UpdateDoughs(Doughs doughs)
    {
        _context.Doughs.Update(doughs);
        _context.SaveChanges();
    }

    public void DeleteDoughs(Doughs doughs)
    {
        _context.Doughs.Remove(doughs); 
        _context.SaveChanges();
    }
}
