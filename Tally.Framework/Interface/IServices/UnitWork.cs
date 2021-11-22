﻿using SQLite;
using System.IO;
using Tally.Framework.Models;

namespace Tally.Framework.Interface
{
    public class UnitWork
    {
        static SQLiteConnection _sqlteDataBase = null;
        private static readonly object _lock = new object();

        public static SQLiteConnection GetDbClient
        {
            get
            {
                lock (_lock)
                {
                    return _sqlteDataBase = _sqlteDataBase ?? GetDb();
                }
            }
        }

        /// <summary>
        /// 主动释放
        /// </summary>
        public static void Dispose()
        {
            _sqlteDataBase.Dispose();
            _sqlteDataBase = null;
        }

        /// <summary>
        /// 删除所有数据
        /// </summary>
        public static void Restore()
        {
            using (GetDbClient)
            {
                _sqlteDataBase.DropTable<SpendLog>();
                //创建消费表
                _sqlteDataBase.CreateTable<SpendLog>();
            }
        }

        /// <summary>
        /// 种子数据初始化
        /// </summary>
        public static void Initalize()
        {
            using (GetDbClient)
            {
                _sqlteDataBase.DropTable<SpendLog>();
                //创建消费表
                _sqlteDataBase.CreateTable<SpendLog>();
            }
            //释放了Sqlite同时初始化
            _sqlteDataBase = null;
        }

        private static SQLiteConnection GetDb()
        {
            //SQLite 数据库地址
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "billApp.db3");
            return new SQLiteConnection(path);
        }
    }
}
