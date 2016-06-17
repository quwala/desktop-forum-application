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
    class WebClient : iWebClient
    {
        static ASCIIEncoding asen = new ASCIIEncoding();
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        Stream outputStream;
        Stream inputStream;
        byte[] input_buffer;
        byte[] notif_buffer;
        int ID;

        public WebClient()
        {
            input_buffer = new byte[1000000];
            notif_buffer = new byte[100000];
            ID = 0;
        }

        public List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser)
        {
            List<string> temp = new List<string>();
            temp.Add(forumName);
            temp.Add(requestingUser);
            serverMessage ans = new serverMessage(3, temp);
            return sendMessage(ans).Threads;
        }

        public List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser)
        {
            List<string> temp = new List<string>();
            temp.Add(forumName);
            temp.Add(subForumName);
            temp.Add(requestingUser);
            serverMessage ans = new serverMessage(temp, openPostNo);
            return sendMessage(ans).getThread;
        }

        public List<string> getForums()
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

        public List<string> getSubForums(string forumName, string requestingUser)
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

        public string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, string content)
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

        public loginStatus login(string forumname, string username, string password)
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

        public permission getUserPermissionsForForum(string forumName, string username)
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

        public permission getUserPermissionsForSubForum(string forumName, string subForumName, string username)
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

        public serverMessage sendMessage(string s, List<string> inputStrings, List<int> inputInts, DateTime time)
        {

            serverMessage.messageType type = serverMessage.messageType.addForum;

            switch (s)
            {
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
            Console.WriteLine(ip);
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
            return true;


        }

        public void updateLocation(List<string> loc)
        {
            sendMessage("updateLocation", loc, new List<int>(), DateTime.Now);
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


    }
}
