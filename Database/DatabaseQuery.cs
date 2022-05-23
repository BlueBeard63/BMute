using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Database
{
    public class DatabaseQuery
    {
        public const string MuteTable = @"
CREATE TABLE IF NOT EXISTS Mutes (
    MuteID INT NOT NULL AUTO_INCREMENT,
    PlayerName VARCHAR(255) NOT NULL,
    PlayerID BIGINT NOT NULL,
    PunisherName VARCHAR(255) NULL DEAFULT `System`,
    PunisherID BIGINT NOT NULL DEFAULT 0,
    Reason VARCHAR(255) NULL,
    Length INT(11) NULL,
    MuteCreated DATETIME NOT NULL,
    IsMuted TINYINT NOT NULL DEFAULT 0,
    CONSTRAINT PK_Mutes PRIMARY KEY (MuteID)
);
        ";

    }
}
