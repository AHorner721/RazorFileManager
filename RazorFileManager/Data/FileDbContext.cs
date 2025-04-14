using Microsoft.EntityFrameworkCore;
using RazorFileManager.Data.Models;

namespace RazorFileManager.Data
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<FileItem> Files { get; set; }
    }
}
