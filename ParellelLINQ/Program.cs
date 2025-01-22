using System.Diagnostics; // To use stopwatch

Write("Press ENTER to start.");
ReadLine();
Stopwatch sw = Stopwatch.StartNew();

sw.Start();

int max = 45;
IEnumerable<int> numbers = Enumerable.Range(1, max);

WriteLine($"Calculating the Fibonacci sequence up to {max}. Please wait...");
int[] fibonacciNumbers = numbers.AsParallel()
    .Select(number => Fibonacci(number))
    .ToArray();
sw.Stop();
WriteLine("{0:#,##0.00} elaspsed milliseconds", sw.ElapsedMilliseconds);
Write("Results:");
foreach(int x in fibonacciNumbers)
{
    Write($"    {x:N0}");
}

static int Fibonacci(int term) =>
    term switch
    {
        1 => 0,
        2 => 1,
        _ => Fibonacci(term - 1) + Fibonacci(term - 2)
    };
    // See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
