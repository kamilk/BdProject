using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    /// <summary>
    /// Zeszyt
    /// </summary>
    class Issue
    {
        public string InstitutionId;                    // id_inst
        public string PublisherId;                      // id_wyd
        public string JournalId;                        // id_serie
        public int? IdWithinJournal;                    // id_w_serii
        public int NumberWithinJournal;                 // nr_w_serii
        public int NumberWithinPublisher;               // nr_w_wydawnictwie

        public string Title;                            // tytul_pl
        public int? YearOfPublication;                  // rok_wydania
        public bool WasVerified;                        // fl_zwer T/N
        public IssueType Type = IssueType.Symposium;    // typ
        public string TypeSave;
        public string TypeNumber;                       // nr_typ

        public Issue()
        {
            IdWithinJournal = -1;
        }

        public Issue(ResearchJournal parentJournal)
        {
            InstitutionId = parentJournal.InstitutionId;
            PublisherId = parentJournal.PublisherId;
            JournalId = parentJournal.IdWithinPublisher;
            IdWithinJournal = -1;
        }

        public Issue(string id_inst, string id_wyd, string id_serie, int? id_w_serii, int nr_w_serii, int nr_w_wydawnictwie,
                        string tytul_pl, int? rok_wydania, bool fl_zwer, string typ, string nr_typ)
        {
            this.InstitutionId = id_inst;
            this.PublisherId = id_wyd;
            this.JournalId = id_serie;
            this.IdWithinJournal = id_w_serii;
            this.NumberWithinJournal = nr_w_serii;
            this.NumberWithinPublisher = nr_w_wydawnictwie;
            this.Title = tytul_pl;
            this.YearOfPublication = rok_wydania;
            this.WasVerified = fl_zwer;
            this.TypeNumber = nr_typ;

            switch (typ)
            {
                case "art": 
                    this.Type = IssueType.Normal;
                    this.TypeSave = typ;
                    break;
                case "hab": 
                    this.Type = IssueType.Habilitation;
                    this.TypeSave = typ;
                    break;
                case "kon": 
                    this.Type = IssueType.Conference;
                    this.TypeSave = typ;
                    break;
                case "mon": 
                    this.Type = IssueType.Monograph;
                    this.TypeSave = typ;
                    break;
                case "ses": 
                    this.Type = IssueType.Session;
                    this.TypeSave = typ;
                    break;
                case "sym": 
                    this.Type = IssueType.Symposium;
                    this.TypeSave = typ;
                    break;
                case "Zwykły": 
                    this.Type = IssueType.Normal;
                    this.TypeSave = "art";
                    break;
                case "Konferencja": 
                    this.Type = IssueType.Conference;
                    this.TypeSave = "kon";
                    break;
                case "Praca habilitacyjna": 
                    this.Type = IssueType.Habilitation;
                    this.TypeSave = "hab";
                    break;
                case "Monografia": 
                    this.Type = IssueType.Monograph;
                    this.TypeSave = "mon";
                    break;
                case "Sesja": 
                    this.Type = IssueType.Session;
                    this.TypeSave = "ses";
                    break;
                case "Sympozjum naukowe": 
                    this.Type = IssueType.Symposium;
                    this.TypeSave = "sym";
                    break;
                default:
                    //error....
                    break;
            }
        }
    }
}
