//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDES___Webcams.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class cam
    {
        public int camId { get; set; }
        public string camName { get; set; }
        public string camShortDesc { get; set; }
        public string camLongDesc { get; set; }
        public string camLocationName { get; set; }
        public string camLocationId { get; set; }
        public decimal camLat { get; set; }
        public decimal camLong { get; set; }
        public byte camInterval { get; set; }
        public string camJpegUri { get; set; }
        public byte camJpegQuality { get; set; }
        public bool isEnabled { get; set; }
        public bool isBlurred { get; set; }
        public System.DateTime created { get; set; }
    }
}