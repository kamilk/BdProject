using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReferenceArchiver.Model
{
    class Category
    {
        public int Id;
        public string Info;
        public int? AboveCategory;

        public Category()
        {
            Id = -1;
        }

        public Category(int id, string info, int? above_cat)
        {
            this.Id = id;
            this.Info = info;
            this.AboveCategory = above_cat;
        }
    }  
}
