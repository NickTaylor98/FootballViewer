using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FootballApp
{
    class ConnectionToInternet
    {
        public static bool Connect()
        {
            PingReply reply = null;
            try
            {
                reply = new Ping().Send(@"sport-express.ru");
            }
            catch (Exception e)
            {
                
            }
            return reply != null && reply.Status == IPStatus.Success;
        }
    }
}
