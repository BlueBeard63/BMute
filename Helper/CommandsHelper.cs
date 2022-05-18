using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B.Mute.Helper
{
    public class CommandsHelper
    {
        public static int? ConvertToBanDuration(IEnumerable<string> args)
        {
            int? seconds = 0;

            if (args != null && args.Count() > 0)
            {
                // char & multiplier
                Dictionary<char, int> timePeriods = new Dictionary<char, int>()
                {
                    { 'd', 86400 },
                    { 'h', 3600 },
                    { 'm', 60 },
                    { 's', 1 }
                };


                foreach (var arg in args)
                {
                    foreach (var pair in timePeriods)
                    {
                        if (arg.Contains(pair.Key))
                        {
                            if (int.TryParse(arg.Trim(pair.Key), out int result))
                            {
                                seconds += result * pair.Value;
                            }
                            break;
                        }
                    }
                }
            }

            return seconds == 0 ? null : seconds;
        }
    }
}
