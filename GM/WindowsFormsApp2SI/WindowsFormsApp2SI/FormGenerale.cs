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
using WindowsFormsGMF;

namespace WindowsFormsApp2SI
{
    public partial class Form2SI : Form
    {
        private string strcon = @"Server = .\SQLEXPRESS; Database=GM;Trusted_Connection=True;";

        public Form2SI()
        {
            InitializeComponent();
        }

        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClients dlg = new FormClients();
            dlg.ShowDialog();
        }

        private void matérielsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMateriel dlg = new FormMateriel();
            dlg.ShowDialog();
        }

        private void Form2SI_Load(object sender, EventArgs e)
        {
           FormConnect dlg = new FormConnect();
           dlg.ShowDialog();


           fillComboMatos();
        }

        private void fillComboMatos()
        {
            comboBoxMatos.Items.Clear();
            SqlConnection cn = new SqlConnection(strcon);
            cn.Open();
            string sql = "select * from MATERIEL";
            SqlCommand com = new SqlCommand(sql, cn);
            SqlDataReader dr = com.ExecuteReader();
            while (dr.Read())
            {
                comboBoxMatos.Items.Add(dr["Nom"].ToString());
            }

            dr.Close();
            cn.Close();
        }

        private int getMatosID(string name)
        {
            SqlConnection cn = new SqlConnection(strcon);
            cn.Open();
            
            string strsqL = "select ID_MAT from MATERIEL where Nom = '" + name + "'";

            SqlCommand com = new SqlCommand(strsqL, cn);
            SqlDataReader dr = com.ExecuteReader();
            dr.Read();
            int nb = Convert.ToInt32(dr["ID_MAT"]);
            cn.Close();

            return nb;
        }


        private void buttonCI_Click(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection(strcon);
            cn.Open();

            string Commentaire = textBoxComment.Text;
            string tech = textBoxTech.Text;
            string dateInstall = dateTimePickerDI.Value.ToString("yyyy-MM-dd");
            int idc = getMatosID(comboBoxMatos.SelectedItem.ToString());
                        
            string strsqL = "insert into Intervention values('" + dateInstall + "','" +
                Commentaire + "','" + tech + "'," + idc + ")";

            SqlCommand com = new SqlCommand(strsqL, cn);
            com.ExecuteNonQuery();
            cn.Close();

            cleanInter();

            MessageBox.Show("Intervention bien ajoutée !", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void cleanInter()
        {
            textBoxTech.Text = textBoxComment.Text = "";

            dateTimePickerDI.Value = DateTime.Today;
            comboBoxMatos.SelectedIndex = -1;
        }
    }
}
