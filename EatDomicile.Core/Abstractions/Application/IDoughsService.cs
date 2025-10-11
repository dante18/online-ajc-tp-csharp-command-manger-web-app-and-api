using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IDoughsService
{
    public IQueryable<Doughs> GetAllDoughs();

    public Doughs GetDoughs(int id);

    public void CreateDoughs(Doughs doughs);

    public void UpdateDoughs(Doughs doughs);

    public void DeleteDoughs(Doughs doughs);
}
