using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFID_SCD
{
    public partial class settings : Form
    {
        Hotspot hotspot = new Hotspot();

        public settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hotspot.Stop();

            Properties.Settings.Default.Password = textBox1.Text; //Récup mdp
            Properties.Settings.Default.Save(); //Sauvegarde XML

            try
            {
                hotspot.Set(Properties.Settings.Default.SSID, Properties.Settings.Default.Password); // Configuration du hotspot
                hotspot.Start(); //Démarrage hotspot
            }
            catch (Exception e1)
            {
                Console.WriteLine("Erreur hotspot : " + e1.StackTrace);
            }

            this.Close();
        }

        private void settings_FormClosing(object sender, FormClosingEventArgs e) {         }

        private void settings_Load(object sender, EventArgs e)
        {
            //Récup infos
            label2.Text = Properties.Settings.Default.SSID;
            textBox1.Text = Properties.Settings.Default.Password;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Pour cacher la textbox
            if (checkBox1.Checked)
                textBox1.UseSystemPasswordChar = true;
            else
                textBox1.UseSystemPasswordChar = false;
        }
    }
}
