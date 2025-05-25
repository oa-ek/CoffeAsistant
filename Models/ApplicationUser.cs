﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
namespace CafeAssistiant.Models;


public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? City { get; set; }
    public string? Address { get; set; }
}

