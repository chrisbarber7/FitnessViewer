using FitnessViewer.Infrastructure.Core.Data;
using FitnessViewer.Infrastructure.Core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
//using EntityFramework.BulkInsert.Extensions;
using Microsoft.Data.SqlClient;

using Microsoft.EntityFrameworkCore;

namespace FitnessViewer.Infrastructure.Core.Repository
{
    internal class StreamRepository : DtoRepository
    {
        internal StreamRepository() : base()
        {
        }

        internal StreamRepository(ApplicationDb context) : base(context)
        {
        }

        internal bool DeleteActivityStream(long activityId)
        {
            try
            {
                _context.Database.ExecuteSqlRaw("exec dbo.ActivityStreamDelete @activityId", new SqlParameter("activityid", activityId));
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
        
        internal void AddStreamBulk(IEnumerable<Stream> s)
        {
            throw new System.NotImplementedException();
          //  _context.BulkInsert(s);
        }

    }
}
