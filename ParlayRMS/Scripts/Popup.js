/* File Created: May 9, 2012 */
var _popup;
$(document).ready(function () {
    _popup = $("#popup");
});

function showPopup(title, width, height, left, top, onCloseFunction) {
    _popup.dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        closeOnEscape: true,
        show: 'fade',
        hide: 'fade',
        close: function () {
            if (typeof onCloseFunction != 'undefined') onCloseFunction();
        }
    });

    // sets default values for params
    title = (typeof title == 'undefined' || title == null) ? '' : title;
    width = (typeof width == 'undefined' || width == null) ? 300 : width;
    height = (typeof height == 'undefined' || height == null) ? 'auto' : height;
    left = (typeof left == 'undefined' || left == null) ? 'center' : left;
    top = (typeof top == 'undefined' || top == null) ? 'center' : top;

    // initializes popup
    _popup.dialog({ title: title, position: [left, top], width: width, height: height });

    // opens popup
    _popup.dialog("open");
}

function hidePopup() {
    _popup.dialog("close");
}

function modalPopup(data) {
    $("#popup").html(data);
    $("#popup").modal({
        opacity: 20,
        overlayCss: { backgroundColor: "#000" },
        onClose: function () {
            Validators.hide();
            $.modal.close();
        }
    });
    $("body").bind("click", function (e) {
        if (e.target.id == "simplemodal-overlay") {
            $.modal.close();
        }
    });

    $("#simplemodal-container").draggable({ handle: "div.title2" });
}

function modalClose() {
    $.modal.close();
}