namespace Tally.App.Helpers
{
    public static class StringExtension
    {
        /// <summary>
        /// string转int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjToInt(this string obj)
        {
            int.TryParse(obj, out int result);
            return result;
        }
    }
}
