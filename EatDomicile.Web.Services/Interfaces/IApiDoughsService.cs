using EatDomicile.Web.Services.Doughs.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiDoughsService
{
    public Task<IEnumerable<DoughsDTO>> GetDoughsAsync();

    public Task<DoughsDTO?> GetDoughAsync(int id);

    public Task CreateDoughAsync(DoughsDTO doughsDTO);

    public Task UpdateDoughAsync(int id, DoughsDTO doughsDTO);

    public Task DeleteDoughAsync(int id);
}