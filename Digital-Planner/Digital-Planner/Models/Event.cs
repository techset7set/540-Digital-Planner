//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Digital_Planner.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public System.DateTime OccursAt { get; set; }
        public System.TimeSpan Duration { get; set; }
        public System.DateTime CompleteBy { get; set; }
        public int Priority { get; set; }
        public bool IsComplete { get; set; }
        public bool AutoAssign { get; set; }
        public string Location { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
    
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
    }
}
