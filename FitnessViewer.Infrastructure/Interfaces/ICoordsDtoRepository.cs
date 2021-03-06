﻿using System.Collections.Generic;
using FitnessViewer.Infrastructure.Models.Dto;

namespace FitnessViewer.Infrastructure.Interfaces
{
    public interface ICoordsDtoRepository
    {
        IEnumerable<CoordsDto> GetActivityCoords(long activityId);
    }
}