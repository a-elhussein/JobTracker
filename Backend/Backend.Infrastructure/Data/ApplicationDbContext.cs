using Backend.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        
    }

    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<User> Users { get; set; }
}