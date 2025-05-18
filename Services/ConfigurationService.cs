// <copyright file="ConfigurationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration service for the application.
    /// </summary>
    public class ConfigurationService : IConfigurationService
    {
        private const ulong DEFAULTKEY = 0x0123456789ABCDEF; // Default 64-bit key
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationService"/> class.
        /// </summary>
        /// <param name="iConfiguration">The configuration.</param>
        public ConfigurationService(IConfiguration iConfiguration)
        {
            this.configuration = iConfiguration;
        }

        /// <summary>
        /// Get the encryption key.
        /// </summary>
        /// <returns>The encryption key.</returns>
        public ulong GetEncryptionKey()
        {
            string? keyString = this.configuration["EncryptionSettings:Key"];

            // If no key is specified in the config, use default key
            if (string.IsNullOrEmpty(keyString))
            {
                return DEFAULTKEY;
            }

            // Try parsing the key as a hexadecimal value
            if (keyString.StartsWith("0x") && ulong.TryParse(
                keyString.Substring(2),
                System.Globalization.NumberStyles.HexNumber,
                null,
                out ulong hexKey))
            {
                return hexKey;
            }

            // Try parsing the key as a decimal value
            if (ulong.TryParse(keyString, out ulong decimalKey))
            {
                return decimalKey;
            }

            // If parsing fails, use default key
            return DEFAULTKEY;
        }
    }
}