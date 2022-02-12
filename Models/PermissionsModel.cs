using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace B.MedicalSystem.Models
{
    public class PermissionsModel
    {
        [XmlIgnore]
        public ulong Player { get; set; }
        public List<string> Permissions { get; set; }

        public PermissionsModel(ulong player, List<string> permissions)
        {
            Player = player;
            Permissions = permissions;
        }

        public PermissionsModel(List<string> permissions)
        {
            Permissions = permissions;
        }

        public PermissionsModel()
        {
        }
    }
}
