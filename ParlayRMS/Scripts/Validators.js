/* File Created: May 10, 2012 */

// Validators library
var Validators = {
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