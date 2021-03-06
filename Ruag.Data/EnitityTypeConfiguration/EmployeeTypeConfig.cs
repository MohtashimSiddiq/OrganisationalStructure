﻿using Ruag.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Data.EnitityTypeConfiguration
{
    public class EmployeeTypeConfig:EntityTypeConfiguration<Employee>
    {
        public EmployeeTypeConfig()
        {
            this.Property<int>(e => e.Id).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            this.HasOptional<OrgRole>(e => e.EmployeeRole);
            this.Ignore(p => p.Manager);
            this.Ignore(p => p.ManagerId);
            this.Ignore(p => p.SubOrdinates);
            this.Property(p => p.IsDeleted).IsOptional();
        }

    }
}
