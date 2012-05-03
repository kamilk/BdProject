using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for StepDataControl.xaml
    /// </summary>
    [ContentProperty("Items")]
    public partial class StepDataControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(StepDataControl));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private List<Control> _items;
        public List<Control> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (value == null)
                    _items = new List<Control>();
                else
                    _items = value;
            }
        }

        public object ParentDataContext
        {
            get;
            set;
        }

        public StepDataControl()
        {
            InitializeComponent();
            this.ParentDataContext = this.DataContext;
            this.DataContext = this;
            this.Items = null;
        }
    }
}
