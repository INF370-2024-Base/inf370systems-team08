using Microsoft.EntityFrameworkCore;

using EduProfileAPI.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;

namespace EduProfileAPI.DataAccessLayer
{
    public class EduProfileDbContext : DbContext
    {
        public EduProfileDbContext(DbContextOptions options) : base(options)
        {
        }

    }
}