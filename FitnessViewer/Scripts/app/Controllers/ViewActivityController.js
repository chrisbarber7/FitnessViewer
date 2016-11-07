$(document).ready(function () {
    var selectedPolyline;
    var fullRouteLatLng;
    // user clicks on any of the options in the lap info panel.  We'll load the summary info for that section and highlight the section on the map.
    $('ul.laps li').click(function (e) {
        $("li.selectedLap").removeClass("selectedLap");
        $(this).addClass("selectedLap");
        var startIndex = $(this).attr("data-start-index");
        var endIndex = $(this).attr("data-end-index");
        var streamStep = $(this).attr("data-stream-step");
        var activityId = document.getElementById('activityId').value;
        var selectedText = encodeURIComponent($(this).find(".lapName").text());
        $("#activitySummaryInformation").load("/Activity/GetSummaryInformation?activityId=" + activityId + "&selection=" + selectedText + "&startIndex=" + startIndex*streamStep + "&endIndex=" + endIndex*streamStep);

        // if previous selection exists then removeit
        if (selectedPolyline!==undefined)
            mymap.removeLayer(selectedPolyline);

        // strip the full route coordinated just keeping the section we are interested in (no need for stream step on the map as the map has reduced data points)
        var selectedLatLng = fullRouteLatLng.slice(startIndex, endIndex);

        selectedPolyline = L.polyline(selectedLatLng, { color : 'blue' }).addTo(mymap);

        // un-comment to zoom in on selected polyline.  Undecided if I like it or not as hard to see where on route you are!
        //mymap.fitBounds(selectedPolyline.getBounds());
    });

    var mymap = L.map('mapid');
    var activityId = document.getElementById('activityId').value;
    var hasMap = document.getElementById('hasMap').value;

    if (hasMap === "True") {
        fullRouteLatLng = getCoords(activityId);
        var polyline = L.polyline(fullRouteLatLng, { color: 'red' }).addTo(mymap);
        mymap.fitBounds(polyline.getBounds());

        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token=sk.eyJ1IjoiY2hyaXNiYXJiZXI3IiwiYSI6ImNpdHlxcDdnOTAwNGUzbm9hMDNueDBla2IifQ.uCc724sMqgSk316I0XuPlA', {
            maxZoom: 18,
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, ' +
            '<a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, ' +
            'Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets'
        }).addTo(mymap);
    }
    function getCoords(activityId) {
        var fullRouteLatLng = null;
        $.ajax({
            async: false,
            global: false,
            dataType: "json",
            type: "POST",
            url: "/api/Activity/GetActivityCoords/" + activityId,
            success: function (data) {
                fullRouteLatLng = data;
            },
            error: function () {
                alert("Error getting coords data!");
            }
        });
        return fullRouteLatLng;
    }


 SetupActivityChart("chartActivity");

        function SetupActivityChart(chartName) {
        $.ajax({
            dataType: "json",
            type: "POST",
            url: "/api/Activity/GetActivityStreams/" + activityId,
            data: JSON,
            success: function (data) {
                BarChart(data);
            },
            error: function () {
                alert("Error loading activity data!");
            }
        });

        function BarChart(data) {
            var barChartData = {
                labels: data.Time,
                datasets: [
                    {
                        label: 'Power',
                        data: data.Watts,
                        radius: 0,            
                    fill: false,      
                    borderColor: 'blue',
                        yAxesID : 'y-axis-0'
                    },
                    {
                        label: 'Heart Rate',
                        data: data.HeartRate,
                        radius: 0,
                        fill: false,
                        borderColor: 'red',
                        yAxesID: 'y-axis-0',
                        lineThickness: 0.1
                    },
                    {
                        label: 'Elevation',
                        data: data.Altitude,
                        radius: 0,
                        fill: false,
                        borderColor: 'green',
                        yAxesID: 'y-axis-1'
                    }
                ]
            };

            Chart.defaults.scale.ticks.autoSkipPadding = 100;

            var ctx = document.getElementById(chartName).getContext("2d");

            var myBarChart = Chart.Line(ctx, {
                data: barChartData,
                options: {
                    animation: false,
                    fill: false,
                    beizierCurve: false,
                    responsive: true,
                    datasetFill: true,
                    scales: {
                        yAxes: [{
                            position: "left",
                            "id": "y-axis-0"
                        }, {
                            position: "right",
                            "id": "y-axis-1"
                        }]
                    }
                }
            });
        }
    }
});
