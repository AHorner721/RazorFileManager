using System.ComponentModel.DataAnnotations;

namespace RazorFileManager.Data.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
