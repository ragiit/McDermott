using LZStringCSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Bpjs_Decrypt_App.MyClass;

namespace Bpjs_Decrypt_App
{
    public partial class VClaimForm : Form
    {
        public VClaimForm()
        {
            InitializeComponent();
        }

        private void VClaimForm_Load(object sender, EventArgs e)
        { // Menentukan lokasi file JSON
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vclaim-config.json");

            try
            {
                string jsonString = File.ReadAllText(filePath);

                VClaimCredential config = System.Text.Json.JsonSerializer.Deserialize<VClaimCredential>(jsonString) ?? new();

                tbBaseUrl.Text = config.BaseURL;
                tbService.Text = config.ServiceName;
                tbURL.Text = config.Url;
                tbUserKey.Text = config.UserKey;
                tbSecretKey.Text = config.SecretKey;
                tbConsId.Text = config.ConsId;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat memuat konfigurasi: {ex.Message}");
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                await SendPCareService();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task SendPCareService()
        {
            try
            {
                using HttpClient httpClient = new();
                DateTime epochStart = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan timeSpan = DateTime.UtcNow - epochStart;

                string t = Convert.ToInt64(timeSpan.TotalSeconds).ToString();
                string sign = Signature($"{tbConsId.Text}&{t}");

                var url = $"{tbBaseUrl.Text}/{tbService.Text}/{tbURL.Text}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("X-cons-id", tbConsId.Text);
                request.Headers.Add("X-timestamp", t);
                request.Headers.Add("X-signature", sign);
                request.Headers.Add("user_key", tbUserKey.Text);

                //if (method == HttpMethod.Post || method == HttpMethod.Put)
                //{
                //    request.Content = new StringContent(requestBody.ToString(), Encoding.UTF8);
                //}

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Ambil data dari respon
                    var responseData = await response.Content.ReadAsStringAsync();

                    try
                    {
                        dynamic responseJson = JsonConvert.DeserializeObject(responseData);

                        if (responseJson.response != null)
                        {
                            string a = tbConsId.Text + tbSecretKey.Text + t;

                            if (responseJson.response is JArray responseArray)
                            {
                                //if (!responseArray.HasValues)
                                //{
                                //    return (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));

                                //}
                                var jsonObject = JsonConvert.DeserializeObject(responseJson);
                                var formattedJson1 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson1;
                                return;
                            }

                            string r = responseJson.response;
                            dynamic metaData = responseJson.metaData;

                            string LZDecrypted = Decrypt(a, r);
                            try
                            {
                                string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);

                                var jsonObject = JsonConvert.DeserializeObject(result);
                                string formattedJson = JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented);

                                richTextBox1.Text = formattedJson;
                                return;
                            }
                            catch (Exception)
                            {
                                var jsonObject = JsonConvert.DeserializeObject(responseJson);
                                var formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson;
                                return;
                            }

                            var formattedJson3 = JsonConvert.SerializeObject(responseJson, Formatting.Indented);
                            richTextBox1.Text = formattedJson3;
                            return;
                        }
                        else
                        {
                            var jsonObject = JsonConvert.DeserializeObject(responseJson);
                            var formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                            richTextBox1.Text = formattedJson;
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        var aaa = Convert.ToString(await response.Content.ReadAsStringAsync());
                        var jsonObject = JsonConvert.DeserializeObject(aaa);
                        var formattedJson = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                        richTextBox1.Text = formattedJson;
                        return;
                    }
                }
                else
                {
                    // Tangani kesalahan jika diperlukan
                    dynamic errorResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        dynamic data = JsonConvert.DeserializeObject<dynamic>(errorResponse);

                        if (data.response != null)
                        {
                            //if (Convert.ToInt32(response.StatusCode) == 412 && data.response is null)
                            //    return (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));

                            string a = tbConsId.Text + tbSecretKey.Text + t;
                            dynamic r = data.response;
                            dynamic metaData = data.metaData;

                            if (r is JArray responseArray)
                            {
                                if (!responseArray.HasValues)
                                {
                                    var responseJson = (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));
                                    var jsonObject1 = JsonConvert.DeserializeObject(responseJson.Item1);
                                    var formattedJson1 = JsonConvert.SerializeObject(jsonObject1, Formatting.Indented);
                                    richTextBox1.Text = formattedJson1;
                                    return;
                                }

                                //var responseJson1 = (await response.Content.ReadAsStringAsync(), Convert.ToInt32(response.StatusCode));
                                var jsonObject = JsonConvert.DeserializeObject(r);
                                var formattedJson12 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson12;
                                return;
                            }

                            if (r is null)
                            {
                                var jsonObject = JsonConvert.DeserializeObject(metaData.message);
                                var formattedJson12 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson12;
                                return;
                            }

                            string LZDecrypted = Decrypt(a, r);
                            try
                            {
                                string result = LZString.DecompressFromEncodedURIComponent(LZDecrypted);

                                var jsonObject = JsonConvert.DeserializeObject(result);
                                var formattedJson12 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson12;

                                return;
                            }
                            catch (Exception)
                            {
                                var jsonObject = JsonConvert.DeserializeObject(data);
                                var formattedJson12 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                                richTextBox1.Text = formattedJson12;

                                return;
                            }
                        }
                        else
                        {
                            var jsonObject = JsonConvert.DeserializeObject(data);
                            var formattedJson12 = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                            richTextBox1.Text = formattedJson12;

                            return;
                        }
                    }
                    catch (Exception)
                    {
                        var formattedJson12 = JsonConvert.SerializeObject(errorResponse, Formatting.Indented);
                        richTextBox1.Text = formattedJson12;

                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                var formattedJson12 = JsonConvert.SerializeObject(ex.Message, Formatting.Indented);
                richTextBox1.Text = formattedJson12;

                return;
            }
        }

        private string Signature(string a)
        {
            var data = a;
            var secretKey = tbSecretKey.Text;

            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            //encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);

            return encodedSignature;
        }

        public string Decrypt(string key, string data)
        {
            string decData = null;
            byte[][] keys = GetHashKeys(key);

            try
            {
                decData = DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
            }
            catch (CryptographicException) { }
            catch (ArgumentNullException) { }

            return decData;
        }

        private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
        {
            byte[] cipherText = Convert.FromBase64String(cipherTextString);

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt =
                            new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        private byte[][] GetHashKeys(string key)
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
    }
}