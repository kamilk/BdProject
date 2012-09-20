using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Authorship
    {
        int ArticleId { get; set; }
        int Number { get; set; }
        int AuthorId { get; set; }
        string InstitutionId { get; set; }
    }
}
