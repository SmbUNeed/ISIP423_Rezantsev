using System;
using System.Collections.Generic;
using System.Linq;

public enum Genre
{
    Fantasy,
    Romance,
    Mystery,
    Thriller,
    ScienceFiction
}

public class Book
{
    private static int _nextId = 1;

    public int Id { get; private set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public Genre Genre { get; set; }
    public int Year { get; set; }
    public decimal Price { get; set; }

    public Book(string title, string author, Genre genre, int year, decimal price)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
        Genre = genre;
        Year = year;
        Price = price;
    }

    public override string ToString()
    {
        return $"ID: {Id}, Название: \"{Title}\", Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
    }
}

public class Library //FFff
{
    private List<Book> _books = new List<Book>();

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public bool RemoveBook(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book != null)
        {
            _books.Remove(book);
            return true;
        }
        return false;
    }

    public List<Book> GetAllBooks()
    {
        return new List<Book>(_books);
    }

    public List<Book> FindBooksByTitle(string title)
    {
        return _books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Book> FindBooksByAuthor(string author)
    {
        return _books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Book> FindBooksByGenre(Genre genre)
    {
        return _books.Where(b => b.Genre == genre).ToList();
    }

    public List<Book> SortByTitle()
    {
        return _books.OrderBy(b => b.Title).ToList();
    }

    public List<Book> SortByYear()
    {
        return _books.OrderBy(b => b.Year).ToList();
    }

    public (Book? mostExpensive, Book? cheapest) GetPriceExtremes()
    {
        if (_books.Count == 0)
            return (null, null);

        var mostExpensive = _books.OrderByDescending(b => b.Price).First();
        var cheapest = _books.OrderBy(b => b.Price).First();

        return (mostExpensive, cheapest);
    }

    public Dictionary<string, int> GetBooksCountByAuthor()
    {
        return _books.GroupBy(b => b.Author)
                    .ToDictionary(g => g.Key, g => g.Count());
    }

    public void InitializeTestData() // Начальные данные
    {
        AddBook(new Book("Убийство в «Восточном экспрессе»", "Агата Кристи", Genre.Thriller, 1934, 320));
        AddBook(new Book("Мастер и Маргарита", "Михаил Булгаков", Genre.Romance, 1967, 480));
        AddBook(new Book("Игра Эндера", "Орсон Скотт Кард", Genre.ScienceFiction, 1985, 380));
        AddBook(new Book("Властелин Колец: Братство Кольца", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 480));
        AddBook(new Book("Оно", "Стивен Кинг", Genre.Mystery, 1986, 1138));
    }
}

class Program
{
    private static Library _library = new Library();

    static void Main(string[] args)
    {
        _library.InitializeTestData();

        while (true)
        {
            Console.Clear();
            Menu();
            var choice = GetUserChoice();
            ProcessChoice(choice);
        }
    }

    static void Menu()
    {
        Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ БИБЛИОТЕКОЙ");
        Console.WriteLine("1. Добавить книгу");
        Console.WriteLine("2. Удалить книгу по ID");
        Console.WriteLine("3. Найти книги");
        Console.WriteLine("4. Сортировать книги");
        Console.WriteLine("5. Самая дорогая и дешёвая книга");
        Console.WriteLine("6. Группировка книг по авторам");
        Console.WriteLine("7. Показать все книги");
        Console.WriteLine("0. Выход");
        Console.Write("Выберите действие: ");
    }

    static int GetUserChoice()
    {
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    static void ProcessChoice(int choice)
    {
        switch (choice)
        {
            case 1:
                AddBook();
                break;
            case 2:
                RemoveBook();
                break;
            case 3:
                SearchBooks();
                break;
            case 4:
                SortBooks();
                break;
            case 5:
                ShowPriceExtremes();
                break;
            case 6:
                GroupBooksByAuthor();
                break;
            case 7:
                ShowAllBooks();
                break;
            case 0:
                Console.WriteLine("До свидания!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Неверный выбор! Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                break;
        }
    }

    static void AddBook()
    {
        Console.Clear();
        Console.WriteLine("ДОБАВЛЕНИЕ НОВОЙ КНИГИ");
        try
        {
            string title = GetValidatedInput("Введите название книги: ",
                input => !string.IsNullOrWhiteSpace(input), "Название не может быть пустым!");

            string author = GetValidatedInput("Введите автора книги: ",
                input => !string.IsNullOrWhiteSpace(input), "Автор не может быть пустым!");

            Console.WriteLine("\nДоступные жанры:");
            foreach (var genre in Enum.GetValues(typeof(Genre)))
            {
                Console.WriteLine($"{(int)genre}. {genre}");
            }

            Genre bookGenre = GetValidatedGenre("Выберите жанр (введите номер): ");

            int year = GetValidatedYear("Введите год издания: ");

            decimal price = GetValidatedPrice("Введите цену книги: ");

            var newBook = new Book(title, author, bookGenre, year, price);
            _library.AddBook(newBook);

            Console.WriteLine($"\nКнига успешно добавлена! ID: {newBook.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при добавлении книги: {ex.Message}");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void RemoveBook()
    {
        Console.Clear();
        Console.WriteLine("УДАЛЕНИЕ КНИГИ");

        ShowAllBooksBrief();

        if (_library.GetAllBooks().Count == 0)
        {
            Console.WriteLine("Нет книг для удаления.");
            Console.ReadKey();
            return;
        }

        Console.Write("Введите ID книги для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (_library.RemoveBook(id))
            {
                Console.WriteLine("Книга успешно удалена!");
            }
            else
            {
                Console.WriteLine("Книга с указанным ID не найдена.");
            }
        }
        else
        {
            Console.WriteLine("Неверный формат ID!");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void SearchBooks()
    {
        Console.Clear();
        Console.WriteLine("ПОИСК КНИГ");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По автору");
        Console.WriteLine("3. По жанру");
        Console.Write("Выберите тип поиска: ");

        if (int.TryParse(Console.ReadLine(), out int searchType))
        {
            List<Book> results = new List<Book>();

            switch (searchType)
            {
                case 1:
                    Console.Write("Введите название для поиска: ");
                    string title = Console.ReadLine() ?? "";
                    results = _library.FindBooksByTitle(title);
                    break;
                case 2:
                    Console.Write("Введите автора для поиска: ");
                    string author = Console.ReadLine() ?? "";
                    results = _library.FindBooksByAuthor(author);
                    break;
                case 3:
                    Console.WriteLine("\nДоступные жанры:");
                    foreach (var genre in Enum.GetValues(typeof(Genre)))
                    {
                        Console.WriteLine($"{(int)genre}. {genre}");
                    }
                    Genre genreSearch = GetValidatedGenre("Выберите жанр для поиска: ");
                    results = _library.FindBooksByGenre(genreSearch);
                    break;
                default:
                    Console.WriteLine("Неверный тип поиска!");
                    break;
            }

            DisplaySearchResults(results);
        }
        else
        {
            Console.WriteLine("Неверный формат ввода!");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void SortBooks()
    {
        Console.Clear();
        Console.WriteLine("СОРТИРОВКА КНИГ");
        Console.WriteLine("1. По названию");
        Console.WriteLine("2. По году издания");
        Console.Write("Выберите тип сортировки: ");

        if (int.TryParse(Console.ReadLine(), out int sortType))
        {
            List<Book> sortedBooks = new List<Book>();

            switch (sortType)
            {
                case 1:
                    sortedBooks = _library.SortByTitle();
                    Console.WriteLine("\nКниги отсортированы по названию:");
                    break;
                case 2:
                    sortedBooks = _library.SortByYear();
                    Console.WriteLine("\nКниги отсортированы по году издания:");
                    break;
                default:
                    Console.WriteLine("Неверный тип сортировки!");
                    return;
            }

            DisplaySearchResults(sortedBooks);
        }
        else
        {
            Console.WriteLine("Неверный формат ввода!");
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowPriceExtremes()
    {
        Console.Clear();
        Console.WriteLine("САМАЯ ДОРОГАЯ И ДЕШЁВАЯ КНИГА");

        var (mostExpensive, cheapest) = _library.GetPriceExtremes();

        if (mostExpensive == null || cheapest == null)
        {
            Console.WriteLine("В библиотеке нет книг.");
        }
        else
        {
            Console.WriteLine("Самая дорогая книга:");
            Console.WriteLine(mostExpensive);
            Console.WriteLine("\nСамая дешёвая книга:");
            Console.WriteLine(cheapest);
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void GroupBooksByAuthor()
    {
        Console.Clear();
        Console.WriteLine("ГРУППИРОВКА КНИГ ПО АВТОРАМ");

        var authorGroups = _library.GetBooksCountByAuthor();

        if (authorGroups.Count == 0)
        {
            Console.WriteLine("В библиотеке нет книг.");
        }
        else
        {
            Console.WriteLine("Количество книг по авторам:");
            foreach (var group in authorGroups.OrderByDescending(g => g.Value))
            {
                Console.WriteLine($"{group.Key}: {group.Value} книг(и)");
            }
        }

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowAllBooks()
    {
        Console.Clear();
        Console.WriteLine("ВСЕ КНИГИ В БИБЛИОТЕКЕ");

        var books = _library.GetAllBooks();
        DisplaySearchResults(books);

        Console.WriteLine("\nНажмите любую клавишу для продолжения...");
        Console.ReadKey();
    }

    static void ShowAllBooksBrief()
    {
        var books = _library.GetAllBooks();
        if (books.Count > 0)
        {
            Console.WriteLine("\nТекущие книги в библиотеке:");
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, \"{book.Title}\" - {book.Author}");
            }
        }
    }

    static void DisplaySearchResults(List<Book> books)
    {
        if (books.Count == 0)
        {
            Console.WriteLine("Книги не найдены.");
        }
        else
        {
            Console.WriteLine($"\nНайдено книг: {books.Count}");
            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }
    }

    static string GetValidatedInput(string prompt, Func<string, bool> validator, string errorMessage)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine() ?? "";

            if (validator(input))
            {
                return input.Trim();
            }

            Console.WriteLine(errorMessage);
        }
    }

    static Genre GetValidatedGenre(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int genreNumber) &&
                Enum.IsDefined(typeof(Genre), genreNumber))
            {
                return (Genre)genreNumber;
            }
            Console.WriteLine("Неверный номер жанра! Попробуйте снова.");
        }
    }

    static int GetValidatedYear(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out int year) && year > 0 && year <= DateTime.Now.Year)
            {
                return year;
            }
            Console.WriteLine($"Год должен быть положительным числом не больше {DateTime.Now.Year}!");
        }
    }

    static decimal GetValidatedPrice(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (decimal.TryParse(Console.ReadLine(), out decimal price) && price >= 0)
            {
                return price;
            }
            Console.WriteLine("Цена должна быть неотрицательным числом!");
        }
    }
}