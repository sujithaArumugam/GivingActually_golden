//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GivingActually_May.DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_ParentComment
    {
        public int CommentID { get; set; }
        public int UserId { get; set; }
        public string CommentMessage { get; set; }
        public Nullable<System.DateTime> CommentDate { get; set; }
        public int StoryId { get; set; }
    
        public virtual Tbl_Campaign Tbl_Campaign { get; set; }
        public virtual Tbl_User Tbl_User { get; set; }
    }
}
