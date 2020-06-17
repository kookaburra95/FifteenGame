using System;
using System.Windows;

namespace Fifteen
{
    public partial class NameWindow : Window
    {
        public string UserName => tbName.Text;

        public NameWindow()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(UserName))
            {
                this.DialogResult = true;
            }
        }
    }
}   
    