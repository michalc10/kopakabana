using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy TeamDialog.xaml
    /// </summary>
    public partial class TournamentDialog : Window
    {
        public Sport TypeOfSport { get; set; }
        public TournamentDialog(string title)
        {
            InitializeComponent();
            Title = title;
        }
        public TournamentDialog(string title, string name, Sport type) : this(title)
        {
            NameTextBox.Text = name;
            switch (type)
            {
                case Sport.Volleyball:
                    SportComboBox.SelectedIndex = 0;
                    break;
                case Sport.Dodgeball:
                    SportComboBox.SelectedIndex = 1;
                    break;
                case Sport.Tugofwar:
                    SportComboBox.SelectedIndex = 2;
                    break;
                default:
                    break;
            }
            SportComboBox.IsEnabled = false;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            NameTextBox.SelectAll();
            NameTextBox.Focus();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || SportComboBox.SelectedIndex<0)
            {
                EmptyLabel.Visibility = Visibility.Visible;
                return;
            }
            NameTextBox.Text = NameTextBox.Text.Trim();
            if (SportComboBox.SelectedIndex==0) TypeOfSport = Sport.Volleyball;
            else if (SportComboBox.SelectedIndex==1) TypeOfSport = Sport.Dodgeball;
            else TypeOfSport = Sport.Tugofwar;
            DialogResult = true;
        }
    }
}
