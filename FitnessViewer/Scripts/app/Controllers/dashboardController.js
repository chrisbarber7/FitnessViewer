var DashboardController = function () {
    var init = function () {
        setupActivitiesDataTable();
        setupWeeklyReport("chart12weekRun", "Run");
        setupWeeklyReport("chart12weekBike", "Ride");
    };

    var setupWeeklyReport = function (chartName, api) {
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/api/Activity/GetRunDistancePerWeek/" + api,
            data: JSON,
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
                        data: data.distance
                    }]
            };

            var ctx = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Bar(ctx, {
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
                { "data": "elapsedTime" }
            ],
            "columnDefs": [
                { "className": "dt-right", "targets": [3, 4] },
            { "width": "60%", "targets": 0 }]
        });
    };

    return {
        init: init
    };
}();