using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    abstract class CentralRepository
    {
        public abstract IEnumerable<Institution> GetInstitutions();
        public abstract IEnumerable<Publisher> GetPublishers();
        public abstract IEnumerable<Publisher> GetPublishersAssignedToInstitution();

        private static CentralRepository _instance;
        public static CentralRepository Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DummyCentralRepository();
                return _instance;
            }
        }
    }
}
