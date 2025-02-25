$(document).ready(function () {


        $("#txtSearch")
        //.on("mouseout", function () {
        //    $("#ui-id-1").css('display', 'none');
        //})
        .on("mouseover", function () {
            $('.ui-autocomplete').css('max-height', '200px');
            window.parent.postMessage({ action: 'expand', height: '200px' }, '*');
        });

    

    $("#ui-id-1")
        .on("mouseout", function () {
            debugger;
            $('.ui-autocomplete').css('max-height', '0px');
        })
        .on("mouseover", function () {
            debugger;
            $('.ui-autocomplete').css('max-height', '200px');
        });


    $("#txtSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "Search.aspx/GetCustomers",
                async: false,
                contentType: "application/json",
                data: '{ "prefix": "' + request.term + '"}',
                dataType: "json",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('#')[0],
                            val: item.split('#')[1]
                        }
                    }))
                    //$('.ui-autocomplete').css('max-height', '200px');
                },
                error: function (data) {
                    alert("Error! Try again...");
                }
            });
        },
        select: function (e, i) {
            $("#hfCustomerId").val(i.item.val);
            //alert($("#hfCustomerId").val());
            //window.location.href = "https://mfiframes.mutualfundsindia.com/askmefund/factsheet.aspx?param=" + i.item.val;
            //window.location.href = "https://askmefund.akswebsoft.men/factsheet/" + i.item.val;
            window.open("https://askmefund.com/factsheet/" + i.item.val, '_blank')
            $('#ui-id-1').css('max-height', '0px');
            window.parent.postMessage({ action: 'collapse' }, '*');
        },
        minLength: 1
    });
});