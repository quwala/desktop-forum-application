using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GUI;
using client;

namespace Main
{
    class Main
    {

        public static void main(string[] args)
        {
            Iclient client = new client.client();
            client.start();
            new Application().Run(new MainWindow(client));
        }
    }
}