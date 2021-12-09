using System;
using System.Data;
using System.Data.SqlClient;

namespace Btec
{
    public abstract class BaseProgram
    {
        protected static string ConnectionString = "Server=.;Database=Btec_DB;Trusted_Connection=true";

        protected static T WithConnection<T>(Func<IDbConnection, T> getData)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    connection.OpenAsync();
                    return getData(connection);
                }
            }
            catch (TimeoutException ex)
            {
                throw new Exception(String.Format("{0}.WithConnection() experienced a SQL timeout"), ex);
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
                //throw new Exception(String.Format("{0}.WithConnection() experienced a SQL exception (not a timeout)", GetType().FullName), ex);
            }
        }

        public static void WriteLog(string message)
        {
            Console.WriteLine(message);
        }

        public static void WriteLog()
        {
            Console.WriteLine();
        }

        public static void WriteLogOneLine(string message)
        {
            Console.Write(message);
        }
    }
}