using Microsoft.EntityFrameworkCore;
using Infrastructure.Configurations;
using Core.DTO.Functions;
using Core.Models;

namespace Infrastructure.Data
{
    public class MyDBcontext : DbContext
    {
        public MyDBcontext(DbContextOptions<MyDBcontext> options):base(options)
        {

        }

        public DbSet<FuncAvgCheckDto> ReportResultsAvg { get; set; }

        public DbSet<FuncSumOnBirthDayDto> ReportResultsSum { get; set; }
        
        public DbSet <Client> Clients { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            modelBuilder.Entity<FuncAvgCheckDto>().HasNoKey();

            modelBuilder.Entity<FuncSumOnBirthDayDto>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }

        public async Task<List<FuncAvgCheckDto>> GetReportAvgAsync()
        {
            return await ReportResultsAvg
                .FromSqlRaw("SELECT * FROM get_avg_check_by_hour()") // или хранимая процедура
                .ToListAsync();
        }

        public async Task<List<FuncSumOnBirthDayDto>> GetReportSumAsync()
        {
            return await ReportResultsSum
                .FromSqlRaw("SELECT * FROM get_sum_by_client_on_birthday()") // или хранимая процедура
                .ToListAsync();
        }

    }
}
