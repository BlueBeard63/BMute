using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace B.Mute.Models
{
    public class Webhook
    {
        public Webhook() { }

        public Webhook(string webhookType, string webhookUrl, string webhookColor, string messageFormat)
        {
            WebhookType = webhookType;
            WebhookUrl = webhookUrl;
            WebhookColor = webhookColor;
            MessageFormat = messageFormat;
        }

        [XmlAttribute]
        public string WebhookType { get; set; }
        [XmlAttribute]
        public string WebhookUrl { get; set; }
        [XmlAttribute]
        public string WebhookColor { get; set; }
        public string MessageFormat { get; set; }
    }
}
