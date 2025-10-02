using EatDomicile.Web.Services.Addresses.DTO;
using System.Net.Http;
using System.Net.Http.Json;

namespace EatDomicile.Web.Services.Addresses;

public class AddressService
{
    private readonly HttpClient httpClient;
    public AddressService(HttpClient httpClient)
    {
        httpClient = httpClient;
    }

    public async Task<IEnumerable<AddressDTO>> GetAddresssAsync()
    {
        var addresss = await httpClient.GetFromJsonAsync<IEnumerable<AddressDTO>>("https://localhost:7001/api/addresss");
        return addresss ?? [];
    }

    public async Task<AddressDTO?> GetAddressAsync(int id)
    {
        var drink = await httpClient.GetFromJsonAsync<AddressDTO>($"https://localhost:7001/api/addresss/{id}");
        return drink;
    }

    public async Task CreateAddressAsync(AddressDTO addressDTO)
    {
        var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/addresss", addressDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task UpdateAddressAsync(int id, AddressDTO addressDTO)
    {
        var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/addresss/{id}", addressDTO);
        _ = response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAddressAsync(int id)
    {
        var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/addresss/{id}");
        _ = response.EnsureSuccessStatusCode();
    }
}
