namespace SecurePreferences.interfaces
{
    public interface IRSAEncryptionProvider : IEncryptionProvider
    {
        string ExportPublicKey();
        string ExportPrivateKey();
    }
}
