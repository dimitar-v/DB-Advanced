namespace BookShop
{
    using Models.Enums;
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
    //using Z.EntityFramework.Plus;
    using Models;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                // 00.	Book Shop Database
                //DbInitializer.ResetDatabase(db);

                // 01.	Age Restriction
                //Write(GetBooksByAgeRestriction(db, "miNor"));

                // 02.	Golden Books
                //Write(GetGoldenBooks(db));

                // 03.	Books by Price
                //Write(GetBooksByPrice(db));

                // 04.	Not Released In
                //Write(GetBooksNotReleasedIn(db, 2000));

                // 05.	Book Titles by Category
                //Write(GetBooksByCategory(db, "horror mystery drama"));

                // 06.	Released Before Date
                //Write(GetBooksReleasedBefore(db, "12-04-1992"));

                // 07.	Author Search
                //Write(GetAuthorNamesEndingIn(db, "dy"));

                // 08.Book Search
                //Write(GetBookTitlesContaining(db, "sK"));

                // 09.	Book Search by Author
                //Write(GetBooksByAuthor(db, "R"));

                // 10.	Count Books
                //Write(CountBooks(db, 12).ToString());

                // 11.	Total Book Copies
                //Write(CountCopiesByAuthor(db));

                // 12.	Profit by Category
                //Write(GetTotalProfitByCategory(db));

                // 13.	Most Recent Books
                //Write(GetMostRecentBooks(db));

                // 14.	Increase Prices
                //IncreasePrices(db);
                //IncreasePrices2(db);

                // 15.	Remove Books
                //Write(RemoveBooks(db).ToString());
                //Write(RemoveBooks2(db).ToString());
            }
        }

        //15.	Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context
                .RemoveRange(booksToRemove);

            context.SaveChanges();

            return booksToRemove.Count;
        }

        // 15. with Z.EntityFramework.Pluse.EFCore
        //public static int RemoveBooks2(BookShopContext context)
        //    => context.Books
        //        .Where(b => b.Copies < 4200)
        //        .Delete();

        // 14.	Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        // 14. with Z.EntityFramework.Pluse.EFCore
        //public static void IncreasePrices2(BookShopContext context)
        //{
        //    context.Books
        //        .Where(b => b.ReleaseDate.Value.Year < 2010)
        //        .Update(b => new Book() {Price = b.Price + 5});

        //    context.SaveChanges();
        //}

        //13.	Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoryBooks = context.Categories
                .Select(c => new
                {
                    Name = c.Name,
                    Books = c.CategoryBooks
                        .Select(cb => new
                        {
                            cb.Book.Title,
                            cb.Book.ReleaseDate
                        })
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .ToList()
                })
                .OrderBy(c => c.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var category in categoryBooks)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.Books)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }
            
            return sb.ToString().TrimEnd();
        }

        //12.	Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfit = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalProfit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(cp => cp.TotalProfit)
                .ThenBy(cp => cp.Name);

            return ListToString(categoryProfit.Select(cp => $"{cp.Name} ${cp.TotalProfit:F2}"));
        }

        //11.	Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var auhors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    TotalCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalCopies);

            return ListToString(auhors.Select(a => $"{a.FirstName} {a.LastName} - {a.TotalCopies}"));
        }
                
        // 10.	Count Books 
        public static int CountBooks(BookShopContext context, int lengthCheck)
            => context.Books
                .Count(b => b.Title.Length > lengthCheck);

        // 09.	Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
            => ListToString(context.Books
                .Where(b => EF.Functions.Like(b.Author.LastName, input + "%"))
                .OrderBy(b => b.BookId)
                .Select(b => $"{b.Title} ({b.Author.FirstName} {b.Author.LastName})"));

        // 08.	Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
            => ListToString(context.Books
                  .Where(b => EF.Functions.Like(b.Title, "%" + input + "%"))
                  .OrderBy(b => b.Title)
                  .Select(b => b.Title));

        // 07.	Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
            => ListToString(context.Authors
                .Where(a => EF.Functions.Like(a.FirstName, "%" + input))
                .Select(a => a.FirstName + " " + a.LastName)
                .OrderBy(a => a));
        
        // 06.	Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
            => ListToString(context.Books
                .Where(b => b.ReleaseDate.Value < DateTime.ParseExact(date, "dd-MM-yyyy", null))
                .OrderByDescending(b => b.ReleaseDate.Value)
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}"));
        
        // 05.	Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title);

            return ListToString(books);
        }

        // 04.	Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
            => ListToString(context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => b.Title));
        
        // 03.	Books by Price
        public static string GetBooksByPrice(BookShopContext context)
            => ListToString(context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:F2}"));

        // 02.	Golden Books
        public static string GetGoldenBooks(BookShopContext context)
            => ListToString(context.Books
            .Where(b => b.Copies < 5000 & b.EditionType == EditionType.Gold)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title));

        // 01.	Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var books = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, books);
        }
        
        public static string ListToString(IEnumerable<string> list)
            => string.Join(Environment.NewLine, list);

        public static void Write(string text)
            => Console.WriteLine(text);
    }
}
