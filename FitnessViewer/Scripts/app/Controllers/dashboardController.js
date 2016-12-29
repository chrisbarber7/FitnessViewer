    var runDistanceChart;
    var rideDistanceChart;
    var swimDistanceChart;

    var initReports = function () {
        $("#runSummaryInformation").load("/Athlete/GetSportSummary?sport=run&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#rideSummaryInformation").load("/Athlete/GetSportSummary?sport=Ride&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#swimSummaryInformation").load("/Athlete/GetSportSummary?sport=Swim&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#otherSummaryInformation").load("/Athlete/GetSportSummary?sport=Other&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));
        $("#allSummaryInformation").load("/Athlete/GetSportSummary?sport=All&From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"));

    };

    var updateWeeklyReports = function () {
        setupWeeklyReport("chart12weekRun", "Run", "#873D48");
        setupWeeklyReport("chart12weekBike", "Ride", "#DC758F");
        setupWeeklyReport("chart12weekSwim", "Swim", "#955E42");
        setupWeightChart();
        setupTimeBySportChart("chartTimeBySport");
        setupTrainingLoadChart();
    };

    var setupWeeklyReport = function (chartName, api, colour) {
        $.ajax({
            dataType: "json",
            url: "/api/Athlete/GetPeriodDistance/" + api + "?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
            success: function (data) {
                BarChart(data);
            },
            error: function () {
                alert("Error loading 12 week data!");
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
                        backgroundColor :colour, 
                        fill: false
                    }]
            };

            var distanceChartContext = document.getElementById(chartName).getContext("2d");

            // setting up each chart seperatly as need to destroy the prvious report
            // before updating when date range changes
            if (api === "Run") {
                if (runDistanceChart !== undefined) {
                    runDistanceChart.destroy();
                }

                runDistanceChart = Chart.Bar(distanceChartContext, {
                    data: sportDistanceChartData,
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

                swimDistanceChart = Chart.Bar(distanceChartContext, {
                    data: sportDistanceChartData,
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

                rideDistanceChart = Chart.Bar(distanceChartContext, {
                    data: sportDistanceChartData,
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

            var weightChartContext = document.getElementById("chartWeight").getContext("2d");

            var weightChart = Chart.Line(weightChartContext, {
                data: weightChartData,
                options: {
                    animation: false
                }
            });
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

            var timeBySportContext = document.getElementById(chartName).getContext("2d");

            var timeBySportPieChart = new Chart(timeBySportContext, {
                type: 'pie',
                data: pieChartData,
                options: options
            });
        }
    };


    var setupTrainingLoadChart = function (chartName, api) {
        $.ajax({
            dataType: "json",
            url: "/api/Athlete/GetTrainingLoad/Ride?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
            success: function (data) {
                TrainingLoadChart(data);
            },
            error: function () {
                alert("Error loading training load data!");
            }
        });


        function TrainingLoadChart(data) {
            var trainingLoadChartData = {
                labels: data.Date,
                datasets: [
                    {
                        label: 'LTL',
                        data: data.LongTermLoad,
                        lineThickness: 0.1,
                        fill: false,
                        radius: 0,
                        borderColor: '#545677'
                    },
                    {
                        label: 'STL',
                        data: data.ShortTermLoad,
                        radius: 0,
                        fill: false,
                        borderColor: '#B1B2C1'
                    }
                ]
            };

            var trainingLoadChartContext = document.getElementById("bikeTrainingLoadChart").getContext("2d");

            var trainloadChart = Chart.Line(trainingLoadChartContext, {
                data: trainingLoadChartData,
                options: {
                    animation: false
                }
            });
        };
    }
        
