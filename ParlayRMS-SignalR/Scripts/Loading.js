/* File Created: June 25, 2012 */
var loading = new Loading();
$(document).ready(function () {
    loading.init();
    loading.hide();
});

function Loading() {
    var myloading;
    this.init = function () {
        myloading = $('#loading');
    };
    this.show = function () {
        myloading.show('fade');
    };
    this.hide = function () {
        myloading.hide('fade');
    };
}