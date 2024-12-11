using System.Net;
using System.Text;

namespace VNPAY.NET.Utilities
{
    internal class PaymentHelper
    {
        private readonly SortedList<string, string> _requestData = new(new Comparer());
        private readonly SortedList<string, string> _responseData = new(new Comparer());

        internal void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        internal void AddResponseData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        internal string GetResponseValue(string key)
        {
            return _responseData.TryGetValue(key, out var value) ? value : string.Empty;
        }

        internal string GetPaymentUrl(string baseUrl, string hashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}&");
            }

            var querystring = data.ToString();

            baseUrl += "?" + querystring;
            var signData = querystring;
            if (signData.Length > 0)
            {
                signData = signData.Remove(data.Length - 1, 1);
            }

            var secureHash = Encoder.AsHmacSHA512(hashSecret, signData);
            baseUrl += "vnp_SecureHash=" + secureHash;

            return baseUrl;
        }

        internal bool IsSignatureValid(string inputHash, string secretKey)
        {
            var rspRaw = GetResponseData();
            var checksum = Encoder.AsHmacSHA512(secretKey, rspRaw);
            return checksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }

        internal string GetResponseData()
        {
            _responseData.Remove("vnp_SecureHashType");
            _responseData.Remove("vnp_SecureHash");

            var data = new StringBuilder();

            foreach (var (key, value) in _responseData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append($"{WebUtility.UrlEncode(key)}={WebUtility.UrlEncode(value)}&");
            }

            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }

            return data.ToString();
        }
    }
}
