using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace McDermott.Extentions
{
    public static class SecureHelper
    {
        public static string EncryptIdToBase64(long id)
        {
            var bytes = BitConverter.GetBytes(id);
            return Convert.ToBase64String(bytes);
        }

        public static long DecryptIdFromBase64(string? base64EncodedId)
        {
            var bytes = Convert.FromBase64String(base64EncodedId);
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
