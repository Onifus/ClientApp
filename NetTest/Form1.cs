using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
             client.WriteLineAndGetReply(textBox1.Text, TimeSpan.FromSeconds(3));
        }
    }
}
