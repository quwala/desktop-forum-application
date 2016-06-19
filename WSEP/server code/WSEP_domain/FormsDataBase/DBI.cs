using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumsDataBase
{
    public interface DBI
    {
        // changed
        Boolean addForumUser(string userName, string password, string email, DateTime registration, string forumName, DateTime lastPassChange, string seccurityQuestion, string answer);   //boolean on sucesses
        Boolean removeForumUser(string userName, string forumName); //boolean on sucesses
        Boolean addSubForum(string forumName, string subForumName);   //boolean on sucesses
        Boolean removeSubForum(string forumName, string subForumName); //boolean on sucesses
        Boolean addPrivateMessage(string writer, string sendTo, DateTime creation, string forumName, string text);   //boolean on sucesses
        Boolean removePrivateMessage(string writer, DateTime creation, string forumName);   //boolean on sucesses
        Boolean addSubForumPost(string title, string content, string forumName, string subForumName, string userName, DateTime creation, int serialNumber, int parentPost);   //boolean on sucesses
        Boolean removeSubForumPost(int serialNumber);   //boolean on sucesses
        Boolean addSubForumModerator(string forumName, string subForumName, string userName, string assigningAdmin, int trialTime, DateTime assignment);   //boolean on sucesses
        Boolean removeSubForumModerator(string forumName, string subForumName, string userName);   //boolean on sucesses
        Boolean addForumMember(string forumName, string userName);            //boolean on sucesses
        Boolean removeForumMember(string forumName, string userName);         //boolean on sucesses
        Boolean addForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup);            //boolean on sucesses
        Boolean removeForumPolicy(string forumName);         //boolean on sucesses
        Boolean changeModeratorTrialTime(string forumName, string subForumName, string userName, int trialTime);   //boolean on sucesses
        Boolean changeForumPost(string title, string content, string forumName, string subForumName, int serialNumber);   //boolean on sucesses
        Boolean addForum(string forumName);            //boolean on sucesses
        Boolean removeForum(string forumName);         //boolean on sucesses
        Boolean addForumAdmin(string forumName, string userName);            //boolean on sucesses
        Boolean removeForumAdmin(string forumName, string userName);         //boolean on sucesses
        Boolean changeForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators, string forumName, int pdp, int passLife, int moderatorSen, int mup);   //boolean on sucesses
        List<Tuple<string, string>> ReturnforumMembers(string forumName);    // forumName, userName
        List<Tuple<string, string>> ReturnforumAdmins(string forumName);        // forumName, userName
        List<string> ReturnForumsList();                                        //list of all forums..
        List<string> ReturnSubForumList(string forumName);                      //list of all subforums of a given forum
        List<Tuple<Tuple<int, int>, Tuple<int, int>, Tuple<int, int>, Tuple<int, int>>> ReturnforumPolicy(string forumName);     //there is tuple with 4 tuples inside, first tuple- maxAdmins, minAdmins second- maxModerators, minModerators third- pdp, passwordLife fourth- moderatorSen, mup 
        List<Tuple<string, string, int, DateTime>> ReturnSubforumModerators(string forumName, string subForumName);        //username,assigningAdmin,trialTime,assignment
        List<Tuple<string, string, string, DateTime, int, int>> ReturnSubforumPosts(string forumName, string subForumName);       //title, content, username, creation, serialNumber, parentPost
        List<Tuple<string, string, DateTime, string>> ReturforumMessages(string forumName);                                   //writer, sendTo, creationDate, text

        //  changed
        List<Tuple<string, string, string, DateTime, DateTime, string, string>> ReturnForumUsers(string forumName);                      //username,password,email,registration,lastPassChange, secutiryQuestion, answer

        // added
        // should have a new table - post deletion followers - with 2 columns
        // columns are post serial number and username
        List<string> ReturnPostFollowers(int postSerialNumber);

        // added
        // add a new line to the table mentioned above
        bool addPostFollower(int postSerialNumber, string username);
    }
}
