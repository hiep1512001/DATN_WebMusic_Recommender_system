﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rs_content.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Web_musicEntities : DbContext
    {
        public Web_musicEntities()
            : base("name=Web_musicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ALBUM> ALBUMs { get; set; }
        public virtual DbSet<COMPOSER> COMPOSERs { get; set; }
        public virtual DbSet<GENRE> GENRES { get; set; }
        public virtual DbSet<PLAYLIST> PLAYLISTs { get; set; }
        public virtual DbSet<RATING> RATINGs { get; set; }
        public virtual DbSet<SINGER> SINGERs { get; set; }
        public virtual DbSet<SONG> SONGs { get; set; }
        public virtual DbSet<SONGPLAYLIST> SONGPLAYLISTs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<USER> USERS { get; set; }
    }
}
