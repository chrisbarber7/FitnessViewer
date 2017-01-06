
    var initReports = function () {
      
    };

    var updateWeeklyReports = function () {
        setupWeightChart('Weight', 'chartDetailedWeight');
        setupDetailedMetricsChart('HeartRate', 'chartDetailedHRV');
    };

    var setupDetailedMetricsChart = function ( metricType, chartName) {
        $.ajax({
            dataType: "json",
            url: "/api/Metric/GetMetrics/" + metricType + "/?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
     
            success: function (data) {
                DetailedChart(data);
            },
            error: function () {
                alert("Error loading weight data!");
            }
        });

        function DetailedChart(data) {
            var barChartData = {
                labels: data.Date,
                datasets: [
                    {
                        label: metricType,
                        data: data.MetricValue,
                        lineThickness: 0.1,
                        fill: false,
                        borderColor: '#3a8904'
                    },
                    {
                        label: 'Rolling 7 Day Average',
                        data: data.Ave7Day,
                        radius: 0,
                        fill: false,
                        borderColor: '#c0ef95'
                    }
                ]
            };

            var ctx = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Line(ctx, {
                data: barChartData,
                options: {
                    animation: false,
                    tooltips: {
                        mode: 'index',
                        intersect: false
                    }
                }
            });
        }
    };
