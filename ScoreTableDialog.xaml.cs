using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Kopakabana.source;
namespace Kopakabana
{
    /// <summary>
    /// Logika interakcji dla klasy ScoreTableDialog.xaml
    /// </summary>
    public partial class ScoreTableDialog : Window
    {
        public ScoreTableDialog(string title, List<ScoreTableRow> data)
        {
            InitializeComponent();
            Title = title;
            ScoreTableDataGrid.DataContext = data;
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
