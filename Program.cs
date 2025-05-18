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
                    Console.Error.WriteLine("Usage: cryptosoft source_file destination_file");
                    return -1;
                }

                string sourceFile = args[0];
                string destinationFile = args[1];

                // Validate source file
                if (!File.Exists(sourceFile))
                {
                    Console.Error.WriteLine($"Source file not found: {sourceFile}");
                    return -2;
                }

                // Get encryption key from configuration
                ulong encryptionKey = configService.GetEncryptionKey();

                // Start timing
                stopwatch.Start();

                // Perform encryption
                await encryptionService.ProcessFileAsync(sourceFile, destinationFile, encryptionKey);

                // Stop timing
                stopwatch.Stop();

                // Return execution time in milliseconds (positive value indicates success)
                return (int)stopwatch.ElapsedMilliseconds;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return -3; // Error code for general exceptions
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
