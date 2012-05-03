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
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", 
            typeof(List<Control>), 
            typeof(StepDataControl), 
            new PropertyMetadata(new List<Control>()), 
            new ValidateValueCallback(ValidateItems));

        private static bool ValidateItems(object value)
        {
            var list = value as List<Control>;
            return list != null;
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public List<Control> Items
        {
            get { return (List<Control>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public StepDataControl()
        {
            InitializeComponent();
            this.Items = new List<Control>();
        }
    }
}
