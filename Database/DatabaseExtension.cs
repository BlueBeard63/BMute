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
            string sql = "INSERT INTO Mutes (PlayerID, PunisherID, Reason, Length, MuteCreated) " +
                "VALUES (@PlayerID, @PunisherID, @Reason, @Length, @MuteCreated);";

            using (var conn = database.Connection)
            {
                conn.Execute(sql, mute);
            }
        }

        public static List<MuteModel> GetAllMutes(this DatabaseManager database)
        {
            string sql = "SELECT m.MuteID, m.PlayerID, m.PunisherID, m.Reason, m.Length, m.MuteCreated, m.IsMuted FROM Mutes AS m;";

            using(var conn = database.Connection)
            {
                return conn.Query<MuteModel>(sql).ToList();
            }
        }

        public static void SetFlag(this DatabaseManager database, int muteid)
        {
            string sql = "UPDATE Mutes SET IsMuted = true WHERE MuteID = @Muteid;";

            using(var conn = database.Connection)
            {
                conn.Execute(sql, new { Muteid = muteid });
            }
        }
    }
}
