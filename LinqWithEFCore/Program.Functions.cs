using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.EntityModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // To use DbSet<T>

partial class Program
{
    private static void FilterAndSort()
    {
        SectionTitle("Filter and sort");
        using NorthwindDb db = new();
        DbSet<Product> allProducts = db.Products;

        IQueryable<Product> filteredProducts = allProducts.Where(product =>
            product.UnitPrice < 10M);

        // Learn more about projection operations at learn.microsoft.com/en-us/dotnet/
        // csharp/programming-guide/concepts/linq/projection-operations
        IOrderedQueryable<Product> sortedAndFilteredProducts = 
            filteredProducts.OrderByDescending(product => product.UnitPrice);
        var projectedProducts = sortedAndFilteredProducts.Select(product => new
        { // Anonymously typed object
            product.ProductID,
            product.ProductName,
            product.UnitPrice
        });

        WriteLine("Products that cost less than $10:");
        foreach (var p in projectedProducts)
        {
            WriteLine("{0}: {1} costs {2:$#,##0.00}", 
                p.ProductID, p.ProductName, p.UnitPrice);
        }
    }

    private static void JoinCategoriesAndProduct()
    {
        /*
         * Three extension methods for joining, grouping, and making grouped lookups:
         * 
         * Join(sequence you want to join with, property or properties on the left to use as keys
         * , property or properties on the right to use as keys, a projection)
         * 
         * GroupJoin, same parameters as join, but combines the matches into a group object
         * with a Key property for the matching value and an IEnumerable<T> for
         * the multiple matches
         * 
         * ToLookup: Creates a new data structure with the sequence grouped by a key.
         */

        SectionTitle("Join categories and products");
        using NorthwindDb db = new();
        //Join every product to its category to return 77 matches
        var queryJoin = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryID,
            innerKeySelector: product => product.CategoryID,
            resultSelector: (c, p) => new { c.CategoryName, p.ProductName, p.ProductID }
            )
            .OrderBy(cp => cp.CategoryName);
        foreach( var p in queryJoin)
        {
            WriteLine($"{p.ProductID}: {p.ProductName} in {p.CategoryName}");
        }
    }

    private static void GroupJoinCategoriesAndProducts()
    {
        SectionTitle("Group join categories and products");
        using NorthwindDb db = new();

        // Group all products by their category to return 8 matches
        var queryGroup = db.Categories.AsEnumerable().GroupJoin(
            inner: db.Products,
            outerKeySelector: category => category.CategoryID,
            innerKeySelector: product => product.CategoryID,
            resultSelector: (c, matchingProducts) => new { c.CategoryName, Products = matchingProducts.OrderBy(p => p.ProductName) 
            });
        foreach(var c in queryGroup)
        {
            WriteLine($"{c.CategoryName} has {c.Products.Count()} products.");
            foreach( var p in c.Products)
            {
                WriteLine($"    {p.ProductName}");
            }
        }
    }

    private static void ProductsLookup()
    {
        SectionTitle("Products Lookup");
        using NorthwindDb db = new();
        // Join all products to their category to return 77 matches
        var productQuery = db.Categories.Join(
            inner: db.Products,
            outerKeySelector: category => category.CategoryID,
            innerKeySelector: product => product.CategoryID,
            resultSelector: (c, p) => new { c.CategoryName, Product = p });

        ILookup<string, Product> productLookup = productQuery.ToLookup(
            keySelector: cp => cp.CategoryName,
            elementSelector: cp => cp.Product);

        foreach (IGrouping<string, Product> group in productLookup)
        {
            //Key is Beverages, Condiments, etc.
            WriteLine($"{group.Key} has {group.Count()} products");

            foreach(var product in group)
            {
                WriteLine($"    {product.ProductName}");
            }
        }

        // We can look up the products by a category name
        Write("Enter a category name: ");
        string categoryName = ReadLine()!;
        WriteLine();
        WriteLine($"Products in {categoryName}:");
        IEnumerable<Product> productsInCategory = productLookup[categoryName];
        foreach(Product product in productsInCategory)
        {
            WriteLine($"    {product.ProductName}");
        }

        /* 
         * Selector paramaeters are lambda expressions that select sub elements for 
         * different purposes. For example, ToLookup hasa  key selector to select 
         * the part for each item that will be the key and an elementSelector to 
         * select the part of each item that will be the value. You can learn more 
         * at the following link: https://learn.Microsoft.com/en-us/dotnet/api/system.linq.enumerable.tolookup
         */
    }
    private static void AggregateProducts()
    {
        SectionTitle("Aggregate products");
        using NorthwindDb db = new();

        // Try to get an efficient count from EF Core DbSet<T>
        WriteLine("Try to get an efficient count from EF Core DbSet<T>");
        if (db.Products.TryGetNonEnumeratedCount(out int countDbSet))
        {
            WriteLine($"{"Product count from DbSet: ",-25} {countDbSet,10}");
        }
        else
        {
            WriteLine($"Products DbSet does not have a Count property");
        }

        // Try to get an efficient count from a list<T>
        WriteLine("Try to get an efficient count from List<T>");

        List<Product> products = db.Products.ToList();

        if (products.TryGetNonEnumeratedCount(out int countList))
        {
            WriteLine($"{"Product count from list:",-25} {countList,11}");
        }
        else
        {
            WriteLine("Products list does not have a Count property");
        }

        WriteLine($"{"Product count from DbSet: ",-25} {db.Products.Count(),10}");

        WriteLine($"{"Discontinued product count:",-27} {db.Products.Where(p => p.Discontinued).Count(),8}");

        // Mark makes great console tables, and I gotta be real, the {someInterpolatable,xPosition} syntax looks so foreign to me
        // Just inexperience really

        // I really like the lambda functionality for IQueryable interface.

        WriteLine($"{"Highest product price:",-25} {db.Products.Max(p => p.UnitPrice),10:$#,##0.00}");

        WriteLine($"{"Average units in stock:",-25} {db.Products.Average(p => p.UnitsInStock)}");

        WriteLine($"{"Sum of units in stock:",-25} {db.Products.Sum(p=>p.UnitsInStock),10:N0}");
        WriteLine($"{"Sum of units on order:",-25} {db.Products.Sum(p=>p.UnitsOnOrder),10:N0}");
        decimal? highestUnitValue = db.Products.Max(p => p.UnitPrice * p.UnitsInStock);
        // In ties, winner is arbitrary based on how IQueryable was sorted last
        Product highestValueProduct = db.Products.Where(p => p.UnitPrice * p.UnitsInStock == highestUnitValue).First();
        WriteLine($"{"Value of units in stock: ",-25} {db.Products.Sum(p=>p.UnitPrice*p.UnitsInStock),10:$#,##0.00}");
        WriteLine($"{"Name of product with most value in stock: ",-25} {highestValueProduct.ProductName}");
        WriteLine($"{"Value of product with most value in stock: ",-25} {highestValueProduct.UnitsInStock * highestValueProduct.UnitPrice,10:$#,##0.00}");

    }
}
