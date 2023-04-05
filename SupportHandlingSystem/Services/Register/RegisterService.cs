using SupportHandlingSystem.Models;
using SupportHandlingSystem.Models.Entities;
using System.Runtime.CompilerServices;

namespace SupportHandlingSystem.Services.Register;

internal class RegisterService
{
    public async Task<UserEntity> CreateUser(MenuModel menuModel)
    {
        Console.CursorVisible = true;
        Console.Clear();
        var userService = new UserService();

        Console.WriteLine("Enter First Name: ");
        var firstName = Console.ReadLine();

        Console.WriteLine("Enter Last Name: ");
        var lastName = Console.ReadLine();

        Console.WriteLine("Enter Email: ");
        var email = Console.ReadLine();

        Console.WriteLine("Enter Phone Number: ");
        var phoneNumber = Console.ReadLine();

        var user = new UserEntity
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
        };

        var savedUser = await userService.SaveAsync(user, x => x.Email == user.Email);

        await menuModel.MainMenuAsync();

        return savedUser;

    }

}
