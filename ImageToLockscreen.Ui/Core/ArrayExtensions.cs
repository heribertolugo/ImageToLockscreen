using System;

namespace ImageToLockscreen.Ui.Core
{
    internal static class ArrayExtensions
    {
        public static string[] Merge(this string[] array, string[] arr)
        {
            string[] newArray = new string[array.Length + arr.Length];

            Array.Copy(array, newArray, array.Length);
            Array.Copy(arr, 0, newArray, array.Length, arr.Length);
            return newArray;
        }
    }
}
