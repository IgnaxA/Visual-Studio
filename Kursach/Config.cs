using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    public class Config
    {
        public static string GetConnect()
        {
            return "data source=nill_kiggers\\SQLEXPRESS;initial catalog=Information_System_to_Record_Project_Activities_Database;trusted_connection=true";
        }

    }
}
