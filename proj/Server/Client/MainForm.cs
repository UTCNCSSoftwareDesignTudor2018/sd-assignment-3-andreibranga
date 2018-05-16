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
using Newtonsoft.Json;
using Server.Models;

namespace Client
{
    public partial class MainForm : Form
    {
        private  TcpClient client;
        private  Stream s;
        private  StreamReader sr;
        private  StreamWriter sw;

        private bool isAuthorized = false;
        private string author = "";
        public MainForm(TcpClient client,Stream s,StreamReader sr,StreamWriter sw)
        {
            InitializeComponent();
            this.client = client;
            this.s = s;
            this.sw = sw;
            this.sr = sr;

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            UserModel user=new UserModel();
            user.Username = textBox1.Text;
            user.Psssword = textBox2.Text;

            sw.WriteLine("login");
            
            sw.WriteLine(JsonConvert.SerializeObject(user));
            var resp = JsonConvert.DeserializeObject<string>(sr.ReadLine());
            if (resp != "false")
            {
                isAuthorized = true;

                author = resp;

                panel1.Visible = true;
                textBox2.Enabled = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
            }
            else
            {
                label3.Visible = true;
            }

            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLineAsync("stop");
            s.Close();
            client.Close();
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ArticleModel articleModel=new ArticleModel();
            articleModel.Date=DateTime.Now;
            articleModel.Author = author;
            articleModel.Title = textBox3.Text;
            articleModel.Abstract = richTextBox1.Text;
            articleModel.Body = richTextBox2.Text;

            var json = JsonConvert.SerializeObject(articleModel);

            sw.WriteLine("publish");
            sw.WriteLine(json);

          
                label4.Visible = true;

                textBox3.Clear();
                richTextBox2.Clear();
                richTextBox1.Clear();
           
                
        }
    }
}
