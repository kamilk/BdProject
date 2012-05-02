using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Publisher
    {
        public string Title { get; set; }

        public Publisher()
        { }

        public Publisher(string title)
        {
            this.Title = title;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
