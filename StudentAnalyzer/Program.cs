using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using StudentLibrary;

/// <summary>
/// ФИО: Кочарян Тигран Самвелович.
/// Группа: БПИ199.
/// </summary>

namespace StudentAnalyzer
{
    /// <summary>
    /// Класс для реализации LINQ и сериализации JSON.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Метод для вывода в консоль содержимого списка.
        /// </summary>
        /// <param name="students"></param>
        public static void ListShow(List<Student> students)
        {
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine(students[i]);
            }
        }

        /// <summary>
        /// Метод для декомпозиции Main, реализующий
        /// Считывание и десериализуем JSON-файл.
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        //public static List<Student> ReadJSON(List<Student> studentsRead)
        //{
        //    try
        //    {
        //        // using Newtonsoft.Json - помощник в десериализации.
        //        // Альтернатива - стандартный десериализатор из System.Text.
        //        using (JsonReader fs = new JsonTextReader(new StreamReader("../../../../students.json")))
        //        {
        //            JsonSerializer jsonSerializer = new JsonSerializer();
        //            studentsRead = jsonSerializer.Deserialize(fs, typeof(List<Student>)) as List<Student>;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        Console.WriteLine("Problems with File...");
        //    }

        //    // Возвращаем полученный список студентов.
        //    return studentsRead;
        //}

        static void Main(string[] args)
        {
            // Объявляем список объекторв Student.
            // В прошлом коммите я декомпозировал эту часть, но после перехода на .Core,
            // появились проблемы и я вернул сюда десериализацию.
            List<Student> students = new List<Student>();
            try
            {
                // using Newtonsoft.Json - помощник в десериализации.
                // Альтернатива - стандартный десериализатор из System.Text.
                using (JsonReader fs = new JsonTextReader(new StreamReader("../../../../students.json")))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    students = jsonSerializer.Deserialize(fs, typeof(List<Student>)) as List<Student>;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Problems with File...");
            }

            // Выводим полученный из десериалации список.
            ListShow(students);
            Console.WriteLine(Environment.NewLine + "Successfully Deserialized...");

            try
            {
                // С помощью LINQ считаем кол-во студентов из МИЭМ.
                int miemStudents = students.Where
                    (student => student.Faculty == Faculty.MIEM).Count();
                Console.WriteLine(Environment.NewLine + $"[MIEM Students in this group]: {miemStudents}");
            }
            // Отлавливаем исключение в случае ошибки создания нового студента.
            catch (Exception)
            {
                Console.WriteLine("ERROR while working with MIEM...");
            }

            try
            {
                // Альтернативное решение - OrderBy(student => -student.Mark).
                // С помощью LINQ сначала сортируем по убыванию и берем ТОП10 в списке.
                var topStudents = students.OrderByDescending(student => student.Mark).Take(10).ToList();
                Console.WriteLine(Environment.NewLine + $"[Top Students in your group]:");
                ListShow(topStudents);
            }
            // Отлавливаем исключение в случае ошибки создания нового студента.
            catch (Exception)
            {
                Console.WriteLine("ERROR while working with Top...");
            }

            try
            {
                // С помощью LINQ группируем студентов по группам и затем применяем между
                // студентами из одной группы переопределенный оператор +.
                var sumStudents = students.GroupBy(student => student.Faculty).
                    Select(e => e.Aggregate((studentA, studentB) => studentA + studentB)).ToList();
                Console.WriteLine(Environment.NewLine + $"[After summing up all your students]:");
                ListShow(sumStudents);

                // Альтернативное решение - OrderBy(student => -student.Mark)
                // Затем сортируем по убыванию оценок или имен, в случае совпадения.
                var descendingStudents = sumStudents.OrderByDescending(student => student.Mark).
                    ThenBy(student => student.Name).ToList();
                Console.WriteLine(Environment.NewLine + $"[After descending the last group]:");
                ListShow(descendingStudents);
            }
            // Отлавливаем исключение в случае ошибки создания нового студента.
            catch (Exception)
            {
                Console.WriteLine("ERROR while aggregation or descending...");
            }

            Console.ReadLine();
        }
    }
}
