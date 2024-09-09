using LZStringCSharp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace McDermott.Application.Features.Services
{
    public class PCareService(IConfiguration configuration, HttpClient httpClient, IMediator mediator) : IPCareService
    {
        private string EncodeToBase64(string input)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(input);
            string base64Encoded = Convert.ToBase64String(bytesToEncode);
            return base64Encoded;
        }

        //private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;

        private async Task<string> Signature(string timestamp)
        {
            var cred = await GetPCareCredential();
            var data = $"{cred.ConsId!}&{timestamp}";
            string secretKey = cred.SecretKey;

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

        private async Task<SystemParameterDto> GetPCareCredential()
        {
            //return (await _mediator.Send(new GetSystemParameterQuery(x => x.Key.Equals(key)))).FirstOrDefault()?.Value ?? string.Empty;

            return (await _mediator.Send(new GetSystemParameterQuery()))?.FirstOrDefault() ?? new();
        }

        public async Task<(dynamic, int)> SendPCareService(string baseURL, string requestURL, HttpMethod method, object? requestBody = null)
        {
            try
            {
                DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;

                SystemParameterDto cred = await GetPCareCredential() ?? new();

                var urls = string.Empty;

                if (nameof(SystemParameter.AntreanFKTPBaseURL) == baseURL)
                    urls = cred.AntreanFKTPBaseURL;
                else if (nameof(SystemParameter.PCareBaseURL) == baseURL)
                    urls = cred.PCareBaseURL;

                string baseUrl = urls;
                string username = cred.Username;
                string password = cred.Password;
                string kpAplikasi = cred.KdAplikasi;
                string userKey = cred.UserKey;
                string secretKey = cred.SecretKey;
                string cons = cred.ConsId;

                string t = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
                string sign = await Signature(t);
                string auth = EncodeToBase64($"{username}:{password}:{kpAplikasi}");

                var url = $"{baseUrl}/{requestURL}";
                var request = new HttpRequestMessage(method, url);
                request.Headers.Add("X-cons-id", cons);
                request.Headers.Add("X-timestamp", t);
                request.Headers.Add("X-signature", sign);
                request.Headers.Add("X-authorization", $"Basic {auth}");
                request.Headers.Add("user_key", userKey);

                if (method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    string requestBodyJson = JsonConvert.SerializeObject(requestBody);
                    request.Content = new StringContent(requestBodyJson, Encoding.UTF8);
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

                    if (LZDecrypted is null)
                        return (r, Convert.ToInt32(response.StatusCode));

                    try
                    {
                        string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);
                        return (result, Convert.ToInt32(response.StatusCode));
                    }
                    catch
                    {
                        return (r, Convert.ToInt32(response.StatusCode));
                    }
                }
                else
                {
                    // Tangani kesalahan jika diperlukan
                    dynamic errorResponse = await response.Content.ReadAsStringAsync();

                    dynamic data = JsonConvert.DeserializeObject<dynamic>(errorResponse);

                    if (data.response is not null)
                    {
                        if (Convert.ToInt32(response.StatusCode) == 412 && data.response is null)
                            return (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));

                        string a = cons + secretKey + t;
                        dynamic r = data.response;
                        dynamic metaData = data.metaData;

                        if (r is JArray responseArray)
                        {
                            // Iterate over each item in the response array
                            var rr = string.Empty;
                            foreach (dynamic item in responseArray)
                            {
                                string field = item.field;
                                string message = item.message;

                                rr += $"{field} {message} ";
                            }
                            return (rr, Convert.ToInt32(response.StatusCode));
                        }

                        if (r is null)
                        {
                            return (metaData.message, Convert.ToInt32(response.StatusCode));
                        }

                        try
                        {
                            string LZDecrypted = PCareWebServiceDecrypt(a, Convert.ToString(r));
                            string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);

                            Console.WriteLine($"Response: {JsonConvert.DeserializeObject(result)}");
                            return (result, Convert.ToInt32(response.StatusCode));
                        }
                        catch (Exception)
                        {
                            return (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Response: {data}");

                        return (data, Convert.ToInt32(response.StatusCode));
                    }
                }
            }
            catch (Exception ex)
            {
                return (ex.Message, 500);
            }
        }
    }
}