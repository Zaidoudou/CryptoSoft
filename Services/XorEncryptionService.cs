// <copyright file="XorEncryptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// XOR encryption service.
    /// </summary>
    public class XorEncryptionService : IEncryptionService
    {
        private const ulong DEFAULTKEY = 0x0123456789ABCDEF; // Default 64-bit key
        private ulong encryptionKey = DEFAULTKEY;

        /// <summary>
        /// Gets or sets the encryption key.
        /// </summary>
        public ulong EncryptionKey
        {
            get => this.encryptionKey;
            set => this.encryptionKey = value;
        }

        /// <summary>
        /// Encrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> EncryptFileAsync(string filePath)
        {
            return await this.ProcessFileAsync(filePath);
        }

        /// <summary>
        /// Decrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> DecryptFileAsync(string filePath)
        {
            return await this.ProcessFileAsync(filePath);
        }

        /// <summary>
        /// Encrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> EncryptDirectoryAsync(string directoryPath)
        {
            return await this.ProcessDirectoryAsync(directoryPath);
        }

        /// <summary>
        /// Decrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> DecryptDirectoryAsync(string directoryPath)
        {
            return await this.ProcessDirectoryAsync(directoryPath);
        }

        private async Task<int> ProcessFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return -3; // File not found
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                await this.ProcessFileInPlaceAsync(filePath);
                stopwatch.Stop();
                return (int)stopwatch.ElapsedMilliseconds;
            }
            catch (Exception)
            {
                return -4; // General exception
            }
        }

        private async Task<int> ProcessDirectoryAsync(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                return -3; // Directory not found
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // Get all files in the directory and subdirectories
                string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

                // Filter out files that shouldn't be processed
                files = files.Where(f => this.ShouldProcessFile(f)).ToArray();

                // Process each file
                foreach (string file in files)
                {
                    await this.ProcessFileInPlaceAsync(file);
                }

                stopwatch.Stop();
                return (int)stopwatch.ElapsedMilliseconds;
            }
            catch (Exception)
            {
                return -4; // General exception
            }
        }

        private async Task ProcessFileInPlaceAsync(string filePath)
        {
            // Create a temporary file for processing
            string tempFile = Path.GetTempFileName();
            
            try
            {
                // Create a byte array from the key (64 bits = 8 bytes)
                byte[] keyBytes = BitConverter.GetBytes(this.encryptionKey);

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

        private bool ShouldProcessFile(string filePath)
        {
            // Get file extension
            string extension = Path.GetExtension(filePath).ToLower();

            // List of extensions to process
            string[] processableExtensions = new[]
            {
                ".cs", ".js", ".py", ".java", ".cpp", ".h", ".hpp", ".c", ".txt",
                ".json", ".xml", ".html", ".css", ".md", ".sql", ".ts", ".tsx",
                ".jsx", ".vue", ".php", ".rb", ".go", ".rs", ".swift", ".kt",
            };

            // List of directories to skip
            string[] skipDirectories = new[]
            {
                "bin", "obj", "node_modules", ".git", ".vs", "dist", "build",
                "target", "Debug", "Release", "packages",
            };

            // Skip files in excluded directories
            string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
            if (skipDirectories.Any(d => directory.Contains(Path.DirectorySeparatorChar + d + Path.DirectorySeparatorChar)))
            {
                return false;
            }

            // Process only files with specified extensions
            return processableExtensions.Contains(extension);
        }
    }
}