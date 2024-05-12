using System;

namespace ImageToLockscreen.Ui.Core
{
    internal static class ArrayExtensions
    {
        public static T[] Merge<T>(this T[] array, T[] arr)
        {
            T[] newArray = new T[array.Length + arr.Length];

            Array.Copy(array, newArray, array.Length);
            Array.Copy(arr, 0, newArray, array.Length, arr.Length);
            return newArray;
        }
    }
}
