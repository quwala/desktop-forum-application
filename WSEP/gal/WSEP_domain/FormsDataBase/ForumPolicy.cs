//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ForumsDataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class ForumPolicy
    {
        public int maxNumOfAdmins { get; set; }
        public int minNumOfAdmins { get; set; }
        public int maxNumOfModerators { get; set; }
        public int minNumOfModerators { get; set; }
        public string forumName { get; set; }
        public int pdp { get; set; }
        public int passwordLifespan { get; set; }
        public int moderatorSeniority { get; set; }
        public int mup { get; set; }
    }
}