using LiteDB;
using System.IO;
using Xamarin.Essentials;

namespace Tally.Framework.Interface
{
    public class UnitWork
    {
        static LiteDatabase _liteDatabase = null;
        private static readonly object _lock = new object();

        public static LiteDatabase GetDbClient
        {
            get
            {
                lock (_lock)
                {
                    return _liteDatabase = _liteDatabase ?? GetDb();
                }
            }
        }

        /// <summary>
        /// 主动释放
        /// </summary>
        public static void Dispose()
        {
            _liteDatabase.Dispose();
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public static void Restore()
        {
            try
            {
                var dbPath = $"{FileSystem.AppDataDirectory}/tally.db";
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tally.db");
                var t = File.Exists(dbPath);
                File.Delete(dbPath);
                var t2 = File.Exists(dbPath);
            }
            catch (System.Exception c)
            {
                throw;
            }
        }

        private static LiteDatabase GetDb()
        {
            //SQLite 数据库地址
            //var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tally.db");
            var path = $"{FileSystem.AppDataDirectory}/tally.db";
            return new LiteDatabase(path);
        }
    }
}
