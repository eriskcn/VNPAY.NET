using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Sockets;

namespace VNPAY.NET.Utilities
{
    public class NetworkHelper
    {
        /// <summary>
        /// Lấy địa chỉ IP từ HttpContext của API Controller.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns
                            .GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null)
                    {
                        return remoteIpAddress.ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return "127.0.0.1";
        }
    }
}
