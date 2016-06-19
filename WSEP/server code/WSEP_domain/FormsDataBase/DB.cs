using FormsDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumsDataBase
{
    public class DB : DBI
    {
        ForumDB db;
        DBLogger dbl;

        public DB()
        {
            db = new ForumDB();
            dbl = new DBLogger();
            Console.WriteLine(ReturnForumsList().Count());
            Console.WriteLine(addForum("forumNames12"));
            Console.WriteLine(removeForum("forumNames12"));

        }

        public bool addForumUser(string userName, string password, string email, DateTime registration, string forumName, DateTime lastPassChange)
        {
            dbl.append("trying to add new forum user");
            bool success = false;
            User user = new User();
            user.userName = userName;
            user.password = password;
            user.email = email;
            user.registration = registration;
            user.forumName = forumName;
            user.lastPassChange = lastPassChange;
            try
            {
                db.Users.Add(user);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }
            dbl.append("result is " + success);
            return success;
        }

        public bool removeForumUser(string userName, string forumName)
        {
            bool success = false;
            try
            {
                User user = db.Users.FirstOrDefault(u => u.userName == userName && u.forumName == forumName);
                db.Users.Remove(user);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addSubForum(string forumName, string subForumName)
        {
            bool success = false;
            SubForum sub = new SubForum();
            sub.forumName = forumName;
            sub.subForumName = subForumName;
            try
            {
                db.SubForums.Add(sub);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeSubForum(string forumName, string subForumName)
        {
            bool success = false;
            try
            {
                SubForum sub = db.SubForums.FirstOrDefault(s => s.forumName == forumName && s.subForumName == subForumName);
                db.SubForums.Remove(sub);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addPrivateMessage(string writer, string sendTo, DateTime creation, string forumName, string text)
        {

            bool success = false;
            PrivateMessage pm = new PrivateMessage();
            pm.writer = writer;
            pm.sendTo = sendTo;
            pm.creation = creation;
            pm.forumName = forumName;
            pm.text = text;
            try
            {
                db.PrivateMessages.Add(pm);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removePrivateMessage(string writer, DateTime creation, string forumName)
        {
            bool success = false;
            try
            {
                PrivateMessage pm = db.PrivateMessages.FirstOrDefault(m => m.writer == writer && m.creation == creation && m.forumName == forumName);
                db.PrivateMessages.Remove(pm);
                db.SaveChanges();
                success = true;
            }
            catch { }
            Console.WriteLine("num of : " + ReturforumMessages("testForum").Count());
            return success;
        }

        public bool addSubForumPost(string title, string content, string forumName, string subForumName, string userName, DateTime creation, int serialNumber, int parentPost)
        {
            bool success = false;
            Post post = new Post();
            post.title = title;
            post.content = content;
            post.forumName = forumName;
            post.subForumName = subForumName;
            post.userName = userName;
            post.creation = creation;
            post.serialNumber = serialNumber;
            post.parentPost = parentPost;
            try
            {
                db.Posts.Add(post);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeSubForumPost(int serialNumber)
        {
            bool success = false;

            try
            {
                Post post = db.Posts.FirstOrDefault(p => p.serialNumber == serialNumber);
                db.Posts.Remove(post);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addSubForumModerator(string forumName, string subForumName, string userName, string assigningAdmin, int trialTime, DateTime assignment)
        {
            bool success = false;
            Moderator mod = new Moderator();
            mod.forumName = forumName;
            mod.subForumName = subForumName;
            mod.userName = userName;
            mod.assigningAdmin = assigningAdmin;
            mod.trialTime = trialTime;
            mod.assignment = assignment;
            try
            {
                db.Moderators.Add(mod);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeSubForumModerator(string forumName, string subForumName, string userName)
        {
            bool success = false;

            try
            {
                Moderator mod = db.Moderators.FirstOrDefault(m => m.forumName == forumName && m.subForumName == subForumName && m.userName == userName);
                db.Moderators.Remove(mod);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addForumMember(string forumName, string userName)
        {
            bool success = false;
            Member member = new Member();
            member.forumName = forumName;
            member.userName = userName;
            try
            {
                db.Members.Add(member);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeForumMember(string forumName, string userName)
        {
            bool success = false;

            try
            {
                Member member = db.Members.FirstOrDefault(m => m.forumName == forumName && m.userName == userName);
                db.Members.Remove(member);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup)
        {
            bool success = false;
            ForumPolicy fp = new ForumPolicy();
            fp.maxNumOfAdmins = maxNumOfAdmins;
            fp.minNumOfAdmins = minNumOfAdmins;
            fp.maxNumOfModerators = maxNumOfModerators;
            fp.minNumOfModerators = minNumOfModerators;
            fp.forumName = forumName;
            fp.pdp = pdp;
            fp.passwordLifespan = passLife;
            fp.moderatorSeniority = moderatorSen;
            fp.mup = mup;
            try
            {
                db.ForumPolicies.Add(fp);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeForumPolicy(string forumName)
        {
            bool success = false;

            try
            {
                ForumPolicy fp = db.ForumPolicies.FirstOrDefault(p => p.forumName == forumName);
                db.ForumPolicies.Remove(fp);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool changeModeratorTrialTime(string forumName, string subForumName, string userName, int trialTime)
        {
            bool success = false;

            try
            {
                Moderator mod = db.Moderators.FirstOrDefault(m => m.forumName == forumName && m.subForumName == subForumName && m.userName == userName);
                mod.trialTime = trialTime;
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool changeForumPost(string title, string content, string forumName, string subForumName, int serialNumber)
        {
            //no need with the forum name and sub forum name if we have serial number
            bool success = false;

            try
            {
                Post post = db.Posts.FirstOrDefault(p => p.serialNumber == serialNumber);
                post.title = title;
                post.content = content;
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addForum(string forumName)
        {
            bool success = false;
            Forum forum = new Forum();
            forum.forumName = forumName;

            try
            {
                db.Forums.Add(forum);
                db.SaveChanges();
                success = true;
            }
            catch (Exception)
            { 
                success = false; 
            }

            return success;
        }

        public bool removeForum(string forumName)
        {
            bool success = false;

            try
            {
                Forum forum = db.Forums.FirstOrDefault(f => f.forumName == forumName);
                db.Forums.Remove(forum);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool addForumAdmin(string forumName, string userName)
        {
            bool success = false;
            Admin admin = new Admin();
            admin.forumName = forumName;
            admin.userName = userName;

            try
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                success = true;
            }
            catch { success = false; }

            return success;
        }

        public bool removeForumAdmin(string forumName, string userName)
        {
            bool success = false;

            try
            {
                Admin admin = db.Admins.FirstOrDefault(a => a.forumName == forumName && a.userName == userName);
                db.Admins.Remove(admin);
                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public bool changeForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup)
        {
            bool success = false;

            try
            {
                ForumPolicy fp = db.ForumPolicies.FirstOrDefault(p => p.forumName == forumName);
                fp.maxNumOfAdmins = maxNumOfAdmins;
                fp.minNumOfAdmins = minNumOfAdmins;
                fp.maxNumOfModerators = maxNumOfModerators;
                fp.minNumOfModerators = minNumOfModerators;
                fp.forumName = forumName;
                fp.pdp = pdp;
                fp.passwordLifespan = passLife;
                fp.moderatorSeniority = moderatorSen;
                fp.mup = mup;

                db.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public List<Tuple<string, string>> ReturnforumMembers(string forumName)
        {
            List<Tuple<string, string>> ans = new List<Tuple<string, string>>();
            try
            {
                var members = db.Members.Where(m => m.forumName == forumName);

                foreach (var member in members)
                {
                    var tmp = new Tuple<string, string>(member.forumName, member.userName);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }

        public List<Tuple<string, string>> ReturnforumAdmins(string forumName)
        {
            List<Tuple<string, string>> ans = new List<Tuple<string, string>>();
            try
            {
                var admins = db.Admins.Where(a => a.forumName == forumName);

                foreach (var admin in admins)
                {
                    var tmp = new Tuple<string, string>(admin.forumName, admin.userName);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }

        public List<string> ReturnForumsList()
        {
            List<string> ans = new List<string>();
            try
            {
                List<Forum> forums = new List<Forum>();

                forums = db.Forums.ToList();

                foreach (Forum forum in forums)
                {
                    ans.Add(forum.forumName);
                }
            }
            catch { }

            return ans;
        }

        public List<string> ReturnSubForumList(string forumName)
        {
            List<string> ans = new List<string>();
            try
            {
                List<SubForum> subforums = new List<SubForum>();

                subforums = db.SubForums.ToList();


                foreach (SubForum subforum in subforums)
                {
                    ans.Add(subforum.forumName);
                }
            }
            catch { }

            return ans;
        }

        public List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> ReturnforumPolicy(string forumName)
        {
            List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> ans = new List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>>();
            try
            {
                var policies = db.ForumPolicies.Where(fp => fp.forumName == forumName);

                foreach (var policy in policies)
                {
                    var tmp1 = new Tuple<int, int>(policy.maxNumOfAdmins, policy.minNumOfAdmins);
                    var tmp2 = new Tuple<int, int>(policy.maxNumOfModerators, policy.minNumOfModerators);
                    var tmp3 = new Tuple<int, int>(policy.pdp, policy.passwordLifespan);
                    var tmp4 = new Tuple<int, int>(policy.moderatorSeniority, policy.mup);
                    var tmp = new Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>(tmp1, tmp2, tmp3, tmp4);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;

        }

        public List<Tuple<string, string, int, DateTime>> ReturnSubforumModerators(string forumName, string subForumName)
        {
            List<Tuple<string, string, int, DateTime>> ans = new List<Tuple<string, string, int, DateTime>>();
            try
            {
                var moderators = db.Moderators.Where(m => m.forumName == forumName && m.subForumName == subForumName);

                foreach (var mod in moderators)
                {
                    DateTime d = (DateTime)mod.assignment;
                    var tmp = new Tuple<string, string, int, DateTime>(mod.userName, mod.assigningAdmin, mod.trialTime, d);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }

        public List<Tuple<string, string, string, DateTime, int, int>> ReturnSubforumPosts(string forumName, string subForumName)
        {
            List<Tuple<string, string, string, DateTime, int, int>> ans = new List<Tuple<string, string, string, DateTime, int, int>>();
            try
            {
                var posts = db.Posts.Where(p => p.forumName == forumName && p.subForumName == subForumName);

                foreach (var post in posts)
                {
                    DateTime d = (DateTime)post.creation;
                    var tmp = new Tuple<string, string, string, DateTime, int, int>(post.title, post.content, post.userName, d, post.serialNumber, post.parentPost);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }

        public List<Tuple<string, string, DateTime, string>> ReturforumMessages(string forumName)
        {
            List<Tuple<string, string, DateTime, string>> ans = new List<Tuple<string, string, DateTime, string>>();
            try
            {
                var messages = db.PrivateMessages.Where(m => m.forumName == forumName);

                foreach (var mes in messages)
                {
                    DateTime d = (DateTime)mes.creation;
                    var tmp = new Tuple<string, string, DateTime, string>(mes.writer, mes.sendTo, d, mes.text);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }

        public List<Tuple<string, string, string, DateTime, DateTime>> ReturnForumUsers(string forumName)
        {
            List<Tuple<string, string, string, DateTime, DateTime>> ans = new List<Tuple<string, string, string, DateTime, DateTime>>();
            try
            {
                var users = db.Users.Where(u => u.forumName == forumName);

                foreach (var user in users)
                {
                    DateTime d = (DateTime)user.registration;
                    DateTime d2 = (DateTime)user.lastPassChange;
                    var tmp = new Tuple<string, string, string, DateTime, DateTime>(user.userName, user.password, user.email, d, d2);
                    ans.Add(tmp);
                }
            }
            catch { }

            return ans;
        }


        List<Tuple<string, string, string, DateTime, DateTime, string, string>> DBI.ReturnForumUsers(string forumName)
        {
            throw new NotImplementedException();
        }

        public List<string> ReturnPostFollowers(int p)
        {
            throw new NotImplementedException();
        }

        public bool addPostFollower(int _sn, string p)
        {
            throw new NotImplementedException();
        }

        public bool addForumUser(string userName, string password, string email, DateTime registration, string forumName, DateTime lastPassChange, string seccurityQuestion, string answer)
        {
            throw new NotImplementedException();
        }
    }
}
