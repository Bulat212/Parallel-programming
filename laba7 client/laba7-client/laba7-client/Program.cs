using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace laba7_client
{
    internal class Program
    {
        static HttpClient client = new HttpClient();

        static async Task Main()
        {
            await RunAsync();
        }

        static async Task RunAsync()
        {
            client.BaseAddress = new Uri("https://localhost:7007");
            while (true)
            {
                Console.WriteLine("Введите ФИО студента: ");
                string FIO = Console.ReadLine();
                HttpResponseMessage response = await client.GetAsync($"https://localhost:7007/api/Values/student/{FIO}");
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }
        }
    }
}
