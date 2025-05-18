using System;
using Microsoft.Extensions.Configuration;

namespace CryptoSoft.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;
        private const ulong DEFAULT_KEY = 0x0123456789ABCDEF; // Default 64-bit key

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public ulong GetEncryptionKey()
        {
            string? keyString = _configuration["EncryptionSettings:Key"];
            
            // If no key is specified in the config, use default key
            if (string.IsNullOrEmpty(keyString))
            {
                return DEFAULT_KEY;
            }
            
            // Try parsing the key as a hexadecimal value
            if (keyString.StartsWith("0x") && ulong.TryParse(keyString.Substring(2), 
                System.Globalization.NumberStyles.HexNumber, 
                null, out ulong hexKey))
            {
                return hexKey;
            }
            
            // Try parsing the key as a decimal value
            if (ulong.TryParse(keyString, out ulong decimalKey))
            {
                return decimalKey;
            }
            
            // If parsing fails, use default key
            return DEFAULT_KEY;
        }
    }
}