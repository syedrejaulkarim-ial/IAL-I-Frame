(function ($) {

    $.confirm = function (params) {
        //debugger;
        if ($('#confirmOverlay').length) {
            // A confirm is already shown on the page:
            return false;
        }

        var buttonHTML = '';
        $.each(params.buttons, function (name, obj) {

            // Generating the markup for the buttons:

            buttonHTML += '<a href="#" class="btn btn-primary btn-sm ">' + name + '<span></span></a>';

            if (!obj.action) {
                obj.action = function () { };
            }
        });

        var markup = [
			'<div id="confirmOverlay">',
			'<div id="confirmBox">',
			'<h1>', params.title, '</h1>',
			'<p>', params.message, '</p>',
			'<div id="confirmButtons" class="">',
			buttonHTML,
			'</div></div></div>'
        ].join('');

        $(markup).hide().appendTo('body').fadeIn();

        var buttons = $('#confirmBox'),
			i = 0;

        $.each(params.buttons, function (name, obj) {
            buttons.eq(i++).click(function () {

                // Calling the action attribute when a
                // click occurs, and hiding the confirm.

                obj.action();
                $.confirm.hide();
                return false;
            });
        });
    }

    $.confirm.hide = function () {
        $('#confirmOverlay').fadeOut(function () {
            $(this).remove();
        });
    }

})(jQuery);