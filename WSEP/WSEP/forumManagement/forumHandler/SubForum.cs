using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.forumHandler
{
    public class SubForum
    {
        private SubForumPolicy _policy;
        private string name;

        public SubForum(string name)
        {
            SubForumPolicy nPolicy = new SubForumPolicy("Default Policy");//defualt policy, can change later
            setPolicy(nPolicy);
            checkForumName(name);
            this.name = name;
        }

        public bool setPolicy(SubForumPolicy p)
        {
            if (p == null)
                throw new Exception("Cannot set a null policy");

            this._policy = p;
            return true;
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
