using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace B.MedicalSystem.Models
{
    public class StorageModel
    {
        [XmlIgnore]
        public int Id { get; set; }
        public ushort StorageID { get; set; }
        public byte Width { get; set; }
        public byte Height { get; set; }
        [XmlIgnore]
        public List<ItemModel> Items { get; set; }
        [XmlIgnore]
        public ulong Owner { get; set; }
        [XmlIgnore]
        public string OwnerName { get; set; }

        public StorageModel()
        {
        }

        public StorageModel(ushort storageID, List<ItemModel> items, ulong owner, byte width, byte height)
        {
            StorageID = storageID;
            Items = items;
            Owner = owner;
            Width = width;
            Height = height;
        }

        public StorageModel(ushort storageID, byte width, byte height)
        {
            StorageID = storageID;
            Width = width;
            Height = height;
        }
    }
}
