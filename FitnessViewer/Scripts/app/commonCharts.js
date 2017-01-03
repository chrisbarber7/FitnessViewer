// used to detroy existing charts before refreshing or else when hovering the previous chart maybe shown.
var runDistanceChart;
var rideDistanceChart;
var swimDistanceChart;
var runLongestChart;
var rideLongestChart;
var swimLongestChart;
var trainingLoadChart;
var weightChart;
var timeBySportChart;

var setupWeeklyReport = function (chartName, sport, colour, type) {
    $.ajax({
        dataType: "json",
        url: "/api/Athlete/GetPeriodDistance/" + sport + "/"+type+"?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
        success: function (data) {
            BarChart(data);
        },
        error: function () {
            alert("Error loading weekly report data!");
        }
    });

    function BarChart(data) {
        var sportDistanceChartData = {
            labels: data.Period,
            datasets: [
                {
                    label: 'Distance',
                    data: data.distance,
                    borderColor: colour,
                    backgroundColor: colour,
                    fill: false
                }]
        };

        var distanceChartContext = document.getElementById(chartName).getContext("2d");

        var chart = Chart.Bar(distanceChartContext, {
            data: sportDistanceChartData,
            options: {
                legend: {
                    display: false
                },


                scales:
                     {
                         xAxes: [{
                             display: false
                         }]
                     }
            }
        });
        if (sport === "Ride") {

            if (type === "Total") {
                if (rideDistanceChart !== undefined) {
                    rideDistanceChart.destroy();
                }
                chart.options.onClick = rideDistanceHandleClick;
                rideDistanceChart = chart;
            };

            if (type == "Max") {
                if (rideLongestChart !== undefined) {
                    rideLongestChart.destroy();
                }
                chart.options.onClick = rideLongestHandleClick;
                rideLongestChart = chart;
            };
        }

        if (sport === "Run") {

            if (type === "Total") {
                if (runDistanceChart !== undefined) {
                    runDistanceChart.destroy();
                }
                chart.options.onClick = runDistanceHandleClick;
                runDistanceChart = chart;
            };

            if (type == "Max") {
                if (runLongestChart !== undefined) {
                    runLongestChart.destroy();
                }
                chart.options.onClick = runLongestHandleClick;
                runLongestChart = chart;
            };
        }
        if (sport === "Swim") {

            if (type === "Total") {
                if (swimDistanceChart !== undefined) {
                    swimDistanceChart.destroy();
                }
                chart.options.onClick = swimDistanceHandleClick;
                swimDistanceChart = chart;
            };

            if (type == "Max") {
                if (swimLongestChart !== undefined) {
                    swimLongestChart.destroy();
                }
                chart.options.onClick = swimLongestHandleClick;
                swimLongestChart = chart;
            };
        }

        function runDistanceHandleClick(evt) {
            var activeElement = runDistanceChart.getElementAtEvent(evt);
        }

        function runHandleClick(evt) {
            var activeElement = runDistanceChart.getElementAtEvent(evt);
        }
        function runLongestHandleClick(evt) {
            var activeElement = runLongestChart.getElementAtEvent(evt);
        }
        function rideDistanceHandleClick(evt) {
            var activeElement = rideDistanceChart.getElementAtEvent(evt);
        }

        function rideLongestHandleClick(evt) {
            var activeElement = rideLongestChart.getElementAtEvent(evt);
        }

        function swimDistanceHandleClick(evt) {
            var activeElement = swimDistanceChart.getElementAtEvent(evt);
        }

        function swimLongestHandleClick(evt) {
            var activeElement = swimLongestChart.getElementAtEvent(evt);
        }
    }
};

