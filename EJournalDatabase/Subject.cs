﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EJournal.Database;

[Table("subjects", Schema = "ejournal")]
public partial class Subject
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("course_id")]
    public int? CourseId { get; set; }

    [Column("teacher_id")]
    public int? TeacherId { get; set; }

    [Column("group_id")]
    public int? GroupId { get; set; }

    [ForeignKey("CourseId")]
    [InverseProperty("Subjects")]
    public virtual Course Course { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("Subjects")]
    public virtual Group Group { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<Lesson> Lessons { get; } = new List<Lesson>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Subjects")]
    public virtual Teacher Teacher { get; set; }
}