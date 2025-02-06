# SecurePreferences

**SecurePreferences** is a lightweight .NET library for securely storing application preferences using multiple encryption algorithms. This library is designed to protect sensitive configuration data and offers robust encryption, easy integration, and comprehensive testing support.

---

## Features

- **Encryption Support**: AES, RSA, and customizable encryption algorithms.
- **Key Management**: Securely handle encryption keys.
- **Easy Integration**: Simple API for saving and retrieving encrypted preferences.
- **Cross-Platform**: Compatible with .NET (Windows, macOS, Linux).
- **Extensive Testing**: Includes unit tests for reliability and correctness.

---

## Installation

Add the library to your project using NuGet:

```bash
dotnet add package SecurePreferences
```
---

# Usage

## 1. Obtaining an encryption algorithm instancea
You must pass an instance of the encryption algorithm you want to use. Either AES or RSA.
An algorthim instance can be obtained by using the Encryption static class.

```csharp
var aesAlgorithm = Encryption.AES;

// Alternatively
// var rsaAlgorithm = Encryption.RSA;
```

NB: If you choose to use RSA algorithm. You can get your public and private keys by calling these methods:

```csharp
var publicKey = rsaAlgorithm.ExportPublicKey();
var privateKey = rsaAlgorithm.ExportPrivateKey();
```
---

## 2. Setting Up SecureStorage
```csharp
using SecurePreferences;

// Initialize SecureStorage with AES encryption
var securePrefs = new SecureStorage(Encryption.AES);

// Alternatively, use RSA encryption
// var securePrefs = new SecureStorage(Encryption.RSA);
```

## 2. Saving Preferences

```csharp
// Save a preference with a key and value
securePrefs.Save("your-encryption-key", "Username", "SecureUser123");

// Save more sensitive data
securePrefs.Save("your-encryption-key", "ApiToken", "very-secret-token");
```

## 3. Retrieving Preferences
```csharp
// Retrieve the saved value
string username = securePrefs.Get("your-decryption-key", "Username");
Console.WriteLine($"Retrieved Username: {username}");

// Retrieve sensitive data
string apiToken = securePrefs.Get("your-decryption-key", "ApiToken");
Console.WriteLine($"Retrieved ApiToken: {apiToken}");
```

## 4. Deleting Preferences

```csharp
// Remove a specific key-value pair
securePrefs.Remove("Username");

// Clear all stored preferences
securePrefs.Clear();
```

---

# Supported Encryption Algorithms
- **AES**: Symmetric encryption for secure storage.
- **RSA**: Asymmetric encryption for scenarios requiring key pairs.
- **Custom**: Define your own encryption algorithm using the extensible interface.

---

# Example: Full Workflow
```csharp
using SecurePreferences;

class Program
{
    static void Main()
    {
        // Initialize SecureStorage with AES encryption
        var securePrefs = new SecureStorage(EncryptionAlgorithm.AES);

        // Save preferences
        securePrefs.Save("your-encryption-key", "Email", "user@example.com");
        securePrefs.Save("your-encryption-key", "Password", "secure-password");

        // Retrieve preferences
        string email = securePrefs.Get("your-decryption-key", "Email");
        Console.WriteLine($"Email: {email}");

        // Delete a preference
        securePrefs.Remove("Email");

        // Clear all preferences
        securePrefs.Clear();
    }
}

```

---

# Advanced Configuration
## Custom Encryption Algorithm

Implement your own encryption logic by extending the `IEncryptionProvider` interface.

```csharp
public class CustomEncryption : IEncryptionProvider
{
    public string Encrypt(string value, string encryptionKey)
    {
        // Custom encryption logic
    }

    public string Decrypt(string encryptedData, string decryptionKey)
    {
        // Custom decryption logic
    }
}

```

Pass your custom provider to `SecureStorage`:

```csharp
var securePrefs = new SecureStorage(new CustomEncryption());
```

# Contributing
We welcome contributions! To contribute:

1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Submit a pull request.

# License
This project is licensed under the `MIT` License. See the LICENSE file for details.

# Contact
For questions or feedback, reach out to William Chavula at quivr.developers@gmail.com.


### Key Points Covered:
1. **Installation**: Instructions for NuGet package installation.
2. **Usage**: Examples of initializing, saving, retrieving, and deleting preferences.
3. **Advanced Features**: Support for custom encryption algorithms.
4. **Testing**: Guidance on running unit tests.
5. **Contributing**: Encouragement for community contributions.
6. **License**: Placeholder for the license.

Let me know if you'd like any additional sections or customization!
