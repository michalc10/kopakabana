using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Kopakabana
{
    /// <summary>
    /// Logika interakcji dla klasy PersonDialog.xaml
    /// </summary>
    public partial class PersonDialog : Window
    {
        private static readonly Regex numberRegex = new Regex("^[0-9]{0,3}$");
        private static readonly Regex textRegex = new Regex("^([^\\d\\W]|[\\s-])*$");
        private string lastCorrectName = "";
        private string lastCorrectSurname = "";
        private string lastCorrectAge = "";
        public PersonDialog(string title,string name = "", string surname= "", string age="")
        {
            InitializeComponent();
            Title = title;
            NameTextBox.Text = name;
            SurnameTextBox.Text = surname;
            AgeTextBox.Text = age;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            NameTextBox.SelectAll();
            NameTextBox.Focus();
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text) || string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(AgeTextBox.Text))
                EmptyLabel.Visibility = Visibility.Visible;
            else
            {
                NameTextBox.Text = NameTextBox.Text.Trim();
                SurnameTextBox.Text = SurnameTextBox.Text.Trim();
                DialogResult = true;
            }
            
        }

        private void AgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            int i = AgeTextBox.CaretIndex;
            if (numberRegex.IsMatch(AgeTextBox.Text))
                lastCorrectAge = AgeTextBox.Text;
            else
            {
                AgeTextBox.Text = lastCorrectAge;
                AgeTextBox.CaretIndex = i;
            }
        }
        private void NameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = NameTextBox.CaretIndex;
            if (textRegex.IsMatch(NameTextBox.Text))
                lastCorrectName = NameTextBox.Text;
            else
            {
                NameTextBox.Text = lastCorrectName;
                NameTextBox.CaretIndex = i;
            }
        }
        private void SurnameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int i = SurnameTextBox.CaretIndex;
            if (textRegex.IsMatch(SurnameTextBox.Text))
                lastCorrectSurname = SurnameTextBox.Text;
            else
            {
                SurnameTextBox.Text = lastCorrectSurname;
                SurnameTextBox.CaretIndex = i;
            }
        }
    }
}
