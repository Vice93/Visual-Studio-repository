using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Publisher.Data.Access;
using Publisher.Data.Model;

namespace Publisher.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {

            DoStuff();
            System.Console.ReadLine();

        }
        private static async void DoStuff()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:65467/api/books")
            };

            var res = await client.GetStringAsync("");
            var books = JsonConvert.DeserializeObject<Book[]>(res);
            foreach (var book in books)
            {
                System.Console.WriteLine(book);
            }
        }
    }
}
