// <copyright file="IConfigurationService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CryptoSoft.Services
{
  /// <summary>
  /// Interface for configuration services.
  /// </summary>
  public interface IConfigurationService
  {
      /// <summary>
      /// Get the encryption key.
      /// </summary>
      /// <returns>The encryption key.</returns>
        ulong GetEncryptionKey();
    }
}