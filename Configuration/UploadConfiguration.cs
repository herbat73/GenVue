using Newtonsoft.Json;

namespace GenVue.Configuration
{
    public class UploadConfiguration
    {
        public string UploadDirectory { get; set; } = "UploadedFiles";

        public string PrivateRoot { get; set; } = "_privateFiles";
        public string GroupRoot { get; set; } = "_groupsFiles";

        public int UserMaxConcurrentUploads { get; set; } = 4;

        /// <summary>
        /// Max file storage quota (in bytes)
        /// </summary>
        // (1 GiB NOT 1 GB [see GIBIBYTES]). 1073741824 bytes
        public long? DefaultQuota { get; set; } = 1 << 30;
        
    }
}
