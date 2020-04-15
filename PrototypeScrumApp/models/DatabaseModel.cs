using System;
using System.Data.Entity;
using System.Windows.Forms;

namespace PrototypeScrumApp.models
{
    public class DatabaseModel : DbContext
    {
        /// <summary>
        /// Function that creates a database if it does not exist
        /// </summary>
        public DatabaseModel() : base(ConnectionString())
        {
            Database.SetInitializer<DatabaseModel>(new CreateDatabaseIfNotExists<DatabaseModel>());
            try
            {
                bool status = Database.CreateIfNotExists();
                if (status != true) return;
                Properties.Settings.Default.db_status = "OK";
                Properties.Settings.Default.Save();
                MessageBox.Show("Database was installed successfully.");
            }
            catch (Exception e)
            {
                Properties.Settings.Default.db_status = "NOK";
                Properties.Settings.Default.Save();
                MessageBox.Show("Unable to install the database, it will only be possible to use the default login.");
                Console.WriteLine(e);
            }
        }

        /// <summary>
        ///  Set for table users
        /// </summary>
        public DbSet<Users> users { get; set; }
        
        /// <summary>
        /// Set the table for udes
        /// </summary>
        public DbSet<IDE> ide { get; set; }

        /// <summary>
        /// Layout for table users
        /// </summary>
        public class Users
        {
            public int Id { get; set; }
            public string email { get; set; }
            public string api_token { get; set; }
        }
        
        /// <summary>
        /// Layout for the table ide
        /// </summary>
        public class IDE
        {
            public int Id { get; set; }
            public int projectId { get; set; }
            public string filePath { get; set; }
            public string ideExecutable { get; set; }
        }

        /// <summary>
        /// Connect string
        /// </summary>
        /// <returns>String connection string</returns>
        private static string ConnectionString()
        {
            return Properties.Settings.Default.connectionString;
        }
    }
}
