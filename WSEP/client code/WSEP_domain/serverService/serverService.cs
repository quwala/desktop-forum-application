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
using System.Runtime.CompilerServices;

namespace serverService
{
    public class serverService : IserverService
    {
        bool test;
        static IForumSystem forumSystem = null;
        JavaScriptSerializer serializer; 
       
         public serverService(bool test)
        {
            lock (typeof(serverService))
            {
                this.test = test;
                if (forumSystem == null)
                {
                    forumSystem = new ForumSystem(test);
                }
            }
             serializer = new JavaScriptSerializer();
        }

         public string parseMessage(string str)
         {
             serverMessage message = serializer.Deserialize<serverMessage>(str);
             serverMessage ans = parseServerMessage(message);
             return serializer.Serialize(ans);
         }

         public Tuple<string, string, List<Tuple<string, string>>> parseMessage2(string str)
         {
             serverMessage message = serializer.Deserialize<serverMessage>(str);
             serverMessage ans = parseServerMessage(message);
             List<Tuple<string, string>> notifList = new List<Tuple<string, string>>();
             notifList.Add(new Tuple<string, string>("Cars", "carsUser1"));

             if (message._messageType == serverMessage.messageType.registerMemberToForum)
             {
                 return new Tuple<string, string, List<Tuple<string, string>>>(serializer.Serialize(ans), "some content", notifList);
             }
             if (message._messageType == serverMessage.messageType.login & ans._messageType.Equals(serverMessage.messageType.success))
             {
                 // Console.WriteLine(message.stringContent.ElementAt(0));
                 // Console.WriteLine(message.stringContent.ElementAt(1));

                 Tuple<string, string> pair = new Tuple<string, string>(message.stringContent.ElementAt(0), message.stringContent.ElementAt(1));
                 notifList.Add(pair);
                 //Console.WriteLine("adding user {0} in forum {1}", pair.Item2, pair.Item1);
                 return new Tuple<string, string, List<Tuple<string, string>>>(serializer.Serialize(ans), "login", notifList);
             }
             return new Tuple<string, string, List<Tuple<string, string>>>(serializer.Serialize(ans), "not notifying", new List<Tuple<string, string>>());
         }

         public static bool checkArgs(System.Collections.IList lst, int num)
         {
             return !(lst.Count == num);
         }

         private serverMessage parseServerMessage(serverMessage message)
         {
                    List<string> strLst = new List<string>();
                    List<int> intLst = new List<int>();
                    DateTime date = new DateTime();
                    string forumName, user,scndUser, subforumName, ans, password;
                    int number;
                    switch (message._messageType)
                    {


                        case serverMessage.messageType.checkForumPolicy:
                            forumName = message.stringContent.ElementAt(0);
                            ans = forumSystem.checkForumPolicy(forumName, message.policy);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, new List<string>(), new List<int>(), date);
                            }
                            strLst.Add(ans);
                            return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);

                        case serverMessage.messageType.addForum:
                            //Console.WriteLine("got to add forum");
                            if (checkArgs(message.stringContent, 2))
                            {
                                strLst.Add("error, needed 2 arguments exactley");
                                return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                                
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            ans = forumSystem.addForum(forumName, user);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            }
                            strLst.Add(ans);
                            return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.addSubForum:
                            if (checkArgs(message.stringContent, 4) & (checkArgs(message.intContent, message.stringContent.Count - 3)))
                            {
                                strLst.Add("error, arguments not valid");
                                return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                            }
                            forumName = message.stringContent.ElementAt(0);
                            subforumName = message.stringContent.ElementAt(1);
                            user = message.stringContent.ElementAt(2);
                            List<Tuple<string, string, int>> tupleList = new List<Tuple<string, string, int>>();
                            for (int i = 0; i < (message.stringContent.Count - 3); i++)
                            {
                                tupleList.Add(new Tuple<string, string, int>(message.stringContent.ElementAt(i + 3), "", message.intContent.ElementAt(i)));
                            }

