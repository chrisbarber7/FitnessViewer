var DashboardController = function () {
    var init = function () {
        setupActivitiesDataTable();
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
                    animation: false
                }
            });
        }
    };

    var setupActivitiesDataTable = function () {
        $('#table_id').dataTable({
            "ajax": "/api/activity/getactivities",
            "autoWidth": false,
            "deferRender": true,
            "order": [[1, "desc"]],
            "columns": [{
                "data": function (data, type, row, meta) {
                    return '<a href="/activity/viewactivity/' + data.id + '">' + data.name + '</a>';
                }
            },
                {
                    "data": {
                        _: "date",
                        sort: "startDateLocal"
                    }
                },
                { "data": "activityTypeId" },
                { "data": "distance" },
                { "data": "elevationGain" },
                { "data": "movingTime" }
            ],
            "columnDefs": [
                { "className": "dt-right", "targets": [3, 4] },
            { "width": "60%", "targets": 0 }]
        });
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
                        label: '7 Day Ave',
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