using EatDomicile.Web.Services.Dtos.Application.Doughs;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiDoughsService
{
    public Task<PaginatedResponseDto<DoughsDTO>> GetDoughsAsync(int page = 1);

    public Task<DoughsDTO?> GetDoughAsync(int id);

    public Task CreateDoughAsync(DoughsDTO doughsDTO);

    public Task UpdateDoughAsync(int id, DoughsDTO doughsDTO);

    public Task DeleteDoughAsync(int id);
}