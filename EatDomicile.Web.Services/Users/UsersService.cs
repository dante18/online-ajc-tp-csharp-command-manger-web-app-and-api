using EatDomicile.Web.Services.Addresses.DTO;
using EatDomicile.Web.Services.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace EatDomicile.Web.Services.Users
{
    public class UsersService
    {
        private readonly HttpClient httpClient;

        public UsersService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<UsersDTO>> GetUsersAsync()
        {
            var users = await httpClient.GetFromJsonAsync<IEnumerable<UsersDTO>>("https://localhost:7001/api/users");
            return users ?? [];
        }

        public async Task<UsersDTO?> GetUserAsync(int id)
        {
            var user = await httpClient.GetFromJsonAsync<UsersDTO>($"https://localhost:7001/api/users/{id}");
            return user;
        }

        public async Task<IEnumerable<AddressDTO>> GetUserAddress(int id)
        {
            var addresses = await httpClient.GetFromJsonAsync<IEnumerable<AddressDTO>>($"https://localhost:7001/api/burgers/{id}/address");
            return addresses;
        }

        public async Task CreateUserAsync(UsersDTO userDTO)
        {
            var response = await this.httpClient.PostAsJsonAsync("https://localhost:7001/api/users", userDTO);
            _ = response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserAsync(int id, UsersDTO userDto)
        {
            var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/users/{id}", userDto);
            _ = response.EnsureSuccessStatusCode();
        }
        public async Task UpdateUserAddAddressAsync(int id, AddressDTO addressDTO)
        {
            var response = await this.httpClient.PostAsJsonAsync($"https://localhost:7001/api/burgers/{id}/addresss", addressDTO);
            _ = response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserUpdateAddressAsync(int id, int addressId, AddressDTO addressDTO)
        {
            var response = await this.httpClient.PutAsJsonAsync($"https://localhost:7001/api/burgers/{id}/addresss/{addressId}", addressDTO);
            _ = response.EnsureSuccessStatusCode();
        }

        public async Task UpdateUserDeleteAddressAsync(int id, int addressId)
        {
            var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/burgers/{id}/addresss/{addressId}");
            _ = response.EnsureSuccessStatusCode();
        }
        public async Task DeleteUserAsync(int id)
        {
            var response = await this.httpClient.DeleteAsync($"https://localhost:7001/api/users/{id}");
            _ = response.EnsureSuccessStatusCode();
        }
    }
}
