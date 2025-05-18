# CryptoSoft

A lightweight, cross-platform file encryption tool written in C#. CryptoSoft provides both single-file and batch encryption capabilities, making it perfect for securing individual files or entire projects.

## Features

- **Cross-Platform**: Works on Windows, macOS, and Linux
- **Dual Operation Modes**:
  - Single file encryption/decryption
  - Recursive folder encryption/decryption
- **Smart File Processing**:
  - Automatically detects file vs folder paths
  - Skips binary files and build artifacts
  - Processes only relevant source code files
- **Progress Reporting**:
  - Shows number of files to process
  - Displays real-time progress
  - Reports success/failure for each operation
- **Configurable Encryption**:
  - Custom encryption key support
  - Default key fallback
  - Key configuration via appsettings.json

## Supported File Types

CryptoSoft processes the following file types:
- `.cs` - C# source files
- `.js` - JavaScript files
- `.py` - Python files
- `.java` - Java source files
- `.cpp`, `.h`, `.hpp`, `.c` - C/C++ files
- `.txt` - Text files
- `.json` - JSON files
- `.xml` - XML files
- `.html`, `.css` - Web files
- `.md` - Markdown files
- `.sql` - SQL files
- `.ts`, `.tsx` - TypeScript files
- `.jsx` - React JSX files
- `.vue` - Vue.js files
- `.php` - PHP files
- `.rb` - Ruby files
- `.go` - Go files
- `.rs` - Rust files
- `.swift` - Swift files
- `.kt` - Kotlin files

## Excluded Directories

The following directories are automatically skipped during batch processing:
- `bin`
- `obj`
- `node_modules`
- `.git`
- `.vs`
- `dist`
- `build`
- `target`
- `Debug`
- `Release`
- `packages`

## Installation

1. Download the latest release for your platform
2. Extract the files to your desired location
3. Make sure you have the .NET 9.0 runtime installed

## Configuration

Create an `appsettings.json` file in the same directory as the executable with the following structure:

```json
{
  "EncryptionSettings": {
    "Key": 12345678901234567890
  }
}
```

You can use either:
- Decimal format: `"Key": 12345678901234567890`
- Hexadecimal format: `"Key": "0x0123456789ABCDEF"`

If no key is specified, a default key will be used.

## Usage

### Single File Encryption/Decryption

```bash
cryptosoft [encrypt|decrypt] path/to/file
```

Example:
```bash
cryptosoft encrypt myfile.cs
cryptosoft decrypt myfile.cs
```

### Batch Folder Encryption/Decryption

```bash
cryptosoft [encrypt|decrypt] path/to/folder
```

Example:
```bash
cryptosoft encrypt ./myproject
cryptosoft decrypt ./myproject
```

## Return Codes

- Positive number: Success (execution time in milliseconds)
- -1: Invalid argument count
- -2: Invalid operation
- -3: File/folder not found
- -4: General exception

## Building from Source

1. Clone the repository
2. Navigate to the project directory
3. Run `dotnet build`
4. Find the executable in the `bin` directory

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

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
