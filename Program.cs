namespace Multi_ThreadedPrimeFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            int endNumber = 12500000;
            var primeNumberFinder = new PrimeNumberFinder(endNumber);
            primeNumberFinder.FindAndReportPrimes();
            primeNumberFinder.ExportPrimesToFile("Primes.txt");
        }
    }
}