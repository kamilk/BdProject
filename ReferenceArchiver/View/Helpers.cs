using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace ReferenceArchiver.View
{
    static class Helpers
    {
        public static void UpdateTextBinding(this TextBox textBox)
        {
            textBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

    }
}
