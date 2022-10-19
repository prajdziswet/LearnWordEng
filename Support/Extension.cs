using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Support;

public static class Extension
{
    public static void CheakAndRemove(this HtmlNode htmlNode, string xpath)
    {
        HtmlNode removeNode = htmlNode.SelectSingleNode(xpath);
        if (removeNode != null)
            removeNode.Remove();
    }

    public static void ReplaceLinkWithoutHost(this HtmlNode htmlNode, string host)
    {
        HtmlNodeCollection collection = htmlNode.SelectNodes("//a");
        if (host.Last() == '/') host = host.Substring(0, host.Length - 1);
        string oldAttribute;
        foreach (HtmlNode element in collection)
        {
            if (element.HasAttributes)
            {
                oldAttribute = element.GetAttributeValue("href", null);
                if (oldAttribute != null && oldAttribute != "/" && oldAttribute?[0] != '#'&& !Uri.IsWellFormedUriString(oldAttribute, UriKind.Absolute))
                {
                    if (!Uri.IsWellFormedUriString(oldAttribute, UriKind.Absolute))
                        element.SetAttributeValue("href", host + oldAttribute);
                }
                else if (oldAttribute == "/")
                {
                    element.SetAttributeValue("href", host);
                }
            }
        }
    }

    public static bool IsNullOrEmpty(this string str)
    {
        return String.IsNullOrEmpty(str);
    }

    public static bool ExitURL(this string URI)
        {
            try
            {
                if (URI.IsNullOrEmpty()) return false;
                var request = (HttpWebRequest)WebRequest.Create(URI);
                request.Method = "HEAD";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

    public static void ShuffleInPlace<T>(this IList<T> source)
    {
        source.ShuffleInPlace(new Random());
    }

    private static void ShuffleInPlace<T>(this IList<T> source, Random rng)
    {
        if (source == null || source.Count == 0) throw new ArgumentNullException("sourceList");
        if (rng == null) throw new ArgumentNullException("Random");

        for (int i = 0; i < source.Count - 1; i++)
        {
            int j = rng.Next(i, source.Count);

            T temp = source[j];
            source[j] = source[i];
            source[i] = temp;
        }
    }
}