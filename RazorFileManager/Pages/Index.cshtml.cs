using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RazorFileManager.Data;
using RazorFileManager.Data.Models;
using RazorFileManager.Services;

namespace RazorFileManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FileManagerService _fileManagerService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, FileManagerService fileManagerService)
        {
            _logger = logger;
            _fileManagerService = fileManagerService;
        }

        public Folder RootFolder { get; set; }
        public List<Folder> AllFolders { get; set; }
        public List<Folder> RootFolders { get; set; }
        [BindProperty]
        public Folder NewFolder { get; set; }

        [BindProperty]
        public FileItem NewFile { get; set; }
        public SelectList Folders { get; set; }
        [BindProperty]
        public int SelectedFolderId { get; set; }
        [BindProperty]
        public int? ParentFolderId { get; set; }

        public async Task OnGet()
        {
            await InitRootFolder();
            await SetupFoldersList();
            NewFolder = new Folder { Name = "", UserId = 1 };
            NewFile = new FileItem { Name = "", UserId = 1 };
        }

        public async Task<IActionResult> OnPostCreateFolder()
        {
            var root = await _fileManagerService.GetRootFolderContent();
            if (root != null) 
            {
                RootFolder = root;
                var folder = new Folder { Name = NewFolder.Name, UserId = 1, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow };
                folder.ParentId = ParentFolderId;
                await _fileManagerService.AddFolder(folder);

                await InitRootFolder();
                await SetupFoldersList();
                NewFolder.Name = string.Empty;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostRenameFolder()
        {
            var folder = await _fileManagerService.GetFolderById(SelectedFolderId);
            if (folder != null) 
            {
                folder.Name = NewFolder.Name;
                folder.LastModifiedDate = DateTime.UtcNow;
                await _fileManagerService.UpdateFolder(folder);

                await InitRootFolder();
                await SetupFoldersList();

                // Clear the input and form state - 
                ModelState.Clear();
                NewFolder.Name = string.Empty;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostMoveFolder()
        {
            var folder = await _fileManagerService.GetFolderById(SelectedFolderId);
            if (folder != null)
            {
                folder.ParentId = ParentFolderId;
                folder.LastModifiedDate = DateTime.UtcNow;
                await _fileManagerService.UpdateFolder(folder);

                await InitRootFolder();
                await SetupFoldersList();

                // Clear the input and form state - 
                ModelState.Clear();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFolder()
        {
            var folder = await _fileManagerService.GetFolderById(SelectedFolderId);
            if (folder != null)
            {
                await _fileManagerService.RemoveFolder(folder);

                await InitRootFolder();
                await SetupFoldersList();

                // Clear the input and form state - 
                ModelState.Clear();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostUploadFile()
        {
            var root = await _fileManagerService.GetRootFolderContent();
            if (root != null)
            {
                RootFolder = root;
                var file = new FileItem { Name = NewFile.Name, ParentId = 1, UserId = 1, CreatedDate = DateTime.UtcNow, LastModifiedDate = DateTime.UtcNow };
                await _fileManagerService.AddFile(file);
                await InitRootFolder();
                
                NewFile = new FileItem { Name = "", UserId = 1 };
            }
            return Page();
        }
        private async Task InitRootFolder()
        {
            //var root = await _fileManagerService.GetRootFolderContent();
            //var root = await _fileManagerService.GetAllContent();
            //if (root != null)
            //{
            //    AllFolders = root;
            //    RootFolder = root.First();
            //}
            var rootFolders = await _fileManagerService.GetRootFolders();
            if (rootFolders != null)
            {
                RootFolders = rootFolders;
            }
        }

        private async Task SetupFoldersList()
        {
            var folders = await _fileManagerService.GetFolders();
            Folders = new SelectList(folders, "FolderId", "Name");
        }
    }
}
