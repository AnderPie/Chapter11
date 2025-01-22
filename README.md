The following is work undertaken to learn C# through use of Mark Price's C#12 and .NET 8 Modern Cross-Platform Development Fundamentals.  
# Exercise 11
11.1 - Test your knowledge

## What are the two required parts of LINQ?
A data source and a query. 
The query contains LINQ extension methods (or their query comprehension syntax equivalent). 
## Which LINQ extensions method would you use to return a subset of properties from a type?
You define which properties you want to select using the Select() method. 
## Which LINQ extension method would you use to filter a sequence?
.Where(something => something.SomeProperty == someCondition)

## List five LINQ extension methods that peform aggregation.
someIQueryable.Aggregate()
someIQueryable.Average
someIQueryable.Max
someIQueryable.Min
someIQueryable.Sum
## What is the difference between the Select and SelectMany extension methods?
Select produces one value for each result, whereas SelectMany produces several 
concatenated values for each result. You might use SelectMany if attempting to flatten data,
say enumerating through a list of departments, and saving each employee within each department to 
a list, like:

IQueryable<Employee> employees = departments.SelectMany(d => d.Employees);
## What is the difference between IEnumerable<T> and IQueryable<T>? How do you switch between them?
IQueryable is derived from IEnumerable. An enumerable gives you the ability to iterate over its
contents, whereas an IQueryable gives you methods for interacting with queried data. 

You can extract an IEnumerable from an IQueryable with the following syntax:
IEnumerable<Product> productsIEnumerable = productsIQueryable.AsEnumerable();
and do the reverse like so:
IQueryable<Product> productsIQueryableAgain = productsIEnumerable.AsQueryable();

## What does the last type parameter T in generic Func delegates like Func<T1, T2, T> represent?
The last type parameter represents the type that the Func delegate would return.
Func<int, string, bool> myFunc = (x,y) => x.ToString() == y is a function that 
checks if an int and a string representation of an int are equal, and return a boolean
based on the results of the check. 
## What is the benefit of a LINQ extension method that ends with OrDefault?
Returns a default if a sequence contains no elements, making it useful for handling potentially
empty sequences (if a default is desired).
## Why is query comprehension syntax optional?
Query comprehension syntax was added in C#3 to allow programmers familiar with SQL to use syntax
that might be more familiar. 

You could represent a query with LINQ extension methods and lambda expressions, or use 
query comprehension syntax to do the same thing. The query comprehension syntax is optional because
you can simply use the extension methods and lambda expression syntax if you prefer (the compiler
doesn't mind either way).
## How can you create your own LINQ extension methods? 
The same way you'd add extension methods to any other class?
Define a static class, define static method with a first parameter prefeixed by this. to specify
the type being extended.