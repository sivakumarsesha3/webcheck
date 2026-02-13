using System;
using System.Runtime.InteropServices;

namespace PortfolioVault.Infrastructure.Security
{
    // DPAPI wrapper via native Windows API, does not rely on ProtectedData type.
    public static class DpapiEncryptionService
    {
        private const int CRYPTPROTECT_UI_FORBIDDEN = 0x1;
        private const int CRYPTPROTECT_LOCAL_MACHINE = 0x4;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct DATA_BLOB
        {
            public int cbData;
            public IntPtr pbData;
        }

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CryptProtectData(
            ref DATA_BLOB pDataIn,
            string? szDataDescr,
            IntPtr pOptionalEntropy,
            IntPtr pvReserved,
            IntPtr pPromptStruct,
            int dwFlags,
            ref DATA_BLOB pDataOut);

        [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool CryptUnprotectData(
            ref DATA_BLOB pDataIn,
            out string? ppszDataDescr,
            IntPtr pOptionalEntropy,
            IntPtr pvReserved,
            IntPtr pPromptStruct,
            int dwFlags,
            ref DATA_BLOB pDataOut);

        [DllImport("kernel32.dll")]
        private static extern IntPtr LocalFree(IntPtr hMem);

        public static byte[] Protect(byte[] data)
        {
            if (data == null || data.Length == 0)
                return Array.Empty<byte>();

            var inputBlob = new DATA_BLOB();
            var outputBlob = new DATA_BLOB();

            try
            {
                inputBlob.pbData = Marshal.AllocHGlobal(data.Length);
                inputBlob.cbData = data.Length;
                Marshal.Copy(data, 0, inputBlob.pbData, data.Length);

                if (!CryptProtectData(
                        ref inputBlob,
                        null,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        CRYPTPROTECT_UI_FORBIDDEN,
                        ref outputBlob))
                {
                    throw new InvalidOperationException("CryptProtectData failed.");
                }

                var encrypted = new byte[outputBlob.cbData];
                Marshal.Copy(outputBlob.pbData, encrypted, 0, outputBlob.cbData);
                return encrypted;
            }
            finally
            {
                if (inputBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(inputBlob.pbData);

                if (outputBlob.pbData != IntPtr.Zero)
                    LocalFree(outputBlob.pbData);
            }
        }

        public static byte[] Unprotect(byte[] encryptedData)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                return Array.Empty<byte>();

            var inputBlob = new DATA_BLOB();
            var outputBlob = new DATA_BLOB();

            try
            {
                inputBlob.pbData = Marshal.AllocHGlobal(encryptedData.Length);
                inputBlob.cbData = encryptedData.Length;
                Marshal.Copy(encryptedData, 0, inputBlob.pbData, encryptedData.Length);

                if (!CryptUnprotectData(
                        ref inputBlob,
                        out _,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        IntPtr.Zero,
                        CRYPTPROTECT_UI_FORBIDDEN,
                        ref outputBlob))
                {
                    throw new InvalidOperationException("CryptUnprotectData failed.");
                }

                var decrypted = new byte[outputBlob.cbData];
                Marshal.Copy(outputBlob.pbData, decrypted, 0, outputBlob.cbData);
                return decrypted;
            }
            finally
            {
                if (inputBlob.pbData != IntPtr.Zero)
                    Marshal.FreeHGlobal(inputBlob.pbData);

                if (outputBlob.pbData != IntPtr.Zero)
                    LocalFree(outputBlob.pbData);
            }
        }
    }
}
