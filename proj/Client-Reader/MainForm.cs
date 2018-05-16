using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Server.Models;

namespace Client_Reader
{
    
    public partial class MainForm : Form
    {
        private Thread t;
        private TcpClient client;
        private Stream s;
        private  StreamReader sr;
        private  StreamWriter sw;
        private List<ArticleModel> articles;
        public MainForm(TcpClient client, Stream s, StreamReader sr, StreamWriter sw)
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

        private void button1_Click(object sender, EventArgs e)
        {
            t.Abort();
            sw.WriteLineAsync("stop");
            s.Close();
            client.Close();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sw.WriteLine("update");

            var json = sr.ReadLine();

            articles = JsonConvert.DeserializeObject<List<ArticleModel>>(json) as List<ArticleModel>;

            textBox1.Text = articles.Count.ToString();

            


            var bindingList=new BindingList<ArticleModel>(articles);

            var source=new BindingSource(bindingList,null);

            dataGridView1.DataSource = source;

             t = new Thread(new ThreadStart(UpdateWorker));
            t.Start();
            //while (true)
            //{
            //    var art = articles.OrderByDescending(p => p.Date).FirstOrDefault();
            //    DateTime last = art == null ? DateTime.MinValue : art.Date;

            //    sw.WriteLine("last");
            //    sw.WriteLine(JsonConvert.SerializeObject(last));

            //    var resp = sr.ReadLine();
            //    if (resp == "update")
            //    {
            //        sw.WriteLine("update");

            //         json = sr.ReadLine();

            //        articles = JsonConvert.DeserializeObject<List<ArticleModel>>(json) as List<ArticleModel>;

            //        textBox1.Text = articles.Count.ToString();




            //         bindingList = new BindingList<ArticleModel>(articles);

            //         source = new BindingSource(bindingList, null);

            //        dataGridView1.DataSource = source;
            //    }
            //}
        }
        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBox1.Text= value;
        }

        public void RefreshDataSource(BindingSource source)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<BindingSource>(RefreshDataSource), new object[] {source});
            }

            dataGridView1.DataSource = source;
        }

        public void RefreshArticles(List<ArticleModel> articleslist)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<List<ArticleModel>>(RefreshArticles), new object[] { articleslist });
            }

            articles = articleslist;
        }
        public  void UpdateWorker()
        {
            while (true)
            {
                Thread.Sleep(200);
                sw.WriteLine("waiting");
                Thread.Sleep(200);


                var art = articles.OrderByDescending(p => p.Date).FirstOrDefault();
                DateTime last = art == null ? DateTime.MinValue : art.Date;
                sw.WriteLine("last");


                sw.WriteLine(JsonConvert.SerializeObject(last));
                Thread.Sleep(300);


                var resp = sr.ReadLine();

                if (resp == "update")
                {
                    sw.WriteLine("update");
                    Thread.Sleep(300);


                    var json = sr.ReadLine();

                    RefreshArticles(JsonConvert.DeserializeObject<List<ArticleModel>>(json) as List<ArticleModel>);

                  //  textBox1.Text = articles.Count.ToString();

                    AppendTextBox(articles.Count.ToString());


                    var bindingList = new BindingList<ArticleModel>(articles);

                    var source = new BindingSource(bindingList, null);

                   // dataGridView1.DataSource = source;
                    RefreshDataSource(source);
                    Thread.Sleep(100);

                }



            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
