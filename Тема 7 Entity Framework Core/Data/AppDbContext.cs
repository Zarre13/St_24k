using Automagazin.Models;
using Microsoft.EntityFrameworkCore;

namespace Automagazin.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cars_Stock> Cars_Stock => Set<Cars_Stock>();
}
