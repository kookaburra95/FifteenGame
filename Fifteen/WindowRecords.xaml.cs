using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Fifteen.Services;

namespace Fifteen
{
    /// <summary>
    /// Interaction logic for WindowRecords.xaml
    /// </summary>
    public partial class WindowRecords : Window
    {
        private readonly string PATH = $"{Environment.CurrentDirectory}\\recordsList.json";
        private BindingList<Records> recordsList;
        private FIleIOService fileIOService;
        
        public WindowRecords()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileIOService = new FIleIOService(PATH);

            try
            {
                recordsList = fileIOService.LoadData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Close();
            }

            if (recordsList != null)
            {
                dgRecords.ItemsSource = from record in recordsList orderby record.Moves select record;
            }
        }

        private void dgRecords_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Tag = e.Row.GetIndex() + 1;
        }
    }
}
