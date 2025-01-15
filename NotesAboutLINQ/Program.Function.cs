using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class Program
{
    private static void DeferredExecution(string[] names)
    {
        SectionTitle("Deferred Execution");

        // Question: Which names end with an M?
        // (Using a LINQ extensions method)
        var query1 = names.Where(name => name.EndsWith("m"));

        // Question: Which names end with an M?
        // (Using LINQ query comprehension syntax)

       
        var query2 = from name in names where name.EndsWith("m") select name;

        // To get the answer we need to materialize the query
        string[] result1 = query1.ToArray();

        List<string> result2 = query2.ToList();

        // Answer returned as we enumerate over the results
        foreach (string name in query1)
        {
            WriteLine(name);

            names[3] = "Bammy"; // Change Bam to Bammy
            // On the second iteration Bammy does not end with m
            // so does not get output.
        }
    }

    // Func<string, bool> delegate returns true if string should be
    // included in results

    static bool NameLongerThanFour(string name)
    {
        // Returns true for a name longer than four characters
        return name.Length > 4;
    }

    static void FilterUsingWhere(string[] names)
    {
        SectionTitle("Filter using where");

        // Where() is an extension method from the System.Linq namespace, not a
        // default array functionality.

        //var query = names.Where(new Func<string, bool>( NameLongerThanFour ));

        // More concise version of above syntax:
        //var query = names.Where(NameLongerThanFour);

        // Similar to above, but this time with lambda
        var query = names
            .Where(name => name.Length > 4)
            .OrderBy(name => name.Length)
            .ThenBy(name => name);


        foreach (string name in query)
        {
            WriteLine(name);
        }
    }

    static void FilteringByType()
    {
        SectionTitle("Filtering by type");

        List<Exception> exceptions = new()
        {
            new ArgumentException(), new SystemException(),
            new IndexOutOfRangeException(), new InvalidOperationException(),
            new NullReferenceException(), new InvalidOperationException(),
            new OverflowException(), new DivideByZeroException(),
            new ArgumentException()
        };

        IEnumerable<ArithmeticException> arithmeticExceptionsQuery =
            exceptions.OfType<ArithmeticException>();

        foreach (ArithmeticException exception in arithmeticExceptionsQuery)
        {
            WriteLine($"{exception}");
        }
    }

    static void Output(IEnumerable<string> cohort, string description = "")
    {
        if (!string.IsNullOrEmpty(description))
        {
            WriteLine(description);
        }
        Write(" ");
        WriteLine(string.Join(", ", cohort.ToArray()));
        WriteLine();
    }

    static void WorkingWithSets()
    {
        string[] cohort1 = { "Rachel", "Gareth", "Jonathan", "George" };
        string[] cohort2 = { "Jack", "Stephen", "Daniel", "Jack", "Jared" };
        string[] cohort3 = { "Declan", "Jack", "Jack", "Jasmine", "Conor" };

        SectionTitle("The cohorts");

        Output(cohort1, "Cohort 1");
        Output(cohort2, "Cohort 2");
        Output(cohort3, "Cohort 3");

        SectionTitle("Set operations");

        Output(cohort2.Distinct(), "cohort2.Distinct()");
        Output(cohort2.DistinctBy(name=>name.Substring(0,2)), 
            "cohort2.DistinctBy(name=>name.Substring(0,2))");
        Output(cohort2.Union(cohort3), "cohort2.Union(cohort3)");
        Output(cohort2.Concat(cohort3), "cohort2.Concat(cohort3)");
        Output(cohort2.Intersect(cohort3), "cohort2.Intersect(cohort3)");
        Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");
        Output(cohort1.Zip(cohort2,(c1,c2) => $"{c1} matched with {c2}"),
            "cohort1.Zip(cohort2)");
    }
}