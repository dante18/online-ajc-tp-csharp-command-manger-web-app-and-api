using EatDomicile.Web.Services.Drinks.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiDrinksService
{
    Task<IEnumerable<DrinkDTO>> GetDrinksAsync();
    Task<DrinkDTO?> GetDrinkAsync(int id);

    Task CreateDrinkAsync(DrinkDTO drinkDTO);
}
