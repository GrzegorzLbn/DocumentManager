using System;
using System.Collections.Generic;

namespace DocumentsManager.Models
{
    public class DocumentHeader
    {
        public const string TableName = "DocumentHeader";

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ClientNumber { get; set; }
        public string Name { get; set; }
        public float NetPrice { get; set; }
        public float GrossPrice { get; set; }

        public List<DocumentItem> DocumentItems { get; set; }
    }
}
