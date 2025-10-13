using System;
using System.Collections.Generic;
using System.Linq;

namespace Prac5
{
    // Абстрактный класс Person (Абстракция)
    public abstract class Person
    {
        // Инкапсуляция - приватные поля
        private string name;
        private int age;

        protected Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        // Свойства с валидацией (Инкапсуляция)
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Имя не может быть пустым");
                name = value;
            }
        }

        public int Age
        {
            get => age;
            set
            {
                if (value < 16 || value > 100)
                    throw new ArgumentException("Возраст должен быть от 16 до 100 лет");
                age = value;
            }
        }

        // Абстрактный метод (Полиморфизм)
        public abstract string GetInfo();

        // Виртуальный метод с базовой реализацией
        public virtual string GetBasicInfo()
        {
            return $"{Name}, {Age} лет";
        }
    }

    // Класс Student (Наследование)
    public class Student : Person
    {
        private static int nextId = 1;
        public int StudentId { get; }
        private List<Course> courses;

        public Student(string name, int age) : base(name, age)
        {
            StudentId = nextId++;
            courses = new List<Course>();
        }

        public void EnrollInCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.AddStudent(this);
            }
        }

        public void DropCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (courses.Contains(course))
            {
                courses.Remove(course);
                course.RemoveStudent(this);
            }
        }

        public List<Course> GetEnrolledCourses()
        {
            return new List<Course>(courses);
        }

        // Полиморфизм - переопределение метода
        public override string GetInfo()
        {
            return $"СТУДЕНТ: {GetBasicInfo()}, ID: {StudentId}";
        }

        public string GetDetailedInfo()
        {
            var courseList = courses.Count > 0
                ? string.Join(", ", courses.Select(c => c.CourseName))
                : "не записан на курсы";

            return $"{GetInfo()}, Курсы: {courseList}";
        }
    }

    // Класс Teacher (Наследование)
    public class Teacher : Person
    {
        private static int nextId = 1;
        public int TeacherId { get; }
        private List<Course> courses;

        public Teacher(string name, int age, string department) : base(name, age)
        {
            TeacherId = nextId++;
            Department = department;
            courses = new List<Course>();
        }

        public string Department { get; set; }

        public void AssignToCourse(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.AssignTeacher(this);
            }
        }

        public void RemoveFromCourse(Course course)
        {
            if (courses.Contains(course))
            {
                courses.Remove(course);
                if (course.Teacher == this)
                    course.RemoveTeacher();
            }
        }

        public List<Course> GetAssignedCourses()
        {
            return new List<Course>(courses);
        }

        // Полиморфизм - переопределение метода
        public override string GetInfo()
        {
            return $"ПРЕПОДАВАТЕЛЬ: {GetBasicInfo()}, ID: {TeacherId}, Кафедра: {Department}";
        }

        public string GetDetailedInfo()
        {
            var courseList = courses.Count > 0
                ? string.Join(", ", courses.Select(c => c.CourseName))
                : "не назначен на курсы";

            return $"{GetInfo()}, Ведет курсы: {courseList}";
        }
    }

    // Класс Course
    public class Course
    {
        private static int nextId = 1;
        public int CourseId { get; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public Teacher Teacher { get; private set; }
        private List<Student> students;

        public Course(string courseName, string description)
        {
            CourseId = nextId++;
            CourseName = courseName;
            Description = description;
            students = new List<Student>();
            Teacher = null;
        }

        // Инкапсуляция - управление доступом к данным
        public void AssignTeacher(Teacher teacher)
        {
            Teacher = teacher;
        }

        public void RemoveTeacher()
        {
            Teacher = null;
        }

        // Добавляем студента
        public void AddStudent(Student student)
        {
            if (student != null && !students.Contains(student))
            {
                students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            if (student != null)
            {
                students.Remove(student);
            }
        }

        public List<Student> GetEnrolledStudents()
        {
            return new List<Student>(students);
        }

        public string GetInfo()
        {
            var teacherInfo = Teacher != null ? Teacher.Name : "не назначен";
            return $"КУРС: {CourseName} (ID: {CourseId})\n" +
                   $"Описание: {Description}\n" +
                   $"Преподаватель: {teacherInfo}\n" +
                   $"Количество студентов: {students.Count}";
        }

        public string GetDetailedInfo()
        {
            var teacherInfo = Teacher != null ? Teacher.GetInfo() : "Преподаватель не назначен";
            var studentList = students.Count > 0
                ? string.Join("\n", students.Select(s => $"  - {s.Name} (ID: {s.StudentId})"))
                : "  Нет записанных студентов";

            return $"{GetInfo()}\n" +
                   $"Преподаватель: {teacherInfo}\n" +
                   $"Студенты:\n{studentList}";
        }
    }

    // Основной класс системы управления университетом
    public class UniversityManager
    {
        private List<Student> students;
        private List<Teacher> teachers;
        private List<Course> courses;

        public UniversityManager()
        {
            students = new List<Student>();
            teachers = new List<Teacher>();
            courses = new List<Course>();
        }

        // Методы для работы со студентами
        public void AddStudent(string name, int age)
        {
            var student = new Student(name, age);
            students.Add(student);
            Console.WriteLine($"Студент добавлен: {student.GetInfo()}");
        }

        public Student FindStudentById(int id)
        {
            return students.FirstOrDefault(s => s.StudentId == id);
        }

        public void DisplayAllStudents()
        {
            Console.WriteLine("\nВСЕ СТУДЕНТЫ");
            if (!students.Any())
            {
                Console.WriteLine("Студентов нет в системе");
                return;
            }

            foreach (var student in students)
            {
                Console.WriteLine(student.GetInfo());
            }
        }

        public void DisplayStudentDetails(int studentId)
        {
            var student = FindStudentById(studentId);
            if (student != null)
            {
                Console.WriteLine($"\nДЕТАЛЬНАЯ ИНФОРМАЦИЯ О СТУДЕНТЕ");
                Console.WriteLine(student.GetDetailedInfo());
            }
            else
            {
                Console.WriteLine("Студент не найден");
            }
        }

        // Методы для работы с преподавателями
        public void AddTeacher(string name, int age, string department)
        {
            var teacher = new Teacher(name, age, department);
            teachers.Add(teacher);
            Console.WriteLine($"Преподаватель добавлен: {teacher.GetInfo()}");
        }

        public Teacher FindTeacherById(int id)
        {
            return teachers.FirstOrDefault(t => t.TeacherId == id);
        }

        public void DisplayAllTeachers()
        {
            Console.WriteLine("\nВСЕ ПРЕПОДАВАТЕЛИ");

            if (!teachers.Any())
            {
                Console.WriteLine("Преподавателей нет в системе");
                return;
            }

            foreach (var teacher in teachers)
            {
                Console.WriteLine(teacher.GetInfo());
            }
        }

        public void DisplayTeacherDetails(int teacherId)
        {
            var teacher = FindTeacherById(teacherId);
            if (teacher != null)
            {
                Console.WriteLine($"\nДЕТАЛЬНАЯ ИНФОРМАЦИЯ О ПРЕПОДАВАТЕЛЕ");
                Console.WriteLine(teacher.GetDetailedInfo());
            }
            else
            {
                Console.WriteLine("Преподаватель не найден");
            }
        }

        // Методы для работы с курсами
        public void AddCourse(string courseName, string description)
        {
            var course = new Course(courseName, description);
            courses.Add(course);
            Console.WriteLine($"Курс добавлен: {course.GetInfo()}");
        }

        public Course FindCourseById(int id)
        {
            return courses.FirstOrDefault(c => c.CourseId == id);
        }

        public void DisplayAllCourses()
        {
            Console.WriteLine("\nВСЕ КУРСЫ");
            if (!courses.Any())
            {
                Console.WriteLine("Курсов нет в системе");
                return;
            }

            foreach (var course in courses)
            {
                Console.WriteLine(course.GetInfo());
            }
        }

        public void DisplayCourseDetails(int courseId)
        {
            var course = FindCourseById(courseId);
            if (course != null)
            {
                Console.WriteLine($"\nДЕТАЛЬНАЯ ИНФОРМАЦИЯ О КУРСЕ");
                Console.WriteLine(course.GetDetailedInfo());
            }
            else
            {
                Console.WriteLine("Курс не найден");
            }
        }

        // Методы для связывания сущностей
        public void EnrollStudentInCourse(int studentId, int courseId)
        {
            var student = FindStudentById(studentId);
            var course = FindCourseById(courseId);

            if (student == null)
            {
                Console.WriteLine("Студент не найден");
                return;
            }

            if (course == null)
            {
                Console.WriteLine("Курс не найден");
                return;
            }

            student.EnrollInCourse(course);
            Console.WriteLine($"Студент {student.Name} записан на курс {course.CourseName}");
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var teacher = FindTeacherById(teacherId);
            var course = FindCourseById(courseId);

            if (teacher == null)
            {
                Console.WriteLine("Преподаватель не найден");
                return;
            }

            if (course == null)
            {
                Console.WriteLine("Курс не найден");
                return;
            }

            teacher.AssignToCourse(course);
            Console.WriteLine($"Преподаватель {teacher.Name} назначен на курс {course.CourseName}");
        }

        // Демонстрация полиморфизма
        public void DisplayAllPeople()
        {
            Console.WriteLine("\nВСЕ УЧАСТНИКИ СИСТЕМЫ");
            var allPeople = students.Cast<Person>().Concat(teachers.Cast<Person>()).ToList();

            if (!allPeople.Any())
            {
                Console.WriteLine("Участников нет в системе");
                return;
            }

            foreach (var person in allPeople)
            {
                // Полиморфизм - один метод, разное поведение
                Console.WriteLine(person.GetInfo());
            }
        }
    }

    // Класс для консольного интерфейса
    public class ConsoleMenu
    {
        private UniversityManager universityManager;

        public ConsoleMenu()
        {
            universityManager = new UniversityManager();
        }

        public void Run()
        {
            Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ");
            InitializeData();

            while (true)
            {
                DisplayMainMenu();

                var choice = GetUserChoice();

                try
                {
                    ProcessMainMenuChoice(choice);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("\nГЛАВНОЕ МЕНЮ");
            Console.WriteLine("1. Управление студентами");
            Console.WriteLine("2. Управление преподавателями");
            Console.WriteLine("3. Управление курсами");
            Console.WriteLine("4. Показать всех участников");
            Console.WriteLine("5. Записать студента на курс");
            Console.WriteLine("6. Назначить преподавателя на курс");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");
        }

        private ConsoleKey GetUserChoice()
        {
            ConsoleKey key = Console.ReadKey().Key;
            Console.Clear();
            return key;
        }

        private void ProcessMainMenuChoice(ConsoleKey choice)
        {
            switch (choice)
            {
                case ConsoleKey.D1:
                    ManageStudents();
                    break;
                case ConsoleKey.D2:
                    ManageTeachers();
                    break;
                case ConsoleKey.D3:
                    ManageCourses();
                    break;
                case ConsoleKey.D4:
                    universityManager.DisplayAllPeople();
                    break;
                case ConsoleKey.D5:
                    EnrollStudentInCourse();
                    break;
                case ConsoleKey.D6:
                    AssignTeacherToCourse();
                    break;
                case ConsoleKey.D0:
                    Console.WriteLine("Выход из системы...");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }

        private void ManageStudents()
        {
            while (true)
            {
                Console.WriteLine("\nУПРАВЛЕНИЕ СТУДЕНТАМИ");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Показать детальную информацию о студенте");
                Console.WriteLine("4. Назад");
                Console.Write("Выберите опцию: ");

                var choice = GetUserChoice();
                switch (choice)
                {
                    case ConsoleKey.D1:
                        AddStudent();
                        break;
                    case ConsoleKey.D2:
                        universityManager.DisplayAllStudents();
                        break;
                    case ConsoleKey.D3:
                        ShowStudentDetails();
                        break;
                    case ConsoleKey.D4:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }

        private void AddStudent()
        {
            Console.Write("Введите имя студента: ");
            var name = Console.ReadLine();

            Console.Write("Введите возраст студента: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный возраст");
                return;
            }

            universityManager.AddStudent(name, age);
        }

        private void ShowStudentDetails()
        {
            Console.Write("Введите ID студента: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                universityManager.DisplayStudentDetails(id);
            }
            else
            {
                Console.WriteLine("Некорректный ID");
            }
        }

        private void ManageTeachers()
        {
            while (true)
            {
                Console.WriteLine("\nУПРАВЛЕНИЕ ПРЕПОДАВАТЕЛЯМИ");
                Console.WriteLine("1. Добавить преподавателя");
                Console.WriteLine("2. Показать всех преподавателей");
                Console.WriteLine("3. Показать детальную информацию о преподавателе");
                Console.WriteLine("4. Назад");
                Console.Write("Выберите опцию: ");

                var choice = GetUserChoice();
                switch (choice)
                {
                    case ConsoleKey.D1:
                        AddTeacher();
                        break;
                    case ConsoleKey.D2:
                        universityManager.DisplayAllTeachers();
                        break;
                    case ConsoleKey.D3:
                        ShowTeacherDetails();
                        break;
                    case ConsoleKey.D4:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }

        private void AddTeacher()
        {
            Console.Write("Введите имя преподавателя: ");
            var name = Console.ReadLine();

            Console.Write("Введите возраст преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Некорректный возраст");
                return;
            }

            Console.Write("Введите кафедру преподавателя: ");
            var department = Console.ReadLine();

            universityManager.AddTeacher(name, age, department);
        }

        private void ShowTeacherDetails()
        {
            Console.Write("Введите ID преподавателя: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                universityManager.DisplayTeacherDetails(id);
            }
            else
            {
                Console.WriteLine("Некорректный ID");
            }
        }

        private void ManageCourses()
        {
            while (true)
            {
                Console.WriteLine("\nУПРАВЛЕНИЕ КУРСАМИ");
                Console.WriteLine("1. Добавить курс");
                Console.WriteLine("2. Показать все курсы");
                Console.WriteLine("3. Показать детальную информацию о курсе");
                Console.WriteLine("4. Назад");
                Console.Write("Выберите опцию: ");

                var choice = GetUserChoice();
                switch (choice)
                {
                    case ConsoleKey.D1:
                        AddCourse();
                        break;
                    case ConsoleKey.D2:
                        universityManager.DisplayAllCourses();
                        break;
                    case ConsoleKey.D3:
                        ShowCourseDetails();
                        break;
                    case ConsoleKey.D4:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }

        private void AddCourse()
        {
            Console.Write("Введите название курса: ");
            var name = Console.ReadLine();

            Console.Write("Введите описание курса: ");
            var description = Console.ReadLine();

            universityManager.AddCourse(name, description);
        }

        private void ShowCourseDetails()
        {
            Console.Write("Введите ID курса: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                universityManager.DisplayCourseDetails(id);
            }
            else
            {
                Console.WriteLine("Некорректный ID");
            }
        }

        private void EnrollStudentInCourse()
        {
            Console.Write("Введите ID студента: ");
            if (!int.TryParse(Console.ReadLine(), out int studentId))
            {
                Console.WriteLine("Некорректный ID студента");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Некорректный ID курса");
                return;
            }

            universityManager.EnrollStudentInCourse(studentId, courseId);
        }

        private void AssignTeacherToCourse()
        {
            Console.Write("Введите ID преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherId))
            {
                Console.WriteLine("Некорректный ID преподавателя");
                return;
            }

            Console.Write("Введите ID курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseId))
            {
                Console.WriteLine("Некорректный ID курса");
                return;
            }

            universityManager.AssignTeacherToCourse(teacherId, courseId);
        }

        private void InitializeData()
        {
            universityManager.AddStudent("Попов Даниил", 17);
            universityManager.AddStudent("Смолин Александр", 17);
            universityManager.AddStudent("Трудова Анастасия", 18);
            universityManager.AddStudent("Резанцев Артемий", 18);
            universityManager.AddStudent("Новиков Дмитрий", 18);

            universityManager.AddTeacher("Гордов Максим", 54, "Великий C#");
            universityManager.AddTeacher("Горланов Владимир", 52, "Py++");
            universityManager.AddTeacher("Сафонова Наталья", 40, "Математика");

            universityManager.AddCourse("C#", "Проггинг");
            universityManager.AddCourse("Арифметика", "Сломайте мозг пересчетами");

            universityManager.AssignTeacherToCourse(1, 1);
            universityManager.AssignTeacherToCourse(2, 1);
            universityManager.AssignTeacherToCourse(3, 2);

            universityManager.EnrollStudentInCourse(1, 1);
            universityManager.EnrollStudentInCourse(2, 1);
            universityManager.EnrollStudentInCourse(3, 2);
            universityManager.EnrollStudentInCourse(4, 2);
            universityManager.EnrollStudentInCourse(5, 2);

            Console.Clear();
        }
    }

    // Главный класс программы
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var menu = new ConsoleMenu();
                menu.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.ReadLine();
            }
        }
    }
}