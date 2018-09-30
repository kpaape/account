using Microsoft.EntityFrameworkCore;
 
namespace LoginReg.Models
{
    public class LoginsContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LoginsContext(DbContextOptions<LoginsContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}