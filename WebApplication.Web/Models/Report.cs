using System;
using System.ComponentModel.DataAnnotations;


namespace WebApplication.Web.Models.Account
{
    public class Report
    {

	    /// <summary>
	    /// The report's id.
	    /// </summary>
	    [Required]
	    public int Id { get; set; }

	    /// <summary>
	    /// The report's submitter.
	    /// </summary>
	    public int Submitter { get; set; }

	    /// <summary>
	    /// The report's creation date.
	    /// </summary>
	    //[Required]
	    public DateTime DateCreated { get; set; }

		/// <summary>
	    /// The pothole latitude.
	    /// </summary>
	    //[Required]
	    public decimal Lattitude { get; set; }

	    /// <summary>
	    /// The pothole longitude.
	    /// </summary>
	    //[Required]
	    public decimal Longitude { get; set; }

		/// <summary>
		/// The date inspected by city employee.
		/// </summary>
		public DateTime DateInspected { get; set; }

	    /// <summary>
	    /// The pothole severity.
	    /// </summary>
	    public int Severity { get; set; }

		/// <summary>
		/// The date repaired by city employee.
		/// </summary>
		public DateTime DateRepaired { get; set; }
		
	    /// <summary>
	    /// The pothole status.
	    /// </summary>
	    public int Status { get; set; }

	    /// <summary>
	    /// How many times this pothole has been reported.
	    /// </summary>
	    //[Required]
	    public int ReportCount { get; set; }

	    /// <summary>
	    /// The pothole description (if any).
	    /// </summary>
	    public string Description { get; set; }

	    /// <summary>
	    /// The new and improved report number.
	    /// </summary>
	    public string ReportNumber { get; set; }

	    /// <summary>
	    /// The new and improved report number.
	    /// </summary>
	    public int AssignedEmployee { get; set; }
	}


}

