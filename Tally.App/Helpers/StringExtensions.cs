namespace Tally.App.Helpers
{
    public static class StringExtensions
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

        /// <summary>
        /// string转decimal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this string obj)
        {
            decimal.TryParse(obj, out decimal result);
            return result;
        }
    }
}
