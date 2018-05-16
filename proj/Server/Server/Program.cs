using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Models;
using Server.Repository;


namespace Server
{
    class Program
    {
        static TcpListener listener;
        const int LIMIT = 5; //5 concurrent clients
        private static UserLoginRepository userLoginRepository;
        private static ArticleRepository articleRepository;

        static void Main(string[] args)
        {
            articleRepository = new ArticleRepository();
            userLoginRepository = new UserLoginRepository();
            Console.WriteLine("Program started..." + DateTime.Now.ToLongTimeString());



            listener = new TcpListener(IPAddress.Parse("10.211.55.5"), 2055);
            listener.Start();
            Console.WriteLine("Server mounted, listening to port 2055");
            List<Thread> currThreads = new List<Thread>();
            while (true)
            {
                if (currThreads.Count < LIMIT)
                {
                    Thread t = new Thread(new ThreadStart(Service));
                    t.Start();
                    currThreads.Add(t);
                }
                else
                {

                    for (int i = currThreads.Count - 1; i >= 0; i--)
                    {
                        if (currThreads[i].ThreadState != ThreadState.Running)
                        {
                            currThreads.RemoveAt(i);
                        }
                    }
                }

            }





            Console.WriteLine("Program stopped..." + DateTime.Now.ToLongTimeString());


        }



        public static void Service()
        {
            try
            {
                Socket soc = listener.AcceptSocket();
                Console.WriteLine("Connected: {0}", soc.RemoteEndPoint);

                NetworkStream s = new NetworkStream(soc);
                StreamReader sr = new StreamReader(s);
                StreamWriter sw = new StreamWriter(s);
                sw.AutoFlush = true;

                while (true)
                {
                    string command = sr.ReadLine();
                    if (command == "stop") break;

                    if (command == "waiting" || command == null)
                    {

                        continue;
                    }

                    if (command == "publish")
                    {
                        var json = sr.ReadLine();

                        ArticleModel article = JsonConvert.DeserializeObject<ArticleModel>(json);

                        articleRepository.PublishArticle(article.Author, article.Date, article.Title, article.Abstract, article.Body);

                        sw.WriteLine("published");
                    }

                    if (command == "login")
                    {
                        string token = sr.ReadLine();

                        UserModel user = JsonConvert.DeserializeObject<UserModel>(token);

                        if (userLoginRepository.Authorize(user.Username, user.Psssword))
                        {
                            sw.WriteLine(JsonConvert.SerializeObject(userLoginRepository.GetAuthor(user.Username)));
                           
                        }

                        sw.WriteLine("false");
                    }

                    if (command == "update")
                    {
                        var json = JsonConvert.SerializeObject(articleRepository.GetAllArticles().ToList());
                        sw.WriteLine(json);
                    }

                    if (command == "last")
                    {
                        DateTime received = JsonConvert.DeserializeObject<DateTime>(sr.ReadLine());

                        if (received.CompareTo(articleRepository.GetLatestArticleTime()) < 0)
                        {
                            sw.WriteLine("update");
                        }
                        else
                        {
                            sw.WriteLine("wait");
                        }
                    }
                }
                Console.WriteLine("Disconnected: {0}", soc.RemoteEndPoint);
                soc.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

    }
}
