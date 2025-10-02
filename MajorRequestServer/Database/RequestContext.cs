using MajorRequestServer.Dto;
using MajorRequestServer.Models;
using Microsoft.EntityFrameworkCore;

namespace MajorRequestServer.Database
{
    public class RequestContext : DbContext
    {
        public RequestContext(DbContextOptions<RequestContext> options) : base(options)
        {
            Database.EnsureCreated(); //Создаем БД если нет
        }
        /// <summary>
        /// Контексты таблиц - Request, Status, Courier
        /// </summary>
        public DbSet<Request> Requests { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Courier> Couriers { get; set; }

        /// <summary>
        /// Свои DTO
        /// </summary>
        public DbSet<RequestDto> RequestDtos { get; set; }

        //Для установления своих имен в БД а не тех что сгенерит EF
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>().ToTable("Request");
            modelBuilder.Entity<Status>().ToTable("Status");
            modelBuilder.Entity<Courier>().ToTable("Courier");
        }
    }
}
