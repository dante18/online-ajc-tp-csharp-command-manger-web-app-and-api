using EatDomicile.Web.Services.Pizzas.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiPizzasService
{
    public Task<IEnumerable<PizzaDTO>> GetPizzasAsync();

    public Task<PizzaDTO?> GetPizzaAsync(int id);

    public Task CreatePizzaAsync(CreatePizzaDTO pizzaDTO);

    public Task UpdatePizzaAsync(int id, CreatePizzaDTO pizzaDTO);

    public Task DeletePizzaAsync(int id);
}