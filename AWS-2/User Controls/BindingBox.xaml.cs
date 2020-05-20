using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AWS_2.User_Controls
{
    /// <summary>
    /// Interaction logic for BindingBox.xaml
    /// </summary>
    public partial class BindingBox : UserControl
    {
        public BindingBox()
        {
            InitializeComponent();
        }

        public BindingBox(double width, double height, double top, double left, string data,bool isLine)
        {
            InitializeComponent();
            this.VerticalAlignment = VerticalAlignment.Top;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.Width = width;
            this.Height = height;
            txtLabel.Text = data;
            Thickness margin = this.Margin;
            margin.Left = left;
            margin.Top = top;
            
            if (isLine)
            {
                bFrame.BorderBrush = Brushes.Red;
            }

            this.Margin = margin;
        }
    }
}
