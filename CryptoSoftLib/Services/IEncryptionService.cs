using System.Threading.Tasks;

namespace CryptoSoftLib.Services
{
    /// <summary>
    /// Interface for encryption service.
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Gets or sets the encryption key.
        /// </summary>
        ulong EncryptionKey { get; set; }

        /// <summary>
        /// Encrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        Task<int> EncryptFileAsync(string filePath);

        /// <summary>
        /// Decrypts a file in place.
        /// </summary>
        /// <param name="filePath">The path to the file to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        Task<int> DecryptFileAsync(string filePath);

        /// <summary>
        /// Encrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to encrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        Task<int> EncryptDirectoryAsync(string directoryPath);

        /// <summary>
        /// Decrypts all files in a directory recursively.
        /// </summary>
        /// <param name="directoryPath">The path to the directory to decrypt.</param>
        /// <returns>A task representing the asynchronous operation. Returns execution time in milliseconds on success, negative value on failure.</returns>
        Task<int> DecryptDirectoryAsync(string directoryPath);
    }
} 