using LZStringCSharp;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace McDermott.TelemedicianApi.ViewModel
{
    public static class LzStringHelper
    {
        private static readonly string USER_KEY = "s3l3m84R<4!|V<3H!DLip4N";
        private static readonly string SECRET_KEY = "S-,9JC&CwNg&khf9_@WwxnQBA_XR#X";

        public static string DoEncrypt(object data) => LzStringHelper.CompressAndEncrypt(data, USER_KEY, SECRET_KEY, GetCurrentTimestamp());

        private static string GetCurrentTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();

        public static bool VerifyKeysFromHeader(string userKeyFromHeader, string secretKeyFromHeader)
        {
            return userKeyFromHeader == USER_KEY && secretKeyFromHeader == SECRET_KEY;
        }

        public static string DecompressAndDecrypt(string encryptedData, string userKey, string secretKey, string timestamp)
        {
            try
            {
                userKey = USER_KEY;
                secretKey = SECRET_KEY;

                // 1. Decode Base64 untuk mendapatkan byte array
                var encryptedBytes = Convert.FromBase64String(encryptedData);

                // 2. Extract IV (16 byte pertama)
                var iv = new byte[16];
                Array.Copy(encryptedBytes, 0, iv, 0, 16);

                // 3. Sisanya adalah ciphertext
                var ciphertext = new byte[encryptedBytes.Length - 16];
                Array.Copy(encryptedBytes, 16, ciphertext, 0, ciphertext.Length);

                // 4. Buat kunci enkripsi dari userKey, secretKey, dan timestamp
                var encryptionKey = $"{userKey}:{secretKey}:{timestamp}";

                using var sha256 = SHA256.Create();
                var keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));

                // 5. Dekripsi data
                using var aes = Aes.Create();
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var decryptor = aes.CreateDecryptor();
                var decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

                // 6. Dekompresi data menggunakan LZ-string
                var decompressedData = LZString.DecompressFromUTF16(Encoding.UTF8.GetString(decryptedBytes));

                return decompressedData;
            }
            catch (CryptographicException ex)
            {
                return "Decryption failed. Please check the encryption key or timestamp.";
            }
            catch (Exception ex)
            {
                return "An unexpected error occurred during decryption.";
            }
        }

        public static string CompressAndEncrypt(object data, string userKey, string secretKey, string timestamp)
        {
            if (!VerifyKeysFromHeader(userKey, secretKey))
            {
                throw new UnauthorizedAccessException("Invalid user_key or secret_key in the request header.");
            }

            var jsonResponse = JsonSerializer.Serialize(data);

            // 1. Kompresi data menggunakan LZ-string
            var compressedData = LZString.CompressToUTF16(jsonResponse);

            // 2. Buat kunci enkripsi dari userKey, secretKey, dan timestamp
            var encryptionKey = $"{userKey}:{secretKey}:{timestamp}";

            using var sha256 = SHA256.Create();
            var keyBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(encryptionKey));

            // 3. Enkripsi data
            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV(); // Generate IV secara acak
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor();
            var plaintextBytes = Encoding.UTF8.GetBytes(compressedData);
            var encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

            // 4. Gabungkan IV (16 byte pertama) dengan ciphertext
            var finalBytes = new byte[aes.IV.Length + encryptedBytes.Length];
            Array.Copy(aes.IV, 0, finalBytes, 0, aes.IV.Length);
            Array.Copy(encryptedBytes, 0, finalBytes, aes.IV.Length, encryptedBytes.Length);

            // 5. Encode hasilnya ke Base64
            return Convert.ToBase64String(finalBytes);
        }
    }
}