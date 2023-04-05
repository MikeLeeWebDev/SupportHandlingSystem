using Microsoft.EntityFrameworkCore;
using SupportHandlingSystem.Contexts;
using SupportHandlingSystem.Models;
using SupportHandlingSystem.Models.Entities;
using System.Linq.Expressions;

namespace SupportHandlingSystem.Services;

internal class CommentService : GenericService<CommentEntity>
{
    private readonly DataContext _context = new DataContext();
    public async Task<CommentEntity> CreateCommentAsync()
    {
        Console.CursorVisible = true;
        Console.Write("Enter Comments: ");
        var commentText = Console.ReadLine();

        var newComment = new CommentEntity
        {
            Comment = commentText,
            Created = DateTime.Now
        };
        await SaveAsync(newComment, x => x.Id == newComment.Id);

        return newComment;
    }

    public override async Task<CommentEntity> SaveAsync(CommentEntity entity, Expression<Func<CommentEntity, bool>> predicate)
    {
        Console.CursorVisible = true;
        Console.Write("Enter case id: ");
        var input = Console.ReadLine();
        Console.Write("Enter case submitter's email: ");
        var email = Console.ReadLine();
        var item = await GetAsync(predicate);
        if (Guid.TryParse(input, out Guid caseId))
        {
            if (item == null)
            {
                entity.Case = await _context.Cases.FindAsync(caseId);
                entity.User = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                _context.Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
        }
        else
        {
            Console.WriteLine($"Invalid input: {input}. Please enter a valid case id.");
        }
        return item;
    }
}
