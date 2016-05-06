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
    class client
    {


        static ASCIIEncoding asen = new ASCIIEncoding();
        static JavaScriptSerializer serializer = new JavaScriptSerializer();
        Stream outputStream;
        Stream inputStream;
        byte[] input_buffer;
        byte[] notif_buffer;
        int ID;
        

  

        public client()
        {
            input_buffer = new byte[1000000];
            notif_buffer = new byte[100000];
            ID = 0;
            
           

        }

        public client(bool testing)
        {
            input_buffer = new byte[1000000];
            notif_buffer = new byte[100000];
            ID = 0;
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
            catch (Exception e)
            {
                return false;
            }


          
            outputStream = _functionClient.GetStream();
            TcpClient _notificationClient = new TcpClient();

            try
            {
                _notificationClient.Connect(ip, 8001);
            }
            catch (Exception e)
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

        public void updateNeeded(){
            Console.WriteLine("i need an update!!!");
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
                catch (Exception e)
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
                    if (str.Equals("something"))
                    {
                        
                        c.updateNeeded();
                    }
                }
            }
            
        }

        public serverMessage sendMessage(string s, List<string> inputStrings, List<int> inputInts, DateTime time)
        {
            int amount_bytes_read = 0;
            serverMessage.messageType type = serverMessage.messageType.addForum;

            switch (s)
            {
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
                catch (IOException )
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
                    //temp.writeData();
                  
                    return temp;
                }
                else
                {
                    Console.WriteLine("no message");
                }
            }
        }

        public specialServerMessage sendMessage(){
            int amount_bytes_read = 0;
            List<string> str1 = new List<string>();
            str1.Add("Cars");
            str1.Add("carsUser1");
            str1.Add("carsUser2");
            serverMessage message = new serverMessage(serverMessage.messageType.getListOfMemberMessages, str1, new List<int>(), DateTime.Now);
            byte[] ba = asen.GetBytes(serializer.Serialize(message));
            try
            {
                outputStream.Write(ba, 0, ba.Length);
            }
            catch (Exception)
            {
                return new specialServerMessage();
            }

            while (true)
            {
                try
                {
                    amount_bytes_read = outputStream.Read(input_buffer, 0, input_buffer.Length);
                }
                catch (IOException)
                {
                    return new specialServerMessage();
                }

                if (amount_bytes_read != 0)
                {
                    string str = "";
                    for (int i = 0; i < amount_bytes_read; i++)
                    {

                        str = str + Convert.ToChar(input_buffer[i]);
                    }

                    specialServerMessage temp = serializer.Deserialize<specialServerMessage>(str);
                    //temp.writeData();

                    return temp;
                }
                else
                {
                    Console.WriteLine("no message");
                }
            }

            //   Console.WriteLine("sent");
            //ba = asen.GetBytes("           ");



            //forum temp = serializer.Deserialize<forum>(str);
            // temp.writeData();
            // while (true) { }

            //client.Close();
        }





         public serverMessage sendMessage2()
        {
            int amount_bytes_read = 0;
            //serverMessage message = new serverMessage(serverMessage.messageType.messageYouRepliedToChanged, "sent message 2 to server");
            //byte[] ba = asen.GetBytes(serializer.Serialize(message));

          //  outputStream.Write(ba, 0, ba.Length);
            //Console.WriteLine("got here");
            while (true)
            {
                try
                {
                    amount_bytes_read = outputStream.Read(input_buffer, 0, input_buffer.Length);
                }
                catch (IOException e) { }
                //Console.WriteLine("should have got here");
                if (amount_bytes_read != 0)
                {
                    string str = "";
                    for (int i = 0; i < amount_bytes_read; i++)
                    {

                        str = str + Convert.ToChar(input_buffer[i]);
                    }
                    serverMessage temp = serializer.Deserialize<serverMessage>(str);
                    //temp.writeData();
                    return temp;
                }
                else
                {
                    Console.WriteLine("no message");
                }
            }

        }
    }
}
