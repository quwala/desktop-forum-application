using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{

    public enum permission { INVALID = 1, GUEST = 2, MEMBER = 3, MODERATOR = 4, ADMIN = 5, SUPER_ADMIN = 6 };
    public enum loginStatus { TRUE, FALSE, UPDATE_PASSWORD };
    public enum postDeletionPermission { INVALID = 1, WRITER = 2, MODERATOR = 3, ADMIN = 4, SUPER_ADMIN = 5 };
    public enum modUnassignmentPermission { INVALID = 1, ADMIN = 2, ASSIGNING_ADMIN = 3, SUPER_ADMIN = 4 };

    public interface Iclient
    {
        bool start();
         string addForum(string forumName, string requestingUser);

        /// <summary>
        /// adds a new sub forum to a forum and sets new moderators
        /// </summary>
        /// <param name="forumName"> the forum wto which the sub forum will be added </param>
        /// <param name="subForumName"> the name of the sub foorum to be added </param>
        /// <param name="moderators"> list of username of the user to become a moderator, assigning admin (may be anything as unused inside), assignment time in days </param>
        /// <param name="requestingUser"> username of the requesting user to verify requesting user is an admin of that forum </param>
        /// <returns>
        /// "true" if succedded
        /// detailed error message otherwise
        /// </returns>
        string addSubForum(string forumName, string subForumName, List<Tuple<string, string, int>> moderators, string requestingUser);
        string registerMemberToForum(string forumName, string username, string password, string eMail);
        string assignAdmin(string forumName, string username, string requestingUser);
        string unassignAdmin(string forumName, string username, string requestingUser);
        string assignModerator(string forumName, string subForumName, string username, string requestingUser, int days);
        string unassignModerator(string forumName, string subForumName, string username, string requestingUser);
        permission getUserPermissionsForForum(string forumName, string username);
        permission getUserPermissionsForSubForum(string forumName, string subForumName, string username);
        string sendPM(string forumName, string from, string to, string msg);
        string checkForumPolicy(string forumName, ForumPolicy policy);
        loginStatus login(string forumname, string username, string password);
        string setForumMaxAdmins(string forumName, int maxAdmins, string requestingUser);
        string setForumMinAdmins(string forumName, int minAdmins, string requestingUser);
        string setForumMaxModerators(string forumName, int maxModerators, string requestingUser);
        string setForumMinModerators(string forumName, int minModerators, string requestingUser);
        string setForumPostDeletionPermissions(string forumName, postDeletionPermission permission, string requestingUser);
        string setForumPasswordLifespan(string forumName, int lifespan, string requestingUser);
        string setForumModeratorsSeniority(string forumName, int seniority, string requestingUser);
        string setForumModUnassignmentPermissions(string forumName, modUnassignmentPermission permission, string requestingUser);

        /// <summary>
        /// creates a new post
        /// can be a new thread in sub forum or a reply to another post
        /// </summary>
        /// <param name="forumName"> name of the forum where that post would appear </param>
        /// <param name="subForumName"> name of the sub forum where that post would appear </param>
        /// <param name="parentPostNo"> 0 for a new thread, actual post number for replying within a thread </param>
        /// <param name="username"> username of the post writer </param>
        /// <param name="title"> post title </param>
        /// <param name="content"> post content </param>
        /// <returns></returns>
        /// 

        // if this post is the first post of a new thread parent post num should be 0
        string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, string content);
        string setModeratorTrialTime(string forumName, string subForumName, string moderator, int newTime, string requestingUser);
        string deletePost(string forumName, string subForumName, int postNo, string requestingUser);
        string editPost(string forumName, string subForumName, int postNo, string requestingUser, string content);
        int getNumOfPostsInSubForum(string forumName, string subForumName, string requestingUser);

        /// <summary>
        /// enables an admin of a forum to get a list of data about all messages of a single member of that forum
        /// </summary>
        /// <param name="forumName"> name of the forum </param>
        /// <param name="memberName"> name of the member to get data about his messages </param>
        /// <param name="requestingUser"> username of requesting user to verify requesting user is an admin of that forum </param>
        /// <returns>
        /// null if invalid input
        /// empty list if no such forum exist
        /// list of message title, message content, date of message creation, message serial number
        /// </returns>
        List<Tuple<string, string, DateTime, int>> getListOfMemberMessages(string forumName, string memberName, string requestingUser);
        
        /// <summary>
        /// enables an admin of a forum to get a list of data about all moderators of that forum
        /// </summary>
        /// <param name="forumName"> name of the forum </param>
        /// <param name="requestingUser"> username of requesting user to verify requesting user is an admin of that forum </param>
        /// <returns>
        /// null if invalid input
        /// empty list if no such forum exist
        /// list of moderator username, assigning admin username, assignment date, sub forum to moderate
        /// </returns>
        List<Tuple<string, string, DateTime, string>> getListOfForumModerators(string forumName, string requestingUser);

        int numOfForums(string requestingUser);
        List<string> getForums();

        /// <summary>
        /// gets a list of the names of all the sub forums of a selected forum
        /// </summary>
        /// <param name="forumName"> name of the forum </param>
        /// <param name="requestingUser"> username of requesting user to verify he have permissions to view sub forums in the selected forum </param>
        /// <returns>
        /// null if invalid input
        /// list of all sub forums names of the given forum
        /// if no such forum exist than an empty list will be returned
        /// </returns>
        List<string> getSubForums(string forumName, string requestingUser);

        /// <summary>
        /// gets a list of data of all threads in a selected sub forum
        /// </summary>
        /// <param name="forumName"> the forum where the selected sub forum is </param>
        /// <param name="subForumName"> selected sub forum name </param>
        /// <param name="requestingUser"> username of requesting user to verify he have permissions to view threads in the selected forum </param>
        /// <returns>
        /// null if invalid input
        /// list of thread title, thread creatin date, opening post serial number
        /// if no such forum or sub forum exist than an empty list will be returned
        /// </returns>
        List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser);

        /// <summary>
        /// get a list of data of all posts of a single thread
        /// </summary>
        /// <param name="forumName"> name of the forum where that thread is </param>
        /// <param name="subForumName"> name of the sub forum where that thread is </param>
        /// <param name="openPostNo"> serial number of the thread's opening post </param>
        /// <param name="requestingUser"> username of requesting user to verify he have permissions to view posts in the selected forum </param>
        /// <returns>
        /// null if invalid input
        /// list of post title, post content, post creation date, post serial number, parent post serial number, username of the writer, writer reegistration date
        /// the posts are added to the list in a DFS
        /// if no such forum, sub forum or post (with the given serial number in that sub forum) exist than an empty list will be returned
        /// </returns>
        List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser);


        List<string> getUsersInForum(string forumName, string requestingUser);
        List<string> getAllUsers( string requestingUser);

    }
}
