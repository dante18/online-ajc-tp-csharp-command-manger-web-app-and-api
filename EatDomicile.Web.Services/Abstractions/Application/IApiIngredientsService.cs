using EatDomicile.Web.Services.Dtos.Application.Ingredients;
using EatDomicile.Web.Services.Dtos.Common;

namespace EatDomicile.Web.Services.Abstractions.Application;

public interface IApiIngredientsService
{
    public Task<PaginatedResponseDto<IngredientDTO>> GetIngredientsAsync(int page = 1);

    public Task<IngredientDTO?> GetIngredientAsync(int id);

    public Task CreateIngredientAsync(IngredientDTO ingredientDTO);

    public Task UpdateIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task DeleteIngredientAsync(int id);
}