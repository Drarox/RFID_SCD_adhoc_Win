using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace RFID_SCD
{
    class TCP
    {
        Socket client;
        TcpListener listener;

        //Configuration Serveur TCP
        public bool ServerTCP(IPAddress ip, int port)
        {
            try
            {
                listener = new TcpListener(ip, port);
                listener.Start();
                client = listener.AcceptSocket(); //Attente connexion
                //UpdateProgress(true, null);
                Console.WriteLine("Config TCP terminé");
                return true;
            }
            catch (Exception e1)
            {
                Console.WriteLine("Erreur création Serveur : " + e1.StackTrace);
                return false;
            }
        }

        public bool GetStatus()
        {
            return client.Connected;
        }

        //Réception des données 
        public String Reception()
        {
            try
            {
                int size = 0;
                byte[] data = new byte[client.Available];
                if (client.Available > 0)
                {
                    size = client.Receive(data); //Réception ici

                    if (client.Available != size)
                        Console.WriteLine("Error size");

                    String text = Encoding.UTF8.GetString(data); //Conversion en string utf8
                    Console.WriteLine("TCP données reçu : " + text);
                    //UpdateProgress(null, text);
                    return text;
                }
                return null;
            }
            catch (Exception e1)
            {
                Console.WriteLine("Erreur réception : " + e1.StackTrace);
                return null;
            }
        }

        //Arret
        public void Stop()
        {
            client.Close();
            listener.Stop();
        }

    }
}
