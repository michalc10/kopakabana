using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Kopakabana.source;
namespace Kopakabana
{
    /// <summary>
    /// Logika interakcji dla klasy MatchDialog.xaml
    /// </summary>
    public partial class MatchDialog : Window
    {
        private static readonly Regex timeRegex = new Regex("^([0-9]|[1][0-9]|[2][0-3]):[0-5][0-9]$");
        private static readonly Regex scoreRegex = new Regex("^[0-9]+ - [0-9]+$");
        private string lastCorrectTime = "";
        private string lastCorrectScore = "";
        private Sport sport;
        private source.Match matchToEdit;
        public MatchDialog(string title, Sport type)
        {
            InitializeComponent();
            Date.DisplayDateStart = DateTime.Now;
            Date.SelectedDate = DateTime.Now;
            Title = title;
            sport = type;
            switch (type)
            {
                case Sport.Volleyball:
                    SportComboBox.SelectedIndex = 0;
                    AssistantReferee1Label.Visibility = Visibility.Visible;
                    AssistantReferee2Label.Visibility = Visibility.Visible;
                    AssistantReferee1ComboBox.Visibility = Visibility.Visible;
                    AssistantReferee2ComboBox.Visibility = Visibility.Visible;
                    ScoreLabel.Margin = new Thickness(5, 253, 0, 0);
                    ScoreTextBox.Margin = new Thickness(130, 253, 5, 0);
                    break;
                case Sport.Dodgeball:
                    SportComboBox.SelectedIndex = 1;
                    break;
                case Sport.Tugofwar:
                    SportComboBox.SelectedIndex = 2;
                    break;
            }
        }
        public MatchDialog(string title, source.Match matchToEdit) : this(title, matchToEdit.TypeOfSportAsEnum)
        {
            this.matchToEdit = matchToEdit;
        }
        private void Window_ContentRendered(object sender, EventArgs e)
        {
            MainWindow mainWindow = Owner as MainWindow;
            if (mainWindow == null) Close();
            switch (sport)
            {
                case Sport.Volleyball:
                    Team1ComboBox.ItemsSource = mainWindow.Manager.Teams.GetVolleyballTeamsList();
                    Team2ComboBox.ItemsSource = mainWindow.Manager.Teams.GetVolleyballTeamsList();
                    break;
                case Sport.Dodgeball:
                    Team1ComboBox.ItemsSource = mainWindow.Manager.Teams.GetDodgeballTeamsList();
                    Team2ComboBox.ItemsSource = mainWindow.Manager.Teams.GetDodgeballTeamsList();
                    break;
                case Sport.Tugofwar:
                    Team1ComboBox.ItemsSource = mainWindow.Manager.Teams.GetTugofwarTeamsList();
                    Team2ComboBox.ItemsSource = mainWindow.Manager.Teams.GetTugofwarTeamsList();
                    break;
            }
            AssistantReferee1ComboBox.ItemsSource = new ObservableCollection<Referee>(mainWindow.Manager.Referees.GetRefereesList());
            AssistantReferee2ComboBox.ItemsSource = new ObservableCollection<Referee>(mainWindow.Manager.Referees.GetRefereesList());
            RefereeComboBox.ItemsSource = new ObservableCollection<Referee>(mainWindow.Manager.Referees.GetRefereesList());

            if (matchToEdit == null) return;
            Team1ComboBox.SelectedItem = matchToEdit.Team1;
            Team2ComboBox.SelectedItem = matchToEdit.Team2;
            if (matchToEdit.Date != null)
                Date.SelectedDate = matchToEdit.Date;
            TimeTextBox.Text = matchToEdit.Time;
            lastCorrectTime = matchToEdit.Time;
            RefereeComboBox.SelectedItem = matchToEdit.Referee;
            ScoreTextBox.Text = matchToEdit.Score;
            lastCorrectScore = matchToEdit.Score;
            VolleyballMatch castedMatch = matchToEdit as VolleyballMatch;
            if (castedMatch != null)
            {
                AssistantReferee1ComboBox.SelectedItem = castedMatch.AssistantReferee1;
                AssistantReferee2ComboBox.SelectedItem = castedMatch.AssistantReferee2;
            }
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (Team1ComboBox.SelectedIndex < 0 || Team2ComboBox.SelectedIndex < 0 || !Date.SelectedDate.HasValue)
                EmptyLabel.Visibility = Visibility.Visible;
            else
            {
                DialogResult = true;
            }
            
        }
        private void TimeTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (timeRegex.IsMatch(TimeTextBox.Text))
                lastCorrectTime = TimeTextBox.Text;
            else
            {
                TimeTextBox.Text = lastCorrectTime;
            }
        }

