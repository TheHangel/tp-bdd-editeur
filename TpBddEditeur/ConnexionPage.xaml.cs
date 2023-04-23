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
using System.Windows.Shapes;

namespace TpBddEditeur
{
    /// <summary>
    /// Logique d'interaction pour ConnexionPage.xaml
    /// </summary>
    public partial class ConnexionPage : Window
    {

        private MainWindow _mainWindow;

        public ConnexionPage(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            this.adresseIPTextBox.Text = Properties.Settings.Default.AdresseIP;
            this.portTextBox.Text = Properties.Settings.Default.Port;
            this.nomTextBox.Text = Properties.Settings.Default.NomUtilisateur;
            this.mdpTextBox.Text = Properties.Settings.Default.MotDePasse;
        }

        private void connect_button_Click(object sender, RoutedEventArgs e)
        {
            if(this.adresseIPTextBox.Text == "" || this.portTextBox.Text == "" || this.nomTextBox.Text == "" || mdpTextBox.Text == "")
            {
                MessageBox.Show("Vous devez compléter tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            this.setParamConnexion(this.adresseIPTextBox.Text, this.portTextBox.Text, this.nomTextBox.Text, this.mdpTextBox.Text);
            try
            {
                MainWindow.bdd = new BddEditeur(Properties.Settings.Default.AdresseIP, Properties.Settings.Default.Port, Properties.Settings.Default.NomUtilisateur, Properties.Settings.Default.MotDePasse);
                DialogResult = true;
                this.Close();
            }
            // Si on entre des caractères autre que des chiffres dans la case 'Port'
            catch (InvalidOperationException)
            {
                MessageBox.Show("Le port ne doit pas contenir du texte.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void setParamConnexion(string NewAdresseIP, string NewPort, string nom, string mdp)
        {
            Properties.Settings.Default.AdresseIP = NewAdresseIP;
            Properties.Settings.Default.Port = NewPort;
            Properties.Settings.Default.NomUtilisateur = nom;
            Properties.Settings.Default.MotDePasse = mdp;
        }

    }
}
