namespace WSEP_domain.forumManagement.forumHandler
{
    public enum postDeletionPermission { INVALID = 1, WRITER = 2, MODERATOR = 3, ADMIN = 4, SUPER_ADMIN = 5 };
    public enum modUnassignmentPermission { INVALID = 1, ADMIN = 2, ASSIGNING_ADMIN = 3, SUPER_ADMIN = 4 };

    public class ForumPolicy
    {
        public const int DEFAULT_MAX_ADMINS = 10;
        public const int DEFAULT_MIN_ADMINS = 1;
        public const int DEFAULT_MAX_MODS = 10;
        public const int DEAFULT_MIN_MODS = 1;
        public const int DEFAULT_PDP = 2;
        public const int DEFAULT_LIFESPAN = 365;
        public const int DEFAULT_SENIORITY = 0;
        public const int DEFAULT_MUP = 2;

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

        public static ForumPolicy create(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators,
            postDeletionPermission pdp, int passwordLifespan, int moderatorSeniority, modUnassignmentPermission mup)
        {
            if (minNumOfAdmins > maxNumOfAdmins || minNumOfModerators > maxNumOfModerators || maxNumOfAdmins <= 0 || maxNumOfModerators <= 0 ||
                passwordLifespan <= 0 || moderatorSeniority < 0 || pdp == postDeletionPermission.INVALID || mup == modUnassignmentPermission.INVALID)
            {
                return null;
            }
            return new ForumPolicy(maxNumOfAdmins, minNumOfAdmins, maxNumOfModerators, minNumOfModerators, pdp, passwordLifespan, moderatorSeniority, mup);
        }

        private ForumPolicy(int maxNumOfAdmins, int minNumOfAdmins, int maxNumOfModerators, int minNumOfModerators,
            postDeletionPermission pdp, int passwordLifespan, int moderatorSeniority, modUnassignmentPermission mup)
        {
            _maxNumOfAdmins = maxNumOfAdmins;
            _minNumOfAdmins = minNumOfAdmins;
            _maxNumOfModerators = maxNumOfModerators;
            _minNumOfModerators = minNumOfModerators;
            _pdp = pdp;
            _passwordLifespan = passwordLifespan;
            _moderatorSeniority = moderatorSeniority;
            _mup = mup;
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
            if (maxAdmins < _minNumOfAdmins)
            {
                return;
            }
            _maxNumOfAdmins = maxAdmins;
        }

        public void setMinAdmins(int minAdmins)
        {
            if (minAdmins > _maxNumOfAdmins)
            {
                return;
            }
            _minNumOfAdmins = minAdmins;
        }

        public void setMaxModerators(int maxModerators)
        {
            if (maxModerators < _minNumOfModerators)
            {
                return;
            }
            _maxNumOfModerators = maxModerators;
        }

        public void setMinModerators(int minModerators)
        {
            if (minModerators > _maxNumOfModerators)
            {
                return;
            }
            _minNumOfModerators = minModerators;
        }

        public void setPasswordLifespan(int lifespan)
        {
            if (lifespan <= 0)
            {
                return;
            }
            _passwordLifespan = lifespan;
        }

        public void setModeratorsSeniority(int seniority)
        {
            if (seniority < 0)
            {
                return;
            }
            _moderatorSeniority = seniority;
        }

        public void setPostDeletionPermissions(postDeletionPermission permission)
        {
            if (permission == postDeletionPermission.INVALID)
            {
                return;
            }
            _pdp = permission;
        }

        public void setModUnassignmentPermission(modUnassignmentPermission permission)
        {
            if (permission == modUnassignmentPermission.INVALID)
            {
                return;
            }
            _mup = permission;
        }
    }
}
