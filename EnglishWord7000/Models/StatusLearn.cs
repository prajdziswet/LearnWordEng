using Support;

namespace EnglishWord7000.Models;

public class StatusLearn
{
    public int Id { get; set; }
    public User User { get; set; }
    public string level { get; set; }
    public List<LearnWord> LearnWords { get; set; }=new List<LearnWord>();
}

public class LearnWord
{
    public int Id { get; set; }
    public Word learnedWord { get; set; }
    public int Repeat { get; set; }
    public DateTime FistTime;
}

public static class Levels
{
    public static List<string> levels = new List<string>(){"a1","a2","b1","b2","c1"};
}