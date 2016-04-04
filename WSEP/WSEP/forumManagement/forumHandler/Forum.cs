using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.forumHandler
{
    class Forum
    {
        private Policy _policy;
        private string name;

        public Forum(string name) 
        {
            Policy nPolicy = new Policy();//defualt policy, can change later
            _policy = nPolicy;
            checkName(name);

                this.name = name;
        }

        private void checkName(string name)
        {
            if(name.Equals("")) 
                 throw new InvalidNameException("Name of the forum cannot be empty");

            if(name.Contains("%") || name.Contains("&") || name.Contains("@"))
                 throw new InvalidNameException("Name of the forum contains illegal character");

        }

        void changePolicy()
        {
            //get params as strings, create new policy object and attach to Policy field.
        }

       public string getName()
        {
            return this.name;
        }
    }
}
