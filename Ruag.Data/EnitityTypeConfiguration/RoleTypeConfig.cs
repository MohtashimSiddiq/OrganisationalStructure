using Ruag.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.EnitityTypeConfiguration
{
    public class OrgRoleTypeConfig:EntityTypeConfiguration<OrgRole>
    {
        public OrgRoleTypeConfig()
        {
            this.Property(p => p.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.Property(p => p.Name).IsRequired().HasMaxLength(64);
            this.Property(p => p.Description).IsRequired().HasMaxLength(128);
            this.HasOptional<OrgRole>(e => e.ParentRole).WithMany(p => p.ChildRoles);
            
        }

    }
}
