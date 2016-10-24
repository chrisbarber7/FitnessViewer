var MetricsIndexController = function () {
    var init = function (period) {

        setupWeightDetailedChart(period);
      
   

    };

    var setupWeightDetailedChart = function (period) {

        // dates in unix format.
        var from = moment().add(period*-1, 'months').utc().format("X");
        var to = moment().utc().format("X");
        
        $.ajax({
            dataType: "json",
            url: "/api/Metric/GetWeightMetrics/?From="+from+"&To="+to,
            success: function (data) {
                WeightDetailedChart(data);
            },
            error: function () {
                alert("Error loading weight data!");
            }
        });

        function WeightDetailedChart(data) {
            var barChartData = {
                labels: data.Date,
                datasets: [
                    {
                        label: 'Weight',
                        data: data.Weight,
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

            var ctx = document.getElementById("chartDetailedWeight").getContext("2d");

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