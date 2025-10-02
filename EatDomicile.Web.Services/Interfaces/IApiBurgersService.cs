using EatDomicile.Web.Services.Burgers.DTO;
using EatDomicile.Web.Services.Ingredient.DTO;

namespace EatDomicile.Web.Services.Interfaces;

public interface IApiBurgersService
{
    public Task<IEnumerable<BurgerDTO>> GetBurgersAsync();

    public Task<BurgerDTO?> GetBurgerAsync(int id);

    public Task<IEnumerable<IngredientDTO>> GetBurgerIngredientsAsync(int id);

    public Task CreateBurgerAsync(BurgerDTO burgerDTO);

    public Task UpdateBurgerAsync(int id, BurgerDTO burgerDTO);

    public Task DeleteBurgerAsync(int id);
}