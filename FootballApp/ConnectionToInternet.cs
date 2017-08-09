using System;
using System.Net.NetworkInformation;
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
            catch (Exception)
            {
                // ignored
            }
            return reply != null && reply.Status == IPStatus.Success;
        }
    }
}
