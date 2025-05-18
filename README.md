# CryptoSoft

A cross-platform file encryption/decryption tool using XOR bit-by-bit encryption with a 64-bit key. This project is designed to be used both as a standalone command-line application and as a service within the EasySave application.

## Features

- XOR bit-by-bit encryption/decryption with 64-bit key
- Cross-platform (Windows, macOS, Linux)
- Command-line interface
- Service integration capability for the EasySave application
- Configuration of encryption key via settings file

## Solution Structure

- **CryptoSoft** - Console application (.NET 9.0)
- **CryptoSoftLib** - Class library for use as a service in other applications

## Requirements

- .NET 9.0 SDK or later

## Getting Started

### Building the Solution

```bash
# Clone the repository
git clone [repository-url]
cd CryptoSoft

# Build the solution
dotnet build

# Publish for your platform
dotnet publish -c Release -r win-x64 --self-contained true
# Or for macOS
dotnet publish -c Release -r osx-x64 --self-contained true
# Or for Linux
dotnet publish -c Release -r linux-x64 --self-contained true
```

### Running the Command-Line Application

```bash
# Basic usage
cryptosoft source_file destination_file

# On Linux/macOS you may need to use
./cryptosoft source_file destination_file
```

### Return Codes

- Positive value: Time taken for encryption in milliseconds (success)
- Negative value: Error code indicating failure

### Configuration

The encryption key can be configured in the `appsettings.json` file:

```json
{
  "EncryptionSettings": {
    "Key": "0x0123456789ABCDEF"
  }
}
```

You can specify the key in hexadecimal (with 0x prefix) or decimal format.

## Using CryptoSoftLib in Your Projects

To use CryptoSoft as a service in your application:

1. Add a reference to the CryptoSoftLib project or DLL
2. Use the `ICryptoService` interface to perform encryption/decryption

Example:

```csharp
using CryptoSoftLib;

// Create an instance of the crypto service
ICryptoService cryptoService = new CryptoService();

// Optional: Set a custom encryption key
cryptoService.SetEncryptionKey(0x0123456789ABCDEF);

// Encrypt/decrypt a file
int result = await cryptoService.ProcessFileAsync("source.txt", "encrypted.txt");
if (result > 0)
{
    // Success - result contains the time taken in milliseconds
    Console.WriteLine($"File processed in {result}ms");
}
else
{
    // Failure - result contains the error code
    Console.WriteLine($"Error processing file: {result}");
}
```

## Integration with EasySave

To integrate CryptoSoft with your EasySave application:

1. Add a reference to the CryptoSoftLib project
2. Inject the `ICryptoService` where needed
3. Call the service methods to encrypt/decrypt files during the save process

## Development Guide

### Cross-Platform Considerations

- Use `Path.Combine()` instead of string concatenation for file paths
- Be mindful of file permissions on different operating systems
- Test on all target platforms (Windows, macOS, Linux)

### Adding Unit Tests

To add unit tests for the application:

```bash
# Create a test project
dotnet new xunit -n CryptoSoft.Tests
dotnet add CryptoSoft.Tests/CryptoSoft.Tests.csproj reference CryptoSoft/CryptoSoft.csproj
dotnet add CryptoSoft.Tests/CryptoSoft.Tests.csproj reference CryptoSoftLib/CryptoSoftLib.csproj
```
