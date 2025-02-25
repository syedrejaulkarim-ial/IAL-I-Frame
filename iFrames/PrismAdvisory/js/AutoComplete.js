$(document).ready(function () {
    //debugger;
    $("#txtSearch").autocomplete({
      
        source: function (request, response) {
            //debugger;
            $.ajax({
                type: "POST",
                url: "SchemeSearch.aspx/GetCustomers",
                async: false,
                contentType: "application/json",
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
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
        select: function (e, i) {
            $("#hfCustomerId").val(i.item.val);
            //alert($("#hfCustomerId").val());
            window.location.href = "SchemeSearch.aspx?param=" + i.item.val;
        },
        minLength: 1
    });
});