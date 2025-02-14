namespace Linq2
{
    internal class Program
    {
        static void Main(string[] args)
        {
        #region Aggregate Operators
        // Get the total units in stock for each product category
        var totalUnitsInStock = ListGenerator.ProductsList
                        .GroupBy(p => p.Category)
                        .Select(g => new { Category = g.Key, TotalStock = g.Sum(p => p.UnitsInStock) });
        Console.WriteLine("Total Units in Stock per Category:");
        foreach (var item in totalUnitsInStock) Console.WriteLine($"{item.Category}: {item.TotalStock}");

        // Get the cheapest price among each category's products
        var cheapestPrices = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .Select(g => new { Category = g.Key, CheapestPrice = g.Min(p => p.UnitPrice) });
        Console.WriteLine("\nCheapest Price per Category:");
        foreach (var item in cheapestPrices) Console.WriteLine($"{item.Category}: {item.CheapestPrice}");

        // Get the products with the cheapest price in each category (Use Let)
        var cheapestProducts = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .SelectMany(g =>
            {
                var minPrice = g.Min(p => p.UnitPrice);
                return g.Where(p => p.UnitPrice == minPrice).Select(p => new { p.Category, p.ProductName, p.UnitPrice });
            });
        Console.WriteLine("\nProducts with Cheapest Price per Category:");
        foreach (var item in cheapestProducts) Console.WriteLine($"{item.Category}: {item.ProductName} - {item.UnitPrice}");

        // Get the most expensive price among each category's products
        var expensivePrices = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .Select(g => new { Category = g.Key, ExpensivePrice = g.Max(p => p.UnitPrice) });
        Console.WriteLine("\nMost Expensive Price per Category:");
        foreach (var item in expensivePrices) Console.WriteLine($"{item.Category}: {item.ExpensivePrice}");

        // Get the products with the most expensive price in each category
        var expensiveProducts = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .SelectMany(g =>
            {
                var maxPrice = g.Max(p => p.UnitPrice);
                return g.Where(p => p.UnitPrice == maxPrice).Select(p => new { p.Category, p.ProductName, p.UnitPrice });
            });
        Console.WriteLine("\nProducts with Most Expensive Price per Category:");
        foreach (var item in expensiveProducts) Console.WriteLine($"{item.Category}: {item.ProductName} - {item.UnitPrice}");

        // Get the average price of each category's products
        var avgPrices = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .Select(g => new { Category = g.Key, AvgPrice = g.Average(p => p.UnitPrice) });
        Console.WriteLine("\nAverage Price per Category:");
        foreach (var item in avgPrices) Console.WriteLine($"{item.Category}: {item.AvgPrice:F2}");
#endregion
// -----------------------------------------------------------------------------------------------
        #region Set Operators
        // Find unique Category names from Product List
        var uniqueCategories = ListGenerator.ProductsList.Select(p => p.Category).Distinct();
        Console.WriteLine("\nUnique Categories:");
        foreach (var category in uniqueCategories) Console.WriteLine(category);

        // Sequence of unique first letters from both product and customer names
        var uniqueFirstLetters = ListGenerator.ProductsList.Select(p => p.ProductName[0])
            .Union(ListGenerator.CustomersList.Select(c => c.CustomerName[0]))
            .Distinct();
        Console.WriteLine("\nUnique First Letters from Products and Customers:");
        foreach (var letter in uniqueFirstLetters) Console.WriteLine(letter);

        // Common first letters between products and customers
        var commonFirstLetters = ListGenerator.ProductsList.Select(p => p.ProductName[0])
            .Intersect(ListGenerator.CustomersList.Select(c => c.CustomerName[0]));
        Console.WriteLine("\nCommon First Letters:");
        foreach (var letter in commonFirstLetters) Console.WriteLine(letter);

        // First letters of product names not in customer names
        var productExclusiveFirstLetters = ListGenerator.ProductsList.Select(p => p.ProductName[0])
            .Except(ListGenerator.CustomersList.Select(c => c.CustomerName[0]));
        Console.WriteLine("\nFirst Letters of Products Not in Customer Names:");
        foreach (var letter in productExclusiveFirstLetters) Console.WriteLine(letter);

        // Last three characters of all names in products and customers
        var lastThreeChars = ListGenerator.ProductsList.Select(p => p.ProductName.Length >= 3 ? p.ProductName[^3..] : p.ProductName)
            .Concat(ListGenerator.CustomersList.Select(c => c.CustomerName.Length >= 3 ? c.CustomerName[^3..] : c.CustomerName));
        Console.WriteLine("\nLast Three Characters of Product and Customer Names:");
        foreach (var chars in lastThreeChars) Console.WriteLine(chars);

        #endregion
// -----------------------------------------------------------------------------------------------
        #region Partitioning Operators
        //Get the first 3 orders from customers in Washington
        //var firstThreeOrders = ListGenerator.OrdersList
        //    .Where(o => o.Customer.Region == "Washington")
        //    .Take(3);
        //Console.WriteLine("\nFirst 3 Orders from Washington Customers:");
        //foreach (var order in firstThreeOrders) Console.WriteLine(order.OrderID);

        //// Get all but the first 2 orders from customers in Washington
        //var ordersSkippingTwo = ListGenerator.OrdersList
        //    .Where(o => o.Customer.Region == "Washington")
        //    .Skip(2);
        //Console.WriteLine("\nOrders from Washington Customers (Skipping First 2):");
        //foreach (var order in ordersSkippingTwo) Console.WriteLine(order.OrderID);

        // Return elements starting from the beginning of the array until a number is hit that is less than its position in the array
        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
        var takeWhileCondition = numbers.TakeWhile((num, index) => num >= index);
        Console.WriteLine("\nNumbers until one is less than its position:");
        foreach (var num in takeWhileCondition) Console.WriteLine(num);

        // Get the elements of the array starting from the first element divisible by 3
        var skipUntilDivisibleBy3 = numbers.SkipWhile(n => n % 3 != 0);
        Console.WriteLine("\nElements from first divisible by 3:");
        foreach (var num in skipUntilDivisibleBy3) Console.WriteLine(num);

        // Get the elements of the array starting from the first element less than its position
        var skipUntilLessThanPosition = numbers.SkipWhile((num, index) => num >= index);
        Console.WriteLine("\nElements from first number less than its position:");
        foreach (var num in skipUntilLessThanPosition) Console.WriteLine(num);

        #endregion
// -----------------------------------------------------------------------------------------------
        #region Quantifiers
        // Determine if any of the words in dictionary_english.txt contain the substring 'ei'
        string[] words = File.ReadAllLines("dictionary_english.txt");
        bool containsEi = words.Any(word => word.Contains("ei"));
        Console.WriteLine("\nAny word contains 'ei': " + containsEi);

        // Return a grouped list of products only for categories that have at least one product that is out of stock
        var outOfStockCategories = ListGenerator.ProductsList
            .Where(p => p.UnitsInStock == 0)
            .GroupBy(p => p.Category);
        Console.WriteLine("\nCategories with at least one out-of-stock product:");
        foreach (var group in outOfStockCategories)
        {
            Console.WriteLine(group.Key);
        }

        // Return a grouped list of products only for categories that have all of their products in stock
        var fullyStockedCategories = ListGenerator.ProductsList
            .GroupBy(p => p.Category)
            .Where(g => g.All(p => p.UnitsInStock > 0));
        Console.WriteLine("\nCategories with all products in stock:");
        foreach (var group in fullyStockedCategories)
        {
            Console.WriteLine(group.Key);
        }

        #endregion
// -----------------------------------------------------------------------------------------------
        #region Grouping Operators
        // – Grouping Operators
        // Group numbers by remainder when divided by 5
        //List<int> numbers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        //var groupedNumbers = numbers.GroupBy(n => n % 5);
        //foreach (var group in groupedNumbers)
        //{
        //    Console.WriteLine($"Numbers with a remainder of {group.Key} when divided by 5:");
        //    foreach (var num in group) Console.WriteLine(num);
        //}

        //// Group words by first letter using dictionary_english.txt


        //string[] words = System.IO.File.ReadAllLines("dictionary_english.txt");
        //var groupedWords = words.GroupBy(w => w[0]);
        //foreach (var group in groupedWords)
        //{
        //    Console.WriteLine($"Words starting with '{group.Key}':");
        //    foreach (var word in group.Take(5)) Console.WriteLine(word); // Displaying only first 5 words for brevity
        //}

        // Group words with the same characters together
        string[] Arr = { "from", "salt", "earn", "last", "near", "form" };
        var anagramGroups = Arr.GroupBy(w => string.Concat(w.OrderBy(c => c)));
        Console.WriteLine("\nAnagram Groups:");
        foreach (var group in anagramGroups)
        {
            foreach (var word in group) Console.WriteLine(word);
            Console.WriteLine("-----");
        }
        #endregion


        }
    }
}
