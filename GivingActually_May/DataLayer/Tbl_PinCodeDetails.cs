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
    
    public partial class Tbl_PinCodeDetails
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public string CountryIndicator { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public int CountryId { get; set; }
        public string pincode { get; set; }
    
        public virtual Tbl_Countries Tbl_Countries { get; set; }
    }
}
