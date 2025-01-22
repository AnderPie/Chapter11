using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.EntityModels; // To use Northwind db
partial class Program
{
    public static void WriteAllCities()
    {
        SectionTitle("Cities in which our customers reside:");
        using NorthwindDb db = new();
        IQueryable<string> cities = db.Customers
            .Select(c => c.City)
            .Distinct();
        foreach (var c in cities.OrderBy(c => c))
        {
            if (cities.OrderBy(c => c).Last() == c)
            {
                Write($"{c}.");
                WriteLine();
            }
            else
            {
                Write($"{c}, ");
            }
        }       
    }
    public static void GetCityFromUser()
    {
        SectionTitle("Please enter a city");
        
        string? input = ReadLine();
        if (input is not null)
        {
            QueryCustomerByCityAndWriteLine(input);
        }
    }
    public static void QueryCustomerByCityAndWriteLine(string input)
    {
        SectionTitle($"Customer(s) in {input}:");
        using NorthwindDb db = new();
        IQueryable<string> customers = db.Customers
            .Where(c => c.City == input)
            .Select(c => c.CompanyName);

        if (customers.Count() == 0)
        {
            Console.WriteLine($"No customers in {input}");
        }
        foreach (var c in customers.OrderBy(c => c))
        {

            if(customers.OrderBy(c => c).Last() == c)
            {
                WriteLine($"{c}. ");
            }
            else
            {
                WriteLine($"{c}, ");
            }
            
        }
    }
    private static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine($"**** {title} ****");
        ForegroundColor = previousColor;
    }
}
