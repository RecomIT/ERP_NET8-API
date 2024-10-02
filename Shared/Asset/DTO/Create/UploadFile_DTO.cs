
namespace Shared.Asset.DTO.Create
{
    public class UploadFile_DTO
    {
        public string Format { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        public string ProductId { get; set; }
        public string Number { get; set; }
        public string PIN { get; set; }
        public string PUK { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public bool Condition { get; set; }
    }
}
