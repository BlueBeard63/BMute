using Dapper;
using MySql.Data.MySqlClient;
using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Database
{
    public class DatabaseManager
    {
        private readonly Main pluginInstance;

        public DatabaseManager(Main instance)
        {
            this.pluginInstance = instance;
        }

        public void InitializeTables()
        {
            using (var conn = Connection)
            {
                try
                {
                    conn.Execute(DatabaseQuery.MuteTable);
                }
                catch (MySqlException e)
                {
                    Logger.LogError("An error occurated while trying to create tables.");
                    Logger.LogException(e);
                }
            }
        }

        public MySqlConnection Connection => new MySqlConnection(Main.Instance.Configuration.Instance.DatabaseConnection);
    }
}
