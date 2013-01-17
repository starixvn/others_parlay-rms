/* File Created: June 25, 2012 */

function LoadingInPopup() {
    var loading = null;

    this.init = function () {
        loading = $('#lineloading');
    };
    this.show = function () {
        this.init();
        loading.show('fade');
    };
    this.hide = function () {
        this.init();
        if (loading != null) {
            loading.hide('fade');
        }
    };
}