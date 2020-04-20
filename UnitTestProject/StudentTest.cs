using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentLibrary;
using StudentGenerator;

namespace UnitTestProject
{
    /// <summary>
    /// Класс тестирующего модуля для проверки работоспособности программы.
    /// </summary>
    [TestClass]
    public class StudentTest
    {
        // Статический генератор случайного числа.
        public static readonly Random random = new Random();

        /// <summary>
        /// Тестирование сериализатора и десериализатора.
        /// </summary>
        [TestMethod]
        public void TestSerialize()
        {
            // Объявляем список студентов.
            List<Student> students = new List<Student>();

            // Заполняем его студентами в количестве 50 человек.
            for (int i = 0; i < 50; i++)
            {
                // Сначала генерируем студенту факультет, затем имя и оценку.
                // Генерация рандомного числа с помощью random.
                Faculty faculty = (Faculty)random.Next(3);
                double mark = random.Next(4, 10) + random.NextDouble();
                students.Add(new Student(Program.NameGenerator(random.Next(6, 11)), 
                    faculty, mark));
            }

            // Альтернативная проверка - создать один объект Student, сериализовать его
            // с помощью потока, затем с помощью потока десериализовать (используя using)
            // и сравнить их с помощью AreEqual.

            // Создаем сериализованный JSON нашего списка студентов с форматом отступа.
            var jsonFirst = JsonConvert.SerializeObject(students, Formatting.Indented);
            // Создаем десериализованный список студентов из нашего JSON.
            var studentsDeserialized = JsonConvert.DeserializeObject<List<Student>>(jsonFirst);
            // Снова сериализуем то, что получили после десериализации JSON
            // тем самым список снова переходит в JSON-файл.
            var jsonSecond = JsonConvert.SerializeObject(studentsDeserialized, Formatting.Indented);


            // Затем сравниваем на достоверность с помощью LINQ, равен ли один список другому.
            Assert.IsTrue(Enumerable.SequenceEqual(students, studentsDeserialized));
            // И сравниваем, равны ли JSON файлы между собой.
            Assert.AreEqual(jsonFirst, jsonSecond);
        }

        /// <summary>
        /// Тестирование ToString(), форматирования и сложения Student.
        /// </summary>
        [TestMethod]
        public void TestToStringOperator()
        {
            // Создаем объекты класса Student для тестирования.
            Student a = new Student("Tigran", Faculty.CS, 4);
            Student b = new Student("Natalia", Faculty.CS, 5.55);
            Student c = new Student("Zachariy", Faculty.MIEM, 6.666);
            Student d = new Student("Pupalupa", Faculty.MIEM, 7.7777);
            Student e = new Student("Gennadiiy", Faculty.Design, 8.88888);
            Student f = new Student("Evdakimalo", Faculty.Design, 9.999999);

            // Тестируем переопределенный оператор +.
            Student result1 = a + b;
            Student result2 = c + d;
            Student result3 = d + c;
            Student result4 = e + f;

            // Проверим, будет ли исключение, если соединить разные факультеты.
            Assert.ThrowsException<ArgumentException>(() => a + c);

            // Создаем объекты для последующего сравнения.
            Student expected1 = new Student("Nataran", Faculty.CS, (4 + 5.55) / 2.0);
            Student expected2 = new Student("Zachlupa", Faculty.MIEM, (6.666 + 7.7777) / 2.0);
            Student expected3 = new Student("Pupaariy", Faculty.MIEM, (7.7777 + 6.666) / 2.0);
            Student expected4 = new Student("Evdakdiiy", Faculty.Design, (8.88888 + 9.999999) / 2.0);

            // Сравниваем то, что планировали получить с тем, что выдает программа.
            Assert.AreEqual(result1, expected1);
            Assert.AreEqual(result2, expected2);
            Assert.AreEqual(result3, expected3);
            Assert.AreEqual(result4, expected4);
        }
    }
}
