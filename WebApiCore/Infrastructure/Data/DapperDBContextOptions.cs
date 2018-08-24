using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace WebApiCore.Infrastructure.Data
{
    public class DapperDbContextOptions : IOptions<DapperDbContextOptions>
    {
        public string Configuration { get; set; }

        DapperDbContextOptions IOptions<DapperDbContextOptions>.Value => this;
    }
}
