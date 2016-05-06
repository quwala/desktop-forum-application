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

namespace server
{
    class Program
    {

        static Hashtable inputSockets = Hashtable.Synchronized(new Hashtable());
        static Hashtable outputSockets = Hashtable.Synchronized(new Hashtable());
        static Hashtable activeForums = Hashtable.Synchronized(new Hashtable());

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
            string ip = GetIP4Address();
            IForumSystem forumSystem;
            if (isTest)
            {
                forumSystem = new ForumSystem(true);
            }
            else
            {
                forumSystem = new ForumSystem();
            }

            IPAddress ipAddress = IPAddress.Parse(ip);
            TcpListener listener = new TcpListener(ipAddress, 8000);
            //IPAddress ipAddressForNotif = IPAddress.Parse("10.0.0.5");
            TcpListener listenerForNotif = new TcpListener(ipAddress, 8001);
            Console.WriteLine("local end point: " + listener.LocalEndpoint);
            
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
                list.Add(forumSystem);
                thread.Start(list);
            }
        }

        public static bool checkArgs(List<string> lst, int num)
        {
            return !(lst.Count == num);
        }

        public static bool checkArgs2(List<int> lst, int num)
        {
            return !(lst.Count >= num);
        }

        public static bool checkArgs2(List<string> lst, int num)
        {
            return !(lst.Count >= num);
        }

        public static bool checkArgs(List<int> lst, int num)
        {
            return !(lst.Count == num);
        }

        public static bool checkArgs(List<string> lst1, int num1, List<int> lst2, int num2)
        {
            return !((lst1.Count == num1) & (lst2.Count == num2));
        }

        public static bool checkArgs(List<string> lst1, int num1, List<int> lst2, int num2, DateTime time)
        {
            return !((lst1.Count == num1) & (lst2.Count == num2) & (time != null));
        }

        public static void threadFunction(object s1)
        {
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ASCIIEncoding asen = new ASCIIEncoding();
            List<object> list = (List<object>)s1;
            Socket guiSocket = (Socket)list.ElementAt(0);
            Socket notifSocket = (Socket)list.ElementAt(1);
            int ID = (int)list.ElementAt(2);
            IForumSystem forumSystem = (IForumSystem)list.ElementAt(3);
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
                    serverMessage message = serializer.Deserialize<serverMessage>(str);
                    message.writeData();

                    serverMessage send_back;
                    List<string> strLst = new List<string>();
                    List<int> intLst = new List<int>();
                    DateTime date = new DateTime();
                    string forumName, user,scndUser, subforumName, ans, password;
                    int number;
                    
                    switch (message._messageType)
                    {
                        case serverMessage.messageType.addForum:
                            if (checkArgs(message.stringContent, 2))
                            {
                                strLst.Add("error, needed 2 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            ans = forumSystem.addForum(forumName, user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back))); 
                            break;
                        case serverMessage.messageType.addSubForum:
                            if (checkArgs2(message.stringContent, 4) & (checkArgs(message.intContent, message.stringContent.Count - 3)))
                            {
                                strLst.Add("error, arguments not valid");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            List<Tuple<string, string, int>> tupleList = new List<Tuple<string, string, int>>();
                            for (int i = 0; i < (message.stringContent.Count - 3); i++)
                            {
                                tupleList.Add(new Tuple<string,string,int>(message.stringContent.ElementAt(i + 3),"",message.intContent.ElementAt(i)));
                            }

                            ans = forumSystem.addSubForum(forumName, subforumName,tupleList, user );
                            if (ans.Equals("true"))
                            { 
                                 send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                 guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                 goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.assignAdmin:
                            if (checkArgs(message.stringContent, 3))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);

                            ans = forumSystem.assignAdmin(forumName, user, scndUser);
                            Console.WriteLine(ans);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.registerMemberToForum:
                           if (checkArgs(message.stringContent, 4))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            password = message.stringContent.ElementAt(2);
                            string mail = message.stringContent.ElementAt(3);
                            ans = forumSystem.registerMemberToForum(forumName, user, password, mail);
                            if (ans.Equals("true"))
                            {
                                 send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                 guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                 goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.unassignAdmin:
                            if (checkArgs(message.stringContent, 3))
                            {
                                strLst.Add("error, needed 3 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);
                            ans = forumSystem.unassignAdmin(forumName, user, scndUser);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.assignModerator:
                            if ((checkArgs(message.stringContent, 4)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            scndUser = message.stringContent.ElementAt(3);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.assignModerator(forumName,subforumName , user, scndUser, number);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.unassignModerator:
                            if (checkArgs(message.stringContent, 4))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            scndUser = message.stringContent.ElementAt(3);
                            ans = forumSystem.unassignModerator(forumName, subforumName, user, scndUser);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.sendPM:
                            if (checkArgs(message.stringContent, 4))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);
                            ans = message.stringContent.ElementAt(3);
                            ans = forumSystem.sendPM(forumName, user, scndUser, ans);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumMaxAdmins:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumMaxAdmins(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumMinAdmins:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumMinAdmins(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumMaxModerators:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumMaxModerators(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumMinModerators:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumMinModerators(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumPostDeletionPermissions:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            postDeletionPermission pdp = postDeletionPermission.WRITER;
                            switch (number)
                            {
                                case 2:
                                    pdp = postDeletionPermission.WRITER;
                                    break;
                                case 3:
                                    pdp = postDeletionPermission.MODERATOR;
                                    break;
                                case 4:
                                    pdp = postDeletionPermission.ADMIN;
                                    break;
                                case 5:
                                    pdp = postDeletionPermission.SUPER_ADMIN;
                                    break;
                                default:
                                    send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                    goto end;
                            }
                            ans = forumSystem.setForumPostDeletionPermissions(forumName, pdp ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumPasswordLifespan:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumPasswordLifespan(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumModeratorsSeniority:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setForumModeratorsSeniority(forumName, number ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setForumModUnassignmentPermissions:
                            if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            number = message.intContent.ElementAt(0);
                            modUnassignmentPermission mup = modUnassignmentPermission.ASSIGNING_ADMIN;
                            switch (number)
                            {
                                case 2:
                                    mup = modUnassignmentPermission.ADMIN;
                                    break;
                                case 3:
                                    mup = modUnassignmentPermission.ASSIGNING_ADMIN;
                                    break;
                                case 4:
                                    mup = modUnassignmentPermission.SUPER_ADMIN;
                                    break;
                                default:
                                    send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                    goto end;
                            }
                            ans = forumSystem.setForumModUnassignmentPermissions(forumName, mup ,user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.writePost:
                            if ((checkArgs(message.stringContent, 5)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            ans = message.stringContent.ElementAt(3);
                            password = message.stringContent.ElementAt(4);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.writePost(forumName,subforumName, number ,user, ans, password);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.setModeratorTrialTime:
                            if ((checkArgs(message.stringContent, 4)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            scndUser = message.stringContent.ElementAt(3);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.setModeratorTrialTime(forumName,subforumName,user, number ,scndUser);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.deletePost:
                            if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.deletePost(forumName,subforumName,number, user);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.editPost:
                            if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            ans = message.stringContent.ElementAt(3);
                            number = message.intContent.ElementAt(0);
                            ans = forumSystem.editPost(forumName,subforumName,number, user, ans);
                            if (ans.Equals("true"))
                            {
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            strLst.Add(ans);
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getNumOfPostsInSubForum:
                            if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            number = forumSystem.getNumOfPostsInSubForum(forumName,subforumName,user);
                            if (number != -1)
                            {
                                intLst.Add(number);
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getListOfMemberMessages:
                            if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);
                            List<Tuple<string, string, DateTime, int>> returnForMemMesages = new List<Tuple<string, string, DateTime, int>>();
                            returnForMemMesages = forumSystem.getListOfMemberMessages(forumName,user,scndUser);
                            if (returnForMemMesages != null)
                            {
                                if(returnForMemMesages.Count != 0)
                                {
                                    specialServerMessage send_back_spec = new specialServerMessage(specialServerMessage.messageType.success, returnForMemMesages);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec)));
                                    goto end;
                                }
                            }
                            specialServerMessage send_back_spec1 = new specialServerMessage(specialServerMessage.messageType.unsuccess, returnForMemMesages);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec1)));
                            break;
                        case serverMessage.messageType.getListOfForummoderators:
                            if ((checkArgs(message.stringContent, 2)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            List<Tuple<string, string, DateTime, string>> returnForMemMesages2 = new List<Tuple<string, string, DateTime, string>>();
                            returnForMemMesages2 = forumSystem.getListOfForumModerators(forumName,user);
                            if (returnForMemMesages2 != null)
                            {
                                if(returnForMemMesages2.Count != 0)
                                {
                                    specialServerMessage2 send_back_spec = new specialServerMessage2(specialServerMessage2.messageType.success, returnForMemMesages2);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec)));
                                    goto end;
                                }
                            }
                            specialServerMessage2 send_back_spec2 = new specialServerMessage2(specialServerMessage2.messageType.unsuccess, returnForMemMesages2);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec2)));
                            break;
                        case serverMessage.messageType.numOfForums:
                            if ((checkArgs(message.stringContent,1)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            user = message.stringContent.ElementAt(0);
                            number = forumSystem.numOfForums(user);
                            if (number != -1)
                            {
                                intLst.Add(number);
                                send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            send_back = new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getForums:
                            List<string> forums = forumSystem.getForums();
                            send_back = new serverMessage(serverMessage.messageType.success, forums, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getSubForums:
                            if ((checkArgs(message.stringContent, 2)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            List<string> subForums = forumSystem.getSubForums(forumName, user);
                            send_back = new serverMessage(serverMessage.messageType.success, subForums, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getThreads:
                            if ((checkArgs(message.stringContent, 3)) )
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            List<Tuple<string, DateTime, int>> returnForMemMesages3 = new List<Tuple<string, DateTime, int>>();
                            returnForMemMesages3 = forumSystem.getThreads(forumName,subforumName,user);
                            if (returnForMemMesages3 != null)
                            {
                                if(returnForMemMesages3.Count != 0)
                                {
                                    specialServerMessage3 send_back_spec = new specialServerMessage3(specialServerMessage3.messageType.success, returnForMemMesages3);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec)));
                                    goto end;
                                }
                            }
                            specialServerMessage3 send_back_spec3 = new specialServerMessage3(specialServerMessage3.messageType.unsuccess, returnForMemMesages3);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec3)));
                            break;
                        case serverMessage.messageType.getThread:
                            if ((checkArgs(message.stringContent, 3)) & checkArgs(message.intContent,1))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            number = message.intContent.ElementAt(0);
                            List<Tuple<string, string, DateTime, int, int, string, DateTime>> returnForMemMesages4 = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
                            returnForMemMesages4 = forumSystem.getThread(forumName, subforumName, number, user);
                            if (returnForMemMesages4 != null)
                            {
                                if(returnForMemMesages4.Count != 0)
                                {
                                    specialServerMessage4 send_back_spec = new specialServerMessage4(specialServerMessage4.messageType.success, returnForMemMesages4);
                                    guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec)));
                                    goto end;
                                }
                            }
                            specialServerMessage4 send_back_spec4 = new specialServerMessage4(specialServerMessage4.messageType.unsuccess, returnForMemMesages4);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back_spec4)));
                            break;
                        case serverMessage.messageType.login:
                            if ((checkArgs(message.stringContent, 3)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            password = message.stringContent.ElementAt(2);
                            loginStatus ls = forumSystem.login(forumName, user, password);
                            number = 1;
                            switch (ls)
                            {
                                case loginStatus.FALSE:
                                    number = 1;
                                    break;
                                case loginStatus.TRUE:
                                    number = 2;
                                    break;
                                case loginStatus.UPDATE_PASSWORD:
                                    number = 3;
                                    break;
                            }
                            intLst.Add(number);
                            send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getUserPermissionsForForum:
                            if ((checkArgs(message.stringContent, 2)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            permission p = forumSystem.getUserPermissionsForForum(forumName, user);
                            number = 1;
                            switch (p)
                            {
                                case permission.INVALID:
                                    number = 1;
                                    break;
                                case permission.GUEST:
                                    number = 2;
                                    break;
                                case permission.MEMBER:
                                    number = 3;
                                    break;
                                case permission.ADMIN:
                                    number = 5;
                                    break;
                                case permission.SUPER_ADMIN:
                                    number = 6;
                                    break;
                            }
                            intLst.Add(number);
                            send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                        case serverMessage.messageType.getUserPermissionsForSubForum:
                            if ((checkArgs(message.stringContent, 2)))
                            {
                                strLst.Add("error");
                                send_back = new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                                goto end;
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            permission p1 = forumSystem.getUserPermissionsForSubForum(forumName, subforumName, user);
                            number = 1;
                            switch (p1)
                            {
                                case permission.INVALID:
                                    number = 1;
                                    break;
                                case permission.GUEST:
                                    number = 2;
                                    break;
                                case permission.MEMBER:
                                    number = 3;
                                    break;
                                case permission.MODERATOR:
                                    number = 4;
                                    break;
                                case permission.ADMIN:
                                    number = 5;
                                    break;
                                case permission.SUPER_ADMIN:
                                    number = 6;
                                    break;
                            }
                            intLst.Add(number);
                            send_back = new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            guiSocket.Send(asen.GetBytes(serializer.Serialize(send_back)));
                            break;
                    }
                    end: ;
                }
                //notifSocket.Send(asen.GetBytes("something"));
            }
        }
    }
}
