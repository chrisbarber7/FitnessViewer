using FitnessViewer.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using FitnessViewer.Infrastructure.Models.Dto;
using FitnessViewer.Infrastructure.Interfaces;

namespace FitnessViewer.Infrastructure.Repository
{
    public class CoordsDtoRepository : DtoRepository, ICoordsDtoRepository
    {
        public CoordsDtoRepository() : base()
        { }

        public CoordsDtoRepository(ApplicationDb context) : base(context)
        { }


        public IEnumerable<CoordsDto> GetActivityCoords(long activityId)
        {
            return _context.Stream
                .Include(a => a.Activity)
                 .Where(s => s.ActivityId == activityId && s.Time % s.Activity.StreamStep == 0)
                 .OrderBy(s => s.Time)
                 .Select(s => new CoordsDto
                 {
                     lat = s.Latitude.Value,
                     lng = s.Longitude.Value
                 })

                 .ToList();
        }
    }
}
