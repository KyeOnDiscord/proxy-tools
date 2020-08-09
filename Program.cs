using System;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;
namespace ProxyGenerator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.Title = "Proxy Tools | Made by Kye | https://github.com/promasterboy";
            int selection = 0;
        start: Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Proxy Tools");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("");
            Console.WriteLine("===============");
            Console.WriteLine("");
            Console.WriteLine("Proxy Generator");
            Console.WriteLine("1 = HTTP");
            Console.WriteLine("2 = Socks4");
            Console.WriteLine("3 = Socks5");
            Console.WriteLine("");
            Console.WriteLine("===============");
            Console.WriteLine("");
            Console.WriteLine("4 = Proxy Checker");
            try
            {
                selection = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.Clear();
                goto start;
            }
            if (selection > 4)
            {
                Console.Clear();
                goto start;
            }
           
            if (selection == 1)
            {
                Console.WriteLine("Getting HTTP Proxies!");
                WriteProxy2("https://api.proxyscrape.com/?request=getproxies&proxytype=http&timeout=10000&country=all&ssl=all&anonymity=all", "https://raw.githubusercontent.com/clarketm/proxy-list/master/proxy-list-raw.txt", "HTTP");
            }
            if (selection == 2)
            {
                Console.WriteLine("Getting Socks4 Proxies!");
                WriteProxy("https://api.proxyscrape.com/?request=getproxies&proxytype=socks4&timeout=10000&country=all", "Socks4");
            }
            if (selection == 3)
            {
                Console.WriteLine("Getting Socks5 Proxies!");
                WriteProxy("https://api.proxyscrape.com/?request=getproxies&proxytype=socks5&timeout=10000&country=all", "Socks5");
            }
            
            if (selection == 4)
            {
                if (!File.Exists("good_proxies.txt"))
                {
                    File.Create("good_proxies.txt").Close();
                }
                if (!File.Exists("bad_proxies.txt"))
                {
                    File.Create("bad_proxies.txt").Close();
                }
                CheckProxies();
            }
            }
        public static int good { get; set; }
        public static int bad { get; set; }
        public static void CheckProxies()
        {
           
            Console.WriteLine("Select a proxy .txt file");
            Thread.Sleep(300);
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "txt files (*.txt)|*.txt";
            o.Title = "Select a Proxy";
            o.ShowDialog();
            Console.Clear();
            string[] proxies = File.ReadAllLines(o.FileName);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Checking Proxies");
            Console.WriteLine(" ");
            Console.WriteLine("============");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Good: " + good);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bad: " + bad);
            foreach (string proxy in proxies)
            {
                Thread.Sleep(800);
                string[] data = proxy.Split(':');
                if (ProxyCheck(data[0], int.Parse(data[1])) == true)
                {
                    string already = File.ReadAllText("good_proxies.txt");
                    using (StreamWriter writer = new StreamWriter("good_proxies.txt"))
                    {
                        writer.WriteLine(already + data[0] + ":" + data[1]);
                    }
                    good++;
                }
                else
                {
                    string already = File.ReadAllText("bad_proxies.txt");
                    using (StreamWriter writer = new StreamWriter("bad_proxies.txt"))
                    {
                        writer.WriteLine(already + data[0] + ":" + data[1]);
                    }
                    bad++;
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Checking Proxies");
                Console.WriteLine(" ");
                Console.WriteLine("============");
                Console.WriteLine(" ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Good: " + good);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Bad: " + bad);
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Proxy Results");
            Console.WriteLine(" ");
            Console.WriteLine("============");
            Console.WriteLine(" ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(good + " good proxies saved to " + AppDomain.CurrentDomain.BaseDirectory + "good_proxies.txt");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(bad + " bad proxies saved to " + AppDomain.CurrentDomain.BaseDirectory + "bad_proxies.txt");
            Console.ReadKey();
        }

        public static void WriteProxy(string url, string type)
        {
            string filename = type + "_proxies.txt";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            WebClient webClient = new WebClient();
            webClient.Timeout = 100000;
            string proxies = webClient.DownloadString(url);
            using (StreamWriter writer = new StreamWriter(filename))
            {
                    writer.Write(proxies);
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            int lineCount = File.ReadLines(filename).Count();
            Console.WriteLine("Exported " + lineCount + " proxies to: " + AppDomain.CurrentDomain.BaseDirectory + filename);
            Console.ReadKey();
        }
        
        public static void WriteProxy2(string url, string url2 ,string type)
        {
            string filename = type + "_proxies.txt";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            WebClient webClient = new WebClient();
            webClient.Timeout = 100000;
            string proxies = webClient.DownloadString(url);
            string proxies2 = webClient.DownloadString(url2);
            using (StreamWriter writer = new StreamWriter(filename))
            {
                    writer.Write(proxies);
                    writer.WriteLine(proxies2);
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            int lineCount = File.ReadLines(filename).Count();
            Console.WriteLine("Exported " + lineCount + " proxies to: " + AppDomain.CurrentDomain.BaseDirectory + filename);
            Console.ReadKey();
        }

        private static readonly string UserAgent = "Mozilla / 5.0(Windows NT 10.0; Win64; x64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 70.0.3538.77 Safari / 537.36";
        public static bool ProxyCheck(string ipAddress, int port)
        {
            try
            {
                ICredentials credentials = CredentialCache.DefaultCredentials;
                IWebProxy proxy = new WebProxy(ipAddress, port);
                proxy.Credentials = credentials;
                using (var wc = new WebClient())
                {
                    wc.Timeout = 5000;
                    wc.Proxy = proxy;
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("User-Agent", UserAgent);
                    string result = wc.DownloadString("http://google.com");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        private class WebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }
            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }
    }
}
