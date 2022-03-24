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
        private Game game;
        private List<GameInfo> gameInfos;
        private const int size = 30;
        private char type = 'X';
        private bool gameOver = false;
        private Pen blackPen = new Pen(Color.Black);
        private static string rootDir = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
        private static string imgDir = rootDir + "\\images\\";
        private static Image symbolO;
        private static Image symbolX;

        public Form1()
        {
            InitializeComponent();
            gameInfos = new List<GameInfo>();
            game = new Game();
            LoadImages();
            TestSymbols();
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
            WRITTER.Write('@');
            WRITTER.Close();
            byte[] bytes = buffer.ToArray();

            client.Write(bytes);
        }

        private void DrawLines()
        {
            var g = pictureBox1.CreateGraphics();

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    g.DrawRectangle(blackPen, j*size, i* size, size, size);
                }
            }
        }

        private void LoadImages()
        {
            symbolX = Image.FromFile(imgDir + "Symbol-X.png");
            symbolX = Image.FromFile(imgDir + "Symbol-O.png");
        }

        private void TestSymbols()
        {
            GameInfo gameInfo = new GameInfo();
            gameInfo.x = 2;
            gameInfo.y = 3;
            gameInfo.type = type;
            this.gameInfos.Add(gameInfo);
        }

        private void DrawGameSymbols()
        {
            var g = pictureBox1.CreateGraphics();

            foreach (var gi in this.gameInfos)
            {
                g.DrawImage(symbolX, new Point(gi.x * size, gi.y * size));
            }

            Pen blackPen = new Pen(Color.Black);

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //var g = pictureBox1.CreateGraphics();
            DrawLines();
            DrawGameSymbols();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            GameInfo gameInfo = new GameInfo();
            gameInfo.x = e.X / 30;
            gameInfo.y = e.Y / 30;
            gameInfo.type = type;
            this.gameInfos.Add(gameInfo);
        }
    }
}
