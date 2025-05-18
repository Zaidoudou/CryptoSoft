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

                // Start timing
                stopwatch.Start();

                // Check if path is a file or directory
                if (File.Exists(path))
                {
                    // Process single file
                    await ProcessFileAsync(encryptionService, operation, path, encryptionKey);
                }
                else if (Directory.Exists(path))
                {
                    // Process directory recursively
                    await ProcessDirectoryAsync(encryptionService, operation, path, encryptionKey);
                }
                else
                {
                    Console.Error.WriteLine($"Path not found: {path}");
                    return -3;
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

        private static async Task ProcessFileAsync(IEncryptionService encryptionService, string operation, string filePath, ulong key)
        {
            if (operation == "encrypt")
            {
                await encryptionService.EncryptFileAsync(filePath, key);
            }
            else
            {
                await encryptionService.DecryptFileAsync(filePath, key);
            }
        }

        private static async Task ProcessDirectoryAsync(IEncryptionService encryptionService, string operation, string directoryPath, ulong key)
        {
            // Get all files in the directory and subdirectories
            string[] files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);

            // Filter out files that shouldn't be encrypted
            files = files.Where(f => ShouldProcessFile(f)).ToArray();

            Console.WriteLine($"Found {files.Length} files to process...");

            // Process each file
            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];
                try
                {
                    Console.Write($"Processing {i + 1}/{files.Length}: {file}... ");
                    await ProcessFileAsync(encryptionService, operation, file, key);
                    Console.WriteLine("Done");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed: {ex.Message}");
                }
            }
        }

        private static bool ShouldProcessFile(string filePath)
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
