using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Support;

// if I deside to download file.
public class Download
{
    private static int count { get; set; }= 0;
    private static int countAll { get; set; } = 0;
    private static object loker = new();
    private static Dictionary<string,string> dictionary=new Dictionary<string,string>();
    public static bool TheEnd
    {
        get
        {
            Task.Delay(100);
            WriteConsole();
            if (countAll==0) return false;
            else if (countAll== count) return true;
            return false;
        }
    }

    private static void WriteConsole()
    {
        Console.WriteLine($"\r{count}/{countAll}");
    }

    public static void Run(HtmlNode htmlNode)
    {
        if (htmlNode==null) return;
        string pathGB = htmlNode.SelectSingleNode("//div[contains(@class,'pron-uk')]")
            ?.GetAttributeValue("data-src-mp3", null);
        string pathUSA = htmlNode.SelectSingleNode("//div[contains(@class,'pron-us')]")
            ?.GetAttributeValue("data-src-mp3", null);
        if (pathGB.ExitURL() && pathUSA.ExitURL()&&!dictionary.ContainsKey(pathGB))
        {
            dictionary.Add(pathGB, pathUSA);
            Start(pathGB);
            Start(pathUSA);
            countAll += 2;
        }
    }

    private static void Start(string uri)
    {
        (new Task(() => _DownloadFile(uri))).Start();
    }

    private static string CreateAndReturnDirectory(string uri)
    {
            string extension = Path.GetExtension(uri);
            if (extension == ".mp3") extension = "/mp3";
            else extension = "/ogg";
            string pathDirectory = Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName+"\\DownloadForWords";
            if (!Directory.Exists(pathDirectory)) Directory.CreateDirectory(pathDirectory);
            if (!Directory.Exists(pathDirectory+ extension)) Directory.CreateDirectory(pathDirectory+ extension);

        return pathDirectory + extension+ uri.Substring(uri.LastIndexOf('/'));
    }

    private static async void _DownloadFile(string uri)
    {
        try
        {
            string pathfile = CreateAndReturnDirectory(uri);
            byte[] data;

            if (!File.Exists(pathfile))
                using (var client = new HttpClient())
                using (HttpResponseMessage response = await client.GetAsync(uri))
                using (HttpContent content = response.Content)
                {
                    data = await content.ReadAsByteArrayAsync();
                    using (FileStream file = File.Create(pathfile)) //path = "wwwroot\\XML\\1.zip"
                    {
                        await file.WriteAsync(data, 0, data.Length);
                    }

                }

            lock (loker)
            {
                count++;
                WriteConsole();
            }
        }
        catch (Exception)
        {
        }
        
    }
}