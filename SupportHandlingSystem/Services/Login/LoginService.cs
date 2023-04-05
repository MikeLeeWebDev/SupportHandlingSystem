using SupportHandlingSystem.Models;
using SupportHandlingSystem.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace SupportHandlingSystem.Services.Login;
internal class LoginService
{
    private readonly UserService _userService = new UserService();
    public static string Email;

    public async Task Login()
    {
        Console.CursorVisible = true;
        var Menu = new MenuModel();
        while (true)
        {
            Console.Write("Enter your email: ");
            string email = Console.ReadLine()?.Trim();
            Email = email;
            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email cannot be empty.");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            UserEntity user = await _userService.GetAsync(u => u.Email == email);

            if (user == null)
            {
                Console.WriteLine("Email not found.");
                Console.ReadKey();
                Console.Clear();
                continue;
            }

            Console.WriteLine($"User type: {user.UserType?.TypeName}");

            switch (user.UserType?.TypeName)
            {
                case "Customer":

                    await Menu.CustomerMenuAsync();
                    break;
                case "Customer Support":
                    await Menu.CustomerSupportMenuAsync();
                    break;
                case "Admin":
                    await Menu.AdminMenuAsync();
                    break;
                default:
                    Console.WriteLine("Invalid user type.");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
            }
            break;
        }
    }

}
