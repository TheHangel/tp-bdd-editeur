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
    /// Logique d'interaction pour AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {

        private MainWindow _mainWindow;

        public AddAuthorWindow(MainWindow main)
        {
            this._mainWindow = main;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.prenomBox.Text != "" && this.nomBox.Text != "")
            {
                if (VerifierSiTexteEstUnNombre(this.isbnBox.Text))
                {
                    if (!MainWindow.bdd.authorExiste(this.isbnBox.Text))
                    {
                        MainWindow.bdd.addAuthor(this.nomBox.Text, this.prenomBox.Text, this.isbnBox.Text);
                        this._mainWindow.reloadAuthors();
                        MessageBox.Show("Auteur ajouté.");
                        DialogResult = true;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Cet auteur(e) existe déjà.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("ISBN invalide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static bool VerifierSiTexteEstUnNombre(string texte)
        {
            foreach (char c in texte)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
