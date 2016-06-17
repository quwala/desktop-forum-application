using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    interface iWebClient
    {
        List<Tuple<string, DateTime, int>> getThreads(string forumName, string subForumName, string requestingUser);

        void updateLocation(List<string> loc);
        List<Tuple<string, string, DateTime, int, int, string, DateTime>> getThread(string forumName, string subForumName, int openPostNo, string requestingUser);
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
        string writePost(string forumName, string subForumName, int parentPostNo, string username, string title, string content);
        loginStatus login(string forumname, string username, string password);

        permission getUserPermissionsForForum(string forumName, string username);
        permission getUserPermissionsForSubForum(string forumName, string subForumName, string username);
        
    }
}
