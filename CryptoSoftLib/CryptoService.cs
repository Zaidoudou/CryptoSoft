using System;
using System.Threading.Tasks;
using CryptoSoftLib.Services;

namespace CryptoSoftLib
{
    /// <summary>
    /// Main service class for encryption and decryption operations.
    /// </summary>
    public class CryptoService
    {
        private readonly IEncryptionService _encryptionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoService"/> class.
        /// </summary>
        public CryptoService()
        {
            _encryptionService = new XorEncryptionService();
        }

        /// <summary>
        /// Sets the encryption key to use for encryption and decryption operations.
        /// </summary>
        /// <param name="key">The encryption key to use.</param>
        public void SetEncryptionKey(ulong key)
        {
            _encryptionService.EncryptionKey = key;
        }

        /// <summary>
        /// Encrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> EncryptFileAsync(string filePath)
        {
            return await _encryptionService.EncryptFileAsync(filePath);
        }

        /// <summary>
        /// Decrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> DecryptFileAsync(string filePath)
        {
            return await _encryptionService.DecryptFileAsync(filePath);
        }

        /// <summary>
        /// Encrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> EncryptDirectoryAsync(string directoryPath)
        {
            return await _encryptionService.EncryptDirectoryAsync(directoryPath);
        }

        /// <summary>
        /// Decrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        public async Task<int> DecryptDirectoryAsync(string directoryPath)
        {
            return await _encryptionService.DecryptDirectoryAsync(directoryPath);
        }
    }
} 