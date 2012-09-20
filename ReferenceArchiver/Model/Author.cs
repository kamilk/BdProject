using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Autor
    /// </summary>
    class Author
    {
        public int Id;
        public string LastName;
        public string Name;
        public string Name2;
        public string Nationality;

        public Author(int id, string nazwisko, string imie, string imie2, string narodowosc)
        {
            this.Id = id;
            this.LastName = nazwisko;
            this.Name = imie;
            this.Name2 = imie2;
            this.Nationality = narodowosc;
        }

        public override string ToString()
        {
            if (Name2 != null)
                return string.Format("{0} {1} {2}", Name, Name2, LastName);
            else
                return string.Format("{0} {1}", Name, LastName);
        }
    }
}
