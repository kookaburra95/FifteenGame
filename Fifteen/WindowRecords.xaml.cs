using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using Fifteen.Services;

namespace Fifteen
{
    public partial class WindowRecords : Window
    {
        private readonly string _path = $"{Environment.CurrentDirectory}\\recordsList.json";
        private BindingList<Records> _recordsList;
        private FIleIOService _fileIoService;
        
        public WindowRecords()
        {
            InitializeComponent();
        }   
            
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _fileIoService = new FIleIOService(_path);

            try
            {
                _recordsList = _fileIoService.LoadData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Close();
            }

            if (_recordsList != null)
            {
                dgRecords.ItemsSource = from record in _recordsList 
                    orderby record.Moves, record.TimeTotalMilliseconds, record.Name  
                    select record;
            }
        }

        private void dgRecords_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Tag = e.Row.GetIndex() + 1;
        }
    }
}
