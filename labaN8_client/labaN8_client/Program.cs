using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace labaN8_client
{
    [Serializable]
    public class Students
    {
        public Students() { }
        public string name { get; set; }
        public List<string> predmets { get; set; }
        public double percentfive { get; set; }
    }

    class Program
    {

        static HttpClient client = new HttpClient();
        static async Task Main()
        {
            await RunAsync();
        }

        static async Task RunAsync()
        {

            //Создание HTTP-клиента
            client.BaseAddress = new Uri("https://localhost:7221");
            HttpResponseMessage response = await client.GetAsync("https://localhost:7221/Home");
            var responseString = await response.Content.ReadAsStringAsync();
            List<Students> receivedmessage = JsonConvert.DeserializeObject<List<Students>>(responseString);

            Console.WriteLine("    ПОЛУЧЕННЫЙ ОБЪЕКТ    \n"); 

            foreach (var student in receivedmessage)
            {
                if (student.percentfive >= 75)
                {
                    Console.WriteLine($"Студент: {student.name}, Процент успеваемости: {student.percentfive}%");

                    foreach (var subject in student.predmets)
                    {
                        Console.WriteLine($" {subject}");
                    }
                    Console.WriteLine("\n");
                }
            }


            //двоичная сериализация

            BinaryFormatter formatter = new BinaryFormatter();
            Console.WriteLine("\n   ДВОИЧНАЯ СЕРИАЛТИЗАЦИЯ    \n");

            using (FileStream fs = new FileStream("Otlichniki.dat", FileMode.Create))
            {

                formatter.Serialize(fs, receivedmessage);
            }

            using (FileStream fs = new FileStream("Otlichniki.dat", FileMode.Open))
            {
                List<Students> std = formatter.Deserialize(fs) as List<Students>;

                foreach (var student in std)
                {
                    if (student.percentfive >= 75)
                    {
                        Console.WriteLine($"Студент: {student.name}, Процент успеваемости: {student.percentfive}%,\nПредметы:\n{string.Join(", ", student.predmets)}\n");
                    }
                }
            }

            //xml сериализация

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Students>));
            Console.WriteLine("\n   XML СЕРИАЛИЗАЦИЯ   \n");
            
            using (FileStream fs = new FileStream("Otlichniki.xml", FileMode.Create))
            {
                 xmlSerializer.Serialize(fs, receivedmessage);
            }

            using (FileStream fs = new FileStream("Otlichniki.xml", FileMode.Open))
            {

                List<Students> std = xmlSerializer.Deserialize(fs) as List<Students>;

                foreach (var student in std)
                {
                    if (student.percentfive >= 75)
                    {
                        Console.WriteLine($"Студент: {student.name}, Процент успеваемости: {student.percentfive}%,\nПредметы: \n{string.Join(", ", student.predmets)}\n");
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
