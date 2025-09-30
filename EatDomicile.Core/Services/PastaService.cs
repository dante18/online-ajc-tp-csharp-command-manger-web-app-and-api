﻿using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Services;

public sealed class PastaService
{
    private readonly CommandStoreContext _context;

    public PastaService(CommandStoreContext context)
    {
        _context = context;
    }

    public List<Pasta> GetAllPastas()
    {
        return this._context.Pastas.ToList();
    }

    public Pasta GetPasta(int id)
    {
        return this._context.Pastas.Where(p => p.Id == id).FirstOrDefault();
    }

    public void CreatePasta(Pasta pasta)
    {
        this._context.Pastas.Add(pasta);
        this._context.SaveChanges();
    }

    public void UpdatePasta(Pasta pasta)
    {
        this._context.Pastas.Update(pasta);
        this._context.SaveChanges();
    }

    public void DeletePasta(Pasta pasta)
    {
        this._context.Pastas.Remove(pasta);
        this._context.SaveChanges();
    }
}
