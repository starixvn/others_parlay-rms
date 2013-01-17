var loading = new Loading();
var loadingInPopup = new LoadingInPopup();

$(document).ready(function () {
    loading.init();
    loadingInPopup.init();
});

// Loading class
function Loading() {
    var container;

    this.init = function () {
        container = $('#loading');
    }

    this.show = function () {
        container.show('fade');
    }

    this.hide = function () {
        container.hide('fade');
    }
};

// LoadingInPopup class
function LoadingInPopup() {
    var container;

    this.init = function () {
        container = $('#lineloading');
    }

    this.show = function () {
        this.init();
        container.show('fade');
    }

    this.hide = function () {
        container.hide('fade');
    }
};

// loadingHelper object helps to show or hide loading based on running tasks
var loadingHelper = function () {
    var taskCount = 0;

    var validateLoading = function () {
        if (taskCount == 0) {
            loading.hide();
        }
        else {
            loading.show();
        }
    }

    return {
        increaseTask: function () {
            taskCount++;
            validateLoading();

            return taskCount;
        },
        decreaseTask: function () {
            taskCount--;
            validateLoading();

            return taskCount;
        }
    }
} ();

// alert box
var alertBox = {
    show: function (titleText, textContent) {
        $("#dialog-message").html(textContent);
        $("#dialog-message").dialog({
            modal: true,
            title: titleText,
            draggable: false,
            resizable: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    },
    showError: function (xhr) {
        if (xhr.status == 403) {
            $("#dialog-message").html("You don't have permission on this function!!!!");
        }
        else if (xhr.status == 401) {
            $("#dialog-message").html("You are not authorized on this function!!!");
        }
        else {
            $("#dialog-message").html(xhr.statusText);
        }
        $("#dialog-message").dialog({
            modal: true,
            title: "Error Message",
            draggable: false,
            resizable: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    }
};

// jQuery extension to create hover effect on table
(function ($) {
    $.fn.cellSelect = function (row, col) {
        var selectedTd = this.find("tr").eq(row).children().eq(col);
        selectedTd.addClass("selected");
    };

    $.fn.cellDeselect = function (row, col) {
        var selectedTd = this.find("tr").eq(row).children().eq(col);
        selectedTd.removeClass("selected");
    };

    $.fn.tableHover = function (selectPosition) {
        var position = selectPosition;
        if (position !== null) {
            this.cellSelect(position.Row, position.Col);
        }

        this.delegate('td', 'mouseover mouseleave', function (e) {
            var focusedTr = $(this).parent();
            var focusedColGroup = $(this).parents("table").find("colgroup").eq($(this).index());

            if (e.type === 'mouseover') {
                $(this).addClass("hover");
                focusedTr.addClass("hover");
                focusedColGroup.addClass("hover");
            }
            else if (e.type === 'mouseleave') {
                $(this).removeClass("hover");
                focusedTr.removeClass("hover");
                focusedColGroup.removeClass("hover");
            }
        });
    };

    $.fn.showLoading = function () {
        var imgURL = resolveClientUrl('Images/Content/icon_wait.gif');
        var htmlContent = '<img width="15px" height="15px" border="0" alt="waiting" src="' + imgURL + '" />';
        this.html(htmlContent);
    };

    $.fn.showError = function () {
        this.html("Error");
    };
})(jQuery);

// jQuery extension functions
$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});

var tableSorter = function () {
    return {
        bind1: function (src, param) {
            tableSorter.bind2(src, src, param);
        },
        bind2: function (src, desc, param) {
            var header = src.find('th.sortable');
            header.attr("title", "Click to sort");
            header.click(function () {
                if (this.sortDirection == "asc") {
                    this.sortDirection = "desc";
                    $(this).removeClass("headerSortDown").addClass("headerSortUp");
                }
                else {
                    this.sortDirection = "asc";
                    $(this).removeClass("headerSortUp").addClass("headerSortDown");
                }
                var type = 'text';
                if (param) {
                    if (param.type) {
                        type = param.type;
                    }
                }
                tableSorter.sortColumn(desc, $(this).index(), this.sortDirection, type);
                if (param) {
                    if (param.onFinish) {
                        param.onFinish(this);
                    }
                }
            });
        },
        sortColumn: function (table, col, sortDirection, type) {
            var selector = $(table).find('td').filter(function () {
                return $(this).index() == col;
            }).sortElements(function (a, b) {
                var x = $.trim($(a).text());
                var y = $.trim($(b).text());
                if (type == 'float') {
                    x = parseFloat(x.replace(',', ''));
                    y = parseFloat(y.replace(',', ''));
                }
                else if (type == 'int') {
                    x = parseInt(x.replace(',', ''));
                    y = parseInt(y.replace(',', ''));
                }
                if (sortDirection == 'asc') {
                    return x > y ? -1 : 1;
                }
                else {
                    return x > y ? 1 : -1;
                }
            }, function () {
                return this.parentNode;
            });
        }
    }
} ();