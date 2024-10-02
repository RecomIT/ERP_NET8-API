using iText.Kernel.Pdf;
using System.IO;
using System.Text;

namespace Shared.Services
{
    public static class FileProtection
    {
        public static byte[] Protected(byte[] fileBytes, string password)
        {
            using (MemoryStream memoryStream = new MemoryStream(fileBytes)) {
                // Create a new MemoryStream to hold the encrypted PDF content
                using (MemoryStream encryptedMemoryStream = new MemoryStream()) {
                    PdfWriter writer = null;

                    if (string.IsNullOrEmpty(password)) {
                        writer = new PdfWriter(encryptedMemoryStream);
                    }
                    else {
                        writer = new PdfWriter(encryptedMemoryStream, new WriterProperties().SetStandardEncryption(
                            Encoding.ASCII.GetBytes(password),
                            Encoding.ASCII.GetBytes(password),
                            EncryptionConstants.ALLOW_PRINTING,
                            EncryptionConstants.ENCRYPTION_AES_256));
                    }

                    PdfDocument pdfDocument = new PdfDocument(writer);

                    // Create a new document
                    using (PdfReader pdfReader = new PdfReader(memoryStream)) {
                        using (PdfDocument originalPdfDocument = new PdfDocument(pdfReader)) {
                            originalPdfDocument.CopyPagesTo(1, originalPdfDocument.GetNumberOfPages(), pdfDocument);
                        }
                    }

                    // Close the document before returning the encrypted PDF bytes
                    pdfDocument.Close();
                    // Return the encrypted PDF bytes
                    return encryptedMemoryStream.ToArray();
                }
            }
        }
    }
}
