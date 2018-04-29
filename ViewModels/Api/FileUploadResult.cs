using Newtonsoft.Json;

namespace GenVue.ViewModels.Api
{
    public class FileUploadResult
    {
        [JsonProperty("fileId")]
        public string FileId { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }
    }
}
