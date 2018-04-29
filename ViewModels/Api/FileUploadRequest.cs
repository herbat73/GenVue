using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenVue.ViewModels.Api
{
    public class FileUploadRequest
    {
        public IFormFile File { get; set; }
        public int Group { get; set; }
        public int Category { get; set; }
        public string TargertTargetDirectory { get; set; }

        public string Description { get; set; }

    }
}
