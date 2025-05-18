// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using CryptoSoft.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The exit code.</returns>
        public static async Task<int> Main(string[] args)
        {
            // Set up dependency injection
            var serviceProvider = ConfigureServices();
            var encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
            var configService = serviceProvider.GetRequiredService<IConfigurationService>();

            try
            {
                // Validate arguments
                if (args.Length != 2)
                {
                    Console.Error.WriteLine("Usage: cryptosoft [encrypt|decrypt] [file_path|folder_path]");
                    return -1;
                }

                string operation = args[0].ToLower();
                string path = args[1];

                // Validate operation
                if (operation != "encrypt" && operation != "decrypt")
                {
                    Console.Error.WriteLine("Invalid operation. Use 'encrypt' or 'decrypt'.");
                    return -2;
                }

                // Get encryption key from configuration
                ulong encryptionKey = configService.GetEncryptionKey();
                encryptionService.EncryptionKey = encryptionKey;

                // Check if path is a file or directory
                if (File.Exists(path))
                {
                    // Process single file
                    if (operation == "encrypt")
                    {
                        return await encryptionService.EncryptFileAsync(path);
                    }
                    else
                    {
                        return await encryptionService.DecryptFileAsync(path);
                    }
                }
                else if (Directory.Exists(path))
                {
                    // Process directory recursively
                    if (operation == "encrypt")
                    {
                        return await encryptionService.EncryptDirectoryAsync(path);
                    }
                    else
                    {
                        return await encryptionService.DecryptDirectoryAsync(path);
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Path not found: {path}");
                    return -3;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return -4; // Error code for general exceptions
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add configuration service
            services.AddSingleton<IConfigurationService, ConfigurationService>();

            // Add encryption service
            services.AddSingleton<IEncryptionService, XorEncryptionService>();

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            return services.BuildServiceProvider();
        }
    }
}
