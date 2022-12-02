﻿using Microsoft.AspNetCore.Identity;

namespace EduHomeFinalProject.DAL.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
