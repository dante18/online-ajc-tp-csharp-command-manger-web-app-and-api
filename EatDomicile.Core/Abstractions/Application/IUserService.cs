using EatDomicile.Core.Entities;

namespace EatDomicile.Core.Abstractions.Application;

public interface IUserService
{
    public IQueryable<User> GetAllUsers();

    public User GetUser(int id);

    public void CreateUser(User user);

    public void UpdateUser(User user);

    public void DeleteUser(User user);
}