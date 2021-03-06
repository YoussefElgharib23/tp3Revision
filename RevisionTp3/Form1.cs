﻿using System;
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
                   telephone = textBoxTel.Text,
                   sNumero = textBoxNum.Text;

            if ( prenom == "" || nom == "" || telephone == "" || sNumero == "" )
            {
                MessageBox.Show("Un ou plusieurs champs sont vides !");
                return;
            }
            int numero = -1;
            if ( !int.TryParse(sNumero, out numero) )
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
                textBoxNum.Text = textBoxNom.Text = textBoxPrenom.Text = textBoxTel.Text = "";
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

        private void clickEventhandle(object sender, EventArgs e)
        {
            string telephone = textBoxTelModifier.Text,
                   sNumero = textBoxNumModifier.Text;

            if ( telephone == "" || sNumero == "")
            {
                MessageBox.Show("Un ou plusieurs champs sont vides !");
                return;
            }

            int numero = -1;

            if ( !int.TryParse(sNumero, out numero) )
            {
                MessageBox.Show("Le format du numero est invalid !");
                return;
            }

            if ( !verifierNumeroStagiaire(numero) )
            {
                MessageBox.Show("Le numero n'existe pas !");
                return;
            }

            command.CommandText = "UPDATE stagiaires SET telephone = @telephone WHERE numero = @numero";
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@numero", numero);

            if ( command.ExecuteNonQuery() > 0 )
            {
                textBoxNumModifier.Text = "";
                textBoxTelModifier.Text = "";
                MessageBox.Show("Le telephone du stagiaires est bien modifiee !");
                actualiserListBox();
            }

            command.Parameters.Clear();
        }

        private bool verifierNumeroStagiaire(int numero)
        {
            command.CommandText = "SELECT COUNT(*) FROM stagiaires WHERE numero = @numero";
            command.Parameters.AddWithValue("@numero", numero);

            int count = (int)command.ExecuteScalar();
            command.Parameters.Clear();

            return count > 0;
        }

        private void btnSupprimerHandle(object sender, EventArgs e)
        {
            string sNumero = textBoxNumSupprimer.Text;
            int numero = -1;

            if ( sNumero == "" )
            {
                MessageBox.Show("Le champ du numero doit etre intilisee avant de supprimer !");
                return;
            }

            if ( !int.TryParse(sNumero, out numero) )
            {
                MessageBox.Show("Le format du numero est invalid !");
                return;
            }

            if ( !verifierNumeroStagiaire(numero) )
            {
                MessageBox.Show("Le numero ne correspond aucun stagiaire !");
                return;
            }

            command.CommandText = "DELETE FROM stagiaires WHERE numero = @numero";
            command.Parameters.AddWithValue("@numero", numero);

            int count = command.ExecuteNonQuery();

            if ( count > 0 )
            {
                textBoxNumSupprimer.Text = "";
                MessageBox.Show("Le stagiaire est bien supprimee !");
            }
            command.Parameters.Clear();

            actualiserListBox();
        }
    }
}
