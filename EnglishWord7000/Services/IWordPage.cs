namespace EnglishWord7000.Services;

public interface IRepeatWordPage
{
    public bool ExistPage { get; }
    public string GetPage();
    public void Next();
}

public interface ILearnWordPage
{
    public string GetRawPage(int id = 0);
    public string GetLinkAdditionWords();
    public bool Next();
}