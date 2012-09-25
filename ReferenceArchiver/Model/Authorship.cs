using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    public class Authorship
    {
        public long ArticleId { get; set; }
        public int Number { get; set; }
        public int AuthorId { get; set; }
        public string InstitutionId { get; set; }
    }
}
