using Microsoft.EntityFrameworkCore;
using SupportHandlingSystem.Contexts;
using SupportHandlingSystem.Models;
using SupportHandlingSystem.Models.Entities;
using System.Linq.Expressions;

namespace SupportHandlingSystem.Services;

internal class UserService : GenericService<UserEntity>
{
    private readonly DataContext _context = new DataContext();
    public override async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        return await _context.Users.Include(x => x.UserType).ToListAsync();
    }

    //public override Task<UserEntity> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    //{
    //    return base.GetAsync(predicate);
    //}

    public override async Task<UserEntity> GetAsync(Expression<Func<UserEntity, bool>> predicate)
    {
        var item = await _context.Users.Include(u => u.UserType).FirstOrDefaultAsync(predicate);
        return item ?? null!;
    }

    public override async Task<UserEntity> SaveAsync(UserEntity entity, Expression<Func<UserEntity, bool>> predicate)
    {
        var item = await GetAsync(predicate);
        if (item == null)
        {
            entity.UserType = await _context.UserTypes.FindAsync(3);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        return item;
    }

    public async Task UpdateUserTypeByEmail(MenuModel menu)
    {
        Console.Write("Enter user email: ");
        string email = Console.ReadLine();

        using (var context = new DataContext())
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                Console.WriteLine($"User found: {user.FirstName} {user.LastName}");

                Console.Write("Enter new user type (1=Admin, 2=Customer Support, 3=Customer): ");
                int userTypeId = int.Parse(Console.ReadLine());

                var userType = await context.UserTypes.FindAsync(userTypeId);

                if (userType != null)
                {
                    user.UserType = userType;
                    await context.SaveChangesAsync();

                    Console.WriteLine($"User's UserType has been updated to: {user.UserType.TypeName}");
                    Console.ReadKey();
                    await menu.AdminMenuAsync();
                }
                else
                {
                    Console.WriteLine("Invalid user type Id.");
                }
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
    }
}


