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
            connectBtn.Enabled = false;
            client.Connect(ip, Convert.ToInt32(port));
        }
        private void Client_DataReceived(object sender, SimpleTCP.Message e)
        {
            if(e.MessageString == "X")
            {
                type = 'X';
            }
            else if(e.MessageString == "O")
            {
                type = 'O';
            }
            else
            {
                MessageBox.Show(e.MessageString);
                this.Close();
            }

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

        private void DrawLines(Graphics g)
        {
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
            symbolO = Image.FromFile(imgDir + "Symbol-O.png");
        }

        private void TestSymbols()
        {
            GameInfo gameInfo = new GameInfo();
            gameInfo.x = 2;
            gameInfo.y = 3;
            gameInfo.type = type;
            this.gameInfos.Add(gameInfo);
        }

        private bool SendMessage(GameInfo newGameInfo)
        {
            var buffer = new MemoryStream();
            var WRITTER = new BinaryWriter(buffer);

            WRITTER.Write(newGameInfo.x);
            WRITTER.Write(newGameInfo.y);
            WRITTER.Write(newGameInfo.type);
            WRITTER.Close();
            byte[] bytes = buffer.ToArray();

            client.Write(bytes);

            return false;
        }

        private void DrawGameSymbols(Graphics g)
        {
            foreach (var gi in this.gameInfos)
            {
                if(type == 'X')
                {
                    g.DrawImage(symbolX, new Point(gi.x * size, gi.y * size));
                }
                else
                {
                    g.DrawImage(symbolO, new Point(gi.x * size, gi.y * size));
                }
            }

            Pen blackPen = new Pen(Color.Black);

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //var g = pictureBox1.CreateGraphics();
            DrawLines(e.Graphics);
            DrawGameSymbols(e.Graphics);

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

            pictureBox1.Refresh();
            SendMessage(gameInfo);
        }
    }
}
