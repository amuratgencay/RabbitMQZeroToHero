using System;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;

namespace RPC.SingletonClientApp
{
    class SingletonClient
    {
        static void Main(string[] args)
        {
            using (var queue = PublisherFactory.Create(PublisherWithoutQueueNameTypes.RPC))
            {
                while (true)
                {
                    var request = ShowMenu();
                    if (string.IsNullOrEmpty(request))
                        break;

                    queue.BasicPublish(
                        "singleton",
                        request,
                        (sender, response) 
                            => ShowCounter(response, request));
                }

            }
        }

        private static void ShowCounter(string response, string request)
        {
            Console.WriteLine(". {0}: counter = {1}", request, response);
        }

        private static string ShowMenu()
        {
            Console.Write("Chosee [1:GetCounter, 2:Increment, 3:Decrement, Press enter for exit...]: ");
            var ans = Console.ReadKey();
            if (ans.Key == ConsoleKey.Enter) return "";
            return GetRequestType(ans);

        }

        private static string GetRequestType(ConsoleKeyInfo answer)
        {
            switch (answer.Key)
            {
                case ConsoleKey.D1: return "get";
                case ConsoleKey.D2: return "inc";
                case ConsoleKey.D3: return "dec";
                default: return "get";
            }
        }
    }
}
