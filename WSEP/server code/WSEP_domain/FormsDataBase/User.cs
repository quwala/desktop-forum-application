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
    
    public partial class User
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public System.DateTime registration { get; set; }
        public string forumName { get; set; }
        public System.DateTime lastPassChange { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
    }
}
