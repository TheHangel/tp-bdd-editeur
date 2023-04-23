using BddediteurContext;
using DllBddEditeur;
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

namespace TpBddEditeur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //entitées bdd
        public static List<Bookauthor> listeauteurs;
        private static List<Booklist> listelivres;
        private static List<User> listeutilisateurs;
        public static BddEditeur bdd;

        public User utilisateur = null;

        public MainWindow()
        {
            InitializeComponent();
            this.helloUserLabel.Visibility = Visibility.Hidden;
            this.addAuthorButton.Visibility = Visibility.Hidden;
            this.addBookButton.Visibility = Visibility.Hidden;
            this.deleteBookButon.Visibility = Visibility.Hidden;
            this.deleteUserButton.Visibility = Visibility.Hidden;
            this.modifUserButton.Visibility = Visibility.Hidden;
            this.tabs.Items.Remove(this.userTab);
            try
            {
                bdd = new BddEditeur(Properties.Settings.Default.AdresseIP, Properties.Settings.Default.Port, Properties.Settings.Default.NomUtilisateur, Properties.Settings.Default.MotDePasse);
                listeauteurs = bdd.getallAuthors();
                listelivres = bdd.getallBooks();
                this.listeViewAuthors.ItemsSource = listeauteurs;
                this.listeViewBooks.ItemsSource = listelivres;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listeViewAuthors_SelChanged(object sender, RoutedEventArgs e)
        {
            // récupérer l'auteur sélectionné dans la liste vue
            Bookauthor auteur = (Bookauthor)(sender as ListView).SelectedItem;
            if (auteur != null)
            {
                this.textBoxAuthorsFirstName.Text = auteur.FirstName;
                this.textBoxAuthorsLastName.Text = auteur.LastName;
                this.textBoxAuthorsISBN.Text = auteur.ISBN;
                List<Booklist> seslivres = bdd.getBooksFromAuthor(auteur.ISBN);
                this.authorsSelectedBooks.ItemsSource = seslivres;
            }
        }

        private void listeViewBooks_SelChanged(object sender, RoutedEventArgs e)
        {
            // récupérer le livre sélectionné dans la liste vue
            Booklist book = (Booklist)(sender as ListView).SelectedItem;
            this.textBoxBookPrice.Text = "";
            if (book != null)
            {
                if (this.utilisateur != null)
                {
                    if (this.utilisateur.Autorisation == true)
                    {
                        this.deleteBookButon.Visibility = Visibility.Visible;
                    }
                }
                this.textBoxBookISBN.Text = book.ISBN;
                this.textBoxBookDate.Text = book.PublicationDate.ToString().Split(' ')[0];
                this.textBoxBookTitle.Text = book.Title;
                List<Bookprice> bookprices = bdd.getCurrencies(book.ISBN);
                this.currenciesCombo.ItemsSource = bookprices;
                Bookauthor auteur = bdd.getAuthor(book.ISBN);
                if (auteur != null)
                {
                    this.isbnAuthorFromBook.Text = auteur.ISBN;
                    this.nomFromBookBox.Text = auteur.LastName;
                    this.prenomFromBookBox.Text = auteur.FirstName;
                }
            }
        }

        private void Connect_Button(object sender, RoutedEventArgs e)
        {
            ConnexionPage conn = new ConnexionPage(this);
            conn.ShowDialog();
            reloadWindow();
        }

        public void reloadWindow()
        {
            this.reloadAuthors();
            this.reloadBooksList();
            this.reloadUsers();
            if (!listeauteurs.Any())
            {
                this.textBoxAuthorsFirstName.Text = "";
                this.textBoxAuthorsLastName.Text = "";
                this.textBoxAuthorsISBN.Text = "";
            }
            if (!listelivres.Any())
            {
                this.textBoxBookTitle.Text = "";
                this.textBoxBookDate.Text = "";
                this.textBoxBookISBN.Text = "";
                this.textBoxBookPrice.Text = "";
            }
        }

        private void Login_Button(object sender, RoutedEventArgs e)
        {
            if (this.utilisateur == null)
            {
                LoginPage conn = new LoginPage(this);
                conn.ShowDialog();
            }
            else
            {
                disconnect();
                MessageBox.Show("Vous avez été déconnecté", "Déconnexion", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void disconnect()
        {
            this.helloUserLabel.Visibility = Visibility.Hidden;
            this.addAuthorButton.Visibility = Visibility.Hidden;
            this.addBookButton.Visibility = Visibility.Hidden;
            this.deleteBookButon.Visibility = Visibility.Hidden;
            this.deleteUserButton.Visibility = Visibility.Hidden;
            this.modifUserButton.Visibility = Visibility.Hidden;
            this.tabs.Items.Remove(this.userTab);
            this.login_button.Content = "Se connecter";
            this.utilisateur = null;
        }

        private void AddAuthorClick(object sender, RoutedEventArgs e)
        {
            AddAuthorWindow window = new AddAuthorWindow(this);
            window.ShowDialog();
        }

        public void reloadAuthors()
        {
            listeauteurs = bdd.getallAuthors();
            this.listeViewAuthors.ItemsSource = listeauteurs;
        }

        public void reloadBooksList()
        {
            listelivres = bdd.getallBooks();
            this.listeViewBooks.ItemsSource = listelivres;
        }

        public void reloadUsers()
        {
            listeutilisateurs = bdd.getallUsers();
            this.usersListView.ItemsSource = listeutilisateurs;
        }

        public void reloadGestionnaireButton()
        {
            if (this.utilisateur.Autorisation == true)
            {
                this.addAuthorButton.Visibility = Visibility.Visible;
                this.addBookButton.Visibility = Visibility.Visible;
                this.tabs.Items.Add(this.userTab);
                listeutilisateurs = bdd.getallUsers();
                this.usersListView.ItemsSource = listeutilisateurs;
            }
            else
            {
                this.tabs.Items.Remove(this.userTab);
            }
        }

        private void currenciesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!this.currenciesCombo.Items.IsEmpty && this.currenciesCombo.SelectedItem != null && this.currenciesCombo.SelectedItem.ToString() != "")
            {
                Bookprice book = (Bookprice)(this.currenciesCombo as ComboBox).SelectedItem;
                this.textBoxBookPrice.Text = book.Price.ToString();
            }
        }

        private void addBookButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow(this);
            window.ShowDialog();
        }

        private void modifUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.usersListView.SelectedItem != null)
            {
                if (this.nomBox.Text != "" && this.prenomBox.Text != "" && this.mdpBox.Text != "")
                {
                    User user = (User)(this.usersListView as ListView).SelectedItem;
                    bdd.UpdateUser(user, this.nomBox.Text, this.prenomBox.Text, this.mdpBox.Text, this.gestionnaireCheckbox.IsChecked.Value);
                    MessageBox.Show("Utilisateur modifié.");
                }
                else
                {
                    MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void usersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // récupérer l'utilisateur sélectionné dans la liste vue
            User user = (User)(sender as ListView).SelectedItem;
            if (user != null)
            {
                this.deleteUserButton.Visibility = Visibility.Visible;
                this.modifUserButton.Visibility = Visibility.Visible;
                this.nomBox.Text = user.Nom;
                this.prenomBox.Text = user.Prenom;
                this.mdpBox.Text = user.Mdp;
                this.gestionnaireCheckbox.IsChecked = user.Autorisation;
            }
        }

        private void addUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow window = new AddUserWindow(this);
            window.ShowDialog();
        }

        private void authorsSelectedBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void deleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)(this.usersListView as ListView).SelectedItem;
            if (user != null && bdd.deleteUser(user))
            {
                MessageBox.Show("L'utilisateur a été supprimé avec succès.");
                this.reloadUsers();
                this.deleteBookButon.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Un problème est survenu...", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void deleteBookButon_Click(object sender, RoutedEventArgs e)
        {
            Booklist book = (Booklist)(this.listeViewBooks as ListView).SelectedItem;
            if (book != null && bdd.deleteUBook(book))
            {
                MessageBox.Show("Le livre a été supprimé avec succès.");
                this.reloadBooksList();
                this.deleteUserButton.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Un problème est survenu...", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }

}
