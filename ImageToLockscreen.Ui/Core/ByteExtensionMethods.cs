namespace ImageToLockscreen.Ui.Core
{
    internal static class ByteExtensionMethods
    {
        /// <summary>
        /// Returns a value indicating whether a specified subarray occurs within array
        /// </summary>
        /// <param name="bytes">Main array</param>
        /// <param name="b">Subarray to seek within main array</param>
        /// <returns>true if a array starts with b subarray or if b is empty; otherwise false</returns>
        /// <remarks><see href="https://stackoverflow.com/a/49683945/6368401">Credit Ricardo González</see></remarks>
        public static bool StartsWith(this byte[] bytes, byte[] b)
        {
            if (bytes.Length < b.Length)
                return false;

            for (int i = 0; i < b.Length; i++)
            {
                if (bytes[i] != b[i])
                    return false;
            }

            return true;
        }

        public static bool EndsWith(this byte[] bytes, byte[] b)
        {
            for (int i = 1; i <= b.Length; i++)
            {
                if (bytes[bytes.Length - i] != b[b.Length - i])
                    return false;
            }
            return true;
        }
    }
}
