using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP_domain.forumManagement.forumHandler
{
    public enum postDeletionPermission { INVALID = 1, WRITER = 2, MODERATOR = 3, ADMIN = 4, SUPER_ADMIN = 5 };
    public enum modUnassignmentPermission { INVALID = 1, ADMIN = 2, ASSIGNING_ADMIN = 3, SUPER_ADMIN = 4 };

    public class ForumPolicy
    {
        private int _maxNumOfAdmins;
        private int _minNumOfAdmins;
        private int _maxNumOfModerators;
        private int _minNumOfModerators;
        private postDeletionPermission _pdp;
        private int _passwordLifespan;
        private int _moderatorSeniority;
        private modUnassignmentPermission _mup;

        public ForumPolicy()
        {
            _maxNumOfAdmins = 10;
            _minNumOfAdmins = 1;
            _maxNumOfModerators = 10;
            _minNumOfModerators = 1;
            _pdp = postDeletionPermission.WRITER;
            _passwordLifespan = 365;
            _moderatorSeniority = 0;
            _mup = modUnassignmentPermission.ADMIN;
        }

        public int getMaxAdmins()
        {
            return _maxNumOfAdmins;
        }

        public int getMinAdmins()
        {
            return _minNumOfAdmins;
        }
       
        public int getMaxModerators()
        {
            return _maxNumOfModerators;
        }

        public int getMinModerators()
        {
            return _minNumOfModerators;
        }

        public int getSeniority()
        {
            return _moderatorSeniority;
        }

        public postDeletionPermission getPostDeletionPermissions()
        {
            return _pdp;
        }

        public int getPasswordLifespan()
        {
            return _passwordLifespan;
        }

        public int getModeratorsSeniority()
        {
            return _moderatorSeniority;
        }

        public modUnassignmentPermission getModUnassignmentPermissions()
        {
            return _mup;
        }

        public void setMaxAdmins(int maxAdmins)
        {
            _maxNumOfAdmins = maxAdmins;
        }

        public void setMinAdmins(int minAdmins)
        {
            _minNumOfAdmins = minAdmins;
        }

        public void setMaxModerators(int maxModerators)
        {
            _maxNumOfModerators = maxModerators;
        }

        public void setMinModerators(int minModerators)
        {
            _minNumOfModerators = minModerators;
        }

        public void setPasswordLifespan(int lifespan)
        {
            _passwordLifespan = lifespan;
        }

        public void setModeratorsSeniority(int seniority)
        {
            _moderatorSeniority = seniority;
        }

        public void setPostDeletionPermissions(postDeletionPermission permission)
        {
            _pdp = permission;
        }

        public void setModUnassignmentPermission(modUnassignmentPermission permission)
        {
            _mup = permission;
        }
    }
}
