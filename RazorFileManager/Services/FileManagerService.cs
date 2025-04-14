using Microsoft.EntityFrameworkCore;
using RazorFileManager.Data;
using RazorFileManager.Data.Models;

namespace RazorFileManager.Services
{
    public class FileManagerService
    {
        private IDbContextFactory<FileDbContext> _dbContext;
        public FileManagerService(IDbContextFactory<FileDbContext> dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Folder?> GetRootFolderContent()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Folders.Include(f => f.SubFolders).Include(f => f.Files).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Folder>> GetAllContent()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Folders.Include(f => f.SubFolders).Include(f => f.Files).ToListAsync();
            }
        }

        public async Task<List<Folder>> GetFolders()
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Folders.ToListAsync();
            }
        }
        public async Task<Folder?> GetFolderById(int id)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                return await context.Folders.FirstOrDefaultAsync(x => x.FolderId == id);
            }
        }

        public async Task AddFolder(Folder folder)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                context.Folders.Add(folder);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateFolder(Folder folder)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                context.Folders.Update(folder);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveFolder(Folder folder)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                context.Folders.Remove(folder);
                await context.SaveChangesAsync();
            }
        }

        public async Task AddFile(FileItem file)
        {
            using (var context = _dbContext.CreateDbContext())
            {
                context.Files.Add(file);
                await context.SaveChangesAsync();
            }
        }
    }
}
