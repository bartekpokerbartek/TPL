using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class WebDownloader
    {
        public async Task<int> GetWebsiteLengthAsync(string uri)
        {
            HttpClient client = new HttpClient();

            var getContent = client.GetStringAsync(uri);

            Console.WriteLine("GetWebsiteLength(): Some other work");

            string content = await getContent;

            return content.Length;
        }
    }
}
