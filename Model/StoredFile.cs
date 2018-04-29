using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenVue.Model
{
    public class StoredFile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long FileSize { get; set; }

        public string ParentDirPath { get; set; }

        // file owner
        public string ApplicationUserId { get; set; }

        // file may belong to the group or null (private file)
        public int? GroupId { get; set; }

        // file can belong to filecategory
        public int? FileCategoryId { get; set; }

        // real path
        public string FilePath { get; set; }

        public string Description { get; set; }

        public DateTime UploadedDate { get; set; } = DateTime.Now;
    }
}
