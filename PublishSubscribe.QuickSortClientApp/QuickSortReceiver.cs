using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Util.ConsoleUtils;
using Util.RabbitMQUtils.Factory;
using Util.RabbitMQUtils.Factory.Enum;
using Util.StringUtils;

namespace PublishSubscribe.QuickSortClientApp
{
    class QuickSortReceiver
    {
        static void Main(string[] args)
        {
            using (var queue = ConsumerFactory.Create(
                ConsumerWithQueueNameTypes.Fanout, 
                "sort"))
            {
                queue.BasicConsume(
                     (sender, message) => QuickSort(message));
                ConsoleExtensions.WriteLineWait("Quick Sort\nPress [enter] to exit.");
            }
        }

        private static void QuickSort(string message)
        {
            var numbers = (int[])JsonConvert.DeserializeObject(message, typeof(int[]));
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            QuickSort(numbers, 0, numbers.Length - 1);
            stopwatch.Stop();
            Console.WriteLine($"{StringExtentions.FromIntArray(numbers)} - elapsed time: {stopwatch.Elapsed}");
        }

        private static void QuickSort(int[] arr, int left, int right)
        {
            if (left < right)
            {
                int pivot = Partition(arr, left, right);

                if (pivot > 1)
                {
                    QuickSort(arr, left, pivot - 1);
                }
                if (pivot + 1 < right)
                {
                    QuickSort(arr, pivot + 1, right);
                }
            }

        }

        private static int Partition(int[] arr, int left, int right)
        {
            int pivot = arr[left];
            while (true)
            {

                while (arr[left] < pivot)
                {
                    left++;
                }

                while (arr[right] > pivot)
                {
                    right--;
                }

                if (left < right)
                {
                    if (arr[left] == arr[right]) return right;

                    int temp = arr[left];
                    arr[left] = arr[right];
                    arr[right] = temp;


                }
                else
                {
                    return right;
                }
            }
        }



    }
}
