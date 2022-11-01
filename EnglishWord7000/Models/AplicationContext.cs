using System.Runtime.ExceptionServices;
using Microsoft.EntityFrameworkCore;
using Support;

namespace EnglishWord7000.Models;

public class AplicationContext:DbContext
{
    bool canConnect = true;
    public AplicationContext(DbContextOptions<AplicationContext> options):base(options)
    {
        canConnect = Database.CanConnect();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!canConnect)
        {
            CreateList createList = new CreateList();
            int wordId = 0, EngId = 0, RuId = 0, CheckId = 0;
            Dictionary<string, int> wordsRu = new Dictionary<string, int>();

            foreach (Word word in createList.Words)
            {
                int TranslateId;
                if (!wordsRu.ContainsKey(word.WordRu.WordEng))
                {
                    Translate Ru = word.WordRu;
                    TranslateId = ++RuId;
                    wordsRu.Add(word.WordRu.WordEng, TranslateId);
                    modelBuilder.Entity<Translate>().HasData(new Translate() { Id = TranslateId, HtmlRu = Ru.HtmlRu, WordEng = Ru.WordEng });
                    if (Ru.CheckWords != null && Ru.CheckWords.Count > 0)
                        foreach (CheckWord checkWord in Ru.CheckWords)
                        {
                            modelBuilder.Entity<CheckWord>().HasData(new CheckWord()
                            {
                                Id = ++CheckId,
                                TranslateId = TranslateId,
                                EngPhrase = checkWord.EngPhrase,
                                RuPhrase = checkWord.RuPhrase
                            });
                        }
                }
                else
                {
                    TranslateId = wordsRu[word.WordRu.WordEng];
                }

                modelBuilder.Entity<Word>().HasData(new Word() { Id = ++wordId, TranslateId = TranslateId });
                WordEng Eng = word.WordEng;
                modelBuilder.Entity<WordEng>().HasData(new WordEng() { Id = ++EngId, WordId = wordId, HtmlEng = Eng.HtmlEng, Level = Eng.Level, link = Eng.link, obj = Eng.obj, Word = Eng.Word });
            } 
        }
    }

    private DbSet<CheckWord> checkWords;
    public DbSet<Word> Words { get; set; }
    public DbSet<LearnWord> LearnWord { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PropertyUser> PropertyUsers { get; set; }
    public DbSet<Translate> WordRu { get; set; }
    public DbSet<WordEng> WordEng { get; set; }

}

