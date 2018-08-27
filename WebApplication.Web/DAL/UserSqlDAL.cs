using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
	public class UserSqlDAL : IUserDAL
    {
        private readonly string connectionString;

        public UserSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Saves the user to the database.
        /// </summary>
        /// <param name="user"></param>
        public void CreateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO users VALUES (@username, @password, @firstname, @lastname, @salt, @phonenumber, @role, @email);", conn);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@firstname", user.FirstName);
                    cmd.Parameters.AddWithValue("@lastname", user.LastName);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
	                cmd.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    cmd.ExecuteNonQuery();

                }
            }
            catch(SqlException ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// Deletes the user from the database.
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE id = @id;", conn);
                    cmd.Parameters.AddWithValue("@id", user.Id);                    

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
        /// Gets the user from the database.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public User GetUser(string username)
        {
            User user = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE username = @username;", conn);
                    cmd.Parameters.AddWithValue("@username", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        user = MapRowToUser(reader);
                    }
                }

                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }            
        }


		/// <summary>
		/// Gets the user's record id's from the database.
		/// </summary>
		/// <param name="userid"></param>
		/// <returns></returns>
		public List<int> GetUserList(int userid)
	    {
			List<int> userReports = new List<int>();
		    try
		    {
			    using (SqlConnection conn = new SqlConnection(connectionString))
			    {
				    conn.Open();
				    SqlCommand cmd = new SqlCommand("SELECT record_id FROM user_records WHERE user_id = @userid;", conn);
				    cmd.Parameters.AddWithValue("@userid", userid);

				    SqlDataReader reader = cmd.ExecuteReader();

				    while (reader.Read())
				    {
					    userReports.Add(Convert.ToInt32(reader["record_id"]));
				    }
			    }

			    return userReports;
		    }
		    catch (SqlException ex)
		    {
			    throw ex;
		    }
	    }

		/// <summary>
		/// Updates the user in the database.
		/// </summary>
		/// <param name="user"></param>
		public void UpdateUser(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE users SET password = @password, salt = @salt, role = @role, phonenumber = @phonenumber, email = @email WHERE id = @id;", conn);                    
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@salt", user.Salt);
                    cmd.Parameters.AddWithValue("@role", user.Role);
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);

                    cmd.ExecuteNonQuery();

                }
                return;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        private User MapRowToUser(SqlDataReader reader)
        {
            return new User()
            {
                Id = Convert.ToInt32(reader["id"]),
                Username = Convert.ToString(reader["username"]),
                FirstName = Convert.ToString(reader["firstname"]),
                LastName = Convert.ToString(reader["lastname"]),
                Password = Convert.ToString(reader["password"]),
                Salt = Convert.ToString(reader["salt"]),
                Role = Convert.ToString(reader["role"]),
				PhoneNumber = Convert.ToString(reader["phonenumber"]),
                Email= Convert.ToString(reader["email"])
            };
        }
    }
}
