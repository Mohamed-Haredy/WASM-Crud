using System.ComponentModel.DataAnnotations;

namespace WASM.Shared
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "This is too long name!!!")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "This is too long name!!!")]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        [EmailAddress (ErrorMessage="This must be an email")]
        public string? Email { get; set; }
        [Phone(ErrorMessage = "This must be an Phone")]
        public string? PhoneNumber { get; set; }
        public int CountryId { get; set; }
        public virtual Country ?Country { get; set; }
        [StringLength(100, ErrorMessage = "This is too long name max 100 !!!")]
        public string? Comment { get; set; }
        public DateTime? JoinedDate { get; set; }
        public DateTime? ExitDate { get; set; }
    }
}
