$(function () {
    $.ajaxSetup({ cache: false });
    $("a[data-modal]").on("click", function (e) {
        $('#fvModalContent').load(this.href, function () {
            $('#fvModal').modal({
                keyboard: true
            }, 'show');
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#fvModal').modal('hide');
                    location.reload();
                } else {
                    $('#fvModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}

