using EatDomicile.Web.Services.Domains.Burgers.DTO;
using EatDomicile.Web.Services.Domains.Ingredients.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiBurgersService
{
    public Task<IEnumerable<BurgerDTO>> GetBurgersAsync();

    public Task<BurgerDTO?> GetBurgerAsync(int id);

    public Task<IEnumerable<IngredientDTO>> GetBurgerIngredientsAsync(int id);

    public Task CreateBurgerAsync(CreateOrUpdateBurgerDTO burgerDTO);

    public Task UpdateBurgerAsync(int id, BurgerDTO burgerDTO);

    public Task UpdateBurgerAddIngredientAsync(int id, IngredientDTO ingredientDTO);

    public Task UpdateBurgerUpdateIngredientAsync(int id, int ingredientId, IngredientDTO ingredientDTO);

    public Task UpdateBurgerDeleteIngredientAsync(int id, int ingredientId);

    public Task DeleteBurgerAsync(int id);
}