using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace OutBackX.Config
{
    public static class SqlConnection
    {
        private static SQLiteConnection privatecon;

        public static SQLiteConnection GetConnection()
        {
            if (privatecon == null)
            {
                var dbConfig = DependencyService.Get<IDbPathConfig>();
                var caminho = Path.Combine(dbConfig.Path, "fiap.db");
                privatecon = new SQLite.SQLiteConnection(caminho, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.FullMutex, true);
            }
            return privatecon;
        }

    }
}
