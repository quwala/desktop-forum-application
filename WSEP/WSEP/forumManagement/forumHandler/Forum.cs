using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.forumHandler
{
     public class Forum
    {
        private ForumPolicy _policy;
        private string _name;

        private List<SubForum> _subForums;
        public List<SubForum> SubForums
        {
            get
            {
                return _subForums;
            }
        }



        public Forum(string name) 
        {
            ForumPolicy nPolicy = new ForumPolicy("Default Policy");//defualt policy, can change later
            setPolicy(nPolicy);
            checkForumName(name);
            this._name = name;
            _subForums = new List<SubForum>();
        }

        public bool setPolicy(ForumPolicy p)
        {
            if (p == null)
                throw new Exception("Cannot set a null policy");

            this._policy = p;
            return true;
        }

        public ForumPolicy getPolicy() { return this._policy; }

        public bool addSubForum(string name)
        {

            foreach (SubForum s in _subForums)
                if (s.getName().Equals(name))
                    throw new Exception("A Sub Forum with that name already exists");

            SubForum sf = new SubForum(name);

            _subForums.Add(sf);
            return true;
        }

         public SubForum getSubForum(string name)
        {
            foreach (SubForum s in _subForums)
                if (s.getName().Equals(name))
                    return s;

           throw new Exception("No Sub Forum with the name: "+name+" was found");
        }

        private void checkForumName(string name)
        {
            if (name == null)
                throw new InvalidNameException("Name of the forum cannot be null");


            if ( name.Equals("")) 
                 throw new InvalidNameException("Name of the forum cannot be empty");

            if(name.Contains("%") || name.Contains("&") || name.Contains("@"))
                 throw new InvalidNameException("Name of the forum contains illegal character");

            if(name[0].Equals(' '))
                throw new InvalidNameException("Name of the forum cannot begin with a space character");

        }

        void changeForumPolicy(string name, int minAdmins, int maxAdmins, string forumRules)
        {
            //create new policy object and attach to Policy field.
        }

       public string getName()
        {
            return this._name;
        }
    }
}
