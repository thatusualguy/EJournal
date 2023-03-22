﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EJournal.Database;

[Table("accounts", Schema = "ejournal")]
public partial class Account
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("email", TypeName = "character varying")]
    public string Email { get; set; }

    [Column("phone", TypeName = "character varying")]
    public string Phone { get; set; }

    [Column("password", TypeName = "character varying")]
    public string Password { get; set; }

    [Column("role")]
    public int? Role { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [ForeignKey("Role")]
    [InverseProperty("Accounts")]
    public virtual UserRole RoleNavigation { get; set; }
}