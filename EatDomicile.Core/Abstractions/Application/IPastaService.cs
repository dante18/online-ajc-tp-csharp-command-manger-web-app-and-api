using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IPastaService
{
    public IQueryable<Pasta> GetAllPastas();

    public Pasta GetPasta(int id);

    public void CreatePasta(Pasta pasta);

    public void UpdatePasta(Pasta pasta);

    public void DeletePasta(Pasta pasta);
}
