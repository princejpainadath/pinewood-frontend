using System.ComponentModel.DataAnnotations;

namespace Pinewood.UI.Models
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }
        [Display(Name = "Email")]
        public required string Email { get; set; }
    }
}
