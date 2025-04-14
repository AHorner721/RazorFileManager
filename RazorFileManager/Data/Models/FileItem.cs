using System.ComponentModel.DataAnnotations.Schema;

namespace RazorFileManager.Data.Models
{
    public class FileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public Folder ParentFolder { get; set; }
        public int UserId { get; set; }
    }
}
