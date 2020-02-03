using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

namespace RFID_SCD
{
    public partial class Form1 : Form
    {
        TCP tcp = new TCP();
        Hotspot hotspot = new Hotspot();

        IPAddress ipAddress = IPAddress.Parse("192.168.137.1"); //ip serveur
        int port = 2502; //port serveur

        String SSID = Properties.Settings.Default.SSID; //ssid wifi
        String mdpWifi = Properties.Settings.Default.Password; //"12345678"; //mot de passe wifi

        bool first=false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //Start
        {
            if (!first)
            {
                bool status = tcp.ServerTCP(ipAddress, port); //Configuration Serveur TCP
                radioButton1.Checked = status;
                timer1.Start();
                first = true;
            }
            else
            {
                timer1.Stop();
                tcp.Stop();
                bool status = tcp.ServerTCP(ipAddress, port); //Configuration Serveur TCP
                radioButton1.Checked = status;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e) // Pour la réception des données 
        {
            Application.DoEvents();

            try
            {
                String text = tcp.Reception(); //Réception données TCP

                if (text != null)
                {
                    String[] SplitArgs = text.Split('-'); //séparation dans tableau
                    codebarre.Text = new String(SplitArgs[0].Where(Char.IsDigit).ToArray()); type.Text = SplitArgs[1];
                    if (SplitArgs[1].Contains("non trouvé")) //livre non trouvé
                    {
                        labelNT.Text = SplitArgs[1];
                        titre.Text = null; editeur.Text = null; annee.Text = null; auteur.Text = null;
                    }
                    else //livre trouvé
                    {
                        labelNT.Text = null;
                        titre.Text = SplitArgs[2];
                        String an = new String(SplitArgs[4].Where(Char.IsDigit).ToArray());
                        if (Regex.IsMatch(an, "^(17|18|19|20)[0-9][0-9]")) //si pas d'auteur
                        {
                            editeur.Text = SplitArgs[3]; annee.Text = SplitArgs[4]; auteur.Text = null;
                        }
                        else // si auteur
                        {
                            auteur.Text = SplitArgs[3]; editeur.Text = SplitArgs[4]; annee.Text = SplitArgs[5];
                        }
                    }
                }

                radioButton1.Checked = tcp.GetStatus();
            }
            catch (Exception e1) {
                Console.WriteLine("Erreur dans timer: " + e1.StackTrace);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) //Fermeture
        {
            try
            {
                timer1.Stop();
                hotspot.Stop();
                tcp.Stop();
            }
            catch (Exception e1)
            {
                Console.WriteLine("Erreur fermeture: " + e1.StackTrace);
            }

            hotspot.Stop(); //Arret hotspot

            Thread.Sleep(350); //Pause pour exécuter la commande

            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e) //Lors du lancement de l'appli
        {
            labelIP.Text = ipAddress.ToString();

            //Création et démarrage du Hotspot WiFi
            try
            {
                hotspot.Set(SSID, mdpWifi); // Configuration du hotspot
                labelssid.Text = SSID;
                hotspot.Start(); //Démarrage hotspot
            }
            catch (Exception e1)
            {
                Console.WriteLine("Erreur hotspot : " + e1.StackTrace);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            settings f2 = new settings();
            f2.ShowDialog();
        }
    }
}
