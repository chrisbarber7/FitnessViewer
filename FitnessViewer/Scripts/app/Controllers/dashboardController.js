var DashboardController = function () {
    var init = function () {

        setupWeeklyReport("chart12weekRun", "Run");
        setupWeeklyReport("chart12weekBike", "Ride");
        setupWeightChart();
    };

    var setupWeeklyReport = function (chartName, api) {
        $.ajax({
            dataType: "json",
            url: "/api/Activity/GetRunDistancePerWeek/" + api,
            success: function (data) {
                BarChart(data);
            },
            error: function () {
                alert("Error loading 12 week data!");
            }
        });

        function BarChart(data) {
            var barChartData = {
                labels: data.Period,
                datasets: [
                    {
                        label: 'Distance',
                        data: data.distance,
                        borderColor: '#3a8904',
                        fill: false
                    }]
            };

            var ctx = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Line(ctx, {
                data: barChartData,
                options: {
                    animation: false,
                    onClick: handleClick
                }



            });

            function handleClick(evt) {
                var activeElement = myBarChart.getElementAtEvent(evt);


 
            };


        };
    
      
    
    };

    var setupWeightChart = function (chartName, api) {
        $.ajax({
            dataType: "json",
            url: "/api/Metric/Get30dayweight/",
            success: function (data) {
                WeightChart(data);
            },
            error: function () {
                alert("Error loading weight data!");
            }
        });

        function WeightChart(data) {
            var barChartData = {
                labels: data.Date,
                datasets: [
                    {
                        label: 'Weight',
                        data: data.Weight,
                        lineThickness:0.1,
                        fill: false,                       
                        borderColor: '#3a8904'
                    },
                    {
                        label: 'Rolling 7 Day Average',
                        data: data.Ave7Day,
                        radius:0,
                        fill: false,
                        borderColor: '#c0ef95'
                    },
                ]
            };

            var ctx = document.getElementById("chartWeight").getContext("2d");

            var myBarChart = Chart.Line(ctx, {
                data: barChartData,
                options: {
                    animation: false
                }
            });
        }
    };



    return {
        init: init
    };
}();