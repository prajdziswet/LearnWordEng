namespace EnglishWord7000.Services;

public class GetCookies : IGetCookies
{
    public int Id { get; }
    public int? count { get; }

    public GetCookies(IRequestCookieCollection requestCookies)
    {
        if (requestCookies.ContainsKey("count"))
        {
            count = Int32.Parse(requestCookies["count"]);
            Id = Int32.Parse(requestCookies["Id"]);

        }
        else count = null;
    }
}


public class SetCookies : ISetCookies
{
    private IResponseCookies responseCookies;
    public SetCookies(IResponseCookies responseCookies)
    {
        this.responseCookies = responseCookies;
    }

    public void Set(int Id, int count)
    {
        responseCookies.Append("Id", Id.ToString());
        responseCookies.Append("count", count.ToString());
    }
}