using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.forumHandler
{
    public class SubForum
    {
        private string name;

        public SubForum(string name)
        {
            checkForumName(name);
            this.name = name;
        }

       

        private void checkForumName(string name)
        {
            if (name == null)
                throw new InvalidNameException("Name of the Sub Forum cannot be null");


            if (name.Equals(""))
                throw new InvalidNameException("Name of the Sub Forum cannot be empty");

            if (name.Contains("%") || name.Contains("&") || name.Contains("@"))
                throw new InvalidNameException("Name of the Sub Forum contains illegal character");

            if (name[0].Equals(' '))
                throw new InvalidNameException("Name of the Sub Forum cannot begin with a space character");

        }

        void changeSubForumPolicy(string name, int minModerators, int maxModerators, string subForumRules)
        {
            //create new policy object and attach to Policy field.
        }

        public string getName()
        {
            return this.name;
        }
    }
}
