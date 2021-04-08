using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
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
using Kopakabana.source;
using Microsoft.Win32;

namespace Kopakabana
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainManager Manager = new MainManager();
        private ObservableCollection<Match> semifinalsCollection = new ObservableCollection<Match>();
        private ObservableCollection<Match> finalCollection = new ObservableCollection<Match>();
        private DataGrid lastUsedDataGrid;
        private bool unsavedChangesFlag = false;
        private string currentPath = "";
        public MainWindow()
        {
            InitializeComponent();
            AssignDataContext();
        }
        private void AssignDataContext()
        {
            RefereeGrid.DataContext = Manager.Referees.GetRefereesList();
            TeamsDataGrid.DataContext = Manager.Teams.GetTeamsList();
            TournamentsDataGrid.DataContext = Manager.Tournaments.GetTournamentList();
            SemifinalsDataGrid.DataContext = semifinalsCollection;
            FinalDataGrid.DataContext = finalCollection;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddRefereeButton_Click(object sender, RoutedEventArgs e)
        {
            PersonDialog dialog = new PersonDialog("Dodaj sędziego");
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                Manager.Referees.AddReferee(new Referee(dialog.NameTextBox.Text, dialog.SurnameTextBox.Text, int.Parse(dialog.AgeTextBox.Text)));
                SetUnsavedChangesFlag(true);
                /*
                try
                {
                    Manager.Referees.AddReferee(new Referee(dialog.NameTextBox.Text, dialog.SurnameTextBox.Text, int.Parse(dialog.AgeTextBox.Text)));
                }
                catch (InvalidRefreeIdException)
                {
                    MessageBox.Show("Błąd dodawania sędziego.","Błąd",MessageBoxButton.OK, MessageBoxImage.Error);
                }*/
            }
        }

        private void DeleteRefereeButton_Click(object sender, RoutedEventArgs e)
        {
            bool safe = SafeRefereeCheckbox.IsChecked.Value;
            while (RefereeGrid.SelectedItem!=null)
            {
                Referee current = RefereeGrid.SelectedItem as Referee;
                if (current == null)
                    continue;
                if (!safe)
                {
                    Manager.Referees.RemoveReferee(current.Id);
                    SetUnsavedChangesFlag(true);
                    continue;
                }
                MessageBoxResult result = MessageBox.Show(this,"Czy chcesz usunąć następujący element?\n" + current, "Potwierdzenie usuwania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Manager.Referees.RemoveReferee(current.Id);
                    SetUnsavedChangesFlag(true);
                }
                else
                {
                    RefereeGrid.SelectedItems[0] = null;
                }
            }
        }

        private void EditRefereeButton_Click(object sender, RoutedEventArgs e)
        {
            Referee selected = RefereeGrid.SelectedItem as Referee;
            if (selected == null) return;
            PersonDialog dialog = new PersonDialog("Edytuj dane sędziego",selected.Name, selected.Surname, selected.Age.ToString());
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                selected.Name = dialog.NameTextBox.Text;
                selected.Surname = dialog.SurnameTextBox.Text;
                selected.Age = int.Parse(dialog.AgeTextBox.Text);
                RefereeGrid.Items.Refresh();
                SetUnsavedChangesFlag(true);
            }
        }

        private void RefereeGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Referee selected = RefereeGrid.SelectedItem as Referee;
            if (selected == null)
            {
                EditRefereeButton.IsEnabled = false;
                DeleteRefereeButton.IsEnabled = false;
            }
            else
            {
                EditRefereeButton.IsEnabled = true;
                DeleteRefereeButton.IsEnabled = true;
            }
        }

        private void AddTeamButton_Click(object sender, RoutedEventArgs e)
        {
            TeamDialog dialog = new TeamDialog("Zgłoś drużynę");
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                Team newTeam = null;
                switch (dialog.TypeOfSport)
                {
                    case Sport.Volleyball:
                        newTeam = new VolleyballTeam(dialog.NameTextBox.Text);
                        break;
                    case Sport.Dodgeball:
                        newTeam = new DodgeballTeam(dialog.NameTextBox.Text);
                        break;
                    case Sport.Tugofwar:
                        newTeam = new TugofwarTeam(dialog.NameTextBox.Text);
                        break;
                    default:
                        break;
                }
                Manager.Teams.AddTeam(newTeam);
                SetUnsavedChangesFlag(true);
                //MessageBox.Show("Dodano drużynę", "Test", MessageBoxButton.OK, MessageBoxImage.Information);
                /*
                try
                {

                }
                catch (InvalidTeamIdException)
                {
                    MessageBox.Show("Błąd dodawania drużyny.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }*/
            }
        }

        private void EditTeamButton_Click(object sender, RoutedEventArgs e)
        {
            Team selected = TeamsDataGrid.SelectedItem as Team;
            if (selected == null) return;
            TeamDialog dialog = new TeamDialog("Edytuj nazwe drużyny", selected.Name, selected.TypeOfSportAsEnum);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                selected.Name = dialog.NameTextBox.Text;
                TeamsDataGrid.Items.Refresh();
                SetUnsavedChangesFlag(true);
            }
        }

        private void DeleteTeamButton_Click(object sender, RoutedEventArgs e)
        {
            bool safe = SafeTeamCheckbox.IsChecked.Value;
            while (TeamsDataGrid.SelectedItem != null)
            {
                Team current = TeamsDataGrid.SelectedItem as Team;
                if (current == null)
                    continue;
                if (!safe)
                {
                    Manager.Teams.RemoveTeam(current.Id);
                    SetUnsavedChangesFlag(true);
                    continue;
                }
                MessageBoxResult result = MessageBox.Show(this, "Czy chcesz usunąć następujący element?\n" + current, "Potwierdzenie usuwania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Manager.Teams.RemoveTeam(current.Id);
                    SetUnsavedChangesFlag(true);
                }
                else
                {
                    TeamsDataGrid.SelectedItems[0] = null;
                }
            }
        }
        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            Team currentTeam = TeamsDataGrid.SelectedItem as Team;
            if (currentTeam == null) return;
            PersonDialog dialog = new PersonDialog("Dodaj gracza");
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    currentTeam.AddMember(new Player(dialog.NameTextBox.Text, dialog.SurnameTextBox.Text, int.Parse(dialog.AgeTextBox.Text)));
                    NumberOfMembersTextBlock.DataContext = null;
                    NumberOfMembersTextBlock.DataContext = currentTeam;
                    SetUnsavedChangesFlag(true);
                }
                catch (FullTeamException)
                {
                    MessageBox.Show("Błąd dodawania członka drużyny. Drużyna jest pełna.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (currentTeam.IsFull())
            {
                AddMemberButton.IsEnabled = false;
            }
        }

        private void EditTMemberButton_Click(object sender, RoutedEventArgs e)
        {
            Player selected = MembersDataGrid.SelectedItem as Player;
            if (selected == null) return;
            PersonDialog dialog = new PersonDialog("Edytuj dane gracza", selected.Name, selected.Surname, selected.Age.ToString());
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                selected.Name = dialog.NameTextBox.Text;
                selected.Surname = dialog.SurnameTextBox.Text;
                selected.Age = int.Parse(dialog.AgeTextBox.Text);
                MembersDataGrid.Items.Refresh();
                SetUnsavedChangesFlag(true);
            }
        }

        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            Team currentTeam = TeamsDataGrid.SelectedItem as Team;
            if (currentTeam == null) return;
            bool safe = SafeMemberCheckbox.IsChecked.Value;
            while (MembersDataGrid.SelectedItem != null)
            {
                Player current = MembersDataGrid.SelectedItem as Player;
                if (current == null)
                    continue;
                if (!safe)
                {
                    currentTeam.RemoveMember(current.Id);
                    SetUnsavedChangesFlag(true);
                    continue;
                }
                MessageBoxResult result = MessageBox.Show(this, "Czy chcesz usunąć następujący element?\n" + current, "Potwierdzenie usuwania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    currentTeam.RemoveMember(current.Id);
                    SetUnsavedChangesFlag(true);
                }
                else
                {
                    MembersDataGrid.SelectedItems[0] = null;
                }
            }

            NumberOfMembersTextBlock.DataContext = null;
            NumberOfMembersTextBlock.DataContext = currentTeam;

            if (!currentTeam.IsFull())
            {
                AddMemberButton.IsEnabled = true;
            }
        }

        private void TeamsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Team currentTeam = TeamsDataGrid.SelectedItem as Team;
            if (currentTeam == null)
            {
                ChooseTeamInfoLabel.Visibility = Visibility.Visible;
                NumberOfMembersTextBlockParent.Visibility = Visibility.Hidden;
                NumberOfMembersTextBlock.DataContext = null;
                MembersDataGrid.DataContext = null;
                EditTeamButton.IsEnabled = false;
                DeleteTeamButton.IsEnabled = false;
                AddMemberButton.IsEnabled = false;
                return;
            }
            ChooseTeamInfoLabel.Visibility = Visibility.Hidden;
            NumberOfMembersTextBlock.DataContext = currentTeam;
            NumberOfMembersTextBlockParent.Visibility = Visibility.Visible;
            MembersDataGrid.DataContext = currentTeam.GetMembersList();
            EditTeamButton.IsEnabled = true;
            DeleteTeamButton.IsEnabled = true;
            AddMemberButton.IsEnabled = true;
        }

        private void MembersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Player currentMember = MembersDataGrid.SelectedItem as Player;
            if (currentMember == null)
            {
                EditMemberButton.IsEnabled = false;
                DeleteMemberButton.IsEnabled = false;
            }
            else
            {
                EditMemberButton.IsEnabled = true;
                DeleteMemberButton.IsEnabled = true;
            }
        }

        private void AddTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            TournamentDialog dialog = new TournamentDialog("Dodaj turniej");
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                Tournament newTournament = null;
                switch (dialog.TypeOfSport)
                {
                    case Sport.Volleyball:
                        newTournament = new VolleyballTournament(dialog.NameTextBox.Text);
                        break;
                    case Sport.Dodgeball:
                        newTournament = new DodgeballTournament(dialog.NameTextBox.Text);
                        break;
                    case Sport.Tugofwar:
                        newTournament = new TugofwarTournament(dialog.NameTextBox.Text);
                        break;
                    default:
                        break;
                }
                Manager.Tournaments.AddTournament(newTournament);
                SetUnsavedChangesFlag(true);
            }
        }

        private void EditTournamentButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament selected = TournamentsDataGrid.SelectedItem as Tournament;
            if (selected == null) return;
            TournamentDialog dialog = new TournamentDialog("Edytuj nazwe turnieju", selected.Name, selected.TypeOfSportAsEnum);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                selected.Name = dialog.NameTextBox.Text;
                TournamentsDataGrid.Items.Refresh();
                SetUnsavedChangesFlag(true);
            }
        }

        private void DeleteTournamentutton_Click(object sender, RoutedEventArgs e)
        {
            bool safe = SafeTournamentCheckbox.IsChecked.Value;
            while (TournamentsDataGrid.SelectedItem != null)
            {
                Tournament current = TournamentsDataGrid.SelectedItem as Tournament;
                if (current == null)
                    continue;
                if (!safe)
                {
                    Manager.Tournaments.RemoveTournament(current.Id);
                    SetUnsavedChangesFlag(true);
                    continue;
                }
                MessageBoxResult result = MessageBox.Show(this, "Czy chcesz usunąć następujący element?\n" + current, "Potwierdzenie usuwania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Manager.Tournaments.RemoveTournament(current.Id);
                    SetUnsavedChangesFlag(true);
                }
                else
                {
                    TournamentsDataGrid.SelectedItems[0] = null;
                }
            }

        }
        private void TournamentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            if (currentTournament == null)
            {
                ChooseTournamentInfoLabel.Visibility = Visibility.Visible;
                MatchesDataGrid.DataContext = null;
                semifinalsCollection.Clear();
                finalCollection.Clear();
                EditTournamentButton.IsEnabled = false;
                DeleteTournamentButton.IsEnabled = false;
                AddMatchButton.IsEnabled = false;
                GenerateSeminifalsButton.IsEnabled = false;
                GenerateFinalButton.IsEnabled = false;
                return;
            }
            ChooseTournamentInfoLabel.Visibility = Visibility.Hidden;
            MatchesDataGrid.DataContext = currentTournament.GetMatchesList();

            semifinalsCollection.Clear();
            finalCollection.Clear();
            if (currentTournament.Semifinal1 != null) semifinalsCollection.Add(currentTournament.Semifinal1);
            if (currentTournament.Semifinal2 != null) semifinalsCollection.Add(currentTournament.Semifinal2);
            if (currentTournament.Final != null) semifinalsCollection.Add(currentTournament.Final);
            EditTournamentButton.IsEnabled = true;
            DeleteTournamentButton.IsEnabled = true;
            GenerateSeminifalsButton.IsEnabled = true;
            GenerateFinalButton.IsEnabled = true;
            AddMatchButton.IsEnabled = true;
        }

        private void AddMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            if (currentTournament == null) return;
            MatchDialog dialog = null;
            switch (currentTournament.TypeOfSportAsEnum)
            {
                case Sport.Volleyball:
                    dialog = new MatchDialog("Dodaj mecz", Sport.Volleyball) { Owner = this };
                    break;
                case Sport.Dodgeball:
                    dialog = new MatchDialog("Dodaj mecz", Sport.Dodgeball) { Owner = this };
                    break;
                case Sport.Tugofwar:
                    dialog = new MatchDialog("Dodaj mecz", Sport.Tugofwar) { Owner = this };
                    break;
            }
            if (dialog.ShowDialog() != true) return;
            Match newMatch = null;
            switch (currentTournament.TypeOfSportAsEnum)
            {
                case Sport.Volleyball:
                    newMatch = new VolleyballMatch();
                    (newMatch as VolleyballMatch).AssistantReferee1 = dialog.AssistantReferee1ComboBox.SelectedItem as Referee;
                    (newMatch as VolleyballMatch).AssistantReferee2 = dialog.AssistantReferee2ComboBox.SelectedItem as Referee;
                    break;
                case Sport.Dodgeball:
                    newMatch = new DodgeballMatch();
                    break;
                case Sport.Tugofwar:
                    newMatch = new TugofwarMatch();
                    break;
            }
            newMatch.Team1 = dialog.Team1ComboBox.SelectedItem as Team;
            newMatch.Team2 = dialog.Team2ComboBox.SelectedItem as Team;
            if (dialog.Date.SelectedDate.HasValue)
            {
                newMatch.Date = dialog.Date.SelectedDate.Value.Date;
            }
                    
            newMatch.Time = dialog.TimeTextBox.Text;
            newMatch.Referee = dialog.RefereeComboBox.SelectedItem as Referee;
            newMatch.Score = dialog.ScoreTextBox.Text;
            currentTournament.AddMatch(newMatch);
            SetUnsavedChangesFlag(true);
        }
        private void EditMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Match selected = lastUsedDataGrid.SelectedItem as Match;
            if (selected == null) return;
            MatchDialog dialog = new MatchDialog("Edytuj mecz", selected) {Owner = this };
            if (dialog.ShowDialog() == true)
            {
                selected.Team1 = dialog.Team1ComboBox.SelectedItem as Team;
                selected.Team2 = dialog.Team2ComboBox.SelectedItem as Team;
                if (dialog.Date.SelectedDate.HasValue)
                {
                    selected.Date = dialog.Date.SelectedDate.Value.Date;
                }

                selected.Time = dialog.TimeTextBox.Text;
                selected.Referee = dialog.RefereeComboBox.SelectedItem as Referee;
                if (selected is VolleyballMatch)
                {
                    (selected as VolleyballMatch).AssistantReferee1 = dialog.AssistantReferee1ComboBox.SelectedItem as Referee;
                    (selected as VolleyballMatch).AssistantReferee2 = dialog.AssistantReferee2ComboBox.SelectedItem as Referee;
                }
                selected.Score = dialog.ScoreTextBox.Text;
                lastUsedDataGrid.Items.Refresh();
                SetUnsavedChangesFlag(true);
            }
        }
        /*
        private void MatchesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Match currentMatch = MatchesDataGrid.SelectedItem as Match;
            if (currentMatch == null)
            {
                EditMatchButton.IsEnabled = false;
                DeleteMatchButton.IsEnabled = false;
                return;
            }
            lastUsedDataGrid = MatchesDataGrid;
            EditMatchButton.IsEnabled = true;
            DeleteMatchButton.IsEnabled = true;
        }
        private void SemifinalsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Match currentMatch = MatchesDataGrid.SelectedItem as Match;
            DeleteMatchButton.IsEnabled = false;
            if (currentMatch == null)
            {
                EditMatchButton.IsEnabled = false;
                DeleteMatchButton.IsEnabled = false;
                return;
            }
            lastUsedDataGrid = SemifinalsDataGrid;
            EditMatchButton.IsEnabled = true;
        }
        */
        private void MatchesDataGridRefreshButtons(object sender, RoutedEventArgs e)
        {
            Match currentMatch = MatchesDataGrid.SelectedItem as Match;
            AddMatchButton.IsEnabled = true;
            if (currentMatch == null)
            {
                EditMatchButton.IsEnabled = false;
                DeleteMatchButton.IsEnabled = false;
                return;
            }
            lastUsedDataGrid = MatchesDataGrid;
            EditMatchButton.IsEnabled = true;
            DeleteMatchButton.IsEnabled = true;
        }

        private void SemifinalsDataGridRefreshButtons(object sender, RoutedEventArgs e)
        {
            Match currentMatch = SemifinalsDataGrid.SelectedItem as Match;
            DeleteMatchButton.IsEnabled = false;
            AddMatchButton.IsEnabled = false;
            if (currentMatch == null)
            {
                EditMatchButton.IsEnabled = false;
                DeleteMatchButton.IsEnabled = false;
                return;
            }
            lastUsedDataGrid = SemifinalsDataGrid;
            EditMatchButton.IsEnabled = true;
        }

        private void FinalDataGridRefreshButtons(object sender, RoutedEventArgs e)
        {
            Match currentMatch = FinalDataGrid.SelectedItem as Match;
            DeleteMatchButton.IsEnabled = false;
            AddMatchButton.IsEnabled = false;
            if (currentMatch == null)
            {
                EditMatchButton.IsEnabled = false;
                DeleteMatchButton.IsEnabled = false;
                return;
            }
            lastUsedDataGrid = FinalDataGrid;
            EditMatchButton.IsEnabled = true;
        }
        private void DeleteMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            if (currentTournament == null) return;
            bool safe = SafeMatchCheckbox.IsChecked.Value;
            while (MatchesDataGrid.SelectedItem != null)
            {
                Match current = MatchesDataGrid.SelectedItem as Match;
                if (current == null)
                    continue;
                if (!safe)
                {
                    currentTournament.RemoveMatch(current.Id);
                    SetUnsavedChangesFlag(true);
                    continue;
                }
                MessageBoxResult result = MessageBox.Show(this, "Czy chcesz usunąć następujący element?\n" + current, "Potwierdzenie usuwania", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    currentTournament.RemoveMatch(current.Id);
                    SetUnsavedChangesFlag(true);
                }
                else
                {
                    MatchesDataGrid.SelectedItems[0] = null;
                }
            }
        }

        private void GenerateSemifinalsButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            if (currentTournament == null) return;
            try
            {
                currentTournament.GenerateSemifinals();
                semifinalsCollection.Clear();
                semifinalsCollection.Add(currentTournament.Semifinal1);
                semifinalsCollection.Add(currentTournament.Semifinal2);
                SetUnsavedChangesFlag(true);
            }
            catch (NotEnoughTeamsException exc)
            {
                MessageBox.Show($"W turnieju bierze udział za mało drużyn, aby wygenerować półfinały. ({exc.GetCurrentCount()}/4)", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (EmptyScoresException exc)
            {
                MessageBox.Show($"Przynajmniej jeden mecz nie ma wpisanego wyniku. (id: {exc.GetMatchId()})", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ImpossibleToCreateSemifinals)
            {
                MessageBox.Show("Nie można wygenerować półfinałów z aktualnych wyników.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GenerateFinalButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            if (currentTournament == null) return;
            try
            {
                currentTournament.GenerateFinal();
                finalCollection.Clear();
                finalCollection.Add(currentTournament.Final);
                SetUnsavedChangesFlag(true);
            }
            catch (EmptyScoresException exc)
            {
                MessageBox.Show($"Przynajmniej jeden mecz nie ma wpisanego wyniku. (id: {exc.GetMatchId()})", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SemifinalsNotGeneratedException)
            {
                MessageBox.Show("Nie można wygenerować finału, ponieważ nie zostały wygenerowane jeszcze półfinały.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowTableButton_Click(object sender, RoutedEventArgs e)
        {
            Tournament currentTournament = TournamentsDataGrid.SelectedItem as Tournament;
            try
            {
                List<ScoreTableRow> table = currentTournament.GetScoreBoard();
                ScoreTableDialog dialog = new ScoreTableDialog("Tabela wyników turnieju " + currentTournament.Name, table);
                dialog.Owner = this;
                dialog.ShowDialog();
            }
            catch(EmptyScoresException exc)
            {
                MessageBox.Show($"Przynajmniej jeden mecz nie ma wpisanego wyniku. (id: {exc.GetMatchId()})", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMenuButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pliki DAT|*.dat";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Manager = MainManager.LoadFromFile(dialog.FileName);
                    AssignDataContext();
                    currentPath = dialog.FileName;
                    SetUnsavedChangesFlag(false);
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Błąd serializacji danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SaveMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(currentPath))
            {
                try
                {
                    Manager.SaveToFile(currentPath);
                    AssignDataContext();
                    SetUnsavedChangesFlag(false);
                    return;
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Błąd serializacji danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Pliki DAT|*.dat";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Manager.SaveToFile(dialog.FileName);
                    AssignDataContext();
                    currentPath = dialog.FileName;
                    SetUnsavedChangesFlag(false);
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Błąd serializacji danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SaveAsMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Pliki DAT|*.dat";
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    Manager.SaveToFile(dialog.FileName);
                    AssignDataContext();
                    currentPath = dialog.FileName;
                    SetUnsavedChangesFlag(false);
                }
                catch (SerializationException)
                {
                    MessageBox.Show("Błąd serializacji danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void SetUnsavedChangesFlag(bool value)
        {
            if (value)
            {
                unsavedChangesFlag = true;
                Title = "Kopakabana - " + currentPath+"*";
            }
            else
            {
                Title = "Kopakabana - " + currentPath;
                unsavedChangesFlag = false;
            }
            
        }
        private void ExitMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (unsavedChangesFlag)
            {
                if (MessageBoxResult.Yes != MessageBox.Show("Ostatnie zmiany nie zostały zapisane. Czy chcesz zamknąć program mimo to?", "Niezapisane zmiany", MessageBoxButton.YesNo, MessageBoxImage.Question))
                    e.Cancel = true;
            }
        }
    }
}
