using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Sockets;

namespace VNPAY.NET.Utilities
{
    public class NetworkHelper
    {
        private const string DefaultIpAddress = "127.0.0.1";

        /// <summary>
        /// Lấy địa chỉ IP của thiết bị thanh toán, sử dụng khi người dùng gọi API tạo URL thanh toán.
        /// </summary>
        /// <param name="context"></param>
        public static string GetClientIpAddress(HttpContext context)
        {
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress == null)
                {
                    return DefaultIpAddress;
                }

                if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    var ipv4Address = Dns.GetHostEntry(remoteIpAddress).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

                    if (ipv4Address != null)
                    {
                        return ipv4Address.ToString();
                    }
                }

                return remoteIpAddress.ToString();
            }
            catch
            {
                return DefaultIpAddress;
            }
        }

        /// <summary>
        /// Lấy địa chỉ IP của thiết bị đang sử dụng
        /// </summary>
        public static string GetLocalIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return DefaultIpAddress;
            }
            catch
            {
                return DefaultIpAddress;
            }
        }
    }
}
