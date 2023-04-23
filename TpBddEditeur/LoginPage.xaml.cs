using BddediteurContext;
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
using System.Windows.Shapes;

namespace TpBddEditeur
{
    /// <summary>
    /// Logique d'interaction pour LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {

        private MainWindow _mainWindow;

        public LoginPage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            tryToConnect();
        }

        //Si on appuye sur Entrer après avoir tapé le mot de passe
        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                tryToConnect();
            }
        }

        private void tryToConnect()
        {
            if(this.nomTextBox.Text == "" || this.mdpTextBox.Password == "")
            {
                MessageBox.Show("Vous devez compléter tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            User user = MainWindow.bdd.userConnecte(this.nomTextBox.Text, this.mdpTextBox.Password);
            if (user == null)
            {
                MessageBox.Show("Echec d'authentification.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _mainWindow.utilisateur = user;
                _mainWindow.reloadGestionnaireButton();
                _mainWindow.login_button.Content = "Se déconnecter";
                _mainWindow.helloUserLabel.Content = "Bonjour, " + user.Nom.ToUpper();
                _mainWindow.helloUserLabel.Visibility = Visibility.Visible;
                DialogResult = true;
                this.Close();
            }
        }

    }
}
