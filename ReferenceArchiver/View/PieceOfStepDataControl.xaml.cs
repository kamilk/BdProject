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

namespace ReferenceArchiver.View
{
    /// <summary>
    /// Interaction logic for PieceOfStepDataControl.xaml
    /// </summary>
    public partial class PieceOfStepDataControl : UserControl
    {
        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register("Property", typeof(string), typeof(PieceOfStepDataControl));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(PieceOfStepDataControl));

        public string Property
        {
            get { return (string)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public PieceOfStepDataControl()
        {
            InitializeComponent();
        }
    }
}
