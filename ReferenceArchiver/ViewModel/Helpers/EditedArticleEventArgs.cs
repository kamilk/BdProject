using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReferenceArchiver.Model;

namespace ReferenceArchiver.ViewModel.Helpers
{
    class EditedArticleEventArgs : EventArgs
    {
        public Article Article { get; private set; }

        public EditedArticleEventArgs(Article article)
        {
            Article = article;
        }
    }
}
