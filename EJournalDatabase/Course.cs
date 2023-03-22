﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EJournal.Database;

[Table("courses", Schema = "ejournal")]
public partial class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name", TypeName = "character varying")]
    public string Name { get; set; }

    [Column("hour_count")]
    public int? HourCount { get; set; }

    [Column("end_mark")]
    public int? EndMark { get; set; }

    [Column("semester")]
    public int? Semester { get; set; }

    [ForeignKey("EndMark")]
    [InverseProperty("Courses")]
    public virtual MarkType EndMarkNavigation { get; set; }

    [InverseProperty("Course")]
    public virtual ICollection<Subject> Subjects { get; } = new List<Subject>();
}