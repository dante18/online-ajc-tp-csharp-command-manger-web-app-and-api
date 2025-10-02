using EatDomicile.Web.Services.Pastas.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiPastasService
{
    public Task<IEnumerable<PastaDTO>> GetPastasAsync();

    public Task<PastaDTO?> GetPastaAsync(int id);

    public Task CreatePastaAsync(PastaDTO pastaDto);

    public Task UpdatePastaAsync(int id, PastaDTO pastaDto);

    public Task DeletePastaAsync(int id);
}