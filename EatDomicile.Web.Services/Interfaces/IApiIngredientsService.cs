using EatDomicile.Web.Services.Ingredient.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiIngredientsService
{
    public Task<IEnumerable<IngredientDTO>> GetIngredientsAsync();

    public Task<IngredientDTO?> GetIngredientAsync(int id);

    public Task CreateIngredientAsync(IngredientDTO ingredientDTO);

    public Task UpdateIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task DeleteIngredientAsync(int id);
}