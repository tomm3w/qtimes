//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace userinfo.dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.UserAddons = new HashSet<UserAddon>();
            this.UserRegions = new HashSet<UserRegion>();
            this.Users1 = new HashSet<User>();
        }
    
        public System.Guid UserId { get; set; }
        public System.Guid ApplicationId { get; set; }
        public string UserName { get; set; }
        public bool IsAnonymous { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public Nullable<System.Guid> ParentUserId { get; set; }
    
        public virtual ICollection<UserAddon> UserAddons { get; set; }
        public virtual UserData UserData { get; set; }
        public virtual ICollection<UserRegion> UserRegions { get; set; }
        public virtual ICollection<User> Users1 { get; set; }
        public virtual User User1 { get; set; }
    }
}