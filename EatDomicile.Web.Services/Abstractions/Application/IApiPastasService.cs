using EatDomicile.Web.Services.Dtos.Application.Pastas;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiPastasService
{
    public Task<PaginatedResponseDto<PastaDTO>> GetPastasAsync(int page = 1);

    public Task<PastaDTO?> GetPastaAsync(int id);

    public Task CreatePastaAsync(PastaDTO pastaDto);

    public Task UpdatePastaAsync(int id, PastaDTO pastaDto);

    public Task DeletePastaAsync(int id);
}