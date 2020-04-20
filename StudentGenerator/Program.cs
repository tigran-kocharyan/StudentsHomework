using System;
using System.Collections.Generic;
using StudentLibrary;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// В данном пространстве имен реализуется 
/// генерация и сериализация списка студентов.
/// </summary>
namespace StudentGenerator
{
    /// <summary>
    /// Класс генерации и сериализации списка студентов
    /// </summary>
    public class Program
    {
        // Статический генератор случайного числа.
        public static readonly Random random = new Random();

        // Метод для реализации генерации имени студента
        // согласно спецификации.
        public static string NameGenerator(int length)
        {
            // Альтернативное решение -  string name = "".
            string name = String.Empty;

            // Добавляем заглавную латинскую букву и 
            // затем добавляем строчные в количестве length-1.
            name += (char)random.Next('A','Z');
            for (int i = 0; i < length-1; i++)
            {
                name += (char)random.Next('a','z');
            }

            // Возвращаем полученное сгенерированное имя.
            return name;
        }

        /// <summary>
        /// Точка входа в программу, которая вызывает метод генерации случайного имени
        /// и запускает затем сериализациюю сгенерированного списка студентов.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Объявляем список студентов.
            List<Student> students = new List<Student>();

            // Заполняем его студентами в количестве 30 человек.
            for (int i = 0; i < 30; i++)
            {
                // Сначала генерируем студенту факультет, затем имя и оценку.
                // Генерация рандомного числа с помощью random.
                try
                {
                    Faculty faculty = (Faculty)random.Next(3);
                    double mark = random.Next(4, 10) + random.NextDouble();
                    students.Add(new Student(NameGenerator(random.Next(6, 11)), faculty, mark));
                }
                // В случае неверных параметров обрабатываем исключение.
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }            
            }
            // Альтернативное решение - поместить вывод students[i].ToString() сразу же после генерации
            // самого элемента в цикле чуть выше.
            for (int i = 0; i < 30; i++)
            {
                Console.WriteLine(students[i].ToString());
            }
            try
            {
                // using Newtonsoft.Json - помощник в реализации сериализации.
                // Создаем файл JSON в папке с .sln именем students.json.
                using (JsonWriter fs = new JsonTextWriter(new StreamWriter("../../../../students.json")))
                {
                    JsonSerializer jsonSerializer = new JsonSerializer();
                    jsonSerializer.Serialize(fs, students);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Problems with File...");
            }

            // Альтернатива Environment.NewLine - "\n".
            // Выводим пользователю сообщение о том,
            // что все успешно сериализовано.
            Console.WriteLine(Environment.NewLine + "Successfully Serialized...");
        }
    }
}
