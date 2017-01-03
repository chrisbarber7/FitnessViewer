

    var initReports = function () {
     
    };

    var updateWeeklyReports = function () {
        setupWeeklyReport("chart12weekBike", "Ride", "#DC758F", "Total");
        setupWeeklyReport("chartBikeLongestRide", "Ride", "#DC758F", "Max");
        setupPowerPeakChart();
        setupTrainingLoadChart("Ride", 1);
        SetupPowerCurveChart("chartPowerCurve");
    };


    var setupPowerPeakChart = function () {
        $.ajax({
            dataType: "json",
            url: "/api/Athlete/GetPeaksByMonth/Ride?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
            success: function (data) {
                PowerPeakChart(data);
            },
            error: function () {
                alert("Error loading power peak data!");
            }
        });

        function PowerPeakChart(data) {
            var powerPeakChartData = {
                labels: data.Period,
       
                datasets: [
                    {
                        label: '1 Min',
                        data: data.Peak60,
                        borderWidth: 0.1,
                        fill: false,
                        borderColor: '#06AED5',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth:1
                    },
                    {
                        label: '5 Min',
                        data: data.Peak300,
                        lineThickness: 0.1,
                        fill: false,
                        borderColor: '#0057A1',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth: 1
                    },
                    {
                        label: '20 Min',
                        data: data.Peak1200,
                        lineThickness: 0.1,
                        fill: false,
                        borderColor: '#F0C808',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth: 1
                    },
                    
                    {
                        label: 'Hour',
                        data: data.Peak3600,
                        lineThickness: 0,
                        fill: false,
                        borderColor: '#DD1C1A',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth: 1
                    }
                ]
            };

            var powerPeakChartContext = document.getElementById("chartPowerPeak").getContext("2d");

            var powerPeakChart = Chart.Line(powerPeakChartContext, {
                data: powerPeakChartData,
                options: {
                    animation: false
                }
            });
        }
    };






    function SetupPowerCurveChart(chartName) {
        $.ajax({
            dataType: "json",
            type: "GET",
            url: "/api/Athlete/GetPowerCurve?From=" + dashboardStart.utc().format("X") + "&To=" + dashboardEnd.utc().format("X"),
            data: JSON,
            success: function (data) {
                PowerCurveChart(data);
            },
            error: function () {
                alert("Error loading power curve data!");
            }
        });

        function PowerCurveChart(data) {
            var powerCurveChartData = {
                labels: data.duration,
                datasets: [
                    {
                        label: 'Power',
                        data: data.watts,
                        radius: 0,
                        fill: false,
                        borderColor: 'blue',
                        lineTension: 0,
                        pointRadius: 0,
                        borderWidth: 1
                    }
                ]
            };

            var powerCurveContext = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Line(powerCurveContext, {
                data: powerCurveChartData,
                options: {
                    animation: false,
                    //fill: false,
                    //beizierCurve: false,
                    responsive: true,
                    datasetFill: true
                    //,
                    //scales: {
                    //    xAxes: [{
                    //        type: "logarithmic",
                    //        position: "bottom",
                    //        ticks: {
                    //            min:1,
                    //        max:2000
                    //        }
                    //    }]
                    //}

                }
            });
        }
    };