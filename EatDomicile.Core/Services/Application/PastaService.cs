using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class PastaService : IPastaService
{
    private readonly ApplicationStoreContext _context;

    public PastaService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<Pasta> GetAllPastas()
    {
        return _context.Pastas.AsNoTracking();
    }

    public Pasta GetPasta(int id)
    {
        return _context.Pastas.Where(p => p.Id == id).FirstOrDefault();
    }

    public void CreatePasta(Pasta pasta)
    {
        _context.Pastas.Add(pasta);
        _context.SaveChanges();
    }

    public void UpdatePasta(Pasta pasta)
    {
        _context.Pastas.Update(pasta);
        _context.SaveChanges();
    }

    public void DeletePasta(Pasta pasta)
    {
        _context.Pastas.Remove(pasta);
        _context.SaveChanges();
    }
}
