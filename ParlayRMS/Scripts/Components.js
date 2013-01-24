var loading = function () {
    var container;
    init = function () {
        if (!container) {
            container = $('#loading');
        }
    }
    return {
        show: function () {
            init();
            container.show('fade');
        },
        hide: function () {
            init();
            container.hide('fade');
        }
    }
} ();

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
    show: function (title, message) {
        $("#dialog-message").html(message);
        $("#dialog-message").dialog({
            modal: true,
            title: title,
            draggable: false,
            resizable: false,
            buttons: {
                Ok: function () {
                    $(this).dialog("close");
                }
            }
        });
    },
    showErrorMessage: function (message) {
        this.show("Error", message);
    },
    showError: function (xhr) {
        var message = "";
        if (xhr.status == 403) {
            message = "You don't have permission on this function!!!!";
        }
        else if (xhr.status == 401) {
            message = "You are not authorized on this function!!!";
        }
        else {
            message = xhr.statusText;
        }
        this.show("Error", message);
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

var popup = function () {
    var container;
    var loadingContainer;
    var messageContainer;

    return {
        show: function (data) {
            container = $("#popup");
            container.html(data);
            container.modal({
                opacity: 20,
                overlayCss: { backgroundColor: "#000" },
                onClose: function () {
                    Validators.hide();
                    $.modal.close();
                }
            });
            messageContainer = container.find("#errMsg");
            loadingContainer = container.find("#lineloading");
        },
        hide: function () {
            container.modal.close();
        },
        showLoading: function () {
            loading.show();
            popup.hideErrorMessage();
            loadingContainer.show('fade');
        },
        hideLoading: function () {
            loadingContainer.hide('fade');
            loading.hide();
        },
        showErrorMessage: function (msg) {
            popup.hideLoading();
            messageContainer.html(msg);
            messageContainer.show('fade');
        },
        hideErrorMessage: function () {
            messageContainer.hide('fade');
            messageContainer.html('');
        }
    }
} ();

var validator = function () {
    var hint = {
        add: function (elementInput, expr, message) {
            // Bind onchange event of input
            // Validate with expr
            // Show message and animate background color
        },
        show: function (elementInput, message, xPos, pointerXPos) {
            // <span class="hint"><span>This is the message.<span><span class="hint-pointer">&nbsp;</span></span>
            var hint = document.getElementById('fHint');
            var hintPointer = null;
            if (hint == null) {
                var hint = document.createElement("SPAN");
                hint.className = 'hint';
                var hintMsg = document.createElement("SPAN");
                hint.appendChild(hintMsg);
                hintPointer = document.createElement("SPAN");
                hintPointer.className = 'hint-pointer';
                hint.appendChild(hintPointer);
                hint.id = 'fHint';
                hint.style.top = '10px';
                hint.style.left = '10px';
                hint.onclick = function () { // Hide onclick
                    hint.style.display = 'none';
                };
                document.body.appendChild(hint);
            };
            hintPointer = hint.lastChild;
            hint.firstChild.innerHTML = message;
            if (this.timeOutHideTooltip) {
                clearTimeout(this.timeOutHideTooltip);
                this.timeOutHideTooltip = null;
            };
            this.timeOutHideTooltip = setTimeout(function () {
                hint.style.display = 'none';
            }, 10000); // Auto hide in 10s
            if (elementInput != null) {
                if (typeof xPos == 'undefined') {
                    xPos = 0;
                }
                if (typeof pointerXPos == 'undefined') {
                    pointerXPos = 10;
                }

                hint.style.backgroundPositionX = (pointerXPos + 'px');
                hintPointer.style.left = (pointerXPos + 'px');

                //var pos = Validators.utils.findPos(elementInput);
                //hint.style.left = (pos[0] + xPos) + 'px';
                //hint.style.top = (pos[1] - 40) + 'px';
                var pos = $(elementInput).offset();
                hint.style.left = (pos.left + xPos) + 'px';
                hint.style.top = (pos.top - 40) + 'px';
                elementInput.onchange = function () {
                    hint.style.display = 'none';
                };
                elementInput.focus();
            };
            hint.style.display = 'block';
        },
        hide: function () {
            if (document.getElementById('fHint')) {
                var hint = document.getElementById('fHint');
                hint.style.display = 'none';
            }
        },
        animate: function (input) {
            var previousColor = input.style.color;
            var previousBackground = input.style.backgroundColor;
            var c = 4;
            if (this.intervalAnimate) {
                clearInterval(this.intervalAnimate);
                this.intervalAnimate = null;
            }
            var int = this.intervalAnimate = setInterval(function () {
                c--;
                if (input.style.backgroundColor == previousBackground) {
                    input.style.backgroundColor = '#ff7700';
                    input.style.color = '#ffffff';
                } else {
                    input.style.backgroundColor = previousBackground;
                    input.style.color = previousColor;
                }
                if (c == 0) {
                    clearInterval(int);
                }
            }, 300);
        },
        animateText: function (input, text) {
            var previousColor = input.style.color;
            var previousBackground = input.style.backgroundColor;
            var previousText = input.value;
            var preType = input.type;
            var c = 6;
            if (this.intervalAnimate) {
                clearInterval(this.intervalAnimate);
                this.intervalAnimate = null;
            }
            var int = this.intervalAnimate = setInterval(function () {
                c--;
                if (input.style.backgroundColor == previousBackground) {
                    input.style.backgroundColor = '#fff7b2';
                    input.style.color = '#fd1718';
                    if (preType == 'password')
                        input.type = 'text';
                    input.value = text;
                } else {
                    input.type = preType;
                    input.style.backgroundColor = previousBackground;
                    input.style.color = previousColor;
                    input.value = previousText;
                }
                if (c == 0) {
                    clearInterval(int);
                    input.focus();
                }
            }, 500);
        },
        utils: {
            // findPos() by quirksmode.org
            // Finds the absolute position of an element on a page
            findPos: function (obj) {
                var curleft = curtop = 0;
                if (obj.offsetParent) {
                    do {
                        curleft += obj.offsetLeft;
                        curtop += obj.offsetTop;
                    } while (obj = obj.offsetParent);
                }
                return [curleft, curtop];
            },
            // getPageScroll() by quirksmode.org
            // Finds the scroll position of a page
            getPageScroll: function () {
                var xScroll, yScroll;
                if (self.pageYOffset) {
                    yScroll = self.pageYOffset;
                    xScroll = self.pageXOffset;
                } else if (document.documentElement && document.documentElement.scrollTop) {
                    yScroll = document.documentElement.scrollTop;
                    xScroll = document.documentElement.scrollLeft;
                } else if (document.body) { // all other Explorers
                    yScroll = document.body.scrollTop;
                    xScroll = document.body.scrollLeft;
                }
                return [xScroll, yScroll]
            },
            // Finds the position of an element relative to the viewport.
            findPosRelativeToViewport: function (obj) {
                var objPos = this.findPos(obj)
                var scroll = this.getPageScroll()
                return [objPos[0] - scroll[0], objPos[1] - scroll[1]]
            }
        }
    };
    return {
        validateRequired: function (element) {
            var value = $.trim($(element).val());
            if (!value) {
                $(element).focus();
                hint.show($(element).get(0), 'This field is required');
                return true;
            }
            return false;
        },
        validateInt: function (element) {
            var value = $.trim($(element).val());
            if (!isNumeric(value) || parseInt(value) < 0) {
                $(element).focus();
                hint.show($(element).get(0), 'Order value must be greater or than 0');
                return true;
            };
            return false;
        }
    };
} ();