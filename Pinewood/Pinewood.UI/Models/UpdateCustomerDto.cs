using System.ComponentModel.DataAnnotations;

namespace Pinewood.UI.Models
{
    public class UpdateCustomerDto
    {
        public required Guid Id { get; set; }
        [Display(Name = "First Name")]
        [StringLength(50)]
        public required string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(50)]
        public required string LastName { get; set; }
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50)]
        public required string Email { get; set; }
    }
}
