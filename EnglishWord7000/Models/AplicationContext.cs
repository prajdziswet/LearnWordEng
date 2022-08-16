using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
        //Levels.levels.AddRange(Words.GroupBy(x => x.WordEng.Level).Where(x => x.Key != null).Select(x => x.Key));
    }

    /*protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var x = 1;

        //Console.WriteLine(Words?.Count());

        CreateList createList = new CreateList();
        modelBuilder.Entity<Word>().HasData(createList.Words);

    }*/

    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
}