using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace client
{
    public class client : Iclient
    {


        static ASCIIEncoding asen = new ASCIIEncoding();
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        Stream outputStream;
        Stream inputStream;
        byte[] input_buffer;
        byte[] notif_buffer;
        int ID;
        private readonly object syncLock;
        

  

        public client()
        {
            input_buffer = new byte[1000000];
            notif_buffer = new byte[100000];
            ID = 0;
            syncLock = new object();
        }

        public client(bool testing)
        {
            input_buffer = new byte[1000000];
            notif_buffer = new byte[100000];
            ID = 0;
            syncLock = new object();
        }

        public string addForum(string forumName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("addForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(requestingUser);
                foreach (Tuple<string, string, int> t in moderators)
                {
                    strList.Add(t.Item1);
                    strList.Add(t.Item2);
                    intList.Add(t.Item3);
                }
                serverMessage ans = sendMessage("addSubForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string registerMemberToForum(string forumName, string username, string password, string eMail)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(username);
                strList.Add(password);
                strList.Add(eMail);
                serverMessage ans = sendMessage("registerMemberToForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string assignAdmin(string forumName, string username, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(username);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("assignAdmin", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }


        public string unassignAdmin(string forumName, string username, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(username);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("unassignAdmin", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(username);
                strList.Add(requestingUser);
                intList.Add(days);
                serverMessage ans = sendMessage("assignModerator", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string unassignModerator(string forumName, string subForumName, string username, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(username);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("unassignModerator", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public permission getUserPermissionsForForum(string forumName, string username)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(username);
                serverMessage ans = sendMessage("getUserPermissionsForForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    switch (ans.intContent.ElementAt(0))
                    {
                        case 1:
                            return permission.INVALID;
                        case 2:
                            return permission.GUEST;
                        case 3:
                            return permission.MEMBER;
                        case 5:
                            return permission.ADMIN;
                        case 6:
                            return permission.SUPER_ADMIN;
                    }
                }
                return permission.INVALID;
            }
            }


        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(username);
                serverMessage ans = sendMessage("getUserPermissionsForSubForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    switch (ans.intContent.ElementAt(0))
                    {
                        case 1:
                            return permission.INVALID;
                        case 2:
                            return permission.GUEST;
                        case 3:
                            return permission.MEMBER;
                        case 4:
                            return permission.MODERATOR;
                        case 5:
                            return permission.ADMIN;
                        case 6:
                            return permission.SUPER_ADMIN;
                    }
                }
                return permission.INVALID;
            }
        }

        public string sendPM(string forumName, string from, string to, string msg)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(from);
                strList.Add(to);
                strList.Add(msg);
                serverMessage ans = sendMessage("sendPM", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string checkForumPolicy(string forumName, ForumPolicy policy)
        {
            lock (syncLock)
            {
                serverMessage ans = sendMessage(forumName, policy);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public loginStatus login(string forumname, string username, string password)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumname);
                strList.Add(username);
                strList.Add(password);
                serverMessage ans = sendMessage("login", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    switch (ans.intContent.ElementAt(0))
                    {
                        case 1:
                            return loginStatus.FALSE;
                        case 2:
                            return loginStatus.TRUE;
                        case 3:
                            return loginStatus.UPDATE_PASSWORD;

                    }
                }
                return loginStatus.FALSE;
            }
        }

        public string setForumMaxAdmins(string forumName, int maxAdmins, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(maxAdmins);
                serverMessage ans = sendMessage("setForumMaxAdmins", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumMinAdmins(string forumName, int minAdmins, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(minAdmins);
                serverMessage ans = sendMessage("setForumMinAdmins", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumMaxModerators(string forumName, int maxModerators, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(maxModerators);
                serverMessage ans = sendMessage("setForumMaxModerators", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumMinModerators(string forumName, int minModerators, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(minModerators);
                serverMessage ans = sendMessage("setForumMinModerators", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                int toAdd = 0;
                switch (permission)
                {
                    case postDeletionPermission.WRITER:
                        toAdd = 2;
                        break;
                    case postDeletionPermission.MODERATOR:
                        toAdd = 3;
                        break;
                    case postDeletionPermission.ADMIN:
                        toAdd = 4;
                        break;
                    case postDeletionPermission.SUPER_ADMIN:
                        toAdd = 5;
                        break;
                }
                intList.Add(toAdd);
                serverMessage ans = sendMessage("setForumPostDeletionPermissions", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumPasswordLifespan(string forumName, int lifespan, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(lifespan);
                serverMessage ans = sendMessage("setForumPasswordLifespan", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumModeratorsSeniority(string forumName, int seniority, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                intList.Add(seniority);
                serverMessage ans = sendMessage("setForumModeratorsSeniority", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                int toAdd = 0;
                switch (permission)
                {
                    case modUnassignmentPermission.ASSIGNING_ADMIN:
                        toAdd = 3;
                        break;
                    case modUnassignmentPermission.SUPER_ADMIN:
                        toAdd = 4;
                        break;
                    case modUnassignmentPermission.ADMIN:
                        toAdd = 2;
                        break;
                }
                intList.Add(toAdd);
                serverMessage ans = sendMessage("setForumModUnassignmentPermissions", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, string content)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(username);
                strList.Add(title);
                strList.Add(content);
                intList.Add(parentPostNo);
                serverMessage ans = sendMessage("writePost", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(moderator);
                strList.Add(requestingUser);
                intList.Add(newTime);
                serverMessage ans = sendMessage("setModeratorTrialTime", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string deletePost(string forumName, string subForumName, int postNo, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(requestingUser);
                intList.Add(postNo);
                serverMessage ans = sendMessage("deletePost", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public string editPost(string forumName, string subForumName, int postNo, string requestingUser, string content)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(requestingUser);
                strList.Add(content);
                intList.Add(postNo);
                serverMessage ans = sendMessage("editPost", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return "true";
                }
                if (ans.stringContent.Count > 0)
                {
                    return ans.stringContent.ElementAt(0);
                }
                return "error, not successful for unknown reason";
            }
        }

        public int getNumOfPostsInSubForum(string forumName, string subForumName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(subForumName);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("getNumOfPostsInSubForum", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return ans.intContent.ElementAt(0);
                }
                return -1;
            }
        }

        public List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> temp = new List<string>();
                temp.Add(forumName);
                temp.Add(memberName);
                temp.Add(requestingUser);
                serverMessage ans = new serverMessage(1, temp);
                return sendMessage(ans).ListOfMemberMessages;
            }
        }

        public List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> temp = new List<string>();
                temp.Add(forumName);
                temp.Add(requestingUser);
                serverMessage ans = new serverMessage(2, temp);
                return sendMessage(ans).ListOfForumModerators;
            }
        }

        public int numOfForums(string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("numOfForums", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return ans.intContent.ElementAt(0);
                }
                return -1;
            }
        }

        public List<string> ForumsByUser(string req_user, string target_user) {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                serverMessage ans = sendMessage("ForumsByUser", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return ans.stringContent;
                }

                return new List<string>();
            }
        
        }

        public List<string> getForums()
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                serverMessage ans = sendMessage("getForums", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return ans.stringContent;
                }

                return new List<string>();
            }
        }

        public List<string> getSubForums(string forumName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> strList = new List<string>();
                List<int> intList = new List<int>();
                strList.Add(forumName);
                strList.Add(requestingUser);
                serverMessage ans = sendMessage("getSubForums", strList, intList, DateTime.Now);
                if (ans._messageType.Equals(serverMessage.messageType.success))
                {
                    return ans.stringContent;
                }

                return new List<string>();
            }
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> temp = new List<string>();
                temp.Add(forumName);
                temp.Add(requestingUser);
                serverMessage ans = new serverMessage(3, temp);
                return sendMessage(ans).Threads;
            }
        }

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
        {
            lock (syncLock)
            {
                List<string> temp = new List<string>();
                temp.Add(forumName);
                temp.Add(subForumName);
                temp.Add(requestingUser);
                serverMessage ans = new serverMessage(temp, openPostNo);
                return sendMessage(ans).getThread;
            }
        }

        private serverMessage sendMessage(string forumName, ForumPolicy policy)
        {

            serverMessage ans = new serverMessage(serverMessage.messageType.checkForumPolicy, policy, forumName);
            return sendMessage(ans);
        }

      
      


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
        public bool start()
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            TcpClient _functionClient = new TcpClient();

         
            string ip = GetIP4Address();
            //Console.WriteLine(ip);
            try
            {
                _functionClient.Connect(ip, 8000);
            }
            catch (Exception)
            {
                return false;
            }


          
            outputStream = _functionClient.GetStream();
            TcpClient _notificationClient = new TcpClient();

            try
            {
                _notificationClient.Connect(ip, 8001);
            }
            catch (Exception)
            {
                _functionClient.Close();
                return false;
            }
           
            inputStream = _notificationClient.GetStream();
            int amount_bytes_read = 0;
            try
            {
                amount_bytes_read = outputStream.Read(input_buffer, 0, input_buffer.Length);
            }
            catch (Exception)
            {
                _functionClient.Close();
                _notificationClient.Close();
                return false;
            }
            string str = "";
            for (int i = 0; i < amount_bytes_read; i++)
            {

                str = str + Convert.ToChar(input_buffer[i]);
            }
            ID = Int32.Parse(str);
            List<object> list = new List<object>();
            list.Add(inputStream);
            list.Add(notif_buffer);
            list.Add(this);
            Thread thr = new Thread(threadFunc);
            thr.Start(list);
            Console.WriteLine("my id is {0}", ID);
            return true;
            

        }

        public void updateLocation(List<string> loc)
        {
             sendMessage("updateLocation", loc, new List<int>(), DateTime.Now);
        }

        /* pillar enter code here. true means that this message is for users who registered for notifications on specific subject
         * false means that its a notification for everyone and each user needs to figureout if that notif is relevant for him
         * so when you get to it remove the "writeline" but keep the substring methods and send yourself the relevant boolean
         * you also need to somehow connect your window to this class so that this method could triger a method in the gui but
         * dont forget to make those mehod synchronized
         * */

        public void updateNeeded(string content){
            lock (syncLock)
            {
                if (content.Substring(0,4).Equals("true"))
                {
                    content = content.Substring(5);
                    Console.WriteLine("this is a notif for specific user: "+content);
                }
                else
                {
                    content = content.Substring(6); 
                    //Console.WriteLine("this is a notif for all users: "+content);
                }
                
            }
        }

        static void threadFunc(object o)
        {
            List<object> list = (List<object>)o;
            Stream s = (Stream)list.ElementAt(0);
            Byte[] buffer = (Byte[])list.ElementAt(1);
            int amount_bytes_read2 = 0;
            client c = (client)list.ElementAt(2);
            while (true)
            {
                try
                {
                    amount_bytes_read2 = s.Read(buffer, 0, buffer.Length);
                }
                catch (Exception)
                {
                    s.Close();
                    break;
                }
                if (amount_bytes_read2 != 0)
                {
                    string str = "";
                    for (int i = 0; i < amount_bytes_read2; i++)
                    {

                        str = str + Convert.ToChar(buffer[i]);
                    }

                    c.updateNeeded(str);
                    
                }
            }
            
        }

        public serverMessage sendMessage(string s, List<string> inputStrings, List<int> inputInts, DateTime time)
        {
            
            serverMessage.messageType type = serverMessage.messageType.addForum;

            switch (s)
            {
                case "ForumsByUser":
                    type = serverMessage.messageType.ForumsByUser;
                    break;
                case "updateLocation":
                    type = serverMessage.messageType.updateLocation;
                    break;
                case "addForum":
                    type = serverMessage.messageType.addForum;
                    break;
                case "addSubForum":
                    type = serverMessage.messageType.addSubForum;
                    break;
                case "registerMemberToForum":
                    type = serverMessage.messageType.registerMemberToForum;
                    break;
                case "assignAdmin":
                    type = serverMessage.messageType.assignAdmin;
                    break;
                case "unassignAdmin":
                    type = serverMessage.messageType.unassignAdmin;
                    break;
                case "assignModerator":
                    type = serverMessage.messageType.assignModerator;
                    break;
                case "unassignModerator":
                    type = serverMessage.messageType.unassignModerator;
                    break;
                case "sendPM":
                    type = serverMessage.messageType.sendPM;
                    break;
                case "setForumMaxAdmins":
                    type = serverMessage.messageType.setForumMaxAdmins;
                    break;
                case "setForumMinAdmins":
                    type = serverMessage.messageType.setForumMinAdmins;
                    break;
                case "setForumMaxModerators":
                    type = serverMessage.messageType.setForumMaxModerators;
                    break;
                case "setForumMinModerators":
                    type = serverMessage.messageType.setForumMinModerators;
                    break;
                case "setForumPostDeletionPermissions":
                    type = serverMessage.messageType.setForumPostDeletionPermissions;
                    break;
                case "setForumPasswordLifespan":
                    type = serverMessage.messageType.setForumPasswordLifespan;
                    break;
                case "setForumModeratorsSeniority":
                    type = serverMessage.messageType.setForumModeratorsSeniority;
                    break;
                case "setForumModUnassignmentPermissions":
                    type = serverMessage.messageType.setForumModUnassignmentPermissions;
                    break;
                case "writePost":
                    type = serverMessage.messageType.writePost;
                    break;
                case "setModeratorTrialTime":
                    type = serverMessage.messageType.setModeratorTrialTime;
                    break;
                case "deletePost":
                    type = serverMessage.messageType.deletePost;
                    break;
                case "editPost":
                    type = serverMessage.messageType.editPost;
                    break;
                case "getNumOfPostsInSubForum":
                    type = serverMessage.messageType.getNumOfPostsInSubForum;
                    break;
                case "getListOfMemberMessages":
                    type = serverMessage.messageType.getListOfMemberMessages;
                    break;
                case "numOfForums":
                    type = serverMessage.messageType.numOfForums;
                    break;
                case "getSubForums":
                    type = serverMessage.messageType.getSubForums;
                    break;
                case "getThreads":
                    type = serverMessage.messageType.getThreads;
                    break;
                case "getThread":
                    type = serverMessage.messageType.getThread;
                    break;
                case "login":
                    type = serverMessage.messageType.login;
                    break;
                
            }
            serverMessage message = new serverMessage(type, inputStrings, inputInts, time);
            return sendMessage(message);
        }

        private serverMessage sendMessage(serverMessage message)
        {
            int amount_bytes_read = 0;
            byte[] ba = asen.GetBytes(serializer.Serialize(message));
            try
            {
                outputStream.Write(ba, 0, ba.Length);
            }
            catch (Exception)
            {
                return new serverMessage("error");
            }

            while (true)
            {
                try
                {
                    amount_bytes_read = outputStream.Read(input_buffer, 0, input_buffer.Length);
                }
                catch (IOException)
                {
                    return new serverMessage("error");
                }

                if (amount_bytes_read != 0)
                {
                    string str = "";
                    for (int i = 0; i < amount_bytes_read; i++)
                    {

                        str = str + Convert.ToChar(input_buffer[i]);
                    }

                    serverMessage temp = serializer.Deserialize<serverMessage>(str);
                    return temp;
                }
                else
                {
                    Console.WriteLine("no message");
                }
            }
        }

        public List<string> getUsersInForum(string forumName, string requestingUser)
        {
            throw new NotImplementedException();
        }

        public List<string> getAllUsers( string requestingUser)
        {
            //implement
            List<string> res = new List<string>();
            res.Add("TestAdmin1");
            res.Add("TestAdmin2");
            return res;
        }
    }
}
