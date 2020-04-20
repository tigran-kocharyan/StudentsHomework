using System;
using System.Text.RegularExpressions;

/// <summary>
/// Библиотека с параметрами студента.
/// </summary>
namespace StudentLibrary
{
    /// <summary>
    /// Основной класс программы для реализации 
    /// свойств студента  и его переопределения.
    /// </summary>
    public class Student
    {
        // Имя студента.
        public string Name { get; }

        // Факультет студента.
        public Faculty Faculty { get; }

        // Оценка студента.
        public double Mark { get; }

        // Конструктор, принимающий 3 параметра
        // для заполнения свойств студента.
        public Student(string name, Faculty faculty, double mark)
        {
            // С помощью регулярных выражений проверяем строку на корректность.
            if (!Regex.IsMatch(name, @"^[A-Z]{1}[a-z]{3,9}$"))
            {
                // В случае ошибки в имени выбрасываем исключение.
                throw new Exception("Wrong Name...");
            }
            // Оценка должна быть в пределе [4,10).
            if (mark >= 10 || mark < 4)
            {
                // В случае неправильной оценки выбрасываем исключение.
                throw new Exception("Wrong Mark...");
            }

            // Заполняем свойства.
            Name = name;
            Mark = mark;
            Faculty = faculty;
        }

        // Создаем необходимый нам Equals для последующей
        // правильной реализации в AreEqual.
        public bool Equals(Student other)
        {
            // Возвращаем true, если значения совпадают.
            return Name.Equals(other.Name) 
                && Faculty.Equals(other.Faculty) 
                && Mark.Equals(other.Mark);

            // Возможная альтернатива:
            // return this.ToString() == other.ToString();
        }

        // Переопределение оператора + для объектов класса Student.
        public static Student operator +(Student firstStudent, Student secondStudent)
        {
            // Заранее создадим все переменные.
            string newName;
            double newMark;
            Faculty newFaculty;

            // Если длина имени первого студента больше, то используем его половину
            // в начале нового имени.
            if (firstStudent.Name.Length >= secondStudent.Name.Length)
            {
                newName = firstStudent.Name.Substring(0, (int)Math.Ceiling(
                    (double)firstStudent.Name.Length / 2.0)) +
                          secondStudent.Name.Substring((int)Math.Ceiling(
                    (double)secondStudent.Name.Length / 2.0));
            }
            // В противном случае, половина имени второго студента будет идти
            // первой в новом имени студента.
            else
            {
                // (double) избыточен, но все равно так спокойнее, что програма
                // понимает точно, что не нужно использовать int в делении.
                newName = secondStudent.Name.Substring(0, (int)Math.Ceiling(
                    (double)secondStudent.Name.Length / 2.0)) +
                          firstStudent.Name.Substring((int)Math.Ceiling(
                    (double)firstStudent.Name.Length / 2.0));
            }

            // Если факультеты совпадают, то новый студент будет иметь
            // факультет любого из студентов (в данном случае первого).
            if (firstStudent.Faculty == secondStudent.Faculty)
            {
                newFaculty = firstStudent.Faculty;
            }
            // В противном случае, выбрасывается исключение с текстом ошибки.
            else
            {
                throw new ArgumentException
                    ("ERROR! Different faculties are impossible to be summed!");
            }

            // Новая оценка студента - среднее арифметическое двух студентов.
            newMark = (firstStudent.Mark + secondStudent.Mark) / 2.0;

            // Возвращаем нового студента с новыми параметрами.
            return new Student(newName, newFaculty, newMark);
        }

        // Переопределяем ToString, чтобы вернуть строку со значениями
        // свойств студента.
        public override string ToString()
        {
            return $"{Faculty} Student {Name}: Mark = {Mark:F3}";
        }

        // Переопределяем Equals, чтобы вызывать написанный в классе
        // Метод Equals от параметра Student.
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Student);
        }
    }
}
