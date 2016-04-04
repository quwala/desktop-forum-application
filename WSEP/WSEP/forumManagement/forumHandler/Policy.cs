using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.forumHandler
{
    public class Policy
    {
        private string name;
        public string Name
        {
            get {
                return name;
            }

            set {
                checkPolicyName(value);
                    name = value;
            }
        }

        private int minAdmins;
        public int MinAdmins
        {
            get { return minAdmins; }
            set {
                if (value < 1)
                    throw new Exception(
                        "Minimum number of admins cannot be smaller than 1.");
                       
                minAdmins = value; }
        }

        private int maxAdmins;
        public int MaxAdmins
        {
            get { return maxAdmins; }
            set {
                if (value < minAdmins)
                    throw new Exception(
                        "Maximum number of admins cannot be smaller than minimal number of admins");
                maxAdmins = value; }
        }

        private int minModerators;
        public int MinModerators
        {
            get { return minModerators; }
            set {
                if (value < 1)
                    throw new Exception(
                        "Minimum number of moderators cannot be smaller than 1.");

                minModerators = value;
            }
        }

        private int maxModerators;
        public int MaxModerators
        {
            get { return maxModerators; }
            set {
                if (value < minModerators)
                    throw new Exception(
                        "Maximum number of moderators cannot be smaller than minimal number of moderators");
                maxModerators = value;
            }
        }

        private string forumRules;
        public string ForumRules
        {
            get { return forumRules; }
            set { forumRules = value; }
        }

        public Policy(string name)
        {
            checkPolicyName(name);
            this.name = name;
            this.minAdmins = 1;
            this.maxAdmins = 10;//default values
            this.minModerators = 1;
            this.maxModerators = 10;//default values
            this.forumRules = "Have Fun!";


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

    }
}
