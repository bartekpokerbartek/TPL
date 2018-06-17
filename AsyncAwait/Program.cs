using System;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            var downloader = new WebDownloader();
            var result = downloader.GetWebsiteLengthAsync("http://facebook.com");

            Console.WriteLine("Main(): Work work " + result.Result);

            Console.ReadKey();
        }
    }
}
