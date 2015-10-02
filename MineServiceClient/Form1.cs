using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineServiceClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tcpConnectButton_Click(object sender, EventArgs e)
        {
            TcpClient clientSocket = new TcpClient();
            clientSocket.Connect("127.0.0.1", 56552);


            StreamReader reader = new StreamReader(clientSocket.GetStream(), Encoding.ASCII);
            StreamWriter writer = new StreamWriter(clientSocket.GetStream(), Encoding.ASCII);
            string line;
            line = reader.ReadLine();
            Console.WriteLine("Message: " + line);
            writer.WriteLine("I Recived: " + line);
            writer.Flush();
        }
    }
}
