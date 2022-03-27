using Microsoft.EntityFrameworkCore;

namespace CMIUICXCore.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AudioRecordingData> AudioRecordingDataSet { get; set; }
    }
}
