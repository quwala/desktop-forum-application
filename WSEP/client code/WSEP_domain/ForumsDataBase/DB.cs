using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ForumsDataBase
{
    public class DB : DBI
    {
        SqlConnection connection;
        string connectionString;
        public DB()
        {
            //add and remove to the rest
            //init..
            connectionString = ConfigurationManager.ConnectionStrings["ForumsDataBase.Properties.Settings.forumDatabaseConnectionString"].ConnectionString;

        }

        public Boolean addForumUser(string userName, string password, string email, DateTime registration, string forumName, DateTime lastPassChange)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Users(userName,password,email,registration,forumName,lastPassChange) VALUES (@userName,@password,@email,@registration,@forumName,@lastPassChange)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;
                command.Parameters.Add("@password", SqlDbType.VarChar, 30).Value = password;
                command.Parameters.Add("@email", SqlDbType.VarChar, 30).Value = email;
                command.Parameters.Add("@registration", SqlDbType.VarChar, 30).Value = registration;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@lastPassChange", SqlDbType.VarChar, 30).Value = lastPassChange;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removeForumUser(string userName, string forumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Users WHERE forumName = @forumName AND userName = @userName ;";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addSubForum(string forumName, string subForumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO SubForums(forumName, subForumName) VALUES (@forumName,@subForumName)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removeSubForum(string forumName, string subForumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM SubForums WHERE forumName = @forumName AND subForumName = @subForumName ;";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addPrivateMessage(string writer, string sendTo, DateTime creation, string forumName, string text)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO PrivateMessages(writer, sendTo, creation, forumName, text) VALUES   (@writer, @sendTo, @creation, @forumName, @text)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@writer", SqlDbType.VarChar, 30).Value = writer;
                command.Parameters.Add("@sendTo", SqlDbType.VarChar, 30).Value = sendTo;
                command.Parameters.Add("@creation", SqlDbType.DateTime, 30).Value = creation;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@text", SqlDbType.VarChar, 30).Value = text;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removePrivateMessage(string writer, DateTime creation, string forumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM PrivateMessages WHERE writer = @writer AND creation = @creation AND forumname = @forumname";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@writer", SqlDbType.VarChar, 30).Value = writer;
                command.Parameters.Add("@creation", SqlDbType.DateTime, 30).Value = creation;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ){ }

                connection.Close();
                return false;
            }
        }

        public Boolean addSubForumPost(string title, string content, string forumName, string subForumName, string userName, DateTime creation, int serialNumber, int parentPost)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Posts(title, content, forumName, subForumName, userName , creation ,serialNumber, parentPost) VALUES   (@title, @content, @forumName, @subForumName, @userName , @creation ,@serialNumber, @parentPost)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = userName;
                command.Parameters.Add("@title", SqlDbType.VarChar, 30).Value = title;
                command.Parameters.Add("@content", SqlDbType.VarChar, 30).Value = content;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;
                command.Parameters.Add("@serialNumber", SqlDbType.Int, 30).Value = serialNumber;
                command.Parameters.Add("@parentPost", SqlDbType.Int, 30).Value = parentPost;
                command.Parameters.Add("@creation", SqlDbType.DateTime, 30).Value = creation;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ){ }

                connection.Close();
                return false;
            }
        }

        public Boolean removeSubForumPost(int serialNumber)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Posts WHERE serialNumber = @serialNumber";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@serialNumber", SqlDbType.Int, 30).Value = serialNumber;
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addSubForumModerator(string forumName, string subForumName, string userName, string assigningAdmin, int trialTime, DateTime assignment)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Moderators(forumName, subForumName, userName , assigningAdmin ,trialTime, assignment) VALUES   (@forumName, @subForumName, @userName , @assigningAdmin ,@trialTime, @assignment)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;
                command.Parameters.Add("@username", SqlDbType.VarChar, 30).Value = userName;
                command.Parameters.Add("@assigningAdmin", SqlDbType.VarChar, 30).Value = assigningAdmin;
                command.Parameters.Add("@trialTime", SqlDbType.Int, 30).Value = trialTime;
                command.Parameters.Add("@assignment", SqlDbType.DateTime, 30).Value = assignment;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removeSubForumModerator(string forumName, string subForumName, string userName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Moderators WHERE forumName = @forumName AND subForumName = @subForumName AND userName = @userName ;";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ){ }

                connection.Close();
                return false;
            }
        }

        public Boolean addForumMember(string forumName, string userName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Members(forumName, userName) VALUES (@forumName,@userName)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }   
        }

        public Boolean removeForumMember(string forumName, string userName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Members WHERE forumName = @forumName AND userName = @userName ;";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addForum(string forumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Forums(forumName) VALUES (@forumName)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                try
                {
                    Console.WriteLine("inside the try");
                    command.ExecuteNonQuery();
                    Console.WriteLine("after exe");
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removeForum(string forumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Forums WHERE forumName = @forumName";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO ForumPolicy(maxNumOfAdminS, minNumOfAdmins, maxNumOfModerators , minNumOfModerators ,forumName, pdp, passwordLifespan , moderatorSeniority , mup) VALUES   (@maxNumOfAdmins, @minNumOfAdmins,  @maxNumOfModerators,@minNumOfModerators,@forumName,@pdp,@passwordLifespan,@moderatorSeniority, @mup)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@maxNumOfAdmins", SqlDbType.Int, 30).Value = maxNumOfAdmins;
                command.Parameters.Add("@minNumOfAdmins", SqlDbType.Int, 30).Value = minNumOfAdmins;
                command.Parameters.Add("@maxNumOfModerators", SqlDbType.Int, 30).Value = maxNumOfModerators;
                command.Parameters.Add("@minNumOfModerators", SqlDbType.Int, 30).Value = minNumOfModerators;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@pdp", SqlDbType.Int, 30).Value = pdp;
                command.Parameters.Add("@passwordLifespan", SqlDbType.Int, 30).Value = passLife;
                command.Parameters.Add("@moderatorSeniority", SqlDbType.Int, 30).Value = moderatorSen;
                command.Parameters.Add("@mup", SqlDbType.VarChar, 30).Value = mup;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean removeForumPolicy(string forumName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM ForumPolicy WHERE forumName = @forumName";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }  

        public Boolean changeModeratorTrialTime(string forumName, string subForumName, string userName, int trialTime)
        {
            connection = new SqlConnection(connectionString);


            String sqlString = "UPDATE Moderators SET trialTime = @trialTime WHERE forumName = @forumName AND subForumName = @subForumName AND userName = @userName";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;
                command.Parameters.Add("@trialTime", SqlDbType.Int, 30).Value = trialTime;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean changeForumPost(string title, string content, string forumName, string subForumName, int serialNumber)    
        {
            connection = new SqlConnection(connectionString);


            String sqlString = "UPDATE Posts SET title = @title, content = @content WHERE serialNumber = @serialNumber AND forumName = @forumName AND subForumName = @subForumName";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@title", SqlDbType.VarChar, 30).Value = title;
                command.Parameters.Add("@content", SqlDbType.VarChar, 30).Value = content;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@subForumName", SqlDbType.VarChar, 30).Value = subForumName;
                command.Parameters.Add("@serialNumber", SqlDbType.Int, 30).Value = serialNumber;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean changeForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup)
        {
            connection = new SqlConnection(connectionString);


            String sqlString = "UPDATE ForumPolicy SET maxNumOfAdmins = @maxNumOfAdmins, minNumOfAdmins = @minNumOfAdmins, maxNumOfModerators = @maxNumOfModerators, minNumOfModerators = @minNumOfModerators, pdp = @pdp, passwordLifespan = @passwordLifespan, moderatorSeniority = @moderatorSeniority, mup = @mup  WHERE forumName = @forumName";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@maxNumOfAdmins", SqlDbType.Int, 30).Value = maxNumOfAdmins;
                command.Parameters.Add("@minNumOfAdmins", SqlDbType.Int, 30).Value = minNumOfAdmins;
                command.Parameters.Add("@maxNumOfModerators", SqlDbType.Int, 30).Value = maxNumOfModerators;
                command.Parameters.Add("@minNumOfModerators", SqlDbType.Int, 30).Value = minNumOfModerators;
                command.Parameters.Add("@pdp", SqlDbType.Int, 30).Value = pdp;
                command.Parameters.Add("@passwordLifespan", SqlDbType.Int, 30).Value = passLife;
                command.Parameters.Add("@moderatorSeniority", SqlDbType.Int, 30).Value = moderatorSen;
                command.Parameters.Add("@mup", SqlDbType.VarChar, 30).Value = mup;
                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException ) { }

                connection.Close();
                return false;
            }
        }

        public Boolean addForumAdmin(string forumName, string userName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "INSERT INTO Admins(forumName, userName) VALUES (@forumName,@userName)";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                {
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (SqlException) { }

                connection.Close();
                return false;
            }

        }

        public Boolean removeForumAdmin(string forumName, string userName)
        {
            connection = new SqlConnection(connectionString);

            String sqlString = "DELETE FROM Admins WHERE forumName = @forumName AND userName = @userName ;";
            using (SqlCommand command = new SqlCommand(sqlString, connection))
            {
                connection.Open();

                command.Parameters.Add("@forumName", SqlDbType.VarChar, 30).Value = forumName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 30).Value = userName;

                try
                    {
                        command.ExecuteNonQuery();
                        connection.Close();
                        return true;
                    }
                catch (SqlException) { }
                
                connection.Close();
                return false;
            }
        }

        public List<Tuple<string, string, string, DateTime, DateTime>> ReturnForumUsers(string forumName)
        {
            List<Tuple<string, string, string, DateTime, DateTime>> users = new List<Tuple<string, string, string, DateTime, DateTime>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Users WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string username = reader.GetString(0);
                            string password = reader.GetString(1);
                            string email = reader.GetString(2);
                            DateTime registration = reader.GetDateTime(3);
                            DateTime lastPassChange = reader.GetDateTime(5);


                            Tuple<string, string, string, DateTime, DateTime> tmpTuple = new Tuple<string, string, string, DateTime, DateTime>(username,password,email,registration,lastPassChange);

                            users.Add(tmpTuple);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return users;
            }
        }

        public List<Tuple<string, string, DateTime, string>> ReturforumMessages(string forumName)
        {
            List<Tuple<string, string, DateTime, string>> messages = new List<Tuple<string, string, DateTime, string>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.PrivateMessages WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string writer = reader.GetString(0);
                            string sendTo = reader.GetString(1);
                            DateTime creation = reader.GetDateTime(2);
                            string Text = reader.GetString(4);
                            Tuple<string, string, DateTime, string> tmpTuple = new Tuple<string, string, DateTime, string>(writer,sendTo,creation,Text);
                            messages.Add(tmpTuple);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return messages;
            }
        }

        public List<Tuple<string, string, string, DateTime, int, int>> ReturnSubforumPosts(string forumName, string subForumName)
        {
            List<Tuple<string, string, string, DateTime, int, int>> subPosts = new List<Tuple<string, string, string, DateTime, int, int>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();


                string[] myVarNames = new string[] { "@forumName", "@subForumName" };
                string[] myValues = new string[] { forumName, subForumName };



                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Posts WHERE forumName = @forumName AND subForumName = @subForumName ;", connection))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        command.Parameters.AddWithValue(myVarNames[i], myValues[i]);
                    }
                    try
                    {

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string content = reader.GetString(1);
                            string username = reader.GetString(4);
                            DateTime creation = reader.GetDateTime(5);
                            int serialNumber = reader.GetInt32(6);
                            int parentPost = reader.GetInt32(7);


                            Tuple<string, string, string, DateTime, int, int> tmpTuple = new Tuple<string, string, string, DateTime, int, int>(title, content, username, creation, serialNumber, parentPost);

                            subPosts.Add(tmpTuple);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return subPosts;
            }
        }

        public List<Tuple<string, string, int, DateTime>> ReturnSubforumModerators(string forumName, string subForumName)
        {
            List<Tuple<string, string, int, DateTime>> moderators = new List<Tuple<string, string, int, DateTime>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string[] myVarNames = new string[] { "@forumName", "@subForumName"};
                string[] myValues = new string[] { forumName,subForumName };
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Moderators WHERE forumName = @forumName AND subForumName = @subForumName ;", connection))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        command.Parameters.AddWithValue(myVarNames[i], myValues[i]);
                    }
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string username = reader.GetString(2);
                            string assigningAdmin = reader.GetString(3);
                            int trialTime = reader.GetInt32(4);
                            DateTime assignment = reader.GetDateTime(5);
                            Tuple<string, string, int, DateTime> tmpTuple = new Tuple<string, string, int, DateTime>(username,assigningAdmin,trialTime,assignment);
                            moderators.Add(tmpTuple);
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return moderators;
            }
        }

        public List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> ReturnforumPolicy(string forumName)
        {
            List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> Policy = new List<Tuple<Tuple<int,int>,Tuple<int,int>,Tuple<int,int>,Tuple<int,int>>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.ForumPolicy WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            int maxAdmins = reader.GetInt32(0);
                            int minAdmins = reader.GetInt32(1);
                            int maxMod = reader.GetInt32(2);
                            int minMod = reader.GetInt32(3);
                            int pdp = reader.GetInt32(5); 
                            int passwordLife = reader.GetInt32(6);
                            int moderatorSen = reader.GetInt32(7);
                            int mup = reader.GetInt32(8);
                           
                            var tmp1 = new Tuple<int, int>(maxAdmins, minAdmins);
                            var tmp2 = new Tuple<int, int>(maxMod, minMod);
                            var tmp3 = new Tuple<int, int>(pdp, passwordLife);
                            var tmp4 = new Tuple<int, int>(moderatorSen, mup);

                            Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>> tmpTuple = new Tuple<Tuple<int,int>,Tuple<int,int>,Tuple<int,int>,Tuple<int,int>>(tmp1,tmp2,tmp3,tmp4);

                            Policy.Add(tmpTuple);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return Policy;
            }
        }

        public List<string> ReturnSubForumList(string forumName)
        {
            List<string> forums = new List<string>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.SubForums WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string sfn = reader.GetString(1);
                            forums.Add(sfn);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return forums;
            }
        }

        public List<string> ReturnForumsList()
        {
            List<string> forums = new List<string>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Forums", connection))
                {
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string fn = reader.GetString(0);
                            forums.Add(fn);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return forums;
            }
        }

        public List<Tuple<string, string>> ReturnforumAdmins(string forumName)
        {


            List<Tuple<string, string>> admins = new List<Tuple<string, string>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Admins WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string fn = reader.GetString(0);
                            string userName = reader.GetString(1);
                            var tmp = new Tuple<string, string>(fn, userName);
                            admins.Add(tmp);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return admins;
            }
        }

        public List<Tuple<string, string>> ReturnforumMembers(string forumName)
        {

            List<Tuple<string, string>> members = new List<Tuple<string, string>>();
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM dbo.Members WHERE forumName = @forumName ;", connection))
                {
                    command.Parameters.AddWithValue("@forumName", forumName);
                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string fn = reader.GetString(0);
                            string userName = reader.GetString(1);
                            var tmp = new Tuple<string, string>(fn, userName);
                            members.Add(tmp);
                        }
                    }

                    catch
                    {
                        Console.WriteLine("Count not insert.");
                    }
                }

                return members;
            }
        }
    }
}
