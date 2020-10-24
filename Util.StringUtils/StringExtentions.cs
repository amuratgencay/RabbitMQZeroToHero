using System;

namespace Util.StringUtils
{
    public class StringExtentions
    {
        public static string FromIntArray(int[] arr)
        {

            var result = "[";
            for (int i = 0; i < arr.Length; i++)
            {
                result += arr[i].ToString().PadLeft(3, ' ') + ((i != arr.Length - 1) ? "," : "");
            }
            return result + "]";
        }
    }
}
