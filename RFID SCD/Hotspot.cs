using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_SCD
{
    //...
    //Classe pour créer un point d'accès
    //...

    class Hotspot
    {
        //Exéctuer une commande cmd
        private void Commande(string com)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine(com);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
        }

        //Paramétrage du hotspot
        public void Set(string SSID, string key)
        {
            Commande("netsh wlan set hostednetwork mode=allow ssid =" + SSID + " key =" + key);
            Console.WriteLine("Paramàtrage WIFI");
        }

        //Démarrage du hotspot
        public void Start()
        {
            Commande("netsh wlan start hostednetwork");
            Console.WriteLine("WIFI Start");
        }

        //Arrêt du hotspot
        public void Stop()
        {
            Commande("netsh wlan stop hostednetwork");
            Console.WriteLine("WIFI Stop");
        }
    }
}
