var DashboardController = function () {
    var init = function () {
  
        $('#reportrange').daterangepicker({
            startDate: start,
            endDate: end,
            "opens": "left",
            ranges: {
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'Last 90 Days': [moment().subtract(89, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                'This Year': [moment().startOf('year'), moment()]
            }
        }, DateRangeSelected);

        DateRangeSelected(start, end);
    };

    // default to last 30 days.
    var start = moment().subtract(29, 'days');
    var end = moment();

    var runDistanceChart;
    var rideDistanceChart;
    var swimDistanceChart;

    function DateRangeSelected(s, e) {
        $('#reportrange span').html(s.format('MMMM D, YYYY') + ' - ' + e.format('MMMM D, YYYY'));
        start = s;
        end = e;
        updateWeeklyReports();
        $("#runSummaryInformation").load("/Athlete/GetSportSummary?sport=run&From=" + start.utc().format("X") + "&To=" + end.utc().format("X"));
        $("#rideSummaryInformation").load("/Athlete/GetSportSummary?sport=Ride&From=" + start.utc().format("X") + "&To=" + end.utc().format("X"));
        $("#swimSummaryInformation").load("/Athlete/GetSportSummary?sport=Swim&From=" + start.utc().format("X") + "&To=" + end.utc().format("X"));
        $("#otherSummaryInformation").load("/Athlete/GetSportSummary?sport=Other&From=" + start.utc().format("X") + "&To=" + end.utc().format("X"));
        $("#allSummaryInformation").load("/Athlete/GetSportSummary?sport=All&From=" + start.utc().format("X") + "&To=" + end.utc().format("X"));

    }

    var updateWeeklyReports = function () {
        setupWeeklyReport("chart12weekRun", "Run", "#873D48");
        setupWeeklyReport("chart12weekBike", "Ride", "#DC758F");
       setupWeeklyReport("chart12weekSwim", "Swim", "#955E42");
        setupWeightChart();
        setupTimeBySportChart("chartTimeBySport");
   
    };

    var setupWeeklyReport = function (chartName, api, colour) {
        $.ajax({
            dataType: "json",
            url: "/api/Activity/GetPeriodDistance/" + api + "?From=" + start.utc().format("X") + "&To=" + end.utc().format("X"),
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
                        borderColor: colour,
                        backgroundColor :colour, 
                        fill: false
                    }]
            };

            var ctx = document.getElementById(chartName).getContext("2d");

            // setting up each chart seperatly as need to destroy the prvious report
            // before updating when date range changes
            if (api === "Run") {
                if (runDistanceChart !== undefined) {
                    runDistanceChart.destroy();
                }

                runDistanceChart = Chart.Bar(ctx, {
                    data: barChartData,
                    options: {
                        legend: {
                            display: false
                        },
                        onClick: runHandleClick,
                        scales:
                             {
                                 xAxes: [{
                                     display: false
                                 }]
                             }
                    }
                });
            }

            if (api === "Swim") {
                if (swimDistanceChart !== undefined) {
                    swimDistanceChart.destroy();
                }

                swimDistanceChart = Chart.Bar(ctx, {
                    data: barChartData,
                    options: {
                        legend: {
                            display: false
                        },

                        onClick: swimHandleClick,
                        scales:
                             {
                                 xAxes: [{
                                     display: false
                                 }]
                             }
                    }
                });
            }

            if (api === "Ride") {
                if (rideDistanceChart !== undefined) {
                    rideDistanceChart.destroy();
                }

                rideDistanceChart = Chart.Bar(ctx, {
                    data: barChartData,
                    options: {
                        legend: {
                            display: false
                        },

                        onClick: rideHandleClick,
                        scales:
                             {
                                 xAxes: [{
                                     display: false
                                 }]
                             }
                    }
                });
            }
 
            function runHandleClick(evt) {
                var activeElement = runDistanceChart.getElementAtEvent(evt); 
            }

            function rideHandleClick(evt) {
                var activeElement = rideDistanceChart.getElementAtEvent(evt);
            }

            function swimHandleClick(evt) {
                var activeElement = swimDistanceChart.getElementAtEvent(evt);
            }
        }
    };

    var setupWeightChart = function (chartName, api) {
        $.ajax({
            dataType: "json",
            url: "/api/Metric/GetMetrics/Weight?From=" + start.utc().format("X") + "&To=" + end.utc().format("X"),
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
                        data: data.MetricValue,
                        lineThickness:0.1,
                        fill: false,                       
                        borderColor: '#545677'
                    },
                    {
                        label: 'Rolling 7 Day Average',
                        data: data.Ave7Day,
                        radius:0,
                        fill: false,
                        borderColor: '#B1B2C1'
                    }
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

    var setupTimeBySportChart = function (chartName) {
        $.ajax({
            dataType: "json",
            url: "/api/Activity/GetTimeAndDistanceBySport?From=" + start.utc().format("X") + "&To=" + end.utc().format("X"),
            success: function (data) {
                TimeBySportChart(data);
            },
            error: function () {
                alert("Error loading time/distance data!");
            }
        });

        function TimeBySportChart(dataSet) {
            var pieChartData = {              
                labels: dataSet.Sport,
                datasets: [
                    {
                        data: dataSet.Duration,
                        backgroundColor: [
                            "#553739",
                            "#873D48",
                            "#DC758F",
                            "#955E42"
                        ],
                        hoverBackgroundColor: [
                            "#FF6384",
                            "#36A2EB",
                            "#FFCE56",
                            "#3498db"
                        ]
                    }]
            };

            var options = {
                options: {
                    legend: {
                        display: true,
                        poisition: 'bottom',
                        fullWidth: false
                    }
                }
            };

            var ctx = document.getElementById(chartName).getContext("2d");

            var myPieChart = new Chart(ctx, {
                type: 'pie',
                data: pieChartData,
        options: options
            });
        }
    };
    return {
        init: init
    };
}();