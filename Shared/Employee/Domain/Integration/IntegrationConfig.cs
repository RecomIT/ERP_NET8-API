using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Integration
{
    [Table("HR_IntegrationConfig")]
    public class IntegrationConfig : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } // Workday/ SAP / AZURE
        [StringLength(100)]
        public string GateWay { get; set; } // API/ SMTP/ FTP / Client Server Path
        [StringLength(200)]
        public string DataType { get; set; } // JSON / EXCEL / XML / CSV
        [StringLength(200)]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Passphrase { get; set; }
        [StringLength(50)]
        public string Host { get; set; }
        [StringLength(20)]
        public string Port { get; set; }
        [StringLength(20)]
        public string DataTransferType { get; set; } // PUSH-PULL
        [StringLength(300)]
        public string AuthenticationPublicKey { get; set; }
        [StringLength(300)]
        public string AuthenticationPrivateKey { get; set; }
        public string AuthenticationPrivateKeyFilePath { get; set; } 
        public string CertificateFilePath { get; set; }
        [StringLength(300)]
        public string EncryptionPublicKey { get; set; }
        [StringLength(300)]
        public string EncryptionPrivateKey { get; set; }
        public string EncryptionPrivateKeyFilePath { get; set; }
        [StringLength(300)]
        public string DecryptionPublicKey { get; set; }
        [StringLength(300)]
        public string DecryptionPrivateKey { get; set; }
        public string DecryptionPrivateKeyFilePath { get; set; }
        public ICollection<IntegrationModule> IntegrationModules { get; set; }
    }
}
