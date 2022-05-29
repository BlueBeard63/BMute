using B.Mute.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Database
{
    public static class DatabaseExtension
    {
        public static void InsertMute(this DatabaseManager database, MuteModel mute)
        {
            string sql = "INSERT INTO Mutes (PlayerName, PlayerID, PunisherName, PunisherID, Reason, Length, MuteCreated) " +
                "VALUES (@PlayerName, @PlayerID, @PunisherName, @PunisherID, @Reason, @Length, @MuteCreated);";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, mute);
            }
        }

        public static List<MuteModel> GetAllMutes(this DatabaseManager database)
        {
            string sql = "SELECT * FROM Mutes;";
            List<MuteModel> list = null;


            using(var conn = database.Connection)
            {
                list = conn.Query<MuteModel>(sql).ToList();
            }

            return list;
        }

        public static MuteModel GetMute(this DatabaseManager database, ulong steamid)
        {
            string sql = "SELECT * FROM Mutes WHERE PlayerID = @player;";

            using(var conn = database.Connection)
            {
                return (MuteModel)conn.Query<MuteModel>(sql, new { player = steamid });
            }
        }

        public static MuteModel GetMute(this DatabaseManager database, int muteid)
        {
            string sql = "SELECT * FROM Mutes WHERE MuteID = @mute;";

            using (var conn = database.Connection)
            {
                return (MuteModel)conn.Query<MuteModel>(sql, new { mute = muteid });
            }
        }

        public static void SetFlag(this DatabaseManager database, int muteid)
        {
            string sql = "UPDATE Mutes SET SendFlag = true WHERE MuteID = @Mute;";

            using(var conn = database.Connection)
            {
                conn.Execute(sql, new { Mute = muteid });
            }
        }
    }
}
