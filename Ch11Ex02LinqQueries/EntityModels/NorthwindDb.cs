using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage; // To use DbContext and DbSet<T>
using Microsoft.Data.SqlClient;

namespace Northwind.EntityModels
{
    public class NorthwindDb : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region To use SQLite
            string database = "Northwind.db";
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;

            // The database file will stay in the project folder
            // We will automatically adjust the relative path to account
            // for running in VS2022 or from terminal.

            if (dir.EndsWith("net8.0"))
            {
                // Running in the <project>/bin/<Debug/Release>/Net8.0 directory
                // Dang I used Linux / instead of Windows \ :P
                path = Path.Combine("..", "..", "..", database);
            }
            else
            {
                // Running in the <project> directory
                path = database;
            }

            // Below is a handy function. Being new is fun, because useful methods
            // are very novel.
            path = Path.GetFullPath(path); // Convert to absolute path
            WriteLine($"SQLite database path: {path}");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(message: $"{path} not found.", 
                    fileName: path);
            }

            // To use SQLite
            optionsBuilder.UseSqlite($"Data Source={path}");
            #endregion
        }
    }
}
