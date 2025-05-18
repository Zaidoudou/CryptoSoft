// <copyright file="IEncryptionService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
  using System.Threading.Tasks;

  /// <summary>
  /// Interface for encryption service.
  /// </summary>
  public interface IEncryptionService
  {
      /// <summary>
      /// Encrypts a file in place.
      /// </summary>
      /// <param name="filePath">The path to the file to encrypt.</param>
      /// <param name="key">The encryption key.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      Task EncryptFileAsync(string filePath, ulong key);

      /// <summary>
      /// Decrypts a file in place.
      /// </summary>
      /// <param name="filePath">The path to the file to decrypt.</param>
      /// <param name="key">The decryption key.</param>
      /// <returns>A task representing the asynchronous operation.</returns>
      Task DecryptFileAsync(string filePath, ulong key);
  }
}