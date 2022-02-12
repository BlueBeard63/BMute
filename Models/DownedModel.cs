using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.MedicalSystem.Models
{
    public class DownedModel
    {
        public DateTime DownedTime { get; set; }
        public int Time { get; set; }

        public DownedModel(DateTime now, int time)
        {
            DownedTime = now;
            Time = time;
        }
    }
}
