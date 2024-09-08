using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace labaN8.Controllers
{
    public class Students
    {
        public string name { get; set; }
        public List<string> predmets { get; set; }
        public double percentfive { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Bulat\Desktop\Students.accdb;");


            DataTable ocenki = new DataTable("Оценки");
            DataTable predmets = new DataTable("Предметы");
            DataTable stud = new DataTable("Студенты");

            OleDbDataAdapter adapter1 = new OleDbDataAdapter("SELECT * FROM Оценки", connection);
            OleDbDataAdapter adapter2 = new OleDbDataAdapter("SELECT * FROM Предметы", connection);
            OleDbDataAdapter adapter3 = new OleDbDataAdapter("SELECT * FROM Студенты", connection);

            adapter1.Fill(ocenki);
            adapter2.Fill(predmets);
            adapter3.Fill(stud);


            var marksGroupedByStudent = (from mark in ocenki.AsEnumerable()
                                         from pred in predmets.AsEnumerable()
                                         from s in stud.AsEnumerable()
                                         where s.Field<int>("Код студента") == mark.Field<int>("Код студента")
                                            && (pred.Field<int>("Код предмета") == mark.Field<int>("Код предмета 1")
                                                || pred.Field<int>("Код предмета") == mark.Field<int>("Код предмета 2"))
                                         select new
                                         {
                                             StudentName = s.Field<string>("ФИО"),
                                             SubjectName = pred.Field<string>("Название предмета"),
                                             Mark = pred.Field<int>("Код предмета") == mark.Field<int>("Код предмета 1")
                                                    ? mark.Field<int>("Оценка 1")
                                                    : mark.Field<int>("Оценка 2")
                                         })
                            .GroupBy(item => item.StudentName); //


            // Расчет процента успеваемости для каждого студента
            var successPercentages = marksGroupedByStudent.Select(group =>
            {
                // Вычисляем общее количество оценок у студента
                int totalMarks = group.Count();

                // Вычисляем количество пятерок у студента
                int excellentMarks = group.Count(item => item.Mark == 5);

                // Вычисляем процент успеваемости студента
                double successPercentage = (double)excellentMarks / totalMarks * 100;

                var predms = group.Where(g => g.Mark == 5).Select(g => g.SubjectName);

                // Возвращаем имя студента и процент успеваемости
                return new
                {
                    StudentName = group.Key,
                    SuccessPercentage = successPercentage,
                    Subjects = predms
                };
            });

            List<Students> students = new();
            List<string> predmetsname = new();

            foreach (var student in successPercentages)
            {
                if (student.SuccessPercentage >= 75)
                {
                    Console.WriteLine($"Студент: {student.StudentName}, Процент успеваемости: {student.SuccessPercentage}%");
                    
                    foreach (var subject in student.Subjects)
                    {
                        predmetsname.Add(subject);
                        Console.WriteLine($" {subject}");
                    }
                    students.Add(new Students { name = student.StudentName, predmets = predmetsname, percentfive = student.SuccessPercentage });
                    predmetsname = new();
                }
            }


            return Ok(students);
        }
    }
}
