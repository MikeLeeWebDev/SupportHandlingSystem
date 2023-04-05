using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupportHandlingSystem.Models.Entities;

internal class UserTypeEntity
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public ICollection<UserEntity> Users { get; set; } = new HashSet<UserEntity>();
}
