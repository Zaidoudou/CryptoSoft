using System;
using System.Threading.Tasks;
using CryptoSoft.Services;

namespace YourNamespace
{
    public class CryptoService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IConfigurationService _configService;

        public CryptoService()
        {
            // Create instances of the services from the DLL
            _encryptionService = new XorEncryptionService();
            _configService = new ConfigurationService(null); // We'll handle the key directly

            // Set default encryption key
            _encryptionService.EncryptionKey = 0x0123456789ABCDEF;
        }

        public async Task<int> EncryptFileAsync(string filePath)
        {
            return await _encryptionService.EncryptFileAsync(filePath);
        }

        public async Task<int> DecryptFileAsync(string filePath)
        {
            return await _encryptionService.DecryptFileAsync(filePath);
        }

        public async Task<int> EncryptDirectoryAsync(string directoryPath)
        {
            return await _encryptionService.EncryptDirectoryAsync(directoryPath);
        }

        public async Task<int> DecryptDirectoryAsync(string directoryPath)
        {
            return await _encryptionService.DecryptDirectoryAsync(directoryPath);
        }

        public void SetEncryptionKey(ulong key)
        {
            _encryptionService.EncryptionKey = key;
        }
    }
} 