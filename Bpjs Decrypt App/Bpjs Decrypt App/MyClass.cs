using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bpjs_Decrypt_App
{
    public class MyClass
    {
        public class PCareCredential
        {
            public string BaseURL { get; set; } = "https://apijkn-dev.bpjs-kesehatan.go.id";
            public string ServiceName { get; set; } = "pcare-rest-dev";
            public string Url { get; set; } = "pcare-rest-dev";
            public string Username { get; set; } = "dermott";
            public string Password { get; set; } = "BPJSKes2024**";
            public string KdAplikasi { get; set; } = "095";
            public string UserKey { get; set; } = "6825c31715d8d748d5944f13b39ac431";
            public string SecretKey { get; set; } = "8nDF24C2AD";
            public string ConsId { get; set; } = "15793";
        }

        public class VClaimCredential
        {
            public string BaseURL { get; set; } = "https://apijkn-dev.bpjs-kesehatan.go.id";
            public string ServiceName { get; set; } = "pcare-rest-dev";
            public string Url { get; set; } = "pcare-rest-dev";
            public string UserKey { get; set; } = "6825c31715d8d748d5944f13b39ac431";
            public string SecretKey { get; set; } = "8nDF24C2AD";
            public string ConsId { get; set; } = "15793";
        }
    }
}