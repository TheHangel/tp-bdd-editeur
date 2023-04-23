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
    /// Logique d'interaction pour AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {

        private MainWindow _mainWindow;

        public AddUserWindow(MainWindow main)
        {
            this._mainWindow = main;
            InitializeComponent();
        }

        private void addButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (this.nomBox.Text != "" && this.prenomBox.Text != "" && this.loginBox.Text != "" && this.mdpBox.Text != "")
            {
                if (!MainWindow.bdd.userExiste(this.loginBox.Text))
                {
                    bool res = MainWindow.bdd.addUser(this.nomBox.Text, this.prenomBox.Text, this.loginBox.Text, this.mdpBox.Text, this.gestionnaireCheckbox.IsChecked.Value);
                    _mainWindow.reloadUsers();
                    MessageBox.Show("Utilisateur ajouté.");
                    DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Ce nom d'utilisateur est déjà pris.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