var setupWeightChart = function() {
    $.ajax({
        dataType: "json",
        url: "/api/Metric/GetMetrics/Weight?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
        success: function (data) {
            WeightChart(data);
        },
        error: function () {
            alert("Error loading weight data!");
        }
    });

    function WeightChart(data) {
        var weightChartData = {
            labels: data.Date,
            datasets: [
                {
                    label: 'Weight',
                    data: data.MetricValue,
                    fill: false,
                    borderColor: '#545677',
                    lineTension: 0,
                    pointRadius: 0,
                    borderWidth: 1
                },
                {
                    label: 'Rolling 7 Day Average',
                    data: data.Ave7Day,
                    radius: 0,
                    fill: false,
                    borderColor: '#B1B2C1',
                    lineTension: 0,
                    pointRadius: 0,
                    borderWidth: 1
                }
            ]
        };

        var weightChartContext = document.getElementById("chartWeight").getContext("2d");

        // if previous chart exists then destroy before refreshing or else the previous chart maybe shown when hovering over chart.
        if (weightChart !== undefined) {
            weightChart.destroy();
        }

        var chartW = Chart.Line(weightChartContext, {
            data: weightChartData,
            options: {
                animation: false
            }
        });

        weightChart = chartW;
    }
};

var setupTimeBySportChart = function (chartName) {
    $.ajax({
        dataType: "json",
        url: "/api/Athlete/GetTimeAndDistanceBySport?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
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

        var timeBySportContext = document.getElementById(chartName).getContext("2d");

        if (timeBySportChart !== undefined) {
            timeBySportChart.destroy();
        }

        var timeBySportPieChart = new Chart(timeBySportContext, {
            type: 'pie',
            data: pieChartData,
            options: options
        });

        timeBySportChart = timeBySportPieChart;
    }
};


var setupTrainingLoadChart = function (sport, labels) {
    $.ajax({
        dataType: "json",
        url: "/api/Athlete/GetTrainingLoad/" + sport + "?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
        success: function (data) {
            TrainingLoadChart(data, labels);
        },
        error: function () {
            alert("Error loading training load data!");
        }
    });


    function TrainingLoadChart(data, labels) {
        var trainingLoadChartData = {
            labels: data.Date,
            datasets: [
                {
                    type: 'line',
                    label: 'LTL',
                    data: data.LongTermLoad,
                    fill: false,
                    radius: 0,
                    borderColor: '#545677',
                    lineTension: 0,
                    pointRadius: 0,
                    borderWidth: 1,
                    yAxisID: "y-axis-0"
                },
                {
                    type: 'line',
                    label: 'STL',
                    data: data.ShortTermLoad,
                    radius: 0,
                    fill: false,
                    borderColor: '#B1B2C1',
                    lineTension: 0,
                    pointRadius: 0,
                    borderWidth: 1,
                    yAxisID: "y-axis-0"
                },
                {
                    type: 'bar',
                    label: 'TSS',
                    data: data.TSS,
                    radius: 0,
                    fill: true,
                    borderColor: '#F0C808',
                    yAxisID: "y-axis-1"
                }
            ]
        };

        var trainingLoadChartContext = document.getElementById("bikeTrainingLoadChart").getContext("2d");


        var trainingLoadOptions = {
            animation: false,
            legend: {
                display: labels === 0 ? false : true
            },
            scales:
                 {
                     xAxes: [{
                         display: labels === 0 ? false : true,


                         gridLines: {
                             display: false
                         }


                     }],
                     yAxes: [{
                         position: "left",
                         "id": "y-axis-0",
                         gridLines: {
                             display: false
                         }
                     }, {
                         position: "right",
                         "id": "y-axis-1",

                         gridLines: {
                             display: false
                         }
                     }
                     ]
                 }
        };

// if previous chart exists then destroy before refreshing or else the previous chart maybe shown when hovering over chart.
        if (trainingLoadChart !== undefined) {
            trainingLoadChart.destroy();
        }
       
            var chartTL = Chart.Bar(trainingLoadChartContext, {
                data: trainingLoadChartData,
                options: trainingLoadOptions
            });

            trainingLoadChart = chartTL;
        }
    };
