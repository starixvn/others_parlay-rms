/* File Created: June 25, 2012 */

function ErrorMessageInPopup() {
    var message = null;

    this.init = function () {
        message = $('#errMsg');
    };
    this.show = function (msg) {
        this.init();

        message.html(msg);
        message.show('fade');
    };
    this.hide = function () {
        this.init();
        if (message != null) {
            message.hide('fade');
            message.html('');
        }
    };
}