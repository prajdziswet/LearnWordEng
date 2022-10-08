using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    CreateList createList = new CreateList();
    //    //modelBuilder.Entity<User>().HasData(new User { Id=1, Login="praj", Password="ifkjvs1",Email="prajdziswet@mail.ru"});
    //    if (createList != null && createList.Words.Count > 0)
    //    { 
    //        foreach (Word word in createList.Words)
    //        {
    //            //int id=word.Id;
    //            //WordEng wordEng = word.WordEng;
    //            //modelBuilder.Entity<WordEng>().HasData(wordEng);
    //            //Translate wordRu = word.WordRu;
    //            //modelBuilder.Entity<Translate>().HasData(wordRu);
    //            //List<CheckWord> checkWords = word.CheckWords;
    //            //modelBuilder.Entity<CheckWord>().HasData(checkWords.ToArray());

    //            modelBuilder.Entity<Word>().HasData(word);
    //        }
    //    }
    //}

    private DbSet<CheckWord> checkWords;
    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
    public DbSet<Translate> WordRu { get; set; }
    public DbSet<WordEng> WordEng { get; set; }

}

