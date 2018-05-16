using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        private static TcpClient client;
        private static Stream s;
        private static StreamReader sr;
        private static StreamWriter sw;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            client = new TcpClient("10.211.55.5", 2055);
            s = client.GetStream();
            sr = new StreamReader(s);
            sw = new StreamWriter(s);
            sw.AutoFlush = true;

            Thread t = new Thread(new ThreadStart(Worker));
            t.Start();

            
        }

        public static void Worker()
        {
            Application.Run(new MainForm(client, s, sr, sw));

        }
    }
}
