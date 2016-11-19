using FitnessViewer.Infrastructure.Data;
using FitnessViewer.Infrastructure.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using EntityFramework.BulkInsert.Extensions;

namespace FitnessViewer.Infrastructure.Repository
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
                _context.Database.ExecuteSqlCommand("dbo.ActivityStreamDelete @activityId", new SqlParameter("activityid", activityId));
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
            _context.BulkInsert(s);
        }

    }
}
