using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    private static int First = 0;
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ;
    }

    private DbSet<CheckWord> checkWords;
    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
    public DbSet<Translate> WordRu { get; set; }
    public DbSet<WordEng> WordEng { get; set; }

}

