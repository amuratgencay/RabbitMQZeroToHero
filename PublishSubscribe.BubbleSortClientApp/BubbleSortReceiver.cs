using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;
using Util.StringUtils;

namespace PublishSubscribe.BubbleSortClientApp
{
    class BubbleSortReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQueueNameTypes.Fanout, 
                "sort"))
            {
                queue.BasicConsume(
                    (sender, message) => BubbleSort(message));
                ConsoleExtensions.WriteLineWait("Bubble Sort\nPress [enter] to exit.");
            }
        }

        private static void BubbleSort(string message)
        {
            Stopwatch stopwatch = new Stopwatch();
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            stopwatch.Start();
            BubbleSort(numbers);
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine($"{StringExtentions.FromIntArray(numbers)} - elapsed time: {stopwatch.Elapsed}");
        }

        private static void BubbleSort(int[] numbers)
        {

            int tmp;
            for (int j = 0; j <= numbers.Length - 2; j++)
            {
                for (int i = 0; i <= numbers.Length - 2; i++)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        tmp = numbers[i + 1];
                        numbers[i + 1] = numbers[i];
                        numbers[i] = tmp;
                    }
                }
            }
        }


    }
}
