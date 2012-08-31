using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel.Helpers
{
    internal class IssueTypeWrapper
    {
        public string DisplayName { get; private set; }
        public IssueType Value { get; private set; }

        public IssueTypeWrapper(IssueType type)
        {
            Value = type;
            switch (type)
            {
                case IssueType.Normal:
                    DisplayName = "Zwykły";
                    break;
                case IssueType.Habilitation:
                    DisplayName = "Praca habilitacyjna";
                    break;
                case IssueType.Conference:
                    DisplayName = "Konferencja";
                    break;
                case IssueType.Monograph:
                    DisplayName = "Monografia";
                    break;
                case IssueType.Session:
                    DisplayName = "Sesja";
                    break;
                case IssueType.Symposium:
                    DisplayName = "Sympozjum naukowe";
                    break;
                default:
                    throw new ArgumentException("The supplied type is unknown.", "type");
            }
        }

        public static ICollection<IssueTypeWrapper> GetAllIssueTypes()
        {
            var result = new List<IssueTypeWrapper>();
            result.Add(new IssueTypeWrapper(IssueType.Normal));
            result.Add(new IssueTypeWrapper(IssueType.Conference));
            result.Add(new IssueTypeWrapper(IssueType.Habilitation));
            result.Add(new IssueTypeWrapper(IssueType.Monograph));
            result.Add(new IssueTypeWrapper(IssueType.Session));
            result.Add(new IssueTypeWrapper(IssueType.Symposium));
            return result;
        }
    }
}
