﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EJournal.Database;

[PrimaryKey("StudentId", "LessonId")]
[Table("marks", Schema = "ejournal")]
public partial class Mark
{
    [Key]
    [Column("student_id")]
    public int StudentId { get; set; }

    [Key]
    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("mark")]
    public int? Mark1 { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("Marks")]
    public virtual Lesson Lesson { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("Marks")]
    public virtual Student Student { get; set; }
}