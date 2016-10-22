var ActivityTableController = function () {
    var init = function () {
        setupActivitiesDataTable();   
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




    return {
        init: init
    };
}();