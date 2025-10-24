using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendar.Infrastructure.Context;
internal class WorkManagerContext : DbContext
{
    public WorkManagerContext(DbContextOptions<WorkManagerContext> options) : base(options)
    {

    }
 }
