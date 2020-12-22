using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevisionTp3
{
    public partial class Form1 : Form
    {
        // A FAIRE CHANGER LE DATA SOURCE ET INITIAL CATALOG A VOTRE PROPRE DONNEES
        static string connectionString = @"Data Source=DESKTOP-S7OEGIL;Initial Catalog=GStagiaireRevision;Integrated Security=True";
        static SqlConnection connection = new SqlConnection(connectionString);
        SqlCommand command = new SqlCommand("", connection);

        public Form1()
        {
            InitializeComponent();
        }

        private void eventClickHandle(object sender, EventArgs e)
        {
            string prenom = textBoxPrenom.Text,
                   nom = textBoxNom.Text,
                   telephone = textBoxTel.Text;
            int numero = -1;
            if ( !int.TryParse(textBoxNum.Text, out numero) )
            {
                MessageBox.Show("Le format du numero est invalid !");
                return;
            }
            // N"OUBLIZ PAS A CHANGER LE NOM DU TABLE
            command.CommandText = "INSERT INTO stagiaires VALUES (@numero, @nom, @prenom, @telephone)";
            command.Parameters.AddWithValue("@numero", numero);
            command.Parameters.AddWithValue("@nom", nom);
            command.Parameters.AddWithValue("@prenom", prenom);
            command.Parameters.AddWithValue("@telephone", telephone);

            if (command.ExecuteNonQuery() > 0)
            {
                command.Parameters.Clear();
                MessageBox.Show("Les donnew sont bien inseree !");
                actualiserListBox();
            }
        }

        private void actualiserListBox()
        {
            command.CommandText = "SELECT * FROM stagiaires";
            SqlDataReader dataReader = command.ExecuteReader();
            if ( dataReader.HasRows )
            {
                listBoxStagiaires.Items.Clear();
                while ( dataReader.Read() )
                {
                    listBoxStagiaires.Items.Add(string.Format(dataReader[0] + ": " + dataReader[1] + " " + dataReader[2] + ", " + dataReader[3]));
                }
            }
            dataReader.Close();
        }

        private void loadEventHandle(object sender, EventArgs e)
        {
            connection.Open();
            actualiserListBox();
        }

        private void formClosingEventHandle(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }
    }
}
