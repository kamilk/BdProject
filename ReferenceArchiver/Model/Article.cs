using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Artykuł
    /// Assuming that TIMESTAMP will work itself inside the database.
    /// </summary>
    class Article
    {
        public long Id;                                  // id
        public string InstitutionId;                    // id_inst
        public string PublisherId;                      // id_wyd
        public string JournalId;                        // id_serie
        public int? IssueId;                            // id_zeszyty

        public string Title;                            // tytul
        public string TitlePl;                          // tytul_pl
        public int? PageBegin;                          // str_od
        public int? PageEnd;                            // str_do
        public int? AlienId;                            // id_wyd_obce
        public string Lang;                             // jezyk
        public DateTime Time;                             // czas_wpr

        public Article()
        {
            this.Id = -1;
        }


        public Article(long id, string id_inst, string id_wyd, string id_serie, int? id_zeszyty, string tytul,
                       string tytul_pl, int? str_od, int? str_do, int? id_wyd_obce, string jezyk, DateTime czas_wpr)
        {
            this.Id = id;
            this.InstitutionId = id_inst;
            this.PublisherId = id_wyd;
            this.JournalId = id_serie;
            this.IssueId = id_zeszyty;
            this.Title = tytul;
            this.TitlePl = tytul_pl;
            this.PageBegin = str_od;
            this.PageEnd = str_do;
            this.AlienId = id_wyd_obce;
            this.Lang = jezyk;
            this.Time = czas_wpr;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Title);
            if (TitlePl != null)
                builder.AppendFormat(" ({0})", TitlePl);
            return builder.ToString();
        }
    }
}
