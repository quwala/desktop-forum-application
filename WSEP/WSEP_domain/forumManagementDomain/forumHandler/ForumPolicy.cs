using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSEP_doamin.forumManagementDomain;

namespace WSEP_domain.forumManagementDomain.forumHandler
{
    public class ForumPolicy
    {
        private string _name;
        public string Name
        {
            get {
                return _name;
            }

            set {
                checkPolicyName(value);
                    _name = value;
            }
        }

        private int _minAdmins;
        public int MinAdmins
        {
            get { return _minAdmins; }
            set {
                if (value < 1)
                    throw new Exception(
                        "Minimum number of admins cannot be smaller than 1");
                       
                _minAdmins = value; }
        }

        private int _maxAdmins;
        public int MaxAdmins
        {
            get { return _maxAdmins; }
            set {
                if (value < MinAdmins)
                    throw new Exception(
                        "Maximum number of admins cannot be smaller than minimal number of admins");
                _maxAdmins = value; }
        }

        private int _minModerators;
        public int MinModerators
        {
            get { return _minModerators; }
            set
            {
                if (value < 1)
                    throw new Exception(
                        "Minimum number of moderators cannot be smaller than 1.");

                _minModerators = value;
            }
        }

        private int _maxModerators;
        public int MaxModerators
        {
            get { return _maxModerators; }
            set
            {
                if (value < MinModerators)
                    throw new Exception(
                        "Maximum number of moderators cannot be smaller than minimal number of moderators");
                _maxModerators = value;
            }
        }

        private string _forumRules;
        public string ForumRules
        {
            get { return _forumRules; }
            set { _forumRules = value; }
        }

        //default policy
        public ForumPolicy(string name)
        {
            this.Name = name;
            this.MinAdmins = 1;
            this.MaxAdmins = 10;//default values
            this.MinModerators = 1;
            this.MaxModerators = 10;//default values
            this.ForumRules = "Have Fun!";

        }

        public ForumPolicy(string name, int minAdmins, int maxAdmins,
            int minModerators, int maxModerators, string forumRules)
        {
            this.Name = name;
            this.MinAdmins = minAdmins;
            this.MaxAdmins = maxAdmins;
            this.MinModerators = minModerators;
            this.MaxModerators = maxModerators;
            this.ForumRules = forumRules;

        }


        private void checkPolicyName(string name)
        {
            if (name == null)
                throw new InvalidNameException("Name of the policy cannot be null");

            if (name.Equals(""))
                throw new InvalidNameException("Name of the policy cannot be empty");

            if (name.Contains("%") || name.Contains("&") || name.Contains("@"))
                 throw new InvalidNameException("Name of the policy contains illegal character");

           if (name[0].Equals(' '))
                throw new InvalidNameException("Name of the policy cannot begin with a space character");

        }

        public string getName()
        {
            throw new NotImplementedException();
        }
    }
}
