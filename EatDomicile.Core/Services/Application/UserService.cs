using EatDomicile.Core.Abstractions.Application;
using EatDomicile.Core.Context;
using EatDomicile.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatDomicile.Core.Services.Application;

internal class UserService : IUserService
{
    private readonly ApplicationStoreContext _context;

    public UserService(ApplicationStoreContext context)
    {
        _context = context;
    }

    public IQueryable<User> GetAllUsers()
    {
        return _context.Users.Include(u => u.Address).AsNoTracking();
    }

    public User GetUser(int id)
    {
        return _context.Users.Include(u => u.Address).Where(u => u.Id == id).FirstOrDefault();
    }

    public void CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }

    public void DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
    }
}