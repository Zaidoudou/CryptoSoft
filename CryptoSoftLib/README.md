# CryptoSoftLib

A lightweight library for file encryption and decryption using XOR encryption. This library provides both single-file and batch encryption capabilities, making it perfect for securing individual files or entire projects.

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
  - Returns execution time in milliseconds
  - Provides error codes for failure cases

## Installation

```bash
dotnet add package CryptoSoftLib
```

## Usage

```csharp
using CryptoSoftLib;

// Create an instance of the service
var cryptoService = new CryptoService();

// Optional: Set a custom encryption key
cryptoService.SetEncryptionKey(0x0123456789ABCDEF);

// Encrypt a file
int result = await cryptoService.EncryptFileAsync("path/to/file.txt");
if (result > 0)
{
    Console.WriteLine($"File encrypted successfully in {result}ms");
}
else
{
    Console.WriteLine($"Error encrypting file: {result}");
}

// Decrypt a file
result = await cryptoService.DecryptFileAsync("path/to/file.txt");
if (result > 0)
{
    Console.WriteLine($"File decrypted successfully in {result}ms");
}
else
{
    Console.WriteLine($"Error decrypting file: {result}");
}

// Encrypt a directory
result = await cryptoService.EncryptDirectoryAsync("path/to/directory");
if (result > 0)
{
    Console.WriteLine($"Directory encrypted successfully in {result}ms");
}
else
{
    Console.WriteLine($"Error encrypting directory: {result}");
}
```

## Return Codes

- Positive number: Success (execution time in milliseconds)
- -1: Invalid argument count
- -2: Invalid operation
- -3: File/folder not found
- -4: General exception

## Supported File Types

The library processes the following file types:
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

## License

This project is licensed under the MIT License - see the LICENSE file for details. 