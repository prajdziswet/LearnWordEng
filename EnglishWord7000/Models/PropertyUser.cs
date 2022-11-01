using HtmlAgilityPack;
using Support;

namespace EnglishWord7000.Models;

public class PropertyUser
{
    public int Id { get; set; }
    public User User { get; set; }
    public string level { get; set; }

    public int WordLearn { get; set; } = 20;
    public int StartLearn { get; set; } = 0;
}

public class LearnWord
{
    public int Id { get; set; }
    public Word learnedWord { get; set; }
    public User User { get; set; }
    public int Repeat { get; set; }
    public DateTime FistTime { get; set; }
}

public class Levels
{
    public static List<string> levels = new List<string>() { "a1", "a2", "b1", "b2", "c1" };
    private int levelint = 0;

    public string Level
    {
        get {
        return levels[levelint];
        }
    }

    public int StartLevel
    {
        get;
        private set;
    }
    public int FinishLevel
    {
        get;
        private set;
    }

    public int FinishAddWord
    {
        get;
        private set;
    }

    public void Next()
    {
        if (levelint + 1 < levels.Count) levelint++;
        SetProperty();
    }

    public Levels(string level)
    {
        switch (level)
        {
            case "a2":levelint = 1;break;
            case "b1":levelint = 2; break;
            case "b2": levelint = 3; break;
            case "c1": levelint = 2; break;
            default: levelint = 0; break;
        }

        SetProperty();
    }

    private void SetProperty()
    {
        switch (levelint)
        {
            case 1: StartLevel=1568;
                FinishLevel = 2547;
                FinishAddWord = 3012; break;
            case 2:
                StartLevel = 3013;
                FinishLevel = 3909;
                FinishAddWord = 4256; break;
            case 3:
                StartLevel = 4257;
                FinishLevel = 5823;
                FinishAddWord = 6323; break;
            case 4:
                StartLevel = 6324;
                FinishLevel = 7727;
                FinishAddWord = 8052; break;
            default: StartLevel = 0;
                FinishLevel = 1051;
                FinishAddWord = 1567; break;
        }
    }

}