        private void ScoreTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (scoreRegex.IsMatch(ScoreTextBox.Text))
                lastCorrectScore = ScoreTextBox.Text;
            else
            {
                ScoreTextBox.Text = lastCorrectScore;
            }
        }

        private void Team1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Team> otherComboBoxCollection = Team2ComboBox.ItemsSource as ObservableCollection<Team>;
            otherComboBoxCollection.Remove(Team1ComboBox.SelectedItem as Team);
            Team previousItem = (e.RemovedItems.Count == 0) ? null : e.RemovedItems[0] as Team;
            if (previousItem != null)
                otherComboBoxCollection.Add(previousItem);
        }

        private void Team2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Team> otherComboBoxCollection = Team1ComboBox.ItemsSource as ObservableCollection<Team>;
            otherComboBoxCollection.Remove(Team2ComboBox.SelectedItem as Team);
            Team previousItem = (e.RemovedItems.Count == 0) ? null : e.RemovedItems[0] as Team;
            if (previousItem != null)
                otherComboBoxCollection.Add(previousItem);
        }

        private void RefereeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Referee> otherComboBoxCollection1 = AssistantReferee1ComboBox.ItemsSource as ObservableCollection<Referee>;
            ObservableCollection<Referee> otherComboBoxCollection2 = AssistantReferee2ComboBox.ItemsSource as ObservableCollection<Referee>;
            Referee itemToHide = RefereeComboBox.SelectedItem as Referee;
            otherComboBoxCollection1.Remove(itemToHide);
            otherComboBoxCollection2.Remove(itemToHide);
            Referee previousItem = (e.RemovedItems.Count == 0) ? null : e.RemovedItems[0] as Referee;
            if (previousItem != null)
            {
                otherComboBoxCollection1.Add(previousItem);
                otherComboBoxCollection2.Add(previousItem);
            }
        }
        private void AssistantReferee1ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Referee> otherComboBoxCollection1 = RefereeComboBox.ItemsSource as ObservableCollection<Referee>;
            ObservableCollection<Referee> otherComboBoxCollection2 = AssistantReferee2ComboBox.ItemsSource as ObservableCollection<Referee>;
            Referee itemToHide = AssistantReferee1ComboBox.SelectedItem as Referee;
            otherComboBoxCollection1.Remove(itemToHide);
            otherComboBoxCollection2.Remove(itemToHide);
            Referee previousItem = (e.RemovedItems.Count == 0) ? null : e.RemovedItems[0] as Referee;
            if (previousItem != null)
            {
                otherComboBoxCollection1.Add(previousItem);
                otherComboBoxCollection2.Add(previousItem);
            }
        }
        private void AssistantReferee2ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObservableCollection<Referee> otherComboBoxCollection1 = RefereeComboBox.ItemsSource as ObservableCollection<Referee>;
            ObservableCollection<Referee> otherComboBoxCollection2 = AssistantReferee1ComboBox.ItemsSource as ObservableCollection<Referee>;
            Referee itemToHide = AssistantReferee2ComboBox.SelectedItem as Referee;
            otherComboBoxCollection1.Remove(itemToHide);
            otherComboBoxCollection2.Remove(itemToHide);
            Referee previousItem = (e.RemovedItems.Count == 0) ? null : e.RemovedItems[0] as Referee;
            if (previousItem != null)
            {
                otherComboBoxCollection1.Add(previousItem);
                otherComboBoxCollection2.Add(previousItem);
            }
        }
    }
}
