using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
  public  class forum
    {
        public string forumName { get; set; }
        public string superAdmin { get; set; }
        public int numOfUsers { get; set; }
        public forum(string p1, string p2, int p3)
        {
            // TODO: Complete member initialization
            forumName = p1;
            superAdmin = p2;
            numOfUsers = p3;
        }

        public forum()
        {
            forumName = "deafault";
            superAdmin = "supAdmin deafault";
            numOfUsers = 0;
        }

        public void writeData(){
            Console.WriteLine("name:" + forumName + "\nsupAdmin:" + superAdmin + "\nnum of users: {0}\n", numOfUsers);
        }

    }
}
