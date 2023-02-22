using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImagenSoft.ModuloWeb.Persistencia.UtileriasPersistencia
{
    public class ServicioEncriptacion
    {
        private const int _keySize = 32;
        private const int _ivSize = 16;
        //byte[] key = UTF8Encoding.UTF8.GetBytes(strKey);
        //byte[] iv  = UTF8Encoding.UTF8.GetBytes(strIv);

        public async Task<string> EncryptString(string plainMessage, string Key, string IV)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(Key);
            byte[] iv = UTF8Encoding.UTF8.GetBytes(IV);

            return await EncryptString(plainMessage, key, iv);
        }

        public async Task<string> EncryptString(string plainMessage, byte[] Key, byte[] IV)
        {
            Array.Resize<byte>(ref Key, _keySize);
            Array.Resize<byte>(ref IV, _ivSize);

            // Establecer un flujo en memoria para el cifrado
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Crear una instancia del algoritmo de Rijndael
                Rijndael RijndaelAlg = Rijndael.Create();

                // Crear un flujo de cifrado basado en el flujo de los datos
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                                    RijndaelAlg.CreateEncryptor(Key, IV),
                                                                    CryptoStreamMode.Write))
                {
                    // Obtener la representación en bytes de la información a cifrar
                    byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(plainMessage);

                    // Cifrar los datos enviándolos al flujo de cifrado
                    await cryptoStream.WriteAsync(plainMessageBytes, 0, plainMessageBytes.Length);
                    cryptoStream.FlushFinalBlock();

                    // Obtener los datos datos cifrados como un arreglo de bytes
                    byte[] cipherMessageBytes = memoryStream.ToArray();

                    // Cerrar los flujos utilizados
                    memoryStream.Close();
                    cryptoStream.Close();

                    // Retornar la representación de texto de los datos cifrados
                    return Convert.ToBase64String(cipherMessageBytes);
                }
            }
        }

        public async Task<string> DecryptString(string encryptedMessage, string Key, string IV)
        {
            byte[] key = UTF8Encoding.UTF8.GetBytes(Key);
            byte[] iv = UTF8Encoding.UTF8.GetBytes(IV);

            return await DecryptString(encryptedMessage, key, iv);
        }

        /**
         * Descifra una cadena texto con el algoritmo de Rijndael
         * 
         * @param	encryptedMessage	mensaje cifrado
         * @param	Key					clave del cifrado para Rijndael
         * @param	IV					vector de inicio para Rijndael
         * @return	string				texto descifrado (plano)
         */
        public async Task<string> DecryptString(string encryptedMessage, byte[] Key, byte[] IV)
        {
            Array.Resize<byte>(ref Key, _keySize);
            Array.Resize<byte>(ref IV, _ivSize);

            // Obtener la representación en bytes del texto cifrado
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedMessage);

            // Crear un flujo en memoria con la representación de bytes de la información cifrada
            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                // Crear una instancia del algoritmo de Rijndael
                Rijndael RijndaelAlg = Rijndael.Create();

                // Crear un flujo de descifrado basado en el flujo de los datos
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
                                                                    RijndaelAlg.CreateDecryptor(Key, IV),
                                                                    CryptoStreamMode.Read))
                {
                    // Crear un arreglo de bytes para almacenar los datos descifrados
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                    // Obtener los datos descifrados obteniéndolos del flujo de descifrado
                    int decryptedByteCount = await cryptoStream.ReadAsync(plainTextBytes, 0, plainTextBytes.Length);

                    // Cerrar los flujos utilizados
                    memoryStream.Close();
                    cryptoStream.Close();

                    // Retornar la representación de texto de los datos descifrados
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
        }

        /**
         * Cifra una cadena texto con el algoritmo de Rijndael y lo almacena en un archivo
         * 
         * @param	plainMessage	mensaje plano (sin cifrar)
         * @param	filename		nombre del archivo donde se almacenará el mensaje cifrado
         * @param	Key				clave del cifrado para Rijndael
         * @param	IV				vector de inicio para Rijndael
         * @return	void
         */
        public async void EncryptToFile(string plainMessage, string filename, byte[] Key, byte[] IV)
        {
            Array.Resize<byte>(ref Key, _keySize);
            Array.Resize<byte>(ref IV, _ivSize);
            // Crear un flujo para el archivo a generarse
            using (FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate))
            {
                // Crear una instancia del algoritmo Rijndael
                Rijndael RijndaelAlg = Rijndael.Create();

                // Crear un flujo de cifrado basado en el flujo de los datos
                using (CryptoStream cryptoStream = new CryptoStream(fileStream,
                                                                    RijndaelAlg.CreateEncryptor(Key, IV),
                                                                    CryptoStreamMode.Write))
                {
                    // Crear un flujo de escritura basado en el flujo de cifrado
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {

                        // Cifrar el mensaje a través del flujo de escritura
                        await streamWriter.WriteLineAsync(plainMessage);

                        // Cerrar los flujos utilizados
                        streamWriter.Close();
                        cryptoStream.Close();
                        fileStream.Close();
                    }
                }
            }
        }

        /**
         * Descifra el contenido de un archivo con el algoritmo de Rijndael y lo retorna
         * como una cadena de texto plano
         * 
         * @param	filename		nombre del archivo donde se encuentra el mensaje cifrado
         * @param	Key				clave del cifrado para Rijndael
         * @param	IV				vector de inicio para Rijndael
         * @return	string			mensaje descifrado (plano)
         */
        public async Task<string> DecryptFromFile(string filename, byte[] Key, byte[] IV)
        {
            Array.Resize<byte>(ref Key, _keySize);
            Array.Resize<byte>(ref IV, _ivSize);
            // Crear un flujo para el archivo a generarse
            using (FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate))
            {
                // Crear una instancia del algoritmo Rijndael
                Rijndael RijndaelAlg = Rijndael.Create();

                // Crear un flujo de cifrado basado en el flujo de los datos
                using (CryptoStream cryptoStream = new CryptoStream(fileStream,
                                                                    RijndaelAlg.CreateDecryptor(Key, IV),
                                                                    CryptoStreamMode.Read))
                {
                    // Crear un flujo de lectura basado en el flujo de cifrado
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {

                        // Descifrar el mensaje a través del flujo de lectura
                        string plainMessage = await streamReader.ReadLineAsync();

                        // Cerrar los flujos utilizados
                        streamReader.Close();
                        cryptoStream.Close();
                        fileStream.Close();

                        return plainMessage;
                    }
                }
            }
        }
    }
}
