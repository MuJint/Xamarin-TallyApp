using LiteDB;
using Tally.Framework.Models;
using Xamarin.Essentials;

namespace Tally.Framework.Interface
{
    public class LiteDbExtend
    {
        private static LiteDatabase database = null;
        private static readonly object _lock = new object();

        public static LiteDatabase DbFactory
        {
            get
            {
                lock (_lock)
                {
                    if (database == null)
                    {
                        database = new LiteDatabase($"{FileSystem.AppDataDirectory}/tally.db");
                    }
                    return database;
                }
            }
        }

        public static void CloseDb()
        {
            lock (_lock)
            {
                database?.Dispose();
                database = null;
            }
        }
    }

    public class UnitWork
    {
        /// <summary>
        /// 主动释放
        /// </summary>
        public void Dispose()
        {
            LiteDbExtend.CloseDb();
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public static void Restore()
        {
            System.IO.File.Delete($"{FileSystem.AppDataDirectory}/tally.db");
            LiteDbExtend.DbFactory.DropCollection(nameof(SpendLog));
            LiteDbExtend.DbFactory.DropCollection(nameof(ErrorLog));
        }

        /// <summary>
        /// 种子数据初始化
        /// </summary>
        public static void Initalize()
        {
            LiteDbExtend.DbFactory.DropCollection(nameof(SpendLog));
            LiteDbExtend.DbFactory.DropCollection(nameof(ErrorLog));
        }
    }
}