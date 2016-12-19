using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace airplaneCA
{
    interface IConnection
    {
        void SetConnection(string connStr);
        bool Open();
        bool Close();
    }
}
