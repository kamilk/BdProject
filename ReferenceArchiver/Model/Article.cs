using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// An entity for data of the database table Artykuly.
    /// </summary>
    class Article
    {
        public long Id { get; set; }                                  // id
        public string InstitutionId { get; set; }                    // id_inst
        public string PublisherId { get; set; }                      // id_wyd
        public string JournalId { get; set; }                        // id_serie
        public int? IssueId { get; set; }                            // id_zeszyty

        public string Title { get; set; }                            // tytul
        public string TitlePl { get; set; }                          // tytul_pl
        public int? PageBegin { get; set; }                          // str_od
        public int? PageEnd { get; set; }                            // str_do
        public int? AlienId { get; set; }                            // id_wyd_obce
        public string Lang { get; set; }                             // jezyk
        public DateTime Time { get; set; }                           // czas_wpr

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

        // USE ONLY WHEN SAVING ARTICLE, ALL GET() REQUIRE THE ABOVE ONE FOR TIMESTAMP
        public Article(long id, string id_inst, string id_wyd, string id_serie, int? id_zeszyty, string tytul,
                       string tytul_pl, int? str_od, int? str_do, int? id_wyd_obce, string jezyk)
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
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(Title);
            if (TitlePl != null)
                builder.AppendFormat(" ({0})", TitlePl);
            return builder.ToString();
        }

        /// <summary>
        /// Sets the article ids so that the article belongs to the issue.
        /// </summary>
        /// <param name="issue">The issue.</param>
        public void SetIssue(Issue issue)
        {
            InstitutionId = issue.InstitutionId;
            PublisherId = issue.PublisherId;
            JournalId = issue.JournalId;
            IssueId = issue.IdWithinJournal;

            AlienId = null;
        }
    }
}
