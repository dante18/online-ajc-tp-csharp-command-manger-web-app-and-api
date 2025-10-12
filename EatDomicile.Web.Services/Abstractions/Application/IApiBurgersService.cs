using EatDomicile.Web.Services.Dtos.Application.Burgers;
using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiBurgersService
{
    public Task<PaginatedResponseDto<BurgerDTO>> GetBurgersAsync(int page = 1);

    public Task<BurgerDTO?> GetBurgerAsync(int id);

    public Task<IEnumerable<IngredientDTO>> GetBurgerIngredientsAsync(int id);

    public Task CreateBurgerAsync(CreateOrUpdateBurgerDTO burgerDTO);

    public Task UpdateBurgerAsync(int id, BurgerDTO burgerDTO);

    public Task UpdateBurgerAddIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task UpdateBurgerUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO);

    public Task UpdateBurgerDeleteIngredientAsync(int id, int ingredientId);

    public Task DeleteBurgerAsync(int id);
}