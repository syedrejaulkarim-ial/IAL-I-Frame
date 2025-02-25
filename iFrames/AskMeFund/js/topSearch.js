$(document).ready(function () {
    $("#divNoRes").hide();
    $("#txtTopSearch").autocomplete({
        minLength: 3,
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Search.aspx/TopSearchScheme",
                async: false,
                contentType: "application/json",
                data: '{ "prefix": "' + request.term + '"}',
                dataType: "json",
                success: function (data) {
                    $("#divNoRes").hide();
                    if (data.d.length == 0) {
                        $("#divNoRes").show();
                        $("#divNoRes").html("No results found.");
                    }
                    response($.map(data.d, function (item) {
                        $("#divNoRes").hide();
                        return {
                            label: item.split('#')[0],
                            val: item.split('#')[1]
                        }
                    }))
                },
                error: function (data) {
                    alert("Error! Try again...");
                }
            });
        },
        focus: function (event, ui) {
            $("#txtTopSearch").val(ui.item.value);
            $("#txtTopSearchID").val(ui.item.val);
            return false;
        },
        select: function (e, i) {
            window.open("https://askmefund.akswebsoft.men/factsheet/" + i.item.val);
        },
    });

    $("#btnSearch").click(function () {
        $("#txtTopSearch").autocomplete("search", $("#txtTopSearch").val());
        //if (typeof $("#txtTopSearchID").val() != 'undefined' && parseInt($("#txtTopSearchID").val()) > 0) {
        //    window.location.href = "factsheet.aspx?param=" + $("#txtTopSearchID").val();
        //}
    });

    $("#txtTopSearch").on('keyup',function () {
        if ($("#txtTopSearch").val() == '') {
            $("#divNoRes").hide();
        }
    })

});