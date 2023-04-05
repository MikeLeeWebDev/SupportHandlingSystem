using SupportHandlingSystem.Services;
using SupportHandlingSystem.Services.Login;
using SupportHandlingSystem.Services.Register;

namespace SupportHandlingSystem.Models;

internal class MenuModel
{
    public int MenuOption = 1;
    public string ColorGreen = "\u001b[32m";
    public string ColorRed = "\u001b[31m";
    public string ColorDefault = "\u001b[0m";
    public ConsoleKeyInfo EnteredKey;
    private readonly UserService _userService = new UserService();
    private readonly CaseService _caseService = new CaseService();

    public async Task MainMenuAsync()
    {
        Console.Clear();
        bool SelectingOption = true;
        var Login = new LoginService();
        var Register = new RegisterService();

        Console.CursorVisible = false;
        while (SelectingOption)
        {

            Console.Write("Use Arrow Keys to Choose\n");
            Console.WriteLine($"{(MenuOption == 1 ? ColorGreen : "")} Login{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 2 ? ColorGreen : "")} Register{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 3 ? ColorRed : "")} Exit{ColorDefault}");

            EnteredKey = Console.ReadKey();

            switch (EnteredKey.Key)
            {
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 3 ? 1 : MenuOption + 1);
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 1 ? 3 : MenuOption - 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();

                    if (MenuOption == 1)
                    {
                        await Login.Login();
                    }
                    else if (MenuOption == 2)
                    {
                        await Register.CreateUser(this);
                    }
                    else
                    {
                        SelectingOption = false;
                    }
                    SelectingOption = false;
                    MenuOption = 1;
                    break;
            }
        }
    }

    public async Task AdminMenuAsync()
    {
        Console.Clear();
        bool SelectingOption = true;

        Console.CursorVisible = false;
        while (SelectingOption)
        {

            Console.Write("Use Arrow Keys to Choose\n");
            Console.WriteLine($"{(MenuOption == 1 ? ColorGreen : "")} Change User Type{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 2 ? ColorRed : "")} Exit{ColorDefault}");

            EnteredKey = Console.ReadKey();

            switch (EnteredKey.Key)
            {
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 2 ? 1 : MenuOption + 1);
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 1 ? 2 : MenuOption - 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();

                    if (MenuOption == 1)
                    {
                        await _userService.UpdateUserTypeByEmail(this);
                    }
                    else
                    {
                        SelectingOption = false;
                    }
                    SelectingOption = false;
                    MenuOption = 1;
                    break;
            }
        }
    }

    public async Task CustomerSupportMenuAsync()
    {
        Console.Clear();
        bool SelectingOption = true;
        var comment = new CommentService();
        Console.CursorVisible = false;
        while (SelectingOption)
        {

            Console.Write("Use Arrow Keys to Choose\n");
            Console.WriteLine($"{(MenuOption == 1 ? ColorGreen : "")} View All Cases{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 2 ? ColorGreen : "")} View All Cases By Specific User{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 3 ? ColorGreen : "")} View Specific Case{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 4 ? ColorGreen : "")} Update Case Status{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 5 ? ColorGreen : "")} Comment On a Case{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 6 ? ColorRed : "")} Exit{ColorDefault}");

            EnteredKey = Console.ReadKey();

            switch (EnteredKey.Key)
            {
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 6 ? 1 : MenuOption + 1);
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 1 ? 6 : MenuOption - 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();

                    if (MenuOption == 1)
                    {
                        await _caseService.PrintAllCasesAsync(this);
                    }
                    else if (MenuOption == 2)
                    {
                        await _caseService.PrintAllCasesByUserAsync(this);
                    }
                    else if (MenuOption == 3)
                    {

                    }
                    else if (MenuOption == 4)
                    {
                        await _caseService.UpdateCaseStatusById();
                    }
                    else if (MenuOption == 5)
                    {
                        await comment.CreateCommentAsync();
                    }
                    else
                    {
                        SelectingOption = false;
                    }
                    SelectingOption = false;
                    MenuOption = 1;
                    break;
            }
        }
    }

    public async Task CustomerMenuAsync()
    {
        Console.Clear();
        bool SelectingOption = true;

        Console.CursorVisible = false;
        while (SelectingOption)
        {

            Console.Write("Use Arrow Keys to Choose\n");
            Console.WriteLine($"{(MenuOption == 1 ? ColorGreen : "")} Submit a Case{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 2 ? ColorGreen : "")} View All Cases{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 3 ? ColorGreen : "")} Details of a Specific Case{ColorDefault}");
            Console.WriteLine($"{(MenuOption == 4 ? ColorRed : "")} Exit{ColorDefault}");

            EnteredKey = Console.ReadKey();

            switch (EnteredKey.Key)
            {
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 4 ? 1 : MenuOption + 1);
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    MenuOption = (MenuOption == 1 ? 4 : MenuOption - 1);
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();

                    if (MenuOption == 1)
                    {
                        await _caseService.CreateCaseAsync(this);
                    }
                    else if (MenuOption == 2)
                    {
                        await _caseService.PrintUserCasesAsync(this);
                    }
                    else if (MenuOption == 3)
                    {
                    }
                    else
                    {
                        SelectingOption = false;
                    }
                    SelectingOption = false;
                    MenuOption = 1;
                    break;
            }
        }
    }
}

