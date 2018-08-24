using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WebApiCore.Infrastructure.Data;

namespace WebApiCore.Common
{
    public class MyDbContext: DapperDbContext
    {
        public MyDbContext(IOptions<DapperDbContextOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        protected override IDbConnection CreateConnection(string connectionString)
        {
            IDbConnection conn=new SqlConnection(connectionString);
            return conn;
        }
    }
}
