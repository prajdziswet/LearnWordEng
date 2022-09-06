namespace EnglishWord7000.Services;

public interface ISetCookies
{
    public void Set(int Id, int count);
}

public interface IGetCookies
{
    public int Id { get; }
    public int? count { get; }

}