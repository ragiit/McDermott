using LZStringCSharp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace McDermott.Application.Features.Services
{
    public class PCareService(IConfiguration configuration, HttpClient httpClient) : IPCareService
    {
        private string EncodeToBase64(string input)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
            string base64Encoded = Convert.ToBase64String(bytesToEncode);
            return base64Encoded;
        }

        private readonly IConfiguration _configuration = configuration;

        private string Signature(string timestamp)
        {
            var data = $"{_configuration["PCareCreds:cons-id"]}&{timestamp}";
            var secretKey = _configuration["PCareCreds:secret-key"];

            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);

            return encodedSignature;

        }

        public string PCareWebServiceDecrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = PCareWebServiceGetHashKeys(key);

            try
            {
                decData = PCareWebServiceDecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decData;
        }

        private byte[][] PCareWebServiceGetHashKeys(string key)
        //public byte[][] GetHashKeys(string key)
        {
            byte[][] result = new byte[2][];
            Encoding enc = Encoding.UTF8;

            SHA256 sha2 = new SHA256CryptoServiceProvider();

            byte[] rawKey = enc.GetBytes(key);
            byte[] rawIV = enc.GetBytes(key);

            byte[] hashKey = sha2.ComputeHash(rawKey);
            byte[] hashIV = sha2.ComputeHash(rawIV);

            Array.Resize(ref hashIV, 16);

            result[0] = hashKey;
            result[1] = hashIV;

            return result;
        }

        private static string PCareWebServiceDecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                plaintext = srDecrypt.ReadToEnd();
            }
            return plaintext;
        }

        private string PCareSignature(string timestamp)
        {
            string secretKey = _configuration["PCareCreds:secret-key"]!;
            var data = $"{_configuration["PCareCreds:cons-id"]!}&{timestamp}";

            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);

            return encodedSignature;

        }

        public async Task<(dynamic, int)> SendPCareService(string requestURL, HttpMethod method, object? requestBody = null)
        {
            try
            {
                DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;

                string baseUrl = _configuration["PCareCreds:baseURL"]!;
                string serviceName = _configuration["PCareCreds:serviceName"]!;
                string username = _configuration["PCareCreds:username"]!;
                string password = _configuration["PCareCreds:password"]!;
                string kpAplikasi = _configuration["PCareCreds:kdAplikasi"]!;
                string userKey = _configuration["PCareCreds:user-key"]!;
                string secretKey = _configuration["PCareCreds:secret-key"]!;
                string cons = _configuration["PCareCreds:cons-id"]!;

                string t = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
                string sign = Signature(t);
                string auth = EncodeToBase64($"{username}:{password}:{kpAplikasi}");

                var url = $"{baseUrl}/{serviceName}/{requestURL}";
                var request = new HttpRequestMessage(method, url);
                request.Headers.Add("X-cons-id", cons);
                request.Headers.Add("X-timestamp", t);
                request.Headers.Add("X-signature", sign);
                request.Headers.Add("X-authorization", $"Basic {auth}");
                request.Headers.Add("user_key", userKey);

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    string requestBodyJson = JsonConvert.SerializeObject(requestBody);
                    request.Content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");
                }

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Ambil data dari respon
                    var responseData = await response.Content.ReadAsStringAsync();

                    string a = cons + secretKey + t;

                    dynamic responseJson = JsonConvert.DeserializeObject(responseData);

                    string r = responseJson.response;
                    dynamic metaData = responseJson.metaData;

                    string LZDecrypted = PCareWebServiceDecrypt(a, r);
                    string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);

                    return (result, Convert.ToInt32(response.StatusCode));
                }
                else
                {
                    // Tangani kesalahan jika diperlukan
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorResponse}");

                    return (errorResponse, Convert.ToInt32(response.StatusCode));
                }
            }
            catch (Exception ex)
            {
                return (ex.Message, 500);
            }
        }
    }
}
