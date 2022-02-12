using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MedicalSystem.Models
{
    public class ItemModel
    {
        public ushort item;
        public byte location_x;
        public byte location_y;
        public byte rot;
        public byte amount;
        public byte quality;
        public byte[] state;

        public ItemModel()
        {
        }

        public ItemModel(ushort Item, byte Location_X, byte Location_Y, byte Rot, byte Amount, byte Quality, byte[] State)
        {
            item = Item;
            location_x = Location_X;
            location_y = Location_Y;
            rot = Rot;
            amount = Amount;
            quality = Quality;
            state = State;
        }
    }
}
