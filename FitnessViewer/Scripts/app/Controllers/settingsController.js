var settingsController = function () {
    var init = function () {
        var dotNetMaxInt = 2147483647;

        $("#add").click(function (e) {
            var row = $("tr[data-id='" + dotNetMaxInt + "']");
            row.removeClass("Hidden");
            $("#add").addClass("Hidden");
            editRow(dotNetMaxInt);
            e.preventDefault();
        });

        // edit
        $(".glyphicon-pencil").click(function (e) {
            var id = $(this).closest('tr').attr("data-id");         
            editRow(id);
        });

        // ok
        $(".glyphicon-ok").click(function (e) {
            var id = $(this).closest('tr').attr("data-id");
           
            var model = { Id: id, Value: $("#valueEdit_" + id).val(), ZoneType: 1, StartDate: $("#dateEdit_" + id).val() };
            $.post("FTPUpdate", model, function (data) {
                if (data.success)
                {
                    refreshGrid();
                }
                else
                {
                    showError(data.responseText);
                    resetIcons(id);
                }
            });
        });

        // delete
        $(".glyphicon-trash").click(function (e) {
            var id = $(this).closest('tr').attr("data-id");

            //var model = { Id: id, Value: $("#valueEdit_" + id).val(), ZoneType: 1, StartDate: $("#dateEdit_" + id).val() };
            $.post("FTPDelete/"+id, id, function (data) {
                if (data.success) {
                    refreshGrid();
                }
                else {
                    showError(data.responseText);
                }
            });

        });

        // cancel edit
        $(".glyphicon-remove").click(function (e) {
            var id = $(this).closest('tr').attr("data-id"); 
            resetIcons(id);
        });

        function editRow(id) {
            toggleHidden(id);

        };

        function resetIcons(id) {
            var row = $("tr[data-id='" + id + "']");
            if (id == dotNetMaxInt) {
                row.addClass("Hidden");
                $("#add").removeClass("Hidden");
            }
            toggleHidden(id);
        }

        function toggleHidden(id) {
            var row = $("tr[data-id='" + id + "']");
            $(row).find("#save").toggleClass("Hidden");
            $(row).find("#cancel").toggleClass("Hidden");
            $(row).find("#valueEdit_" + id).toggleClass("Hidden");
            $(row).find("#dateEdit_" + id).toggleClass("Hidden");
            $(row).find("#valueView").toggleClass("Hidden");
            $(row).find("#dateView").toggleClass("Hidden");
            $(row).find("#delete").toggleClass("Hidden");
            $(row).find("#edit").toggleClass("Hidden");
        }

        function showError(message) {
            $("#error").removeClass("Hidden");
            $("#errorMessage").text(message);
        };
        
        function refreshGrid() {
            //$("#zoneGrid").load("/settings/GetZoneGridInformation");
            //settingsController.init();
            location.reload();
        };
    };
    return {
        init: init
    };
}();