using EatDomicile.Web.Services.Dtos.Application.Drinks;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiDrinksService
{
    public Task<PaginatedResponseDto<DrinkDTO>> GetDrinksAsync(int page = 1);

    public Task<DrinkDTO?> GetDrinkAsync(int id);

    public Task CreateDrinkAsync(DrinkDTO drinkDTO);

    public Task UpdateDrinkAsync(int id, DrinkDTO drinkDto);

    public Task DeleteDrinkAsync(int id);
}
