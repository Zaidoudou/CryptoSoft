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
        /// Process a file using XOR encryption/decryption.
        /// </summary>
        /// <param name="sourceFilePath">The path to the source file.</param>
        /// <param name="destinationFilePath">The path to the destination file.</param>
        /// <param name="key">The key to use for encryption/decryption.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task ProcessFileAsync(string sourceFilePath, string destinationFilePath, ulong key)
        {
            // Create destination directory if it doesn't exist
            string? destinationDir = Path.GetDirectoryName(destinationFilePath);
            if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Create a byte array from the key (64 bits = 8 bytes)
            byte[] keyBytes = BitConverter.GetBytes(key);

            // Use streams for efficient file processing
            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[4096]; // 4KB buffer for reading
                int bytesRead;

                // Process the file in chunks
                while ((bytesRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    // Apply XOR encryption/decryption to the buffer
                    for (int i = 0; i < bytesRead; i++)
                    {
                        buffer[i] = (byte)(buffer[i] ^ keyBytes[i % keyBytes.Length]);
                    }

                    // Write encrypted/decrypted data to destination
                    await destinationStream.WriteAsync(buffer, 0, bytesRead);
                }
            }
        }
    }
}