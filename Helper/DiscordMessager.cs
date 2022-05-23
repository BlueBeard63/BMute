using B.Mute.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Helper
{
    public class DiscordMessager
    {
        private readonly Main pluginInstance;
        private Configuration config => pluginInstance.Configuration.Instance;

        public DiscordMessager(Main pluginInstance)
        {
            this.pluginInstance = pluginInstance;
        }

        public void SendMessage(string content, EMessageType messageType)
        {
            Webhook webhook = config.Webhooks.FirstOrDefault(x => x.WebhookType == messageType.ToString());

            if (webhook == null || string.IsNullOrEmpty(webhook.WebhookUrl))
                return;

            using (var wc = new WebClient())
            {
                DiscordWebhookMessage msg = new DiscordWebhookMessage(new Embed(content, Convert.ToInt32(webhook.WebhookColor.Trim('#'), 16)));
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                wc.UploadString(webhook.WebhookUrl, JsonConvert.SerializeObject(msg));
            }
        }

        public void SendMessage(EMessageType messageType, params string[] args)
        {
            Webhook webhook = config.Webhooks.FirstOrDefault(x => x.WebhookType == messageType.ToString());

            if (webhook == null || string.IsNullOrEmpty(webhook.WebhookUrl))
                return;

            string[] array = webhook.MessageFormat.Split(new string[] { ": ", ", " }, StringSplitOptions.RemoveEmptyEntries);
            int num = 0;

            using (var wc = new WebClient())
            {
                List<Field> fields = new List<Field>();
                while (num < array.Length - 1)
                {
                    string[] arr = array.Skip(num).Take(2).ToArray();

                    string value = arr[1]
                        .Replace("{name}", args[0])
                        .Replace("{steamid}", args[1])
                        .Replace("{punisher}", args[2])
                        .Replace("{servername}", SDG.Unturned.Provider.serverName);

                    if (messageType != EMessageType.Mute)
                        value = value.Replace("{reason}", args[3]);
                    if (messageType == EMessageType.UnMute)
                        value = value.Replace("{duration}", args[4]);

                    fields.Add(new Field(arr[0], value, true));
                    num += 2;
                }

                DiscordWebhookMessage msg = new DiscordWebhookMessage(new Embed(fields, Convert.ToInt32(webhook.WebhookColor.Trim('#'), 16)));
                wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                wc.UploadString(webhook.WebhookUrl, JsonConvert.SerializeObject(msg));
            }
        }
    }

    public class DiscordWebhookMessage
    {
        public DiscordWebhookMessage(Embed embed)
        {
            embeds = new List<Embed>();
            embeds.Add(embed);
        }

        public DiscordWebhookMessage(string content)
        {
            this.content = content;
        }

        public List<Embed> embeds { get; set; }
        public string content { get; set; }
    }

    public class Embed
    {
        public Embed(string description, int color)
        {
            this.description = description;
            this.color = color;
            this.timestamp = DateTime.UtcNow.ToString("u");
        }

        public Embed(List<Field> fields, int color)
        {
            this.fields = fields;
            this.color = color;
            this.timestamp = DateTime.UtcNow.ToString("u");
        }

        public int color { get; set; }
        public string timestamp { get; set; }
        public string description { get; set; }
        public List<Field> fields { get; set; }
    }

    public class Field
    {
        public Field(string name, string value, bool inline)
        {
            this.name = name;
            this.value = value;
            this.inline = inline;
        }

        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }

    public enum EMessageType
    {
        Mute,
        UnMute
    }
}
