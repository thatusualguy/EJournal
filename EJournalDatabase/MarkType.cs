﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EJournal.Database;

[Table("mark_types", Schema = "ejournal")]
public partial class MarkType
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; }

    [InverseProperty("EndMarkNavigation")]
    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}