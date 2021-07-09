//using Fitbit.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FitnessViewer.Infrastructure.Core.Helpers.Conversions
//{
//    public static class FitbitConversion
//    {
//        public const decimal InvalidValue = -1;

//        public static decimal ConvertFitbitValue(TimeSeriesResourceType type, string value)
//        {
//            // if value is "0" we're going to ignore it so no point converting.
//            if (value == "0")
//                return FitbitConversion.InvalidValue;

//            if (type == TimeSeriesResourceType.TimeEnteredBed)
//                return ConvertFitbitTimeEnteredBed(value);

//            return Convert.ToDecimal(value);
//        }

//        private static decimal ConvertFitbitTimeEnteredBed(string value)
//        {
//            try
//            {
//                // time should be in the format hh:mm
//                if (!value.Contains(':'))
//                    return FitbitConversion.InvalidValue;


//                var time = value.Split(':');
//                int hours = Convert.ToInt32(time[0]);
//                int minutes = Convert.ToInt32(time[1]);

//                if (hours < 0 || hours > 23)
//                    return FitbitConversion.InvalidValue;

//                if ((minutes < 0) || (minutes > 59))
//                    return FitbitConversion.InvalidValue;

//                return Convert.ToDecimal((hours * 60) + minutes);
//            }
//            catch (Exception)
//            {
//                // ignore value;
//                return FitbitConversion.InvalidValue;
//            }
//        }
//    }
//}
