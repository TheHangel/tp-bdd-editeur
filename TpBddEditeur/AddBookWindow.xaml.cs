using BddediteurContext;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace TpBddEditeur
{
    /// <summary>
    /// Logique d'interaction pour AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {

        private MainWindow _mainWindow;

        public AddBookWindow(MainWindow main)
        {
            this._mainWindow = main;
            InitializeComponent();
            this.authorsCombo.ItemsSource = MainWindow.listeauteurs;
        }

        public static bool VerifierSiTexteEstUneDate(string texte)
        {
            DateTime date;
            if (DateTime.TryParseExact(texte, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return true;
            }
            return false;
        }

        private void authorsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addBookButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            Bookauthor author = (Bookauthor)(this.authorsCombo as ComboBox).SelectedItem;
            if (author != null)
            {
                if (VerifierSiTexteEstUneDate(this.dateBox.Text))
                {
                    if (this.bookTitleBox.Text != "")
                    {
                        DateTime date = DateTime.ParseExact(this.dateBox.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        MainWindow.bdd.AddBook(author.ISBN, this.bookTitleBox.Text, date);
                        this._mainWindow.reloadBooksList();
                        MessageBox.Show("Livre ajouté.");
                        DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Veuillez insérer un titre.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Date incorrect. Vous devez l'écrire au format dd/mm/yyyy.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un auteur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

    }
}
