// <copyright file="IEncryptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
  using System.Threading.Tasks;

  /// <summary>
  /// Interface for encryption services.
  /// </summary>
  public interface IEncryptionService
  {
      /// <summary>
      /// Process a file using encryption/decryption.
      /// </summary>
      /// <param name="sourceFilePath">The path to the source file.</param>
      /// <param name="destinationFilePath">The path to the destination file.</param>
      /// <param name="key">The key to use for encryption/decryption.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      Task ProcessFileAsync(string sourceFilePath, string destinationFilePath, ulong key);
  }
}