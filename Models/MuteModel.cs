using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Models
{
    public class MuteModel
    {
        private Main pluginInstance => Main.Instance;


        public int MuteID { get; set; }
        public ulong PlayerID { get; set; }
        public ulong PunisherID { get; set; }
        public string Reason { get; set; }
        public int? Length { get; set; }
        public DateTime MuteCreated { get; set; }
        public bool IsMuted { get; set; }

        public MuteModel(ulong player, ulong punisher, string reason, int? length)
        {
            PlayerID = player;
            PunisherID = punisher;
            Reason = reason;
            Length = length;
            MuteCreated = Main.Instance.Configuration.Instance.UseUTC ? DateTime.UtcNow : DateTime.Now;
        }

        public MuteModel()
        {
        }

        public bool IsExpired
        {
            get
            {
                if (Length.HasValue)
                    return (pluginInstance.Configuration.Instance.UseUTC ? DateTime.UtcNow : DateTime.Now) > MuteCreated.AddSeconds(Length.Value);
                else
                    return false;
            }
        }

        public string ReasonString => Reason ?? pluginInstance.Translate("ReasonUnkown");
        public string DurationString
        {
            get
            {
                if (Length.HasValue)
                {
                    var span = TimeSpan.FromSeconds(Length.Value);
                    return TimeSpanString(span);
                }
                else
                {
                    return pluginInstance.Translate("DurationPermanent");
                }
            }
        }
        public string TimeLeftString
        {
            get
            {
                if (Length.HasValue)
                {
                    var span = MuteCreated.AddSeconds(Length.Value) - (pluginInstance.Configuration.Instance.UseUTC ? DateTime.UtcNow : DateTime.Now);
                    return TimeSpanString(span);
                }
                else
                {
                    return pluginInstance.Translate("DurationPermanent");
                }
            }
        }

        private string TimeSpanString(TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}",
                span.Duration().Days > 0 ? string.Format("{0:0}d ", span.Days) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0}h ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0}m ", span.Minutes) : string.Empty);

            if (formatted.EndsWith(" ")) formatted = formatted.Substring(0, formatted.Length - 1);
            if (string.IsNullOrEmpty(formatted)) formatted = "<1m";

            return formatted;
        }
    }
}
