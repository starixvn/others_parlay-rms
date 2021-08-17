/* File Created: May 31, 2012 */

function confirm(title, message, callback) {
    $('#confirm').modal({
        closeHTML: "<a href='#' title='Close' class='modal-close'>x</a>",
        position: ["30%",],
        overlayId: 'confirm-overlay',
        containerId: 'confirm-container',
        opacity: 20,
        overlayCss: { backgroundColor: "#000" },
        onShow: function (dialog) {
            var modal = this;

            $('.message', dialog.data[0]).append(message);
            $('.header span', dialog.data[0]).append(title);
            // if the user clicks "yes"
            $('.yes', dialog.data[0]).click(function () {
                // call the callback
                if ($.isFunction(callback)) {
                    callback.apply();
                }
                // close the dialog
                modal.close(); // or $.modal.close();
            });
        }
    });
    $("#simplemodal-container").draggable();
}

function confirmWithFullEvent(title, message, showEvent, closeEvent) {
    $('#confirm').modal({
        position: ["30%",],
        overlayId: 'confirm-overlay',
        containerId: 'confirm-container',
        opacity: 20,
        overlayCss: { backgroundColor: "#000" },
        onShow: function (dialog) {
            var modal = this;

            $('.message', dialog.data[0]).append(message);
            $('.header span', dialog.data[0]).append(title);
            // if the user clicks "yes"
            $('.yes', dialog.data[0]).click(function () {
                // call the callback
                if ($.isFunction(showEvent)) {
                    showEvent.apply();
                }
                // close the dialog
                modal.close(); // or $.modal.close();
            });
            $('.no', dialog.data[0]).click(function () {
                // call the callback
                if ($.isFunction(closeEvent)) {
                    closeEvent.apply();
                }
                // close the dialog
                modal.close(); // or $.modal.close();
            });
        },
    });
    $("#simplemodal-container").draggable();
}