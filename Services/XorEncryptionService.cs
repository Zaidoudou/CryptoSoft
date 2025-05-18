// <copyright file="XorEncryptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// XOR encryption service.
    /// </summary>
    public class XorEncryptionService : IEncryptionService
    {
        /// <summary>
        /// Encrypts a file in place using XOR encryption.
        /// </summary>
        /// <param name="filePath">The path to the file to encrypt.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task EncryptFileAsync(string filePath, ulong key)
        {
            await this.ProcessFileInPlaceAsync(filePath, key);
        }

        /// <summary>
        /// Decrypts a file in place using XOR decryption.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <param name="key">The decryption key.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task DecryptFileAsync(string filePath, ulong key)
        {
            // XOR encryption is symmetric, so decryption is the same as encryption
            await this.ProcessFileInPlaceAsync(filePath, key);
        }

        private async Task ProcessFileInPlaceAsync(string filePath, ulong key)
        {
            // Create a temporary file for processing
            string tempFile = Path.GetTempFileName();

            try
            {
                // Create a byte array from the key (64 bits = 8 bytes)
                byte[] keyBytes = BitConverter.GetBytes(key);

                // Process the file in chunks
                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                using (FileStream tempStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[4096]; // 4KB buffer for reading
                    int bytesRead;

                    while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        // Apply XOR encryption/decryption to the buffer
                        for (int i = 0; i < bytesRead; i++)
                        {
                            buffer[i] = (byte)(buffer[i] ^ keyBytes[i % keyBytes.Length]);
                        }

                        // Write processed data to temp file
                        await tempStream.WriteAsync(buffer, 0, bytesRead);
                    }
                }

                // Replace original file with processed file
                File.Delete(filePath);
                File.Move(tempFile, filePath);
            }
            catch
            {
                // Clean up temp file if something goes wrong
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                throw;
            }
        }
    }
}