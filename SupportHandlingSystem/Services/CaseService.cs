using Microsoft.EntityFrameworkCore;
using SupportHandlingSystem.Contexts;
using SupportHandlingSystem.Models;
using SupportHandlingSystem.Models.Entities;
using SupportHandlingSystem.Services.Login;
using System.Linq.Expressions;

namespace SupportHandlingSystem.Services;

internal class CaseService : GenericService<CaseEntity>
{
    private readonly DataContext _context = new DataContext();


    public async Task<CaseEntity> CreateCaseAsync(MenuModel menu)
    {
        Console.CursorVisible = true;
        Console.Write("Enter title: ");
        var title = Console.ReadLine();

        Console.Write("Enter description: ");
        var description = Console.ReadLine();

        // create a new case
        var newCase = new CaseEntity
        {
            Title = title,
            Description = description,
            Created = DateTime.Now,
            Modified = DateTime.Now
        };
        await SaveAsync(newCase, x => x.Id == newCase.Id);

        await menu.CustomerMenuAsync();
        return newCase;
    }

    public override async Task<CaseEntity> SaveAsync(CaseEntity entity, Expression<Func<CaseEntity, bool>> predicate)
    {
        var item = await GetAsync(predicate);
        if (item == null)
        {
            entity.StatusType = await _context.StatusTypes.FindAsync(1);
            entity.User = await _context.Users.FirstOrDefaultAsync(u => u.Email == LoginService.Email);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        return item;
    }

    public override async Task<IEnumerable<CaseEntity>> GetAllAsync()
    {
        return await _context.Cases.Include(x => x.User).Include(x => x.StatusType).ToListAsync();
    }

    public async Task PrintAllCasesAsync(MenuModel menu)
    {
        var cases = await GetAllAsync();

        Console.WriteLine($"Prints all cases:");
        foreach (var x in cases)
        {
            string date;
            if (x.Created <= x.Modified)
            {
                date = "Created: " + x.Created.ToString();
            }
            else
            {
                date = "Modified: " + x.Modified.ToString();
            }
            Console.WriteLine($"Id: {x.Id}, Name: {x.User.FirstName} {x.User.LastName}, Email: {x.User.Email}, {date}, Status: {x.StatusType.StatusName}\n");
        }
        Console.ReadKey();
        await menu.CustomerSupportMenuAsync();
    }

    public async Task PrintAllCasesByUserAsync(MenuModel menu)
    {
        Console.CursorVisible = true;
        Console.Write("Enter user's email: ");
        string email = Console.ReadLine();
        var cases = await _context.Cases
            .Include(x => x.StatusType)
            .Where(x => x.User.Email == email)
            .ToListAsync();

        Console.WriteLine($"Cases connected to {email}:");
        foreach (var x in cases)
        {
            string date;
            if (x.Created <= x.Modified)
            {
                date = "Created: " + x.Created.ToString();
            }
            else
            {
                date = "Modified: " + x.Modified.ToString();
            }
            Console.WriteLine($"Id: {x.Id}, {date}, Status: {x.StatusType.StatusName}\n");
        }
        Console.ReadKey();
        await menu.CustomerSupportMenuAsync();
    }

    public override async Task<CaseEntity> GetAsync(Expression<Func<CaseEntity, bool>> predicate)
    {
        var item = await _context.Cases
            .Include(x => x.User)
            .Include(x => x.StatusType)
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(predicate);

        if (item != null)
        {
            return item;
        }
        return null!;
    }

    public async Task PrintUserCasesAsync(MenuModel menu)
    {
        var cases = await _context.Cases
            .Include(x => x.StatusType)
            .Where(x => x.User.Email == LoginService.Email)
            .ToListAsync();

        Console.WriteLine($"Cases connected to {LoginService.Email}:");
        foreach (var x in cases)
        {
            string date;
            if (x.Created <= x.Modified)
            {
                date = "Created: " + x.Created.ToString();
            }
            else
            {
                date = "Modified: " + x.Modified.ToString();
            }
            Console.WriteLine($"Id: {x.Id}, {date}, Status: {x.StatusType.StatusName}\n");
        }
        Console.ReadKey();
        await menu.CustomerMenuAsync();
    }

    public async Task PrintCaseByIdAsync()
    {
        Console.CursorVisible = true;
        Console.Write("Enter case id: ");
        var input = Console.ReadLine();
        if (Guid.TryParse(input, out Guid caseId))
        {
            var x = await GetAsync(x => x.Id == caseId);
            if (x != null)
            {
                Console.WriteLine($"Full Name: {x.User.FirstName} {x.User.FirstName}");
                Console.WriteLine($"Email: {x.User.Email}");
                Console.WriteLine($"Case Id: {x.Id}");
                Console.WriteLine($"Title: {x.Title}");
                Console.WriteLine($"Description: {x.Description}");
                Console.WriteLine($"Created: {x.Created}");
                Console.WriteLine($"Modified: {x.Modified}");
                Console.WriteLine($"Status: {x.StatusType.StatusName}");

                if (x.Comments != null && x.Comments.Any())
                {
                    Console.WriteLine("Comments:");
                    foreach (var comment in x.Comments)
                    {
                        Console.WriteLine($"- {comment} ({comment.Created})");
                    }
                }
                else
                {
                    Console.WriteLine("No comments added");
                }
            }
            else
            {
                Console.WriteLine($"Case with Id {caseId} not found.");
            }
        }
        else
        {
            Console.WriteLine($"Invalid input: {input}. Please enter a valid case id.");
        }

        Console.ReadKey();
    }

    public async Task UpdateCaseStatusById()
    {
        Console.CursorVisible = true;
        Console.Write("Enter case id: ");
        Guid caseId = Guid.Parse(Console.ReadLine());

        using (var context = new DataContext())
        {
            var caseEntity = await context.Cases.SingleOrDefaultAsync(x => x.Id == caseId);

            if (caseEntity != null)
            {
                Console.WriteLine($"Case found: {caseEntity.Title}");

                Console.Write("Enter new case status (1=Submitted, 2=In Progress, 3=Solved): ");
                int caseStatusId = int.Parse(Console.ReadLine());

                var caseStatus = await context.StatusTypes.FindAsync(caseStatusId);

                if (caseStatus != null)
                {
                    caseEntity.StatusType = caseStatus;
                    caseEntity.Modified = DateTime.Now;
                    await context.SaveChangesAsync();

                    Console.WriteLine($"Case's status has been updated to: {caseEntity.StatusType.StatusName}");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid case status Id.");
                }
            }
            else
            {
                Console.WriteLine("Case not found.");
            }
        }
    }

    public async Task AddCommentToCaseAsync(MenuModel menu)
    {
        Console.CursorVisible = true;
        Console.Write("Enter CaseId: ");
        var caseId = Guid.Parse(Console.ReadLine());

        var existingCase = await GetAsync(x => x.Id == caseId);
        if (existingCase == null)
        {
            Console.WriteLine($"Case with Id '{caseId}' does not exist.");
            return;
        }

        Console.Write("Enter Comment: ");
        var commentText = Console.ReadLine();

        var comment = new CommentEntity
        {
            Comment = commentText,
            Created = DateTime.Now,
        };

        existingCase.Comments.Add(comment);
        existingCase.Modified = DateTime.Now;

        await _context.SaveChangesAsync();

        Console.WriteLine($"Comment added to Case with Id '{caseId}'.");

        Console.ReadKey();
        await menu.CustomerSupportMenuAsync();
    }
}
