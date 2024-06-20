using Libaray_Management_System.Entities;
using Libaray_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System;

namespace Libaray_Management_System.Data
{
    public class MemberDBContext : DbContext
    {
        /*private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public MemberDBContext(IConfiguration configuratation)
        {
            _configuration = configuratation;
            _connectionString = _configuration.GetConnectionString("MySqlDBString");

        }*/

        public MemberDBContext(DbContextOptions<MemberDBContext> context) : base(context)
        {
        }
        public DbSet<MemberEntity> MemberEntity { get; set; }

      
       /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
            }
        }*/
    }
}
