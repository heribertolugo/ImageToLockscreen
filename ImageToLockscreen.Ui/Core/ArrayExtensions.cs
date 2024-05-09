namespace ImageToLockscreen.Ui.Core
{
    internal static class ArrayExtensions
    {
        public static string[] Merge(this string[] array, string[] arr)
        {
            string[] newArray = new string[array.Length + arr.Length];

            for (int i = 0; i < array.Length; i++)
                newArray[i] = array[i];
            for (int j = 0; j < arr.Length; j++)
                newArray[array.Length + j] = arr[j];
            return newArray;
        }
    }
}
