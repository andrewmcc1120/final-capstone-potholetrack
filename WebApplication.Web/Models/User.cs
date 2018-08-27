using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.Models
{
    public class User
    {
        /// <summary>
        /// The user's id.
        /// </summary>
        [Required]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The user's username.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

	    /// <summary>
	    /// The user's first name.
	    /// </summary>
	    [Required]
	    [MaxLength(50)]
	    public string FirstName { get; set; }

	    /// <summary>
	    /// The user's last name.
	    /// </summary>
	    [Required]
	    [MaxLength(50)]
	    public string LastName { get; set; }


		/// <summary>
		/// The user's password.
		/// </summary>
		[Required]
        public string Password { get; set; }

        /// <summary>
        /// The user's salt.
        /// </summary>
        [Required]
        public string Salt { get; set; }

		/// <summary>
		/// The user's role.
		/// </summary>
		[Required]
		public string Role { get; set; }

	    /// <summary>
	    /// The user's phone number.
	    /// </summary>
	    [Required]
	    public string PhoneNumber { get; set; }

	    /// <summary>
	    /// The user's phone number.
	    /// </summary>
	    [Required]
	    public List<int> ReportIds { get; set; }
	}
}
