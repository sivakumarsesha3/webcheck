using System.Security.Cryptography;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Infrastructure.Security;

public sealed class DpapiEncryptionService : IEncryptionService
{
    public byte[] Protect(byte[] plaintext) => ProtectedData.Protect(plaintext, null, DataProtectionScope.CurrentUser);

    public byte[] Unprotect(byte[] cipherText) => ProtectedData.Unprotect(cipherText, null, DataProtectionScope.CurrentUser);
}
