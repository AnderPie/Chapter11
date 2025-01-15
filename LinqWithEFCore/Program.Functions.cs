using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.EntityModels;
using Microsoft.EntityFrameworkCore; // To use DbSet<T>

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
            );
        foreach( var p in queryJoin)
        {
            WriteLine($"{p.ProductID}: {p.ProductName} in {p.CategoryName}");
        }
    }
}
