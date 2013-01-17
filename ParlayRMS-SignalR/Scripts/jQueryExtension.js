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