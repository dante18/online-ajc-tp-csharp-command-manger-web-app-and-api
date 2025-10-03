using EatDomicile.Web.Services.Domains.Drinks.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiDrinksService
{
    public Task<IEnumerable<DrinkDTO>> GetDrinksAsync();

    public Task<DrinkDTO?> GetDrinkAsync(int id);

    public Task CreateDrinkAsync(DrinkDTO drinkDTO);

    public Task UpdateDrinkAsync(int id, DrinkDTO drinkDto);

    public Task DeleteDrinkAsync(int id);
}
