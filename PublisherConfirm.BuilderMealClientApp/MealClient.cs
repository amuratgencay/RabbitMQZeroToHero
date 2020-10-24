using PublisherConfirm.BuilderService;
using System;

namespace PublisherConfirm.BuilderMealClientApp
{
    class MealClient
    {
        static void Main(string[] args)
        {
            using (var mealBuilder = new MealBuilder())
            {

                while (true)
                {
                    var request = ShowMenu();
                    if (string.IsNullOrEmpty(request))
                        break;
                    HandleRequest(mealBuilder, request);
                }
            }

        }

        private static void HandleRequest(MealBuilder mealBuilder, string request)
        {
            if (request == "veg" || request == "nonveg")
            {
                mealBuilder.Add(request);
                mealBuilder.Show();

            }
            else if (request == "show")
            {
                mealBuilder.Show();
            }
            else if (request == "pay")
            {
                mealBuilder.CompleteOrder();

            }
            Console.WriteLine();
        }

        private static string GetRequestType(ConsoleKeyInfo answer)
        {
            switch (answer.Key)
            {
                case ConsoleKey.D1: return "veg"; 
                case ConsoleKey.D2: return  "nonveg";;
                case ConsoleKey.D3: return "show"; 
                case ConsoleKey.D4: return  "pay";
                default: return "show";
            }
        }

        private static string ShowMenu()
        {
            Console.WriteLine("Chosee [1:VegMeal, 2:NonVegMeal, 3: Show Items, 4:Pay, Press enter for exit...]: ");
            var ans = Console.ReadKey(true);
            if (ans.Key == ConsoleKey.Enter) return "";

            return GetRequestType(ans); 

        }
    }
}
