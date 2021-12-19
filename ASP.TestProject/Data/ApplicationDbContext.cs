using ASP.TestProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.TestProject.Data;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

}