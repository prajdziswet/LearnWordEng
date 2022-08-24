using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
    public DbSet<Translate> WordRu { get; set; }
    public DbSet<WordEng> WordEng { get; set; }

}

