using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Script.Serialization;
using System.Threading;
using System.Collections;
using System.IO;
using System.Net.NetworkInformation;
using WSEP_service.forumManagement;
using WSEP_domain.forumManagement.forumHandler;
using WSEP_domain.userManagement;
using serverService;

namespace server
{
    class Program
    {

        static Hashtable inputSockets = Hashtable.Synchronized(new Hashtable());
        static Hashtable outputSockets = Hashtable.Synchronized(new Hashtable());

        static Hashtable loggedInUsers = Hashtable.Synchronized(new Hashtable());
       

        static int userID = 0;
        //static int numOfUsers = 0;


        public static string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        static void Main(string[] args)
        {

            bool isTest = false;
            if(args.Length == 1)
            {
                isTest = args[0].Equals("true");
            }

            isTest = true;

            IserverService service = new serverService.serverService(isTest);
            string ip = GetIP4Address();
          

            IPAddress ipAddress = IPAddress.Parse(ip);
            TcpListener listener = new TcpListener(ipAddress, 8000);
            //IPAddress ipAddressForNotif = IPAddress.Parse("10.0.0.5");
            TcpListener listenerForNotif = new TcpListener(ipAddress, 8001);
           // Console.WriteLine("local end point: " + listener.LocalEndpoint);
            
            listener.Start();
            listenerForNotif.Start();
            while (true)
            {
                Socket s = listener.AcceptSocket();
                Socket s2 = listenerForNotif.AcceptSocket();
                userID++;
                inputSockets.Add(userID, s);
                outputSockets.Add(userID, s2);
                Thread thread = new Thread(threadFunction);
                List<object> list = new List<object>();
                list.Add(s);
                list.Add(s2);
                int id = userID;
                list.Add(id);
                list.Add(service);
                thread.Start(list);
            }
        }



        public static void threadFunction(object s1)
        {
            
            
            ASCIIEncoding asen = new ASCIIEncoding();
            List<object> list = (List<object>)s1;
            Socket guiSocket = (Socket)list.ElementAt(0);
            Socket notifSocket = (Socket)list.ElementAt(1);
            int ID = (int)list.ElementAt(2);
            IserverService service = (IserverService)list.ElementAt(3);
          //  Console.WriteLine("remote endpoint: " + s.RemoteEndPoint);
            //Console.WriteLine("remote endpoint: " + s2.RemoteEndPoint);
            try
            {
                guiSocket.Send(asen.GetBytes(ID.ToString()));
            }
            catch (Exception)
            {
                inputSockets.Remove(ID);
                outputSockets.Remove(ID);
                guiSocket.Close();
                notifSocket.Close();
                return;
            }
            byte[] b = new byte[1000000];
            int n = 0;
           

            while (true)
            {
                try
                {
                    n = guiSocket.Receive(b);
                }
                catch (SocketException) {
                    inputSockets.Remove(ID);
                    outputSockets.Remove(ID);
                    guiSocket.Close();
                    notifSocket.Close();
                    break;
                }
                string str = "";
                for (int i = 0; i < n; i++)
                {

                    str = str + Convert.ToChar(b[i]);
                }
                n = 0;
                if (!str.Equals(""))
                {
                    //string send_back = service.parseMessage(str);
                    Tuple<string, string, List<Tuple<string, string>>> ans = service.parseMessage2(str);
                    string send_back = ans.Item1;
                    string content = ans.Item2;
                    if(content.Equals("login"))
                    {
                        Tuple<string, string> pair = ans.Item3.ElementAt(0);
                        string forumName = pair.Item1;
                        string name = pair.Item2;
                      //  Console.WriteLine("got forum {0} and user {1}", forumName, name);
                        
                        if (!loggedInUsers.ContainsKey(pair))
                        {
                           // Console.WriteLine("adding pair to llogedin users");
                            loggedInUsers.Add(pair, notifSocket);
                        }
                    }
                   
                        foreach (int id in outputSockets.Keys)
                        {
                            if(id != ID){
                                Socket temp =(Socket) outputSockets[id];
                                temp.Send(asen.GetBytes("false " + content));
                            }
                        }
                    if ((!content.Equals("login")) & (ans.Item3.Count > 0))
                    {
                       // Console.WriteLine("got here");
                        foreach (Tuple<string, string> pair in loggedInUsers.Keys)
                        {
                            //Console.WriteLine("pair: first argument is {0} and second is {1}", pair.Item1, pair.Item2);
                            Socket temp = (Socket)loggedInUsers[pair];
                            if (temp.Connected && ans.Item3.Contains(pair))
                            {
                                Console.WriteLine("found user to notif!!!!!!!!!!!!!!!!!");
                                temp.Send(asen.GetBytes("true " + content));
                            }
                            else
                            {
                                loggedInUsers.Remove(pair);
                            }
                        }
                    }
                    
                    guiSocket.Send(asen.GetBytes(send_back));
                }
            
            }
        }
    }
}
