using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSEP.forumManagement.threadsHandler
{
    public class Post
    {
        private string _id;
        private string _title;
        private string _userName;
        private string _content;
        private DateTime _date;
        private List<Post> _replies;

        public Post()
        {
        }

    }
}
