using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetTest
{

    public partial class Form1 : Form
    {
        private int port = 6969;
        private string ip = "185.71.232.171";
        SimpleTcpClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
            client.DataReceived += Client_DataReceived;
        }
        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            //Update message to txtStatus
            textBox2.Invoke((MethodInvoker)delegate ()
            {
                textBox2.Text += e.MessageString;
            });
        }

        private void connectBtn_Click(object sender, EventArgs e)
        {
           
            connectBtn.Enabled = false;
            //Connect to server
            client.Connect(ip, Convert.ToInt32(port));
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {

            var buffer = new MemoryStream();
            var WRITTER = new BinaryWriter(buffer);
            WRITTER.Write(1);
            WRITTER.Write(1);
            WRITTER.Write('@');
            WRITTER.Close();
            byte[] bytes = buffer.ToArray();

            client.Write(bytes);
        }
    }
}
