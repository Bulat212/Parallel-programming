using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;

namespace laba7_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Bulat\Desktop\Students.accdb");
        [HttpGet, Route("student/{FIO}")]
        public string Get(string FIO)
        {

            DataTable dt_students = new DataTable("Студенты");
            //OleDbDataAdapter adapter_stud = new OleDbDataAdapter($"SELECT * FROM [Студенты] WHERE [ФИО]='{FIO}'", connection);
            OleDbDataAdapter adapter_stud = new OleDbDataAdapter("SELECT * FROM [Студенты]", connection);
            adapter_stud.Fill(dt_students);
            var poiskStud = from StudentsRow in dt_students.AsEnumerable()
                             where (string)StudentsRow["ФИО"] == FIO
                             select StudentsRow;
            if (!poiskStud.Any()) return "Такого студента нет";
            DataTable filteredDt_stud = poiskStud.CopyToDataTable();



            DataTable dt_marks = new DataTable("Оценки");
            //OleDbDataAdapter adapter_mark = new OleDbDataAdapter($"SELECT * FROM [Оценки] WHERE [Код студента] = {dt_students.Rows[0]["Код студента"]}", connection);
            OleDbDataAdapter adapter_mark = new OleDbDataAdapter($"SELECT * FROM [Оценки]", connection);
            adapter_mark.Fill(dt_marks);
            var poiskocenok = from markRow in dt_marks.AsEnumerable()
                              where (int)markRow["Код студента"] == (int)filteredDt_stud.Rows[0]["Код студента"]
                              select markRow;
            
            if (!poiskocenok.Any()) return "Ошибка";

            DataTable filteredDt_marks = poiskocenok.CopyToDataTable();


            DataTable dt_subjects = new DataTable("Предметы");
            OleDbDataAdapter adapter_subject;

            string response = "";
            List<string> missedExams = new List<string>();

            if ( (int)filteredDt_marks.Rows[0]["Оценка 1"] == 0)
            {
                //adapter_subject = new OleDbDataAdapter($"SELECT * FROM [Предметы] WHERE [Код предмета] = {dt_marks.Rows[0]["Код предмета 1"]}", connection);
                
                adapter_subject = new OleDbDataAdapter($"SELECT * FROM [Предметы]", connection);

                adapter_subject.Fill(dt_subjects);
                var subject_1 = (from subjectRow in dt_subjects.AsEnumerable()
                                 where (int)subjectRow["Код предмета"] == (int)filteredDt_marks.Rows[0]["Код предмета 1"]
                                 select (string)subjectRow["Название предмета"]).FirstOrDefault();
                if (subject_1 == null) return "Ошибка";
                else missedExams.Add(subject_1);
            }

            if ((int)filteredDt_marks.Rows[0]["Оценка 2"] == 0)
            {
                //adapter_subject = new OleDbDataAdapter($"SELECT * FROM [Предметы] WHERE [Код предмета] = {dt_marks.Rows[0]["Код предмета 2"]}", connection);
               
                adapter_subject = new OleDbDataAdapter($"SELECT * FROM [Предметы]", connection);

                dt_subjects.Clear();
                adapter_subject.Fill(dt_subjects);
                var subject_2 = (from subjectRow in dt_subjects.AsEnumerable()
                                where (int)subjectRow["Код предмета"] == (int)filteredDt_marks.Rows[0]["Код предмета 2"]
                                select (string)subjectRow["Название предмета"]).FirstOrDefault();
                if (subject_2 == null) return "Ошибка";
                missedExams.Add(subject_2);
            }

            var student = (from studentRow in dt_students.AsEnumerable()
                          where (string)studentRow["ФИО"] == FIO
                          select (string)studentRow["ФИО"]).FirstOrDefault();

            if (missedExams.Count == 0)
                response += student + " не имеет пропущенных экзаменов";
            else if (missedExams.Count == 1)
                response += student + " пропустил(-а) экзамен по предмету: " + missedExams[0];
            else
                response += student + " пропустил(-а) экзамены по предметам: " + missedExams[0] + ", " + missedExams[1];
            return response;
        }

    }
}