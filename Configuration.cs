using B.MedicalSystem.Models;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace B.MedicalSystem
{
    public class Configuration : IRocketPluginConfiguration
    {
        public int DownedDuration { get; set; }
        public byte DownedHealth { get; set; }
        public string DragPermission { get; set; }
        [XmlArrayItem("ReviveInfomation")]
        public List<ReviveModel> Revive { get; set; }
        [XmlArray("PermissionsToBlock")]
        [XmlArrayItem("Permission")]
        public List<string> Permissions { get; set; }
        [XmlArray("MedicalBags")]
        [XmlArrayItem("MedicalBag")]
        public List<StorageModel> Storages { get; set; }

        public void LoadDefaults()
        {
            DownedDuration = 500;
            DownedHealth = 10;
            DragPermission = "medicalsystem.drag";
            Revive = new List<ReviveModel>()
            {
                new ReviveModel("medicalsystem.revive", 10, true, 100, true)
            };
            Permissions = new List<string>()
            {
                "kit",
                "warp",
                "tp",
                "tphere",
                "teleport",
                "kill",
                "i",
                "v",
                "home",
            };
            Storages = new List<StorageModel>()
            {
                new StorageModel(328, 10, 10),
                new StorageModel(366, 5, 5)
            };
        }
    }

    public class ReviveModel
    {
        public string Permission { get; set; }
        public float Time { get; set; }
        public bool ResetRadiation { get; set; }
        public byte Health { get; set; }
        public bool BrokenBones { get; set; }

        public ReviveModel()
        {
        }

        public ReviveModel(string permission, float time, bool radiation, byte health, bool bones)
        {
            Permission = permission;
            Time = time;
            ResetRadiation = radiation;
            Health = health;
            BrokenBones = bones;
        }
    }
}
