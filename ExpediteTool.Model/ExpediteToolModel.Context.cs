﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpediteTool.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ACPReportsEntities : DbContext
    {
        public ACPReportsEntities()
            : base("name=ACPReportsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<HotLot_BUAllocation> HotLot_BUAllocation { get; set; }
        public virtual DbSet<Hotlot_BUs> Hotlot_BUs { get; set; }
        public virtual DbSet<HotLot_Roles> HotLot_Roles { get; set; }
        public virtual DbSet<GP_vSCMInvLatestSnapshot> GP_vSCMInvLatestSnapshot { get; set; }
        public virtual DbSet<HotLot_Users> HotLot_Users { get; set; }
        public virtual DbSet<HotLot_Data> HotLot_Data { get; set; }
    }
}
