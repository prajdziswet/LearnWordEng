using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    private static int First = 0;
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //First++;
        //if (First==2)
        //{
        //    First = 1;
            CreateList createList = new CreateList();
            //int index = 0;
            //createList.Words.Select(x=>x.CheckWords).ToList().ForEach(x=>x.ForEach(y=>y.Id=++index));
            Console.Clear();
            foreach (Word word in createList.Words)
            {
                //Console.WriteLine(word.WordEng.Id);
                
                modelBuilder.Entity<Word>().HasData(new Word(){Id= word.WordEng.WordId});
                modelBuilder.Entity<WordEng>().HasData(word.WordEng);
                //modelBuilder.Entity<Translate>().HasData(word.WordRu);
                //modelBuilder.Entity<CheckWord>().HasData(word.CheckWords);
                //foreach (CheckWord VARIABLE in word.CheckWords)
                //{
                //    Console.WriteLine(VARIABLE.Id);
                //}

            }
            ////modelBuilder.Entity<User>().HasData(new User { Id=1, Login="praj", Password="ifkjvs1",Email="prajdziswet@mail.ru"});
            //if (createList != null && createList.Words.Count > 0)
            //{
            //    //foreach (Word word in createList.Words)
            //    //{
            //    //    //int id=word.Id;
            //    //    //WordEng wordEng = word.WordEng;
            //    //    //modelBuilder.Entity<WordEng>().HasData(wordEng);
            //    //    //Translate wordRu = word.WordRu;
            //    //    //modelBuilder.Entity<Translate>().HasData(wordRu);
            //    //    //List<CheckWord> checkWords = word.CheckWords;
            //    //    //modelBuilder.Entity<CheckWord>().HasData(checkWords.ToArray());

            //    //    modelBuilder.Entity<Word>().HasData(word);
            //    //}
            //    //modelBuilder.Entity<CheckWord>().HasKey(x => x.Id);
            //    //modelBuilder.Entity<WordEng>().HasKey(x => x.Id);
            //    //modelBuilder.Entity<Translate>().HasKey(x => x.Id);
            //    //modelBuilder.Entity<Word>().HasKey(x => x.Id);
            //    modelBuilder.Entity<Word>().HasData(createList.Words.ToArray());
            //    //foreach (Word word in createList.Words)
            //    //{

            //    //}
            //} 
        //}
    }

    private DbSet<CheckWord> checkWords;
    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
    public DbSet<Translate> WordRu { get; set; }
    public DbSet<WordEng> WordEng { get; set; }

}