                            ans = forumSystem.addSubForum(forumName, subforumName, tupleList, user);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            }
                            strLst.Add(ans);
                            return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.assignAdmin:
                            if (checkArgs(message.stringContent, 3))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);

                            ans = forumSystem.assignAdmin(forumName, user, scndUser);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            }
                            strLst.Add(ans);
                            return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.registerMemberToForum:
                           // Console.WriteLine("got to register member");
                            if (checkArgs(message.stringContent, 4))
                            {
                                strLst.Add("error, needed 4 arguments exactley");
                                return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            password = message.stringContent.ElementAt(2);
                            string mail = message.stringContent.ElementAt(3);
                            ans = forumSystem.registerMemberToForum(forumName, user, password, mail);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            }
                            strLst.Add(ans);
                            return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.unassignAdmin:
                            if (checkArgs(message.stringContent, 3))
                            {
                                strLst.Add("error, needed 3 arguments exactley");
                                return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date); 
                            }
                            forumName = message.stringContent.ElementAt(0);
                            user = message.stringContent.ElementAt(1);
                            scndUser = message.stringContent.ElementAt(2);
                            ans = forumSystem.unassignAdmin(forumName, user, scndUser);
                            if (ans.Equals("true"))
                            {
                                return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                            }
                            strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.assignModerator:
                           if ((checkArgs(message.stringContent, 4)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           scndUser = message.stringContent.ElementAt(3);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.assignModerator(forumName, subforumName, user, scndUser, number);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.sendPM:
                           if (checkArgs(message.stringContent, 4))
                           {
                               strLst.Add("error, needed 4 arguments exactley");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           scndUser = message.stringContent.ElementAt(2);
                           ans = message.stringContent.ElementAt(3);
                           ans = forumSystem.sendPM(forumName, user, scndUser, ans);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumMaxAdmins:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumMaxAdmins(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumMinAdmins:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumMinAdmins(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumMaxModerators:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumMaxModerators(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumMinModerators:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumMinModerators(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumPostDeletionPermissions:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
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
                                   return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                           }
                           ans = forumSystem.setForumPostDeletionPermissions(forumName, pdp, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumPasswordLifespan:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumPasswordLifespan(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumModeratorsSeniority:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setForumModeratorsSeniority(forumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setForumModUnassignmentPermissions:
                           if ((checkArgs(message.stringContent, 2)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
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
                                   return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                           }
                           ans = forumSystem.setForumModUnassignmentPermissions(forumName, mup, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.writePost:
                           if ((checkArgs(message.stringContent, 5)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           ans = message.stringContent.ElementAt(3);
                           password = message.stringContent.ElementAt(4);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.writePost(forumName, subforumName, number, user, ans, password);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.setModeratorTrialTime:
                           if ((checkArgs(message.stringContent, 4)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           scndUser = message.stringContent.ElementAt(3);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.setModeratorTrialTime(forumName, subforumName, user, number, scndUser);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.deletePost:
                           if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.deletePost(forumName, subforumName, number, user);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.editPost:
                           if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           ans = message.stringContent.ElementAt(3);
                           number = message.intContent.ElementAt(0);
                           ans = forumSystem.editPost(forumName, subforumName, number, user, ans);
                           if (ans.Equals("true"))
                           {
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           strLst.Add(ans);
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.getNumOfPostsInSubForum:
                           if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           number = forumSystem.getNumOfPostsInSubForum(forumName, subforumName, user);
                           if (number != -1)
                           {
                               intLst.Add(number);
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                        case serverMessage.messageType.getListOfMemberMessages:
                           if ((checkArgs(message.stringContent, 3)) & (checkArgs(message.intContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           scndUser = message.stringContent.ElementAt(2);
                           List<Tuple<string, string, DateTime, int>> returnForMemMesages = new List<Tuple<string, string, DateTime, int>>();
                           returnForMemMesages = forumSystem.getListOfMemberMessages(forumName, user, scndUser);
                           if (returnForMemMesages != null)
                           {
                               if (returnForMemMesages.Count != 0)
                               {
                                   return new serverMessage(serverMessage.messageType.success, returnForMemMesages);

                               }
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, returnForMemMesages);
                        case serverMessage.messageType.getListOfForummoderators:
                           if ((checkArgs(message.stringContent, 2)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           List<Tuple<string, string, DateTime, string>> returnForMemMesages2 = new List<Tuple<string, string, DateTime, string>>();
                           returnForMemMesages2 = forumSystem.getListOfForumModerators(forumName, user);
                           if (returnForMemMesages2 != null)
                           {
                               if (returnForMemMesages2.Count != 0)
                               {
                                   return new serverMessage(serverMessage.messageType.success, returnForMemMesages2);
                               }
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, returnForMemMesages2);
                        case serverMessage.messageType.numOfForums:
                           if ((checkArgs(message.stringContent, 1)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           user = message.stringContent.ElementAt(0);
                           number = forumSystem.numOfForums(user);
                           if (number != -1)
                           {
                               intLst.Add(number);
                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);
                       /* case serverMessage.messageType.ForumsByUser:
                           if ((checkArgs(message.stringContent, 2)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           user = message.stringContent.ElementAt(0);
                           scndUser = message.stringContent.ElementAt(1);
                           strLst = forumSystem.ForumsByUser(user, scndUser);
                           if (strLst.Count != 0)
                           {

                               return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, strLst, intLst, date);*/
                        case serverMessage.messageType.getForums:
                           List<string> forums = forumSystem.getForums();
                           return new serverMessage(serverMessage.messageType.success, forums, intLst, date);
                        case serverMessage.messageType.getSubForums:
                           if ((checkArgs(message.stringContent, 2)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           user = message.stringContent.ElementAt(1);
                           List<string> subForums = forumSystem.getSubForums(forumName, user);
                           return new serverMessage(serverMessage.messageType.success, subForums, intLst, date);
                        case serverMessage.messageType.getThreads:
                           if ((checkArgs(message.stringContent, 3)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           List<Tuple<string, DateTime, int>> returnForMemMesages3 = new List<Tuple<string, DateTime, int>>();
                           returnForMemMesages3 = forumSystem.getThreads(forumName, subforumName, user);
                           if (returnForMemMesages3 != null)
                           {
                               if (returnForMemMesages3.Count != 0)
                               {
                                   return new serverMessage(serverMessage.messageType.success, returnForMemMesages3);
                               }
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, returnForMemMesages3);
                        case serverMessage.messageType.getThread:
                           if ((checkArgs(message.stringContent, 3)) & checkArgs(message.intContent, 1))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
                           }
                           forumName = message.stringContent.ElementAt(0);
                           subforumName = message.stringContent.ElementAt(1);
                           user = message.stringContent.ElementAt(2);
                           number = message.intContent.ElementAt(0);
                           List<Tuple<string, string, DateTime, int, int, string, DateTime>> returnForMemMesages4 = new List<Tuple<string, string, DateTime, int, int, string, DateTime>>();
                           returnForMemMesages4 = forumSystem.getThread(forumName, subforumName, number, user);
                           if (returnForMemMesages4 != null)
                           {
                               if (returnForMemMesages4.Count != 0)
                               {
                                   return new serverMessage(serverMessage.messageType.success, returnForMemMesages4);
                               }
                           }
                           return new serverMessage(serverMessage.messageType.unsuccess, returnForMemMesages4);
                        case serverMessage.messageType.login:
                           if ((checkArgs(message.stringContent, 3)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
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
                           return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                        case serverMessage.messageType.getUserPermissionsForForum:
                           if ((checkArgs(message.stringContent, 2)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
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
                           return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                        case serverMessage.messageType.getUserPermissionsForSubForum:
                           if ((checkArgs(message.stringContent, 2)))
                           {
                               strLst.Add("error");
                               return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
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
                           return new serverMessage(serverMessage.messageType.success, strLst, intLst, date);
                    }

                    return new serverMessage(serverMessage.messageType.errorHappened, strLst, intLst, date);
              }

         

         

    }
}
