using System.Collections.Generic;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
    public interface IPotholeDAL
    {
	    /// <summary>
	    /// Retrieves a report from the system by reportId.
	    /// </summary>
	    /// <param name="reportId"></param>
	    /// <returns>The report for this pothole</returns>
	    Report GetReport(int reportId);

		/// <summary>
		/// Adds a user report count to an existing report.
		/// </summary>
		/// <param name="report"></param>
		/// <returns>The report for this pothole</returns>
		void AddReport(Report report);

        /// <summary>
        /// Creates a new report.
        /// </summary>
        /// <param name="report"></param>
        /// <returns>The report Id for this pothole report.</returns>
        int CreateReport(Report report);

		/// <summary>
		/// Updates a report.
		/// </summary>
		/// <param name="reportId"></param>
		void UpdateReport(Report reportId);

        /// <summary>
        /// Returns a list of all recorded potholes
        /// </summary>
        /// <returns></returns>
        IList<Report> GetAllReports();
        /// <summary>
        /// Returns a list of all the reports for a given user
        /// </summary>
        /// <param name="id">The User's Id</param>
        /// <returns></returns>
        IList<Report> GetReportsByUser(int id);

	    /// <summary>
	    /// Updates a report.
	    /// </summary>
	    /// <param name="reportId"></param>
	    /// <param name="employeeId"></param>
	    void AssignEmployee(int reportId, int employeeId);


    }
}
