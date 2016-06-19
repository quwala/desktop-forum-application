using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ForumsDataBase
{
    class Program
    {
        static void Main(string[] args)
        {
            DB db = new DB();

            DateTime d = db.ReturnSubforumModerators("forumName1", "subForumName1").First().Item4;
            Boolean b = true;
           // Console.WriteLine(b);
            Console.WriteLine(db.ReturnforumMembers("forumName").Count());                               //bad removing

            b = db.removeForumUser("userName","forumName22");
          //  Console.WriteLine(b);
            Console.Read();
        }
    }
}
