using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using WebApplication.Web.Models.Account;

namespace WebApplication.Web.DAL
{
	public class PotholeReportDAL : IPotholeDAL
	{

		private readonly string ConnectionString;

		public PotholeReportDAL(string connectionString)
		{
			this.ConnectionString = connectionString;
		}

        /// <summary>
        /// Saves the new report to the database.
        /// </summary>
        /// <param name="report"></param>
        /// <returns>The ID of the report after it has been saved in the database.</returns>
		public int CreateReport(Report report)
		{
            int newestId;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					SqlCommand cmd =
						new SqlCommand(
                            $"INSERT INTO records(submitter, datecreated, severity, lattitude, longitude, status, reportnumber, assignedemployee)" +
                            $" VALUES (@submitter, @datecreated, @severity, @lattitude, @longitude, @status, @reportnumber, @assignedemployee); Select Max(id) from records;",
							conn);
					cmd.Parameters.AddWithValue("@submitter", report.Submitter);
					cmd.Parameters.AddWithValue("@datecreated", report.DateCreated);
                    cmd.Parameters.AddWithValue("@severity", report.Severity);
                    cmd.Parameters.AddWithValue("@longitude", report.Longitude);
					cmd.Parameters.AddWithValue("@lattitude", report.Lattitude);
                    cmd.Parameters.AddWithValue("@status", report.Status);
					cmd.Parameters.AddWithValue("@reportnumber", report.ReportNumber);
					cmd.Parameters.AddWithValue("@assignedemployee", report.AssignedEmployee);

                     newestId= Convert.ToInt32(cmd.ExecuteScalar());

				}
                return newestId;
            }
            catch (SqlException ex)
			{
				throw ex;
			}

		}

        public IList<Report> GetAllReports()
        {
            List<Report> reports = new List<Report>();
            string sql = "Select * From records;";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(MapRowToReport(reader));
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            return reports;
        }

        public IList<Report> GetReportsByUser(int id)
        {
            List<Report> reports = new List<Report>();
            string sql = $"Select * From records " +
                         $"Inner Join user_records on records.id = user_records.record_id " +
                         $"Where user_id = @id;";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        reports.Add(MapRowToReport(reader));
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return reports;
        }

		/// <summary>
		/// Returns the report.
		/// </summary>
		/// <param name="reportId"></param>
		public Report GetReport(int reportId)
		{
			Report report = null;
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand("SELECT * FROM records WHERE id = @reportid;", conn);
					cmd.Parameters.AddWithValue("@reportid", reportId);

					SqlDataReader reader = cmd.ExecuteReader();

					if (reader.Read())
					{
						report = MapRowToReport(reader);
					}
				}
			}

			catch (SqlException ex)
			{
				throw ex;
			}

			return report;
		}

		/// <summary>
		/// Adds a count to an existing report.
		/// </summary>
		/// <param name="report"></param>
		public void AddReport(Report report)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand(
                        $"Begin Try " +
                        $"Insert Into user_records (user_id, record_id) Values (@user, @reportid); " +
                        $"Update records Set reportcount = reportcount + 1 Where id = @reportid;" +
                        $"End Try " +
                        $"Begin Catch " +
                        $"End Catch;", conn);
					cmd.Parameters.AddWithValue("@reportid", report.Id);
					cmd.Parameters.AddWithValue("@user", report.Submitter);

					cmd.ExecuteNonQuery();
					
				}
				return;
			}

			catch (SqlException ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Updates the assigned employee.
		/// </summary>
		/// <param name="reportId"></param>
		/// <param name="employeeId"></param>
		public void AssignEmployee(int reportId, int employeeId)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					SqlCommand cmd =
						new SqlCommand(
							$"UPDATE records SET assignedemployee = @assignedemployee WHERE id = @id",
							conn);
					cmd.Parameters.AddWithValue("@id", reportId);
					cmd.Parameters.AddWithValue("@assignedemployee", employeeId);

					cmd.ExecuteNonQuery();

				}
				return;
			}
			catch (SqlException ex)
			{
				throw ex;
			}

		}

		/// <summary>
		/// Updates the report.
		/// </summary>
		/// <param name="report"></param>
		public void UpdateReport(Report report)
		{
			try
			{
				using (SqlConnection conn = new SqlConnection(ConnectionString))
				{
					conn.Open();

					SqlCommand cmd =
						new SqlCommand(
							$"UPDATE records SET dateinspected = @dateinspected," +
                            $" severity = @severity," +
                            $" repairdate = @daterepaired," +
                            $" status = @status," +
                            $" description = @description" +
                            $" WHERE id = @id;",
							conn);
                    cmd.Parameters.AddWithValue("@dateinspected", report.DateInspected);
					cmd.Parameters.AddWithValue("@severity", report.Severity);
					cmd.Parameters.AddWithValue("@daterepaired", report.DateRepaired);
					cmd.Parameters.AddWithValue("@status", report.Status);
					cmd.Parameters.AddWithValue("@description", report.Description);
                    cmd.Parameters.AddWithValue("@id", report.Id);

					cmd.ExecuteNonQuery();

				}
                return;
            }
            catch (SqlException ex)
			{
				throw ex;
			}

		}


		/// <summary>
		/// Maps the entire row a single report object.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns>A report object<returns>
		private Report MapRowToReport(SqlDataReader reader)
		{
            Report report = new Report();


            report.Id = Convert.ToInt32(reader["id"]);
            report.Submitter = Convert.ToInt32(reader["submitter"]);
            report.DateCreated = Convert.ToDateTime(reader["datecreated"]);
            report.Lattitude = Convert.ToDecimal(reader["lattitude"]);
            report.Longitude = Convert.ToDecimal(reader["longitude"]);
            if (reader["dateinspected"] is DBNull)
            {
                report.DateInspected = SqlDateTime.MinValue.Value;
            }
            else
            {
                report.DateInspected = Convert.ToDateTime(reader["dateinspected"]);

            }
            report.Severity = Convert.ToInt32(reader["severity"]);

            if (reader["repairdate"] is DBNull)
            {
                report.DateRepaired = SqlDateTime.MinValue.Value;
            }
            else
            {
                report.DateRepaired = Convert.ToDateTime(reader["repairdate"]);

            }
            report.Status = Convert.ToInt32(reader["status"]);
            report.ReportCount = Convert.ToInt32(reader["reportcount"]);
            report.Description = Convert.ToString(reader["description"]);
            report.ReportNumber = Convert.ToString(reader["reportnumber"]);
			if (reader["assignedemployee"] is DBNull)
			{
				report.AssignedEmployee = 0;
			}
			else
			{
				report.AssignedEmployee = Convert.ToInt32(reader["assignedemployee"]);

			}
			

            return report;
        }


		
	}

}
