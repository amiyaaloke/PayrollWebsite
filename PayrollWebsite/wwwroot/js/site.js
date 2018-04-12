// Write your Javascript code.
$(function () {
    var hide = true;

    $('#Table1 td').each(function () {
        var td_content = $(this).text();

        if (td_content != "") {
            hide = false;
        }
    })

    if (!hide) {
        $('#myTable1div').show();
    }
})
