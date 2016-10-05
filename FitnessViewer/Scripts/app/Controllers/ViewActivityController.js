$(document).ready(function () {
    $('ul.laps li').click(function (e) {
        var id = $(this).attr("data-id");
        var type = $(this).attr("data-type");
        $("#activitySummaryInformation").load("/Activity/GetSummaryInformation?id=" + id + "&type=" + type);
    });
});



function initActivityMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        //   zoom: 14,
        //     center: { lat: 53.450128, lng: -2.078496 },
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

    var activityId = document.getElementById('activityId').value;

    var routeCoords = (function () {
        var routeCoords = null;
        $.ajax({
            async: false,
            global: false,
            dataType: "json",
            type: "POST",
            url: "/api/Activity/GetMapCoords/" + activityId,
            success: function (data) {
                routeCoords = data;
            },
            error: function () {
                alert("Error getting coords data!");
            }
        });
        return routeCoords;
    })();

    // override centre/boundaries
    var latlngbounds = new google.maps.LatLngBounds();
    routeCoords.forEach(function (latLng) {
        latlngbounds.extend(latLng);
    });

    map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);

    var routePath = new google.maps.Polyline({
        path: routeCoords,
        geodesic: true,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2
    });

    routePath.setMap(map);
}