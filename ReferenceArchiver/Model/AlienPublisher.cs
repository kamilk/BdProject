using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class AlienPublisher
    {
        public int Id;
        public string Type;
        public string Title;
        public int? YearOfPublication;
        public string Number;
        public string Country;


        public AlienPublisher()
        {
            this.Id = -1;
        }

        public AlienPublisher(int id, string typ, string tytul, int? rok_wyd, string numer, string kraj)
        {
            this.Id = id;
            this.Type = typ;
            this.Title = tytul;
            this.YearOfPublication = rok_wyd;
            this.Number = numer;
            this.Country = kraj;
        }
    }
}
