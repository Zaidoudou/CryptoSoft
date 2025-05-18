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

            // Create stopwatch to measure execution time
            var stopwatch = new Stopwatch();

            try
            {
                // Validate arguments
                if (args.Length != 2)
                {
                    Console.Error.WriteLine("Usage: cryptosoft [encrypt|decrypt] file_path");
                    return -1;
                }

                string operation = args[0].ToLower();
                string filePath = args[1];

                // Validate operation
                if (operation != "encrypt" && operation != "decrypt")
                {
                    Console.Error.WriteLine("Invalid operation. Use 'encrypt' or 'decrypt'.");
                    return -2;
                }

                // Validate file
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"File not found: {filePath}");
                    return -3;
                }

                // Get encryption key from configuration
                ulong encryptionKey = configService.GetEncryptionKey();

                // Start timing
                stopwatch.Start();

                // Perform operation
                if (operation == "encrypt")
                {
                    await encryptionService.EncryptFileAsync(filePath, encryptionKey);
                }
                else
                {
                    await encryptionService.DecryptFileAsync(filePath, encryptionKey);
                }

                // Stop timing
                stopwatch.Stop();

                // Return execution time in milliseconds (positive value indicates success)
                return (int)stopwatch.ElapsedMilliseconds;
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
