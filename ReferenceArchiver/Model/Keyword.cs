using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Keyword
    {
        // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
        public string Key;
        public int ArticleId;
        public string Country;

        public Keyword()
        {
            this.ArticleId = -1;
        }

        public Keyword(string klucz, int artykuly_id = -1, string kraj = "")
        {
            // WSZYSTKIE SLOWA KLUCZE PISZEMY MALYMI
            this.Key = klucz;
            this.ArticleId = artykuly_id;
            this.Country = kraj;
        }
    }
}
