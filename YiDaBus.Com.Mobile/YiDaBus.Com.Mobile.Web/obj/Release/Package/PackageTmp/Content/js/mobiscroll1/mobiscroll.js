var mobiscroll = mobiscroll || {};

(function (window, document, undefined) {

    var cssNumber = {
            'column-count': 1,
            'columns': 1,
            'font-weight': 1,
            'line-height': 1,
            'opacity': 1,
            'z-index': 1,
            'zoom': 1
        },
        propMap = {
            'readonly': 'readOnly'
        },
        emptyArray = [],
        slice = Array.prototype.slice;

    function isFunction(value) {
        return typeof value === "function";
    }

    function isObject(obj) {
        return typeof obj === "object";
    }

    function likeArray(obj) {
        return typeof obj.length == 'number';
    }

    function camelize(str) {
        return str.replace(/-+(.)?/g, function (match, chr) {
            return chr ? chr.toUpperCase() : '';
        });
    }

    function extend(target, source, deep) {
        for (var key in source) {
            if (deep && ($.isPlainObject(source[key]) || $.isArray(source[key]))) {
                if ($.isPlainObject(source[key]) && !$.isPlainObject(target[key]) || $.isArray(source[key]) && !$.isArray(target[key])) {
                    target[key] = {};
                }
                extend(target[key], source[key], deep);
            } else if (source[key] !== undefined) {
                target[key] = source[key];
            }
        }
    }

    function dasherize(str) {
        return str.replace(/::/g, '/')
            .replace(/([A-Z]+)([A-Z][a-z])/g, '$1_$2')
            .replace(/([a-z\d])([A-Z])/g, '$1_$2')
            .replace(/_/g, '-')
            .toLowerCase();
    }

    function maybeAddPx(name, value) {
        return (typeof value == "number" && !cssNumber[dasherize(name)]) ? value + "px" : value;
    }

    var Dom = (function () {
        var Dom = function (arr) {
            var _this = this,
                i = 0;
            // Create array-like object
            for (i = 0; i < arr.length; i++) {
                _this[i] = arr[i];
            }
            _this.length = arr.length;
            // Return collection with methods
            return $(this);
        };

        var $ = function (selector, context) {
            var arr = [],
                i = 0;
            if (selector && !context) {
                if (selector instanceof Dom) {
                    return selector;
                }
            }

            if (isFunction(selector)) {
                return $(document).ready(selector);
            }

            if (selector) {
                // String
                if (typeof selector === 'string') {
                    var els, tempParent, html;
                    selector = html = selector.trim();
                    if (html.indexOf('<') >= 0 && html.indexOf('>') >= 0) {
                        var toCreate = 'div';
                        if (html.indexOf('<li') === 0) {
                            toCreate = 'ul';
                        }
                        if (html.indexOf('<tr') === 0) {
                            toCreate = 'tbody';
                        }
                        if (html.indexOf('<td') === 0 || html.indexOf('<th') === 0) {
                            toCreate = 'tr';
                        }
                        if (html.indexOf('<tbody') === 0) {
                            toCreate = 'table';
                        }
                        if (html.indexOf('<option') === 0) {
                            toCreate = 'select';
                        }
                        tempParent = document.createElement(toCreate);
                        tempParent.innerHTML = html;
                        for (i = 0; i < tempParent.childNodes.length; i++) {
                            arr.push(tempParent.childNodes[i]);
                        }
                    } else {
                        if (!context && selector[0] === '#' && !selector.match(/[ .<>:~]/)) {
                            // Pure ID selector
                            els = [document.getElementById(selector.split('#')[1])];
                        } else {
                            if (context instanceof Dom) {
                                context = context[0];
                            }
                            // Other selectors
                            els = (context || document).querySelectorAll(selector);

                        }
                        for (i = 0; i < els.length; i++) {
                            if (els[i]) {
                                arr.push(els[i]);
                            }
                        }
                    }
                }
                // Node/element
                else if (selector.nodeType || selector === window || selector === document) {
                    arr.push(selector);
                }
                //Array of elements or instance of Dom
                else if (selector.length > 0 && selector[0].nodeType) {
                    for (i = 0; i < selector.length; i++) {
                        arr.push(selector[i]);
                    }
                } else if ($.isArray(selector)) {
                    arr = selector;
                }
            }
            return new Dom(arr);
        };

        Dom.prototype = {
            ready: function (callback) {
                if (document.attachEvent ? document.readyState == 'complete' : document.readyState != 'loading') {
                    callback($);
                } else {
                    document.addEventListener('DOMContentLoaded', function () {
                        callback($);
                    }, false);
                }
                return this;
            },
            concat: emptyArray.concat,
            empty: function () {
                return this.each(function () {
                    this.innerHTML = '';
                });
            },
            map: function (fn) {
                return $($.map(this, function (el, i) {
                    return fn.call(el, i, el);
                }));
            },
            slice: function () {
                return $(slice.apply(this, arguments));
            },
            // Classes and attriutes
            // NOTE: element.classList attribure is not supported on android 2.3!!!
            addClass: function (className) {
                if (typeof className === 'undefined') {
                    return this;
                }

                var classes = className.split(' ');

                for (var i = 0; i < classes.length; i++) {
                    for (var j = 0; j < this.length; j++) {
                        if (typeof this[j].classList !== 'undefined' && classes[i] !== '') {
                            this[j].classList.add(classes[i]);
                        }
                    }
                }
                return this;
            },
            removeClass: function (className) {
                if (typeof className === 'undefined') {
                    return this;
                }

                var classes = className.split(' ');
                for (var i = 0; i < classes.length; i++) {
                    for (var j = 0; j < this.length; j++) {
                        if (typeof this[j].classList !== 'undefined' && classes[i] !== '') {
                            this[j].classList.remove(classes[i]);
                        }
                    }
                }
                return this;
            },
            hasClass: function (className) {
                return this[0] ? this[0].classList.contains(className) : false;
            },
            toggleClass: function (className) {
                var classes = className.split(' ');
                for (var i = 0; i < classes.length; i++) {
                    for (var j = 0; j < this.length; j++) {
                        if (typeof this[j].classList !== 'undefined') {
                            this[j].classList.toggle(classes[i]);
                        }
                    }
                }
                return this;
            },
            closest: function (selector, context) {
                var node = this[0],
                    collection = false;

                if (isObject(selector)) {
                    collection = $(selector);
                }
                while (node && !(collection ? collection.indexOf(node) >= 0 : $.matches(node, selector))) {
                    node = node !== context && node.nodeType !== node.DOCUMENT_NODE && node.parentNode;
                }

                return $(node);
            },
            attr: function (attrs, value) {
                var attr;

                if (arguments.length === 1 && typeof attrs === 'string' && this.length) {
                    // Get attr
                    attr = this[0].getAttribute(attrs);
                    return this[0] && (attr || attr === '') ? attr : undefined;
                } else {
                    // Set attrs
                    for (var i = 0; i < this.length; i++) {
                        if (arguments.length === 2) {
                            // String
                            this[i].setAttribute(attrs, value);
                        } else {
                            // Object
                            for (var attrName in attrs) {
                                this[i][attrName] = attrs[attrName];
                                this[i].setAttribute(attrName, attrs[attrName]);
                            }
                        }
                    }
                    return this;
                }
            },
            removeAttr: function (attr) {
                for (var i = 0; i < this.length; i++) {
                    this[i].removeAttribute(attr);
                }
                return this;
            },
            prop: function (props, value) {
                props = propMap[props] || props;
                if (arguments.length === 1 && typeof props === 'string') {
                    // Get prop
                    return this[0] ? this[0][props] : undefined;
                } else {
                    // Set props
                    for (var i = 0; i < this.length; i++) {
                        this[i][props] = value;
                    }
                    return this;
                }
            },
            val: function (value) {
                if (typeof value === 'undefined') {
                    if (this.length && this[0].multiple) {
                        return $.map(this.find('option:checked'), function (v) {
                            return v.value;
                        });
                    }
                    return this[0] ? this[0].value : undefined;
                }

                if (this.length && this[0].multiple) {
                    $.each(this[0].options, function () {
                        this.selected = value.indexOf(this.value) != -1;
                    });
                } else {
                    for (var i = 0; i < this.length; i++) {
                        this[i].value = value;
                    }
                }

                return this;
            },
            //Events
            on: function (eventName, targetSelector, listener, capture) {
                var events = eventName.split(' '),
                    i, j;

                function handleLiveEvent(e) {
                    var k,
                        parents,
                        target = e.target;

                    if ($(target).is(targetSelector)) {
                        listener.call(target, e);
                    } else {
                        parents = $(target).parents();
                        for (k = 0; k < parents.length; k++) {
                            if ($(parents[k]).is(targetSelector)) {
                                listener.call(parents[k], e);
                            }
                        }
                    }
                }

                function handleNamespaces(elm, name, listener, capture) {
                    var namespace = name.split('.');

                    if (!elm.DomNameSpaces) {
                        elm.DomNameSpaces = [];
                    }

                    elm.DomNameSpaces.push({
                        namespace: namespace[1],
                        event: namespace[0],
                        listener: listener,
                        capture: capture
                    });

                    elm.addEventListener(namespace[0], listener, capture);
                }

                for (i = 0; i < this.length; i++) {
                    if (isFunction(targetSelector) || targetSelector === false) {
                        // Usual events
                        if (isFunction(targetSelector)) {
                            capture = listener || false;
                            listener = targetSelector;
                        }
                        for (j = 0; j < events.length; j++) {
                            // check for namespaces
                            if (events[j].indexOf('.') != -1) {
                                handleNamespaces(this[i], events[j], listener, capture);
                            } else {
                                this[i].addEventListener(events[j], listener, capture);
                            }
                        }
                    } else {
                        // Live events
                        for (j = 0; j < events.length; j++) {
                            if (!this[i].DomLiveListeners) {
                                this[i].DomLiveListeners = [];
                            }

                            this[i].DomLiveListeners.push({
                                listener: listener,
                                liveListener: handleLiveEvent
                            });

                            if (events[j].indexOf('.') != -1) {
                                handleNamespaces(this[i], events[j], handleLiveEvent, capture);
                            } else {
                                this[i].addEventListener(events[j], handleLiveEvent, capture);
                            }
                        }
                    }
                }
                return this;
            },
            off: function (eventName, targetSelector, listener, capture) {
                var events,
                    i, j, k,
                    that = this;

                function removeEvents(event) {
                    var i, j,
                        item,
                        parts = event.split('.'),
                        name = parts[0],
                        ns = parts[1];

                    for (i = 0; i < that.length; ++i) {
                        if (that[i].DomNameSpaces) {
                            for (j = 0; j < that[i].DomNameSpaces.length; ++j) {
                                item = that[i].DomNameSpaces[j];

                                if (item.namespace == ns && (item.event == name || !name)) {
                                    that[i].removeEventListener(item.event, item.listener, item.capture);
                                    item.removed = true;
                                }
                            }
                            // remove the events from the DomNameSpaces array
                            for (j = that[i].DomNameSpaces.length - 1; j >= 0; --j) {
                                if (that[i].DomNameSpaces[j].removed) {
                                    that[i].DomNameSpaces.splice(j, 1);
                                }
                            }
                        }
                    }
                }

                events = eventName.split(' ');

                for (i = 0; i < events.length; i++) {
                    for (j = 0; j < this.length; j++) {
                        if (isFunction(targetSelector) || targetSelector === false) {
                            // Usual events
                            if (isFunction(targetSelector)) {
                                capture = listener || false;
                                listener = targetSelector;
                            }

                            if (events[i].indexOf('.') === 0) { // remove namespace events
                                removeEvents(events[i].substr(1), listener, capture);
                            } else {
                                this[j].removeEventListener(events[i], listener, capture);
                            }
                        } else {
                            // Live event
                            if (this[j].DomLiveListeners) {
                                for (k = 0; k < this[j].DomLiveListeners.length; k++) {
                                    if (this[j].DomLiveListeners[k].listener === listener) {
                                        this[j].removeEventListener(events[i], this[j].DomLiveListeners[k].liveListener, capture);
                                    }
                                }
                            }
                            if (this[j].DomNameSpaces && this[j].DomNameSpaces.length && events[i]) {
                                removeEvents(events[i]);
                            }
                        }
                    }
                }

                return this;
            },
            trigger: function (eventName, eventData) {
                var events = eventName.split(' ');
                for (var i = 0; i < events.length; i++) {
                    for (var j = 0; j < this.length; j++) {
                        var evt;
                        try {
                            evt = new CustomEvent(events[i], {
                                detail: eventData,
                                bubbles: true,
                                cancelable: true
                            });
                        } catch (e) {
                            evt = document.createEvent('Event');
                            evt.initEvent(events[i], true, true);
                            evt.detail = eventData;
                        }
                        this[j].dispatchEvent(evt);
                    }
                }
                return this;
            },
            // Sizing/Styles
            width: function (dim) {
                if (dim !== undefined) {
                    return this.css('width', dim);
                }

                if (this[0] === window) {
                    return window.innerWidth;
                } else if (this[0] === document) {
                    return document.documentElement.scrollWidth;
                } else {
                    return this.length > 0 ? parseFloat(this.css('width')) : null;
                }
            },
            height: function (dim) {
                if (dim !== undefined) {
                    return this.css('height', dim);
                }

                if (this[0] === window) {
                    return window.innerHeight;
                } else if (this[0] === document) {
                    var body = document.body,
                        html = document.documentElement;

                    return Math.max(body.scrollHeight, body.offsetHeight, html.clientHeight, html.scrollHeight, html.offsetHeight);
                } else {
                    return this.length > 0 ? parseFloat(this.css('height')) : null;
                }
            },
            innerWidth: function () {
                var elm = this;
                if (this.length > 0) {
                    if (this[0].innerWidth) {
                        return this[0].innerWidth;
                    } else {
                        var size = this[0].offsetWidth,
                            sides = ['left', 'right'];

                        sides.forEach(function (side) {
                            size -= parseInt(elm.css(camelize('border-' + side + '-width')) || 0, 10);
                        });
                        return size;
                    }
                }
            },
            innerHeight: function () {
                var elm = this;
                if (this.length > 0) {
                    if (this[0].innerHeight) {
                        return this[0].innerHeight;
                    } else {
                        var size = this[0].offsetHeight,
                            sides = ['top', 'bottom'];

                        sides.forEach(function (side) {
                            size -= parseInt(elm.css(camelize('border-' + side + '-width')) || 0, 10);
                        });

                        return size;
                    }
                }
            },
            offset: function () {
                if (this.length > 0) {
                    var el = this[0],
                        box = el.getBoundingClientRect(),
                        body = document.body,
                        clientTop = el.clientTop || body.clientTop || 0,
                        clientLeft = el.clientLeft || body.clientLeft || 0,
                        scrollTop = window.pageYOffset || el.scrollTop,
                        scrollLeft = window.pageXOffset || el.scrollLeft;

                    return {
                        top: box.top + scrollTop - clientTop,
                        left: box.left + scrollLeft - clientLeft
                    };
                }
            },
            hide: function () {
                for (var i = 0; i < this.length; i++) {
                    this[i].style.display = 'none';
                }
                return this;
            },
            show: function () {
                for (var i = 0; i < this.length; i++) {
                    if (this[i].style.display == "none") {
                        this[i].style.display = '';
                    }

                    if (getComputedStyle(this[i], '').getPropertyValue("display") == "none") {
                        this[i].style.display = 'block';
                    }
                }

                return this;
            },
            clone: function () {
                return this.map(function () {
                    return this.cloneNode(true);
                });
            },
            styles: function () {
                return this[0] ? window.getComputedStyle(this[0], null) : undefined;
            },
            css: function (property, value) {
                var i,
                    key,
                    element = this[0],
                    css = '';

                if (arguments.length < 2) {
                    if (!element) {
                        return;
                    }
                    if (typeof property === 'string') {
                        return element.style[property] || getComputedStyle(element, '').getPropertyValue(property);
                    }
                }

                if (typeof property === 'string') {
                    if (!value && value !== 0) {
                        this.each(function () {
                            this.style.removeProperty(dasherize(property));
                        });
                    } else {
                        css = dasherize(property) + ":" + maybeAddPx(property, value);
                    }
                } else {
                    for (key in property) {
                        if (!property[key] && property[key] !== 0) {
                            for (i = 0; i < this.length; i++) {
                                this[i].style.removeProperty(dasherize(key));
                            }
                        } else {
                            css += dasherize(key) + ':' + maybeAddPx(key, property[key]) + ';';
                        }
                    }
                }

                return this.each(function () {
                    this.style.cssText += ';' + css;
                });
            },
            each: function (callback) {
                for (var i = 0; i < this.length; i++) {
                    if (callback.apply(this[i], [i, this[i]]) === false) {
                        break;
                    }
                }
                return this;
            },
            filter: function (callback) {
                var matchedItems = [];

                for (var i = 0; i < this.length; i++) {
                    if (isFunction(callback)) {
                        if (callback.call(this[i], i, this[i])) {
                            matchedItems.push(this[i]);
                        }
                    } else if ($.matches(this[i], callback)) {
                        matchedItems.push(this[i]);
                    }
                }

                return new Dom(matchedItems);
            },
            html: function (html) {
                if (typeof html === 'undefined') {
                    return this[0] ? this[0].innerHTML : undefined;
                } else {
                    this.empty();
                    for (var i = 0; i < this.length; i++) {
                        this[i].innerHTML = html;
                    }
                    return this;
                }
            },
            text: function (text) {
                if (typeof text === 'undefined') {
                    return this[0] ? this[0].textContent.trim() : null;
                } else {
                    for (var i = 0; i < this.length; i++) {
                        this[i].textContent = text;
                    }
                    return this;
                }
            },
            is: function (selector) {
                return this.length > 0 && $.matches(this[0], selector);
            },
            not: function (selector) {
                var nodes = [];
                if (isFunction(selector) && selector.call !== undefined) {
                    this.each(function (idx) {
                        if (!selector.call(this, idx)) {
                            nodes.push(this);
                        }
                    });
                } else {
                    var excludes = typeof selector == 'string' ? this.filter(selector) : (likeArray(selector) && isFunction(selector.item)) ? slice.call(selector) : $(selector);

                    if (isObject(excludes)) {
                        excludes = $.map(excludes, function (el) {
                            return el;
                        });
                    }

                    this.each(function (i, el) {
                        if (excludes.indexOf(el) < 0) {
                            nodes.push(el);
                        }
                    });
                }

                return $(nodes);
            },
            indexOf: function (el) {
                for (var i = 0; i < this.length; i++) {
                    if (this[i] === el) {
                        return i;
                    }
                }
            },
            index: function (element) {
                return element ? this.indexOf($(element)[0]) : this.parent().children().indexOf(this[0]);
            },
            get: function (idx) {
                return idx === undefined ? slice.call(this) : this[idx >= 0 ? idx : idx + this.length];
            },
            eq: function (index) {
                if (typeof index === 'undefined') {
                    return this;
                }
                var length = this.length,
                    returnIndex;

                if (index > length - 1) {
                    return new Dom([]);
                }
                if (index < 0) {
                    returnIndex = length + index;
                    return returnIndex < 0 ? new Dom([]) : new Dom([this[returnIndex]]);
                }
                return new Dom([this[index]]);
            },
            append: function (newChild) {
                var i, j;
                for (i = 0; i < this.length; i++) {
                    if (typeof newChild === 'string') {
                        var tempDiv = document.createElement('div');
                        tempDiv.innerHTML = newChild;
                        while (tempDiv.firstChild) {
                            this[i].appendChild(tempDiv.firstChild);
                        }
                    } else if (newChild instanceof Dom) {
                        for (j = 0; j < newChild.length; j++) {
                            this[i].appendChild(newChild[j]);
                        }
                    } else {
                        this[i].appendChild(newChild);
                    }
                }
                return this;
            },
            appendTo: function (parent) {
                $(parent).append(this);
                return this;
            },
            prepend: function (newChild) {
                var i, j;
                for (i = 0; i < this.length; i++) {
                    if (typeof newChild === 'string') {
                        var tempDiv = document.createElement('div');
                        tempDiv.innerHTML = newChild;
                        for (j = tempDiv.childNodes.length - 1; j >= 0; j--) {
                            this[i].insertBefore(tempDiv.childNodes[j], this[i].childNodes[0]);
                        }
                        // this[i].insertAdjacentHTML('afterbegin', newChild);
                    } else if (newChild instanceof Dom) {
                        for (j = 0; j < newChild.length; j++) {
                            this[i].insertBefore(newChild[j], this[i].childNodes[0]);
                        }
                    } else {
                        this[i].insertBefore(newChild, this[i].childNodes[0]);
                    }
                }
                return this;
            },
            prependTo: function (parent) {
                $(parent).prepend(this);
                return this;
            },
            insertBefore: function (selector) {
                var before = $(selector);

                for (var i = 0; i < this.length; i++) {
                    if (before.length === 1) {
                        before[0].parentNode.insertBefore(this[i], before[0]);
                    } else if (before.length > 1) {
                        for (var j = 0; j < before.length; j++) {
                            before[j].parentNode.insertBefore(this[i].cloneNode(true), before[j]);
                        }
                    }
                }
                return this;
            },
            insertAfter: function (selector) {
                var after = $(selector);
                for (var i = 0; i < this.length; i++) {
                    if (after.length === 1) {
                        after[0].parentNode.insertBefore(this[i], after[0].nextSibling);
                    } else if (after.length > 1) {
                        for (var j = 0; j < after.length; j++) {
                            after[j].parentNode.insertBefore(this[i].cloneNode(true), after[j].nextSibling);
                        }
                    }
                }

                return this;
            },
            next: function (selector) {
                if (this.length > 0) {
                    if (selector) {
                        if (this[0].nextElementSibling && $(this[0].nextElementSibling).is(selector)) {
                            return new Dom([this[0].nextElementSibling]);
                        } else {
                            return new Dom([]);
                        }
                    } else {
                        if (this[0].nextElementSibling) {
                            return new Dom([this[0].nextElementSibling]);
                        } else {
                            return new Dom([]);
                        }
                    }
                } else {
                    return new Dom([]);
                }
            },
            nextAll: function (selector) {
                var nextEls = [],
                    el = this[0];

                if (!el) {
                    return new Dom([]);
                }
                while (el.nextElementSibling) {
                    var next = el.nextElementSibling;
                    if (selector) {
                        if ($(next).is(selector)) {
                            nextEls.push(next);
                        }
                    } else {
                        nextEls.push(next);
                    }
                    el = next;
                }
                return new Dom(nextEls);
            },
            prev: function (selector) {
                if (this.length > 0) {
                    if (selector) {
                        if (this[0].previousElementSibling && $(this[0].previousElementSibling).is(selector)) {
                            return new Dom([this[0].previousElementSibling]);
                        } else {
                            return new Dom([]);
                        }
                    } else {
                        if (this[0].previousElementSibling) {
                            return new Dom([this[0].previousElementSibling]);
                        } else {
                            return new Dom([]);
                        }
                    }
                } else {
                    return new Dom([]);
                }
            },
            prevAll: function (selector) {
                var prevEls = [];
                var el = this[0];
                if (!el) {
                    return new Dom([]);
                }
                while (el.previousElementSibling) {
                    var prev = el.previousElementSibling;
                    if (selector) {
                        if ($(prev).is(selector)) {
                            prevEls.push(prev);
                        }
                    } else {
                        prevEls.push(prev);
                    }
                    el = prev;
                }
                return new Dom(prevEls);
            },
            parent: function (selector) {
                var parents = [];
                for (var i = 0; i < this.length; i++) {
                    if (this[i].parentNode !== null) {
                        if (selector) {
                            if ($(this[i].parentNode).is(selector)) {
                                parents.push(this[i].parentNode);
                            }
                        } else {
                            parents.push(this[i].parentNode);
                        }
                    }
                }
                return $($.unique(parents));
            },
            parents: function (selector) {
                var parents = [];
                for (var i = 0; i < this.length; i++) {
                    var parent = this[i].parentNode;
                    while (parent) {
                        if (selector) {
                            if ($(parent).is(selector)) {
                                parents.push(parent);
                            }
                        } else {
                            parents.push(parent);
                        }
                        parent = parent.parentNode;
                    }
                }
                return $($.unique(parents));
            },
            find: function (selector) {
                var foundElements = [];
                for (var i = 0; i < this.length; i++) {
                    var found = this[i].querySelectorAll(selector);
                    for (var j = 0; j < found.length; j++) {
                        foundElements.push(found[j]);
                    }
                }
                return new Dom(foundElements);
            },
            children: function (selector) {
                var children = [];
                for (var i = 0; i < this.length; i++) {
                    var childNodes = this[i].childNodes;

                    for (var j = 0; j < childNodes.length; j++) {
                        if (!selector) {
                            if (childNodes[j].nodeType === 1) {
                                children.push(childNodes[j]);
                            }
                        } else {
                            if (childNodes[j].nodeType === 1 && $(childNodes[j]).is(selector)) {
                                children.push(childNodes[j]);
                            }
                        }
                    }
                }

                return new Dom($.unique(children));
            },
            remove: function () {
                for (var i = 0; i < this.length; i++) {
                    if (this[i].parentNode) {
                        this[i].parentNode.removeChild(this[i]);
                    }
                }
                return this;
            },
            add: function () {
                var dom = this;
                var i, j;
                for (i = 0; i < arguments.length; i++) {
                    var toAdd = $(arguments[i]);
                    for (j = 0; j < toAdd.length; j++) {
                        dom[dom.length] = toAdd[j];
                        dom.length++;
                    }
                }
                return dom;
            },
            before: function (elm) {
                $(elm).insertBefore(this);
                return this;
            },
            after: function (elm) {
                $(elm).insertAfter(this);
                return this;
            },
            scrollTop: function (value) {
                if (!this.length) {
                    return;
                }
                var hasScrollTop = 'scrollTop' in this[0];

                if (value === undefined) {
                    return hasScrollTop ? this[0].scrollTop : this[0].pageYOffset;
                }
                return this.each(hasScrollTop ? function () {
                    this.scrollTop = value;
                } : function () {
                    this.scrollTo(this.scrollX, value);
                });
            },
            scrollLeft: function (value) {
                if (!this.length) {
                    return;
                }
                var hasScrollLeft = 'scrollLeft' in this[0];

                if (value === undefined) {
                    return hasScrollLeft ? this[0].scrollLeft : this[0].pageXOffset;
                }
                return this.each(hasScrollLeft ? function () {
                    this.scrollLeft = value;
                } : function () {
                    this.scrollTo(value, this.scrollY);
                });
            },
            contents: function () {
                return this.map(function (i, v) {
                    return slice.call(v.childNodes);
                });
            },
            nextUntil: function (selector) {
                var n = this,
                    array = [];

                while (n.length && !n.filter(selector).length) {
                    array.push(n[0]);
                    n = n.next();
                }

                return $(array);
            },
            prevUntil: function (selector) {
                var n = this,
                    array = [];

                while (n.length && !$(n).filter(selector).length) {
                    array.push(n[0]);
                    n = n.prev();
                }

                return $(array);
            },
            detach: function () {
                return this.remove();
            }
        };

        // Link to prototype
        $.fn = Dom.prototype;

        return $;
    })();

    // Export to local scope
    var $ = Dom;

    // Export to mobiscroll
    mobiscroll.$ = Dom;

    // DOM Library Utilites
    $.inArray = function (elem, array, i) {
        return emptyArray.indexOf.call(array, elem, i);
    };

    $.extend = function (target) {
        var deep,
            args = slice.call(arguments, 1);

        if (typeof target == 'boolean') {
            deep = target;
            target = args.shift();
        }

        target = target || {};

        args.forEach(function (arg) {
            extend(target, arg, deep);
        });

        return target;
    };

    $.isFunction = isFunction;

    $.isArray = function (arr) {
        return Object.prototype.toString.apply(arr) === '[object Array]';
    };

    $.isPlainObject = function (obj) {
        return isObject(obj) && obj !== null && obj !== obj.window && Object.getPrototypeOf(obj) == Object.prototype;
    };

    $.each = function (obj, callback) {
        var i, prop;

        if (!isObject(obj) || !callback) {
            return;
        }

        if ($.isArray(obj) || obj instanceof Dom) {
            // Array
            for (i = 0; i < obj.length; i++) {
                if (callback.call(obj[i], i, obj[i]) === false) {
                    break;
                }
            }
        } else {
            // Object
            for (prop in obj) {
                if (obj.hasOwnProperty(prop) && prop !== 'length') {
                    if (callback.call(obj[prop], prop, obj[prop]) === false) {
                        break;
                    }
                }
            }
        }

        return this;
    };

    $.unique = function (arr) {
        var unique = [];
        for (var i = 0; i < arr.length; i++) {
            if (unique.indexOf(arr[i]) === -1) {
                unique.push(arr[i]);
            }
        }
        return unique;
    };

    $.map = function (elements, callback) {
        var value, values = [],
            i, key;
        if (likeArray(elements)) {
            for (i = 0; i < elements.length; i++) {
                value = callback(elements[i], i);
                if (value !== null) {
                    values.push(value);
                }
            }
        } else {
            for (key in elements) {
                value = callback(elements[key], key);
                if (value !== null) {
                    values.push(value);
                }
            }
        }

        return values.length > 0 ? $.fn.concat.apply([], values) : values;
    };

    $.matches = function (element, selector) {
        if (!selector || !element || element.nodeType !== 1) {
            return false;
        }

        var matchesSelector = element.matchesSelector || element.webkitMatchesSelector || element.mozMatchesSelector || element.msMatchesSelector;

        return matchesSelector.call(element, selector);

    };

})(window, document);
/*!
 * Mobiscroll v3.0.1
 * http://mobiscroll.com
 *
 * Copyright 2010-2016, Acid Media
 * Licensed under the MIT license.
 *
 */

var mobiscroll = mobiscroll || {};

(function (window, document, undefined) {

    function testProps(props) {
        var i;
        for (i in props) {
            if (mod[props[i]] !== undefined) {
                return true;
            }
        }
        return false;
    }

    function testPrefix() {
        var prefixes = ['Webkit', 'Moz', 'O', 'ms'],
            p;

        for (p in prefixes) {
            if (testProps([prefixes[p] + 'Transform'])) {
                return '-' + prefixes[p].toLowerCase() + '-';
            }
        }
        return '';
    }

    function init(that, options, args) {
        var ret = that;

        // Init
        if (typeof options === 'object') {
            return that.each(function () {
                if (instances[this.id]) {
                    instances[this.id].destroy();
                }
                new ms.classes[options.component || 'Scroller'](this, options);
            });
        }

        // Method call
        if (typeof options === 'string') {
            that.each(function () {
                var r,
                    inst = instances[this.id];

                if (inst && inst[options]) {
                    r = inst[options].apply(this, Array.prototype.slice.call(args, 1));
                    if (r !== undefined) {
                        ret = r;
                        return false;
                    }
                }
            });
        }

        return ret;
    }

    var ms,
        platform,
        vers,
        empty = function () {},
        $ = typeof jQuery == 'undefined' ? mobiscroll.$ : jQuery,
        id = +new Date(),
        instances = {},
        extend = $.extend,
        userAgent = navigator.userAgent,
        device = userAgent.match(/Android|iPhone|iPad|iPod|Windows Phone|Windows|MSIE/i),
        mod = document.createElement('modernizr').style,
        has3d = testProps(['perspectiveProperty', 'WebkitPerspective', 'MozPerspective', 'OPerspective', 'msPerspective']),
        hasFlex = testProps(['flex', 'msFlex', 'WebkitBoxDirection']),
        prefix = testPrefix(),
        pr = prefix.replace(/^\-/, '').replace(/\-$/, '').replace('moz', 'Moz'),
        version = [];

    if (/Android/i.test(device)) {
        platform = 'android';
        vers = navigator.userAgent.match(/Android\s+([\d\.]+)/i);
        if (vers) {
            version = vers[0].replace('Android ', '').split('.');
        }
    } else if (/iPhone|iPad|iPod/i.test(device)) {
        platform = 'ios';
        vers = navigator.userAgent.match(/OS\s+([\d\_]+)/i);
        if (vers) {
            version = vers[0].replace(/_/g, '.').replace('OS ', '').split('.');
        }
    } else if (/Windows Phone/i.test(device)) {
        platform = 'wp';
    } else if (/Windows|MSIE/i.test(device)) {
        platform = 'windows';
    }

    ms = mobiscroll = {
        $: $,
        version: '3.0.1',
        util: {
            prefix: prefix,
            jsPrefix: pr,
            has3d: has3d,
            hasFlex: hasFlex,
            preventClick: function () {
                // Prevent ghost click
                ms.tapped++;
                setTimeout(function () {
                    ms.tapped--;
                }, 500);
            },
            testTouch: function (e, elm) {
                if (e.type == 'touchstart') {
                    $(elm).attr('data-touch', '1');
                } else if ($(elm).attr('data-touch')) {
                    $(elm).removeAttr('data-touch');
                    return false;
                }
                return true;
            },
            objectToArray: function (obj) {
                var arr = [],
                    i;

                for (i in obj) {
                    arr.push(obj[i]);
                }

                return arr;
            },
            arrayToObject: function (arr) {
                var obj = {},
                    i;

                if (arr) {
                    for (i = 0; i < arr.length; i++) {
                        obj[arr[i]] = arr[i];
                    }
                }

                return obj;
            },
            isNumeric: function (a) {
                return a - parseFloat(a) >= 0;
            },
            isString: function (s) {
                return typeof s === 'string';
            },
            getCoord: function (e, c, page) {
                var ev = e.originalEvent || e,
                    prop = (page ? 'page' : 'client') + c;

                // Multi touch support
                if (ev.targetTouches && ev.targetTouches[0]) {
                    return ev.targetTouches[0][prop];
                }

                if (ev.changedTouches && ev.changedTouches[0]) {
                    return ev.changedTouches[0][prop];
                }

                return e[prop];
            },
            getPosition: function (t, vertical) {
                var style = getComputedStyle(t[0]),
                    matrix,
                    px;

                $.each(['t', 'webkitT', 'MozT', 'OT', 'msT'], function (i, v) {
                    if (style[v + 'ransform'] !== undefined) {
                        matrix = style[v + 'ransform'];
                        return false;
                    }
                });
                matrix = matrix.split(')')[0].split(', ');
                px = vertical ? (matrix[13] || matrix[5]) : (matrix[12] || matrix[4]);


                return px;
            },
            constrain: function (val, min, max) {
                return Math.max(min, Math.min(val, max));
            },
            vibrate: function (time) {
                if ('vibrate' in navigator) {
                    navigator.vibrate(time || 50);
                }
            },
            throttle: function (fn, threshhold) {
                var last,
                    timer;

                threshhold = threshhold || 100;

                return function () {
                    var context = this,
                        now = +new Date(),
                        args = arguments;

                    if (last && now < last + threshhold) {
                        clearTimeout(timer);
                        timer = setTimeout(function () {
                            last = now;
                            fn.apply(context, args);
                        }, threshhold);
                    } else {
                        last = now;
                        fn.apply(context, args);
                    }
                };
            }
        },
        tapped: 0,
        autoTheme: 'mobiscroll',
        presets: {
            scroller: {},
            numpad: {},
            listview: {},
            menustrip: {}
        },
        themes: {
            form: {},
            frame: {},
            scroller: {},
            listview: {},
            menustrip: {},
            progress: {}
        },
        platform: {
            name: platform,
            majorVersion: version[0],
            minorVersion: version[1]
        },
        i18n: {},
        instances: instances,
        classes: {},
        components: {},
        settings: {},
        setDefaults: function (o) {
            extend(this.settings, o);
        },
        presetShort: function (name, c, p) {
            ms[name] = function (selector, s) {
                var inst,
                    instIds,
                    ret = {},
                    options = s || {};

                $.extend(options, {
                    preset: p === false ? undefined : name
                });

                $(selector).each(function () {
                    if (instances[this.id]) {
                        instances[this.id].destroy();
                    }

                    inst = new ms.classes[c || 'Scroller'](this, options);
                    ret[this.id] = inst;
                });

                instIds = Object.keys(ret);

                return instIds.length == 1 ? ret[instIds[0]] : ret;
            };

            this.components[name] = function (s) {
                return init(this, extend(s, {
                    component: c,
                    preset: p === false ? undefined : name
                }), arguments);
            };
        }
    };

    $.mobiscroll = ms;

    $.fn.mobiscroll = function (method) {
        extend(this, ms.components);
        return init(this, method, arguments);
    };

    ms.classes.Base = function (el, settings) {

        var lang,
            preset,
            s,
            theme,
            themeName,
            trigger,
            defaults,
            util = ms.util,
            getCoord = util.getCoord,
            that = this;

        that.settings = {};

        that._init = empty;

        that._destroy = empty;

        that._processSettings = empty;

        that.init = function (ss) {
            var key;

            // Reset settings object
            for (key in that.settings) {
                delete that.settings[key];
            }

            s = that.settings;

            // Update original user settings
            extend(settings, ss);

            // Load user defaults
            if (that._hasDef) {
                defaults = ms.settings;
            }

            // Create settings object
            extend(s, that._defaults, defaults, settings);

            // Get theme defaults
            if (that._hasTheme) {

                themeName = s.theme;

                if (themeName == 'auto' || !themeName) {
                    themeName = ms.autoTheme;
                }

                if (themeName == 'default') {
                    themeName = 'mobiscroll';
                }

                settings.theme = themeName;

                theme = ms.themes[that._class] ? ms.themes[that._class][themeName] : {};
            }

            // Get language defaults
            if (that._hasLang) {
                lang = ms.i18n[s.lang];
            }

            if (that._hasTheme) {
                trigger('onThemeLoad', {
                    lang: lang,
                    settings: settings
                });
            }

            // Update settings object
            extend(s, theme, lang, defaults, settings);

            that._processSettings();

            trigger('onProcessSettings');

            // Load preset settings
            if (that._hasPreset) {

                preset = ms.presets[that._class][s.preset];

                if (preset) {
                    preset = preset.call(el, that);
                    extend(s, preset, settings);
                }
            }

            that._init(ss);

            trigger('onInit');
        };

        that.destroy = function () {
            if (that) {
                that._destroy();
                trigger('onDestroy');

                // Delete scroller instance
                delete instances[el.id];

                that = null;
            }
        };

        /**
         * Attach tap event to the given element.
         */
        that.tap = function (el, handler, prevent, tolerance, time) {
            var startX,
                startY,
                target,
                moved,
                startTime,
                $elm = $(el);

            tolerance = tolerance || 9;

            function onStart(ev) {
                if (!target) {
                    // Can't always call preventDefault here, it kills page scroll
                    if (prevent) {
                        ev.preventDefault();
                    }
                    target = this;
                    startX = getCoord(ev, 'X');
                    startY = getCoord(ev, 'Y');
                    moved = false;
                    startTime = new Date();
                }
            }

            function onMove(ev) {
                // If movement is more than 20px, don't fire the click event handler
                if (target && !moved && (Math.abs(getCoord(ev, 'X') - startX) > tolerance || Math.abs(getCoord(ev, 'Y') - startY) > tolerance)) {
                    moved = true;
                }
            }

            function onEnd(ev) {
                if (target) {
                    if ((time && new Date() - startTime < 100) || !moved) {
                        ev.preventDefault();
                        handler.call(target, ev, that);
                    }

                    target = false;

                    util.preventClick();
                }
            }

            function onCancel() {
                target = false;
            }

            if (s.tap) {
                $elm
                    .on('touchstart.mbsc', onStart)
                    .on('touchcancel.mbsc', onCancel)
                    .on('touchmove.mbsc', onMove)
                    .on('touchend.mbsc', onEnd);
            }

            $elm.on('click.mbsc', function (ev) {
                ev.preventDefault();
                // If handler was not called on touchend, call it on click;
                handler.call(this, ev, that);
            });
        };

        /**
         * Triggers an event
         */
        that.trigger = function (name, ev) {
            var ret,
                i,
                v,
                s = [defaults, theme, preset, settings];

            for (i = 0; i < 4; i++) {
                v = s[i];
                if (v && v[name]) {
                    ret = v[name].call(el, ev || {}, that);
                }
            }

            return ret;
        };

        /**
         * Sets one ore more options.
         */
        that.option = function (opt, value) {
            var obj = {};
            if (typeof opt === 'object') {
                obj = opt;
            } else {
                obj[opt] = value;
            }
            that.init(obj);
        };

        /**
         * Returns the mobiscroll instance.
         */
        that.getInst = function () {
            return that;
        };

        settings = settings || {};
        trigger = that.trigger;

        $(el).addClass('mbsc-comp');

        // Autogenerate id
        if (!el.id) {
            el.id = 'mobiscroll' + (++id);
        }

        // Save instance
        instances[el.id] = that;
    };

    // Prevent standard behaviour on body click
    function preventClick(ev) {
        // Textarea needs the mousedown event
        if (ms.tapped && !ev.tap && !(ev.target.nodeName == 'TEXTAREA' && ev.type == 'mousedown')) {
            ev.stopPropagation();
            ev.preventDefault();
            return false;
        }
    }

    if (document.addEventListener) {
        $.each(['mouseover', 'mousedown', 'mouseup', 'click'], function (i, ev) {
            document.addEventListener(ev, preventClick, true);
        });
    }

})(window, document);
(function (undefined) {
    var ms = mobiscroll,
        $ = ms.$,
        classes = ms.classes,
        util = ms.util,
        constrain = util.constrain,
        pr = util.jsPrefix,
        pref = util.prefix,
        getCoord = util.getCoord,
        getCurrentPosition = util.getPosition,
        testTouch = util.testTouch,
        isNumeric = util.isNumeric,
        isString = util.isString,
        isIOS = /(iphone|ipod|ipad)/i.test(navigator.userAgent),
        empty = function () {},
        //transEnd = 'webkitTransitionEnd transitionend',
        raf = window.requestAnimationFrame || function (x) {
            x();
        },
        rafc = window.cancelAnimationFrame || empty;

    classes.ScrollView = function (el, settings, inherit) {
        var $btn,
            btnTimer,
            contSize,
            diffX,
            diffY,
            diff,
            dir,
            easing,
            elastic,
            endX,
            endY,
            eventObj,
            isBtn,
            lastX,
            maxScroll,
            maxSnapScroll,
            minScroll,
            move,
            moving,
            nativeScroll,
            rafID,
            //rafMoveID,
            rafRunning,
            scrolled,
            scrollDebounce,
            scrollTimer,
            snap,
            snapPoints,
            startPos,
            startTime,
            startX,
            startY,
            style,
            target,
            transTimer,
            trigger,
            vertical,
            that = this,
            currPos,
            currSnap = 0,
            currSnapDir = 1,
            s = settings,
            $elm = $(el);

        function onStart(ev) {

            trigger('onStart');

            // Better performance if there are tap events on document
            if (s.stopProp) {
                ev.stopPropagation();
            }

            if (s.prevDef || ev.type == 'mousedown') {
                // Prevent touch highlight and focus
                ev.preventDefault();
            }

            if (s.readonly || (s.lock && moving)) {
                return;
            }

            if (testTouch(ev, this) && !move) {

                if ($btn) {
                    $btn.removeClass('mbsc-btn-a');
                }

                // Highlight button
                isBtn = false;

                if (!moving) {
                    $btn = $(ev.target).closest('.mbsc-btn-e', this);

                    if ($btn.length && !$btn.hasClass('mbsc-btn-d')) {
                        isBtn = true;
                        btnTimer = setTimeout(function () {
                            $btn.addClass('mbsc-btn-a');
                        }, 100);
                    }
                }

                move = true;
                scrolled = false;
                nativeScroll = false;

                that.scrolled = moving;

                startX = getCoord(ev, 'X');
                startY = getCoord(ev, 'Y');
                endX = lastX = startX;
                diffX = 0;
                diffY = 0;
                diff = 0;

                startTime = new Date();

                startPos = +getCurrentPosition(target, vertical) || 0;

                // Stop scrolling animation, 1ms is needed for Android 4.0
                if (moving) {
                    scroll(startPos, isIOS ? 0 : 1);
                }

                if (ev.type === 'mousedown') {
                    $(document).on('mousemove', onMove).on('mouseup', onEnd);
                }
            }
        }

        function onMove(ev) {
            if (move) {
                if (s.stopProp) {
                    ev.stopPropagation();
                }

                endX = getCoord(ev, 'X');
                endY = getCoord(ev, 'Y');
                diffX = endX - startX;
                diffY = endY - startY;
                diff = vertical ? diffY : diffX;

                if (isBtn && (Math.abs(diffY) > 5 || Math.abs(diffX) > 5)) {
                    clearTimeout(btnTimer);
                    $btn.removeClass('mbsc-btn-a');
                    isBtn = false;
                }

                if (that.scrolled || (!nativeScroll && Math.abs(diff) > 5)) {

                    if (!scrolled) {
                        trigger('onGestureStart', eventObj);
                    }

                    that.scrolled = scrolled = true;

                    if (!rafRunning) {
                        rafRunning = true;
                        rafID = raf(onMoving);
                    }
                }

                if (vertical || s.scrollLock) {
                    // Always prevent native scroll, if vertical
                    ev.preventDefault();
                } else {
                    if (that.scrolled) {
                        // Prevent native scroll
                        ev.preventDefault();
                    } else if (Math.abs(diffY) > 7) {
                        nativeScroll = true;
                        that.scrolled = true;
                        $elm.trigger('touchend');
                    }
                }
            }
        }

        function onMoving() {
            //var time = new Date();

            if (maxSnapScroll) {
                diff = constrain(diff, -snap * maxSnapScroll, snap * maxSnapScroll);
            }

            scroll(constrain(startPos + diff, minScroll - elastic, maxScroll + elastic));

            //if (s.momentum) {
            //    startTime = time;
            //    lastX = endX;
            //}

            rafRunning = false;
        }

        function onEnd(ev) {
            if (move) {
                var speed,
                    time = new Date() - startTime;

                // Better performance if there are tap events on document
                if (s.stopProp) {
                    ev.stopPropagation();
                }

                rafc(rafID);
                rafRunning = false;

                if (!nativeScroll && that.scrolled) {
                    // Calculate momentum distance
                    if (s.momentum && time < 300) {
                        speed = diff / time;
                        //speed = Math.abs(lastX - endX) / time;
                        diff = Math.max(Math.abs(diff), (speed * speed) / s.speedUnit) * (diff < 0 ? -1 : 1);
                    }

                    finalize(diff);
                }

                if (isBtn) {
                    clearTimeout(btnTimer);
                    $btn.addClass('mbsc-btn-a');
                    setTimeout(function () {
                        $btn.removeClass('mbsc-btn-a');
                    }, 100);

                    if (!nativeScroll && !that.scrolled) {

                        // Prevent phantom clicks
                        //if (ev.type === 'touchend') {
                        //    util.preventClick();
                        //}

                        trigger('onBtnTap', {
                            target: $btn[0]
                        });
                    }
                }

                // Detach document events
                if (ev.type == 'mouseup') {
                    $(document).off('mousemove', onMove).off('mouseup', onEnd);
                }

                move = false;
            }
        }

        function onScroll(ev) {
            ev = ev.originalEvent || ev;

            diff = vertical ? ev.deltaY || ev.wheelDelta || ev.detail : ev.deltaX;

            trigger('onStart');

            if (s.stopProp) {
                ev.stopPropagation();
            }

            if (diff) {

                ev.preventDefault();

                //diff = diff < 0 ? 20 : -20;

                if (ev.deltaMode && ev.deltaMode == 1) {
                    diff *= 5;
                }

                diff = constrain(-diff, -20, 20);

                startPos = currPos;

                if (s.readonly || startPos + diff < minScroll || startPos + diff > maxScroll) {
                    return;
                }

                if (!scrolled) {
                    eventObj = {
                        posX: vertical ? 0 : currPos,
                        posY: vertical ? currPos : 0,
                        originX: vertical ? 0 : startPos,
                        originY: vertical ? startPos : 0,
                        direction: diff > 0 ? (vertical ? 270 : 360) : (vertical ? 90 : 180)
                    };
                    trigger('onGestureStart', eventObj);
                }

                if (!rafRunning) {
                    rafRunning = true;
                    rafID = raf(onMoving);
                }

                scrolled = true;

                clearTimeout(scrollDebounce);
                scrollDebounce = setTimeout(function () {
                    rafc(rafID);
                    rafRunning = false;
                    scrolled = false;

                    finalize(diff);
                }, 200);
            }
        }

        function finalize(diff) {
            var i,
                time,
                newPos;

            // Limit scroll to snap size
            if (maxSnapScroll) {
                diff = constrain(diff, -snap * maxSnapScroll, snap * maxSnapScroll);
            }

            // Calculate snap and limit between min and max
            newPos = constrain(Math.round((startPos + diff) / snap) * snap, minScroll, maxScroll);
            currSnap = Math.round(newPos / snap);

            // Snap to nearest element
            if (snapPoints) {
                if (diff < 0) {
                    for (i = snapPoints.length - 1; i >= 0; i--) {
                        if (Math.abs(newPos) + contSize >= snapPoints[i].breakpoint) {
                            currSnap = i;
                            currSnapDir = 2;
                            newPos = snapPoints[i].snap2;
                            break;
                        }
                    }
                } else if (diff >= 0) {
                    for (i = 0; i < snapPoints.length; i++) {
                        if (Math.abs(newPos) <= snapPoints[i].breakpoint) {
                            currSnap = i;
                            currSnapDir = 1;
                            newPos = snapPoints[i].snap1;
                            break;
                        }
                    }
                }
                newPos = constrain(newPos, minScroll, maxScroll);
            }

            time = s.time || (currPos < minScroll || currPos > maxScroll ? 1000 : Math.max(1000, Math.abs(newPos - currPos) * s.timeUnit));

            eventObj.destinationX = vertical ? 0 : newPos;
            eventObj.destinationY = vertical ? newPos : 0;
            eventObj.duration = time;
            eventObj.transitionTiming = easing;

            trigger('onGestureEnd', eventObj);

            // Scroll to the calculated position
            scroll(newPos, time);
        }

        function scroll(pos, time, tap, callback) {
            var changed = pos != currPos,
                anim = time > 1,
                done = function () {
                    clearInterval(scrollTimer);
                    clearTimeout(transTimer);
                    //rafc(rafMoveID);

                    moving = false;
                    currPos = pos;
                    eventObj.posX = vertical ? 0 : pos;
                    eventObj.posY = vertical ? pos : 0;

                    if (changed) {
                        trigger('onMove', eventObj);
                    }

                    if (anim) {
                        //that.scrolled = false;
                        trigger('onAnimationEnd', eventObj);
                    }

                    if (callback) {
                        callback();
                    }
                };

            eventObj = {
                posX: vertical ? 0 : currPos,
                posY: vertical ? currPos : 0,
                originX: vertical ? 0 : startPos,
                originY: vertical ? startPos : 0,
                direction: pos - currPos > 0 ? (vertical ? 270 : 360) : (vertical ? 90 : 180)
            };

            currPos = pos;

            if (anim) {
                eventObj.destinationX = vertical ? 0 : pos;
                eventObj.destinationY = vertical ? pos : 0;
                eventObj.duration = time;
                eventObj.transitionTiming = easing;

                trigger('onAnimationStart', eventObj);
            }

            style[pr + 'Transition'] = time ? pref + 'transform ' + Math.round(time) + 'ms ' + easing : '';
            style[pr + 'Transform'] = 'translate3d(' + (vertical ? '0,' + pos + 'px,' : pos + 'px,' + '0,') + '0)';

            if ((!changed && !moving) || !time || time <= 1) {
                done();
            } else if (time) {
                moving = !tap;

                clearInterval(scrollTimer);
                scrollTimer = setInterval(function () {
                    //rafMoveID = raf(function () {
                    var p = +getCurrentPosition(target, vertical) || 0;
                    eventObj.posX = vertical ? 0 : p;
                    eventObj.posY = vertical ? p : 0;
                    trigger('onMove', eventObj);
                    // Trigger done if close to the end
                    if (Math.abs(p - pos) < 2) {
                        done();
                    }
                    //});
                }, 100);

                clearTimeout(transTimer);
                transTimer = setTimeout(function () {
                    done();
                    //style[pr + 'Transition'] = '';
                }, time);

                // target.off(transEnd).on(transEnd, function (e) {
                //     if (e.target === target[0]) {
                //         target.off(transEnd);
                //         style[pr + 'Transition'] = '';
                //         done();
                //     }
                // });
            }

            if (s.sync) {
                s.sync(pos, time, easing);
            }
        }

        // Call the parent constructor
        classes.Base.call(this, el, settings, true);

        that.scrolled = false;

        /**
         * Scroll to the given position or element
         */
        that.scroll = function (pos, time, tap, callback) {
            // If position is not numeric, scroll to element
            if (!isNumeric(pos)) {
                pos = Math.ceil(($(pos, el).length ? Math.round(target.offset()[dir] - $(pos, el).offset()[dir]) : currPos) / snap) * snap;
            } else {
                pos = Math.round(pos / snap) * snap;
            }

            pos = constrain(pos, minScroll, maxScroll);

            currSnap = Math.round(pos / snap);

            startPos = currPos;

            scroll(pos, time, tap, callback);
        };

        that.refresh = function (noScroll) {
            var tempScroll;

            contSize = s.contSize === undefined ? vertical ? $elm.height() : $elm.width() : s.contSize;
            minScroll = s.minScroll === undefined ? Math.min(0, vertical ? contSize - target.height() : contSize - target.width()) : s.minScroll;
            maxScroll = s.maxScroll === undefined ? 0 : s.maxScroll;
            snapPoints = null;

            if (!vertical && s.rtl) {
                tempScroll = maxScroll;
                maxScroll = -minScroll;
                minScroll = -tempScroll;
            }

            if (isString(s.snap)) {
                snapPoints = [];
                target.find(s.snap).each(function () {
                    var offset = vertical ? this.offsetTop : this.offsetLeft,
                        size = vertical ? this.offsetHeight : this.offsetWidth;

                    snapPoints.push({
                        breakpoint: offset + size / 2,
                        snap1: -offset,
                        snap2: contSize - offset - size
                    });
                });
            }

            snap = isNumeric(s.snap) ? s.snap : 1;
            maxSnapScroll = s.snap ? s.maxSnapScroll : 0;
            easing = s.easing;
            elastic = s.elastic ? (isNumeric(s.snap) ? snap : (isNumeric(s.elastic) ? s.elastic : 0)) : 0; // && s.snap ? snap : 0;

            if (currPos === undefined) {
                currPos = s.initialPos;
                currSnap = Math.round(currPos / snap);
            }

            if (!noScroll) {
                that.scroll(s.snap ? (snapPoints ? snapPoints[currSnap]['snap' + currSnapDir] : (currSnap * snap)) : currPos);
            }
        };

        that._processSettings = function () {
            vertical = s.axis == 'Y';
            dir = vertical ? 'top' : 'left';
            target = s.moveElement || $elm.children().eq(0);
            style = target[0].style;
        };

        that._init = function () {
            that.refresh();

            $elm.on('touchstart mousedown', onStart)
                .on('touchmove', onMove)
                .on('touchend touchcancel', onEnd);

            if (s.mousewheel) {
                $elm.on('wheel mousewheel', onScroll);
            }

            if (el.addEventListener) {
                el.addEventListener('click', function (ev) {
                    if (that.scrolled) {
                        that.scrolled = false;
                        ev.stopPropagation();
                        ev.preventDefault();
                    }
                }, true);
            }

            //el.addEventListener('touchend', function (ev) {
            //    if (scrolled) {
            //        ev.stopPropagation();
            //    }
            //}, true);
        };

        /**
         * Destroy
         */
        that._destroy = function () {
            clearInterval(scrollTimer);

            $elm.off('touchstart mousedown', onStart)
                .off('touchmove', onMove)
                .off('touchend touchcancel', onEnd)
                .off('wheel mousewheel', onScroll);
        };

        // Constructor

        s = that.settings;
        trigger = that.trigger;

        if (!inherit) {
            that.init(settings);
        }
    };

    classes.ScrollView.prototype = {
        _class: 'scrollview',
        _defaults: {
            speedUnit: 0.0022,
            //timeUnit: 0.8,
            timeUnit: 3,
            initialPos: 0,
            axis: 'Y',
            //easing: 'ease-out',
            easing: 'cubic-bezier(0.190, 1.000, 0.220, 1.000)',
            stopProp: true,
            momentum: true,
            mousewheel: true,
            elastic: true
        }
    };

    ms.presetShort('scrollview', 'ScrollView', false);
})();
(function (window, document, undefined) {
    var $activeElm,
        preventShow,
        ms = mobiscroll,
        $ = ms.$,
        platform = ms.platform,
        util = ms.util,
        constrain = util.constrain,
        isString = util.isString,
        getCoord = util.getCoord,
        needsFixed = /(iphone|ipod)/i.test(navigator.userAgent) && platform.majorVersion >= 7,
        isIOS8 = platform.name == 'ios' && platform.majorVersion == 8,
        animEnd = 'webkitAnimationEnd.mbsc animationend.mbsc',
        empty = function () {},
        prevdef = function (ev) {
            ev.preventDefault();
        };

    ms.classes.Frame = function (el, settings, inherit) {
        var $ariaDiv,
            $ctx,
            $header,
            $lock,
            $markup,
            $overlay,
            $persp,
            $popup,
            $wnd,
            $wrapper,
            buttons,
            btn,
            doAnim,
            hasContext,
            isModal,
            isInserted,
            lockClass,
            markup,
            modalWidth,
            modalHeight,
            needsDimensions,
            needsLock,
            overlay,
            popup,
            posEvents,
            preventPos,
            s,
            scrollLeft,
            scrollLock,
            scrollTop,
            trigger,
            wndWidth,
            wndHeight,

            that = this,
            $elm = $(el),
            elmList = [],
            posDebounce = {};

        function onBtnStart(ev) {
            // Can't call preventDefault here, it kills page scroll
            if (btn) {
                btn.removeClass('mbsc-fr-btn-a');
            }

            btn = $(this);

            // Active button
            if (!btn.hasClass('mbsc-fr-btn-d') && !btn.hasClass('mbsc-fr-btn-nhl')) {
                btn.addClass('mbsc-fr-btn-a');
            }

            if (ev.type === 'mousedown') {
                $(document).on('mouseup', onBtnEnd);
            } else if (ev.type === 'pointerdown') {
                $(document).on('pointerup', onBtnEnd);
            }
        }

        function onBtnEnd(ev) {
            if (btn) {
                btn.removeClass('mbsc-fr-btn-a');
                btn = null;
            }

            if (ev.type === 'mouseup') {
                $(document).off('mouseup', onBtnEnd);
            } else if (ev.type === 'pointerup') {
                $(document).off('pointerup', onBtnEnd);
            }
        }

        function onWndKeyDown(ev) {
            if (ev.keyCode == 13) {
                that.select();
            } else if (ev.keyCode == 27) {
                that.cancel();
            }
        }

        function onShow(prevFocus) {
            if (!prevFocus) {
                overlay.focus();
            }
            that.ariaMessage(s.ariaMessage);
        }

        function onHide(prevAnim) {
            var $activeEl = $activeElm,
                focus = s.focusOnClose;

            that._markupRemove();

            $markup.remove();

            // The follwing should be done only if no other
            // instance was opened during the hide animation
            if (!ms.activeInstance) {
                if (isModal) {
                    $lock.removeClass(lockClass);
                    if (needsLock) {
                        $ctx.css({
                            top: '',
                            left: ''
                        });
                        $wnd.scrollLeft(scrollLeft);
                        $wnd.scrollTop(scrollTop);
                    }
                }

                if (!prevAnim) {
                    if (!$activeEl) {
                        $activeEl = $elm;
                    }
                    setTimeout(function () {
                        if (focus === undefined || focus === true) {
                            preventShow = true;
                            $activeEl[0].focus();
                        } else if (focus) {
                            $(focus)[0].focus();
                        }
                    }, 200);
                }
            }

            $activeElm = null;

            that._isVisible = false;

            isInserted = false;

            trigger('onHide');
        }

        function onPosition(ev) {
            clearTimeout(posDebounce[ev.type]);
            posDebounce[ev.type] = setTimeout(function () {
                var h,
                    isScroll = ev.type == 'scroll';

                if (isScroll && !scrollLock) {
                    return;
                }

                that.position(!isScroll);

                if (ev.type == 'orientationchange') {
                    // Trigger reflow
                    popup.style.display = 'none';
                    h = popup.offsetHeight;
                    popup.style.display = '';
                }
            }, 200);
        }

        function onFocus(ev) {
            if (ev.target.nodeType && !popup.contains(ev.target)) {
                popup.focus();
            }
        }

        function hideKeyBoard() {
            if ($(document.activeElement).is('input,textarea')) {
                document.activeElement.blur();
            }
        }

        function show(beforeShow, $elm) {
            if (beforeShow) {
                beforeShow();
            }

            if (that.show() !== false) {
                $activeElm = $elm;

                setTimeout(function () {
                    preventShow = false;
                }, 300); // With jQuery < 1.9 focus is fired twice in IE
            }
        }

        function set() {
            that._fillValue();
            trigger('onSet', {
                valueText: that._value
            });
        }

        function cancel() {
            trigger('onCancel', {
                valueText: that._value
            });
        }

        function clear() {
            that.setVal(null, true);
        }

        // Call the parent constructor
        ms.classes.Base.call(this, el, settings, true);

        /**
         * Positions the scroller on the screen.
         */
        that.position = function (check) {
            var anchor,
                anchorWidth,
                anchorHeight,
                anchorPos,
                anchorTop,
                anchorLeft,
                arrow,
                arrowWidth,
                arrowHeight,
                docHeight,
                docWidth,
                newHeight,
                newWidth,
                width,
                top,
                left,
                css = {},
                scrollLeft = 0,
                scrollTop = 0,
                minWidth = 0,
                totalWidth = 0;

            if (preventPos || !isInserted) {
                return;
            }

            that._position($markup);

            newHeight = markup.offsetHeight;
            newWidth = markup.offsetWidth;

            if (wndWidth === newWidth && wndHeight === newHeight && check) {
                return;
            }

            if (that._isFullScreen || /top|bottom/.test(s.display)) {
                // Set width, if document is larger than viewport, needs to be set before onPosition (for calendar)
                $popup.width(newWidth);
            }

            if (trigger('onPosition', {
                    target: markup,
                    windowWidth: newWidth,
                    windowHeight: newHeight
                }) === false || !isModal) {
                return;
            }

            // Set / unset liquid layout based on screen width, but only if not set explicitly by the user
            // if (that._isLiquid && s.layout !== 'liquid') {
            //     if (newWidth < 415) {
            //         $markup.addClass('mbsc-fr-liq');
            //     } else {
            //         $markup.removeClass('mbsc-fr-liq');
            //     }
            // }

            // Call position for nested mobiscroll components
            $('.mbsc-comp', $markup).each(function () {
                var inst = ms.instances[this.id];
                if (inst && inst !== that && inst.position) {
                    inst.position();
                }
            });

            if (!that._isFullScreen && /center|bubble/.test(s.display)) {
                $('.mbsc-w-p', $markup).each(function () {
                    // Need fractional values here, so offsetWidth is not ok
                    width = this.getBoundingClientRect().width;
                    totalWidth += width;
                    minWidth = (width > minWidth) ? width : minWidth;
                });

                $wrapper.css({
                    'width': totalWidth > newWidth ? minWidth : totalWidth,
                    'white-space': totalWidth > newWidth ? '' : 'nowrap'
                });
            }

            modalWidth = popup.offsetWidth;
            modalHeight = popup.offsetHeight;

            that.scrollLock = scrollLock = modalHeight <= newHeight && modalWidth <= newWidth;

            if (needsDimensions) {
                scrollLeft = $wnd.scrollLeft();
                scrollTop = $wnd.scrollTop();
            }

            if (s.display == 'center') {
                left = Math.max(0, scrollLeft + (newWidth - modalWidth) / 2);
                top = Math.max(0, scrollTop + (newHeight - modalHeight) / 2);
            } else if (s.display == 'bubble') {
                anchor = s.anchor === undefined ? $elm : $(s.anchor);

                arrow = $('.mbsc-fr-arr-i', $markup)[0];
                anchorPos = anchor.offset();
                anchorTop = anchorPos.top + (hasContext ? scrollTop - $ctx.offset().top : 0);
                anchorLeft = anchorPos.left + (hasContext ? scrollLeft - $ctx.offset().left : 0);

                anchorWidth = anchor[0].offsetWidth;
                anchorHeight = anchor[0].offsetHeight;

                arrowWidth = arrow.offsetWidth;
                arrowHeight = arrow.offsetHeight;

                // Horizontal positioning
                left = constrain(anchorLeft - (modalWidth - anchorWidth) / 2, scrollLeft + 8, scrollLeft + newWidth - modalWidth - 8);

                // Vertical positioning
                // Above the input
                top = anchorTop - modalHeight - arrowHeight / 2;
                // If doesn't fit above or the input is out of the screen
                if ((top < scrollTop) || (anchorTop > scrollTop + newHeight)) {
                    $popup.removeClass('mbsc-fr-bubble-top').addClass('mbsc-fr-bubble-bottom');
                    // Below the input
                    top = anchorTop + anchorHeight + arrowHeight / 2;
                } else {
                    $popup.removeClass('mbsc-fr-bubble-bottom').addClass('mbsc-fr-bubble-top');
                }

                // Set arrow position
                $('.mbsc-fr-arr', $markup).css({
                    left: constrain(anchorLeft + anchorWidth / 2 - (left + (modalWidth - arrowWidth) / 2), 0, arrowWidth)
                });

            } else {
                left = scrollLeft;
                top = s.display == 'top' ? scrollTop : Math.max(0, scrollTop + newHeight - modalHeight);
            }

            if (needsDimensions) {
                // If top + modal height > doc height, increase doc height
                docHeight = Math.max(top + modalHeight, hasContext ? $ctx[0].scrollHeight : $(document).height());
                docWidth = Math.max(left + modalWidth, hasContext ? $ctx[0].scrollWidth : $(document).width());
                $persp.css({
                    width: docWidth,
                    height: docHeight
                });

                // Check if scroll needed
                if (s.scroll && s.display == 'bubble' && ((top + modalHeight + 8 > scrollTop + newHeight) || (anchorTop > scrollTop + newHeight) || (anchorTop + anchorHeight < scrollTop))) {
                    preventPos = true;
                    setTimeout(function () {
                        preventPos = false;
                    }, 300);
                    $wnd.scrollTop(Math.min(anchorTop, top + modalHeight - newHeight + 8, docHeight - newHeight));
                }
            }

            css.top = top;
            css.left = left;

            $popup.css(css);

            wndWidth = newWidth;
            wndHeight = newHeight;
        };

        /**
         * Show mobiscroll on focus and click event of the parameter.
         * @param {HTMLElement} elm - Events will be attached to this element.
         * @param {Function} [beforeShow=undefined] - Optional function to execute before showing mobiscroll.
         */
        that.attachShow = function (elm, beforeShow) {
            var $label,
                $elm = $(elm),
                readOnly = $elm.prop('readonly');

            if (s.display !== 'inline') {
                if ((s.showOnFocus || s.showOnTap) && $elm.is('input,select')) {
                    $elm.prop('readonly', true).on('mousedown.mbsc', function (ev) {
                        // Prevent input to get focus on tap (virtual keyboard pops up on some devices)
                        ev.preventDefault();
                    }).on('focus.mbsc', function () {
                        if (that._isVisible) {
                            // Don't allow input focus if mobiscroll is being opened
                            this.blur();
                        }
                    });

                    $label = $('label[for="' + $elm.attr('id') + '"]');

                    if (!$label.length) {
                        $label = $elm.closest('label');
                    }
                }

                if ($elm.is('select')) {
                    return;
                }

                if (s.showOnFocus) {
                    $elm.on('focus.mbsc', function () {
                        if (!preventShow) {
                            show(beforeShow, $elm);
                        }
                    });
                }

                if (s.showOnTap) {
                    $elm.on('keydown.mbsc', function (ev) {
                        if (ev.keyCode == 32 || ev.keyCode == 13) { // Space or Enter
                            ev.preventDefault();
                            ev.stopPropagation();
                            show(beforeShow, $elm);
                        }
                    });

                    that.tap($elm, function () {
                        show(beforeShow, $elm);
                    });

                    if ($label && $label.length) {
                        that.tap($label, function () {
                            show(beforeShow, $elm);
                        });
                    }
                }

                elmList.push({
                    readOnly: readOnly,
                    el: $elm,
                    lbl: $label
                });
            }
        };

        /**
         * Set button handler.
         */
        that.select = function () {
            if (isModal) {
                that.hide(false, 'set', false, set);
            } else {
                set();
            }
        };

        /**
         * Cancel and hide the scroller instance.
         */
        that.cancel = function () {
            if (isModal) {
                that.hide(false, 'cancel', false, cancel);
            } else {
                cancel();
            }
        };

        /**
         * Clear button handler.
         */
        that.clear = function () {
            that._clearValue();
            trigger('onClear');
            if (isModal && that._isVisible && !that.live) {
                that.hide(false, 'clear', false, clear);
            } else {
                clear();
            }
        };

        /**
         * Enables the scroller and the associated input.
         */
        that.enable = function () {
            s.disabled = false;
            if (that._isInput) {
                $elm.prop('disabled', false);
            }
        };

        /**
         * Disables the scroller and the associated input.
         */
        that.disable = function () {
            s.disabled = true;
            if (that._isInput) {
                $elm.prop('disabled', true);
            }
        };

        /**
         * Shows the scroller instance.
         * @param {Boolean} prevAnim - Prevent animation if true
         * @param {Boolean} prevFocus - Prevent focusing if true
         */
        that.show = function (prevAnim, prevFocus) {
            var hasButtons,
                html;

            if (s.disabled || that._isVisible) {
                return;
            }

            // Parse value from input
            that._readValue();

            if (trigger('onBeforeShow') === false) {
                return false;
            }

            doAnim = s.animate;
            buttons = s.buttons || [];

            needsDimensions = hasContext || s.display == 'bubble';
            needsLock = needsFixed && !needsDimensions;

            hasButtons = buttons.length > 0;

            if (doAnim !== false) {
                if (s.display == 'top') {
                    doAnim = 'slidedown';
                } else if (s.display == 'bottom') {
                    doAnim = 'slideup';
                } else if (s.display == 'center' || s.display == 'bubble') {
                    doAnim = s.animate || 'pop';
                }
            }

            if (isModal) {
                lockClass = 'mbsc-fr-lock' + (needsLock ? ' mbsc-fr-lock-ios' : '') + (hasContext ? ' mbsc-fr-lock-ctx' : '');
                scrollTop = Math.max(0, $wnd.scrollTop());
                scrollLeft = Math.max(0, $wnd.scrollLeft());
                wndWidth = 0;
                wndHeight = 0;

                if (needsLock && !$lock.hasClass('mbsc-fr-lock')) {
                    //$lock.scrollTop(0);
                    $ctx.css({
                        top: -scrollTop + 'px',
                        left: -scrollLeft + 'px'
                    });
                }

                $lock.addClass(lockClass);

                // Hide virtual keyboard
                hideKeyBoard();

                // Hide active instance
                if (ms.activeInstance) {
                    ms.activeInstance.hide();
                }

                // Set active instance
                ms.activeInstance = that;
            }

            // Create wheels containers
            html = '<div lang="' + s.lang + '" class="mbsc-fr mbsc-no-touch mbsc-' + s.theme + (s.baseTheme ? ' mbsc-' + s.baseTheme : '') + ' mbsc-fr-' + s.display + ' ' +
                (s.cssClass || '') + ' ' +
                (s.compClass || '') +
                (that._isLiquid ? ' mbsc-fr-liq' : '') +
                (needsLock ? ' mbsc-platform-ios' : '') +
                (hasButtons ? (buttons.length >= 3 ? ' mbsc-fr-btn-block ' : '') : ' mbsc-fr-nobtn') + '">' +
                (isModal ? '<div class="mbsc-fr-persp"><div class="mbsc-fr-overlay"></div><div role="dialog" tabindex="-1" class="mbsc-fr-scroll">' : '') + // Overlay
                '<div class="mbsc-fr-popup' +
                (s.rtl ? ' mbsc-rtl' : ' mbsc-ltr') +
                (s.headerText ? ' mbsc-fr-has-hdr' : '') +
                '">' + // Popup
                (s.display === 'bubble' ? '<div class="mbsc-fr-arr-w"><div class="mbsc-fr-arr-i"><div class="mbsc-fr-arr"></div></div></div>' : '') + // Bubble arrow
                '<div class="mbsc-fr-w">' + // Popup content
                '<div aria-live="assertive" class="mbsc-fr-aria mbsc-fr-hdn"></div>' +
                (s.headerText ? '<div class="mbsc-fr-hdr">' + (isString(s.headerText) ? s.headerText : '') + '</div>' : '') + // Header
                '<div class="mbsc-fr-c">'; // Wheel group container

            html += that._generateContent();

            html += '</div>';

            if (hasButtons) {
                html += '<div class="mbsc-fr-btn-cont">';
                $.each(buttons, function (i, b) {
                    b = isString(b) ? that.buttons[b] : b;

                    if (b.handler === 'set') {
                        b.parentClass = 'mbsc-fr-btn-s';
                    }

                    if (b.handler === 'cancel') {
                        b.parentClass = 'mbsc-fr-btn-c';
                    }

                    html += '<div' + (s.btnWidth ? ' style="width:' + (100 / buttons.length) + '%"' : '') + ' class="mbsc-fr-btn-w ' + (b.parentClass || '') + '"><div tabindex="0" role="button" class="mbsc-fr-btn' + i + ' mbsc-fr-btn-e ' + (b.cssClass === undefined ? s.btnClass : b.cssClass) + (b.icon ? ' mbsc-ic mbsc-ic-' + b.icon : '') + '">' + (b.text || '') + '</div></div>';
                });
                html += '</div>';
            }
            html += '</div></div></div></div>' + (isModal ? '</div></div>' : '');

            $markup = $(html);
            $persp = $('.mbsc-fr-persp', $markup);
            $overlay = $('.mbsc-fr-scroll', $markup);
            $wrapper = $('.mbsc-fr-w', $markup);
            $header = $('.mbsc-fr-hdr', $markup);
            $popup = $('.mbsc-fr-popup', $markup);
            $ariaDiv = $('.mbsc-fr-aria', $markup);

            markup = $markup[0];
            overlay = $overlay[0];
            popup = $popup[0];

            that._markup = $markup;
            that._header = $header;
            that._isVisible = true;

            posEvents = 'orientationchange resize';

            that._markupReady($markup);

            trigger('onMarkupReady', {
                target: markup
            });

            // Attach events
            if (isModal) {
                // Enter / ESC
                $(window).on('keydown', onWndKeyDown);

                // Prevent scroll if not specified otherwise
                if (s.scrollLock) {
                    $markup.on('touchmove mousewheel wheel', function (ev) {
                        if (scrollLock) {
                            ev.preventDefault();
                        }
                    });
                }

                if (s.focusTrap) {
                    $wnd.on('focusin', onFocus);
                }

                if (s.closeOnOverlayTap) {
                    var moved,
                        target,
                        startX,
                        startY;

                    $overlay
                        .on('touchstart mousedown', function (ev) {
                            if (!target && ev.target == $overlay[0]) {
                                target = true;
                                moved = false;
                                startX = getCoord(ev, 'X');
                                startY = getCoord(ev, 'Y');
                            }
                        })
                        .on('touchmove mousemove', function (ev) {
                            if (target && !moved && (Math.abs(getCoord(ev, 'X') - startX) > 9 || Math.abs(getCoord(ev, 'Y') - startY) > 9)) {
                                moved = true;
                            }
                        })
                        .on('touchcancel', function () {
                            target = false;
                        })
                        .on('touchend touchcancel mouseup', function (ev) {
                            if (target && !moved) {
                                that.cancel();
                                if (ev.type != 'mouseup') {
                                    util.preventClick();
                                }
                            }
                            target = false;
                        });
                }

                if (needsDimensions) {
                    posEvents += ' scroll';
                }
            }

            // Wait for the toolbar and addressbar to appear on iOS
            setTimeout(function () {
                // Show
                if (isModal) {
                    $markup.appendTo($ctx);
                } else if ($elm.is('div') && !that._hasContent) {
                    // Insert inside the element on which was initialized
                    $elm.empty().append($markup);
                } else {
                    // Insert after the element
                    if ($elm.hasClass('mbsc-control')) {
                        var $wrap = $elm.closest('.mbsc-control-w');
                        $markup.insertAfter($wrap);
                        if ($wrap.hasClass('mbsc-select')) {
                            $wrap.addClass('mbsc-select-inline');
                        }
                    } else {
                        $markup.insertAfter($elm);
                    }
                }

                isInserted = true;

                that._markupInserted($markup);

                trigger('onMarkupInserted', {
                    target: markup
                });

                $markup
                    .on('selectstart mousedown', prevdef) // Prevents blue highlight on Android and text selection in IE
                    .on('click', '.mbsc-fr-btn-e', prevdef)
                    .on('keydown', '.mbsc-fr-btn-e', function (ev) {
                        if (ev.keyCode == 32) { // Space
                            ev.preventDefault();
                            ev.stopPropagation();
                            this.click();
                        }
                    })
                    .on('keydown', function (ev) { // Trap focus inside modal
                        if (ev.keyCode == 32) { // Space
                            ev.preventDefault();
                        } else if (ev.keyCode == 9 && isModal && s.focusTrap) { // Tab
                            var $focusable = $markup.find('[tabindex="0"]').filter(function () {
                                    return this.offsetWidth > 0 || this.offsetHeight > 0;
                                }),
                                index = $focusable.index($(':focus', $markup)),
                                i = $focusable.length - 1,
                                target = 0;

                            if (ev.shiftKey) {
                                i = 0;
                                target = -1;
                            }

                            if (index === i) {
                                $focusable.eq(target)[0].focus();
                                ev.preventDefault();
                            }
                        }
                    })
                    .on('touchstart mousedown pointerdown', '.mbsc-fr-btn-e', onBtnStart)
                    .on('touchend', '.mbsc-fr-btn-e', onBtnEnd)
                    .on('touchstart', function () {
                        $markup.removeClass('mbsc-no-touch');
                    });

                $('input,select,textarea', $markup).on('selectstart mousedown', function (ev) {
                    ev.stopPropagation();
                }).on('keydown', function (ev) {
                    if (ev.keyCode == 32) { // Space
                        ev.stopPropagation();
                    }
                });

                // Init buttons
                $.each(buttons, function (i, b) {
                    that.tap($('.mbsc-fr-btn' + i, $markup), function (ev) {
                        b = isString(b) ? that.buttons[b] : b;
                        (isString(b.handler) ? that.handlers[b.handler] : b.handler).call(this, ev, that);
                    }, true);
                });

                that._attachEvents($markup);

                // Set position
                that.position();

                $wnd.on(posEvents, onPosition);

                if (isModal) {
                    if (doAnim && !prevAnim) {
                        $markup.addClass('mbsc-anim-in mbsc-anim-trans mbsc-anim-trans-' + doAnim).on(animEnd, function () {
                            $markup
                                .off(animEnd)
                                .removeClass('mbsc-anim-in mbsc-anim-trans mbsc-anim-trans-' + doAnim)
                                .find('.mbsc-fr-popup')
                                .removeClass('mbsc-anim-' + doAnim);
                            onShow(prevFocus);
                        }).find('.mbsc-fr-popup').addClass('mbsc-anim-' + doAnim);
                    } else {
                        onShow(prevFocus);
                    }
                }

                trigger('onShow', {
                    target: markup,
                    valueText: that._tempValue
                });

            }, needsLock ? 100 : 0);
        };

        /**
         * Hides the scroller instance.
         */
        that.hide = function (prevAnim, btn, force, callback) {
            // If onClose handler returns false, prevent hide
            if (!that._isVisible || (!force && !that._isValid && btn == 'set') || (!force && trigger('onBeforeClose', {
                    valueText: that._tempValue,
                    button: btn
                }) === false)) {
                return false;
            }

            // Hide wheels and overlay
            if ($markup) {
                // If mbsc-anim-trans class was not removed, means that there was no animation
                if (isModal && doAnim && !prevAnim && !$markup.hasClass('mbsc-anim-trans')) {
                    $markup.addClass('mbsc-anim-out mbsc-anim-trans mbsc-anim-trans-' + doAnim).on(animEnd, function () {
                        $markup.off(animEnd);
                        onHide(prevAnim);
                    }).find('.mbsc-fr-popup').addClass('mbsc-anim-' + doAnim);
                } else {
                    onHide(prevAnim);
                }

                that._detachEvents($markup);

                // Stop positioning on window resize
                $wnd
                    .off(posEvents, onPosition)
                    .off('focusin', onFocus);
            }

            if (isModal) {
                hideKeyBoard();
                $(window).off('keydown', onWndKeyDown);
                delete ms.activeInstance;
            }

            if (callback) {
                callback();
            }

            trigger('onClose', {
                valueText: that._value
            });

        };

        that.ariaMessage = function (txt) {
            $ariaDiv.html('');
            setTimeout(function () {
                $ariaDiv.html(txt);
            }, 100);
        };

        /**
         * Return true if the scroller is currently visible.
         */
        that.isVisible = function () {
            return that._isVisible;
        };

        // Protected functions to override

        that.setVal = empty;

        that.getVal = empty;

        that._generateContent = empty;

        that._attachEvents = empty;

        that._detachEvents = empty;

        that._readValue = empty;

        that._clearValue = empty;

        that._fillValue = empty;

        that._markupReady = empty;

        that._markupInserted = empty;

        that._markupRemove = empty;

        that._position = empty;

        that.__processSettings = empty;

        that.__init = empty;

        // Generic frame functions

        /**
         * Destroys the mobiscroll instance.
         */
        that._destroy = function () {
            // Force hide without animation
            that.hide(true, false, true);

            $elm.off('.mbsc');

            // Remove all events from elements
            $.each(elmList, function (i, v) {
                v.el.off('.mbsc').prop('readonly', v.readOnly);
                if (v.lbl) {
                    v.lbl.off('.mbsc');
                }
            });
        };

        that._processSettings = function () {
            var b, i;

            that.__processSettings();

            // Add default buttons
            s.buttons = s.buttons || (s.display !== 'inline' ? ['set', 'cancel'] : []);

            // Hide header text in inline mode by default
            s.headerText = s.headerText === undefined ? (s.display !== 'inline' ? '{value}' : false) : s.headerText;

            buttons = s.buttons || [];
            isModal = s.display !== 'inline';
            hasContext = s.context != 'body';
            $ctx = $(s.context);
            $lock = hasContext ? $ctx : $('body,html');

            that._isLiquid = (s.layout || (/top|bottom|inline/.test(s.display) ? 'liquid' : '')) === 'liquid';
            that._window = $wnd = $(hasContext ? s.context : window);
            that._context = $ctx;
            that.live = true;

            // If no set button is found, live mode is activated
            for (i = 0; i < buttons.length; i++) {
                b = buttons[i];
                if (b == 'ok' || b == 'set' || b.handler == 'set') {
                    that.live = false;
                }
            }

            that.buttons.set = {
                text: s.setText,
                handler: 'set'
            };

            that.buttons.cancel = {
                text: (that.live) ? s.closeText : s.cancelText,
                handler: 'cancel'
            };

            that.buttons.clear = {
                text: s.clearText,
                handler: 'clear'
            };

            that._isInput = $elm.is('input');
        };

        /**
         * Scroller initialization.
         */
        that._init = function () {

            if (that._isVisible) {
                that.hide(true, false, true);
            }

            // Unbind all events (if re-init)
            $elm.off('.mbsc');

            that.__init();

            if (isModal) {
                that._readValue();
                if (!that._hasContent) {
                    that.attachShow($elm);
                }
            } else {
                that.show();
            }

            $elm.on('change.mbsc', function () {
                if (!that._preventChange) {
                    that.setVal($elm.val(), true, false);
                }
                that._preventChange = false;
            });
        };

        that.buttons = {};
        that.handlers = {
            set: that.select,
            cancel: that.cancel,
            clear: that.clear
        };

        that._value = null;

        that._isValid = true;
        that._isVisible = false;

        // Constructor

        s = that.settings;
        trigger = that.trigger;

        if (!inherit) {
            that.init(settings);
        }
    };

    ms.classes.Frame.prototype._defaults = {
        // Localization
        lang: 'en',
        setText: 'Set',
        selectedText: '{count} selected',
        closeText: 'Close',
        cancelText: 'Cancel',
        clearText: 'Clear',
        // Options
        context: 'body',
        disabled: false,
        closeOnOverlayTap: true,
        showOnFocus: false,
        showOnTap: true,
        display: 'center',
        scroll: true,
        scrollLock: true,
        tap: true,
        btnClass: 'mbsc-fr-btn',
        btnWidth: true,
        focusTrap: true,
        focusOnClose: !isIOS8 // Temporary for iOS8
    };

    ms.themes.frame.mobiscroll = {
        headerText: false,
        btnWidth: false
    };

    ms.themes.scroller.mobiscroll = $.extend({}, ms.themes.frame.mobiscroll, {
        rows: 5,
        showLabel: false,
        selectedLineBorder: 1,
        weekDays: 'min',
        checkIcon: 'ion-ios7-checkmark-empty',
        btnPlusClass: 'mbsc-ic mbsc-ic-arrow-down5',
        btnMinusClass: 'mbsc-ic mbsc-ic-arrow-up5',
        btnCalPrevClass: 'mbsc-ic mbsc-ic-arrow-left5',
        btnCalNextClass: 'mbsc-ic mbsc-ic-arrow-right5'
    });

    // Prevent re-show on window focus
    $(window).on('focus', function () {
        if ($activeElm) {
            preventShow = true;
        }
    });

})(window, document);
(function (window, document, undefined) {

    var ms = mobiscroll,
        $ = ms.$,
        extend = $.extend,
        classes = ms.classes,
        platform = ms.platform,
        util = ms.util,
        pr = util.jsPrefix,
        pref = util.prefix,
        getCoord = util.getCoord,
        testTouch = util.testTouch,
        force2D = platform.name == 'wp' || platform.name == 'android' || (platform.name == 'ios' && platform.majorVersion < 8);

    ms.presetShort('scroller', 'Scroller', false);

    classes.Scroller = function (el, settings, inherit) {
        var $markup,
            $stepBtn,
            batchSize3d,
            batchSize = 40,
            animTime = 1000,
            scroll3dAngle,
            scroll3d,
            selectedClass,
            showScrollArrows,
            stepTimer,
            stepRunning,
            stepSkip,
            stepBtnX,
            stepBtnY,
            tempWheelArray,
            itemHeight,
            itemHeight3d,
            isValidating,
            s,
            trigger,
            lines,
            wheels,
            wheelsMap,
            that = this,
            $elm = $(el);

        // Event handlers

        function onBtnStart(ev) {
            var i = +$(this).attr('data-index');

            ev.stopPropagation();

            if (ev.type === 'mousedown') {
                // Prevent focus
                ev.preventDefault();
            }

            if (testTouch(ev, this) && !isReadOnly(i)) {

                $stepBtn = $(this).addClass('mbsc-sc-btn-a');

                stepBtnX = getCoord(ev, 'X');
                stepBtnY = getCoord(ev, 'Y');

                stepRunning = true;
                stepSkip = false;
                setTimeout(function () {
                    runStepper(i, $stepBtn.attr('data-dir') == 'inc' ? 1 : -1);
                }, 100);

                if (ev.type === 'mousedown') {
                    $(document)
                        .on('mousemove', onBtnMove)
                        .on('mouseup', onBtnEnd);
                }
            }
        }

        function onBtnMove(ev) {
            if (Math.abs(stepBtnX - getCoord(ev, 'X')) > 7 || Math.abs(stepBtnY - getCoord(ev, 'Y')) > 7) {
                stopStepper(true);
            }
        }

        function onBtnEnd(ev) {
            stopStepper();

            // Prevent scroll on double tap on iOS
            ev.preventDefault();

            if (ev.type === 'mouseup') {
                $(document)
                    .off('mousemove', onBtnMove)
                    .off('mouseup', onBtnEnd);
            }
        }

        function onKeyDown(ev) {
            var i = $(this).attr('data-index'),
                handle,
                direction;

            if (ev.keyCode == 38) { // Up
                handle = true;
                direction = -1;
            } else if (ev.keyCode == 40) { // Down
                handle = true;
                direction = 1;
            } else if (ev.keyCode == 32) { // Space
                handle = true;
                toggleItem(i);
            }

            if (handle) {
                ev.stopPropagation();
                ev.preventDefault();

                if (direction && !stepRunning) {
                    stepRunning = true;
                    stepSkip = false;
                    runStepper(i, direction);
                }
            }
        }

        function onKeyUp() {
            stopStepper();
        }

        // Private functions

        function getIndex(wheel, val) {
            return (wheel._array ? wheel._map[val] : wheel.getIndex(val, that)) || 0;
        }

        function getItem(wheel, i) {
            var data = wheel.data;

            if (i >= wheel.min && i <= wheel.max) {
                return wheel._array ?
                    (wheel.circular ? $(data).get(i % wheel._length) : data[i]) :
                    ($.isFunction(data) ? data(i, that) : '');
            }
        }

        function getItemValue(item) {
            return $.isPlainObject(item) ? (item.value !== undefined ? item.value : item.display) : item;
        }

        function getItemText(item) {
            var text = $.isPlainObject(item) ? item.display : item;
            return text === undefined ? '' : text;
        }

        function getValue(wheel, i) {
            return getItemValue(getItem(wheel, i));
        }

        function toggleItem(i, $selected) {
            var wheel = wheels[i],
                $item = $selected || wheel._$markup.find('.mbsc-sc-itm[data-val="' + tempWheelArray[i] + '"]'),
                idx = +$item.attr('data-index'),
                val = getValue(wheel, idx),
                selected = that._tempSelected[i],
                maxSelect = util.isNumeric(wheel.multiple) ? wheel.multiple : Infinity;

            if (wheel.multiple && !wheel._disabled[val]) {
                if (selected[val] !== undefined) {
                    $item.removeClass(selectedClass).removeAttr('aria-selected');
                    delete selected[val];
                } else if (util.objectToArray(selected).length < maxSelect) {
                    $item.addClass(selectedClass).attr('aria-selected', 'true');
                    selected[val] = val;
                }
                return true;
            }
        }

        function runStepper(index, direction) {
            if (!stepSkip) {
                step(index, direction);
            }

            if (stepRunning) {
                clearInterval(stepTimer);
                stepTimer = setInterval(function () {
                    step(index, direction);
                }, s.delay);
            }
        }

        function stopStepper(skip) {
            clearInterval(stepTimer);
            stepSkip = skip;
            stepRunning = false;

            if ($stepBtn) {
                $stepBtn.removeClass('mbsc-sc-btn-a');
            }
        }

        function step(index, direction) {
            var wheel = wheels[index];
            setWheelValue(wheel, index, wheel._current + direction, animTime, direction == 1 ? 1 : 2);
        }

        function isReadOnly(i) {
            return $.isArray(s.readonly) ? s.readonly[i] : s.readonly;
        }

        function initWheel(w, l, keep) {
            var index = w._index - w._batch;

            w.data = w.data || [];
            w.key = w.key !== undefined ? w.key : l;
            w.label = w.label !== undefined ? w.label : l;

            w._map = {};
            w._array = $.isArray(w.data);

            // Map keys to index
            if (w._array) {
                w._length = w.data.length;
                $.each(w.data, function (i, v) {
                    w._map[getItemValue(v)] = i;
                });
            }

            w.circular = s.circular === undefined ?
                (w.circular === undefined ? (w._array && w._length > s.rows) : w.circular) :
                ($.isArray(s.circular) ? s.circular[l] : s.circular);
            w.min = w._array ? (w.circular ? -Infinity : 0) : (w.min === undefined ? -Infinity : w.min);
            w.max = w._array ? (w.circular ? Infinity : w._length - 1) : (w.max === undefined ? Infinity : w.max);

            w._nr = l;
            w._index = getIndex(w, tempWheelArray[l]);
            w._disabled = {};
            w._batch = 0;
            w._current = w._index;
            w._first = w._index - batchSize; //Math.max(w.min, w._current - batchSize);
            w._last = w._index + batchSize; //Math.min(w.max, w._first + 2 * batchSize);
            w._offset = w._first;

            if (keep) {
                w._offset -= w._margin / itemHeight + (w._index - index);
                w._margin += (w._index - index) * itemHeight;
            } else {
                w._margin = 0; //w._first * itemHeight;
            }

            w._refresh = function (noScroll) {
                var maxScroll = -(w.min - w._offset + (w.multiple && !scroll3d ? Math.floor(s.rows / 2) : 0)) * itemHeight,
                    minScroll = Math.min(maxScroll, -(w.max - w._offset - (w.multiple && !scroll3d ? Math.floor(s.rows / 2) : 0)) * itemHeight);

                extend(w._scroller.settings, {
                    minScroll: minScroll,
                    maxScroll: maxScroll
                });

                w._scroller.refresh(noScroll);
            };

            wheelsMap[w.key] = w;

            return w;
        }

        function generateItems(wheel, index, start, end, is3d) {
            var i,
                css,
                item,
                value,
                text,
                lbl,
                invalid,
                selected,
                html = '',
                checked = that._tempSelected[index],
                disabled = wheel._disabled || {};

            for (i = start; i <= end; i++) {
                item = getItem(wheel, i);
                text = getItemText(item);
                value = getItemValue(item);
                css = item && item.cssClass !== undefined ? item.cssClass : '';
                lbl = item && item.label !== undefined ? item.label : '';
                invalid = item && item.invalid;
                selected = value !== undefined && value == tempWheelArray[index] && !wheel.multiple;

                // TODO: don't generate items with no value (use margin or placeholder instead)
                html += '<div role="option" aria-selected="' + (checked[value] ? true : false) +
                    '" class="mbsc-sc-itm ' + (is3d ? 'mbsc-sc-itm-3d ' : '') + css + ' ' +
                    (selected ? 'mbsc-sc-itm-sel ' : '') +
                    (checked[value] ? selectedClass : '') +
                    (value === undefined ? ' mbsc-sc-itm-ph' : ' mbsc-btn-e') +
                    (invalid ? ' mbsc-sc-itm-inv-h mbsc-btn-d' : '') +
                    (disabled[value] ? ' mbsc-sc-itm-inv mbsc-btn-d' : '') +
                    '" data-index="' + i +
                    '" data-val="' + value + '"' +
                    (lbl ? ' aria-label="' + lbl + '"' : '') +
                    (selected ? ' aria-selected="true"' : '') +
                    ' style="height:' + itemHeight + 'px;line-height:' + itemHeight + 'px;' +
                    (is3d ? pref + 'transform:rotateX(' + ((wheel._offset - i) * scroll3dAngle % 360) + 'deg) translateZ(' + (itemHeight * s.rows / 2) + 'px);' : '') +
                    '">' +
                    (lines > 1 ? '<div class="mbsc-sc-itm-ml" style="line-height:' + Math.round(itemHeight / lines) + 'px;font-size:' + Math.round(itemHeight / lines * 0.8) + 'px;">' : '') +
                    text +
                    (lines > 1 ? '</div>' : '') +
                    '</div>';
            }

            return html;
        }

        function formatHeader(v) {
            var t = s.headerText;
            return t ? (typeof t === 'function' ? t.call(el, v) : t.replace(/\{value\}/i, v)) : '';
        }

        function infinite(wheel, i, pos) {
            var index = Math.round(-pos / itemHeight) + wheel._offset,
                diff = index - wheel._current,
                first = wheel._first,
                last = wheel._last,
                first3d = first + batchSize - batchSize3d + 1,
                last3d = last - batchSize + batchSize3d;

            if (diff) {
                wheel._first += diff;
                wheel._last += diff;

                wheel._current = index;

                // Generate items
                //setTimeout(function () {
                if (diff > 0) {
                    wheel._$scroller.append(generateItems(wheel, i, Math.max(last + 1, first + diff), last + diff));
                    $('.mbsc-sc-itm', wheel._$scroller).slice(0, Math.min(diff, last - first + 1)).remove();

                    // 3D
                    if (scroll3d) {
                        wheel._$3d.append(generateItems(wheel, i, Math.max(last3d + 1, first3d + diff), last3d + diff, true));
                        $('.mbsc-sc-itm', wheel._$3d).slice(0, Math.min(diff, last3d - first3d + 1)).attr('class', 'mbsc-sc-itm-del');
                    }
                } else if (diff < 0) {
                    wheel._$scroller.prepend(generateItems(wheel, i, first + diff, Math.min(first - 1, last + diff)));
                    $('.mbsc-sc-itm', wheel._$scroller).slice(Math.max(diff, first - last - 1)).remove();

                    // 3D
                    if (scroll3d) {
                        wheel._$3d.prepend(generateItems(wheel, i, first3d + diff, Math.min(first3d - 1, last3d + diff), true));
                        $('.mbsc-sc-itm', wheel._$3d).slice(Math.max(diff, first3d - last3d - 1)).attr('class', 'mbsc-sc-itm-del');
                    }
                }

                wheel._margin += diff * itemHeight;
                wheel._$scroller.css('margin-top', wheel._margin + 'px');
                //}, 10);
            }
        }

        function getValid(index, val, dir, dis) {
            var counter,
                wheel = wheels[index],
                disabled = dis || wheel._disabled,
                idx = getIndex(wheel, val),
                v1 = val,
                v2 = val,
                dist1 = 0,
                dist2 = 0;

            if (val === undefined) {
                val = getValue(wheel, idx);
            }

            // TODO: what if all items are invalid
            if (disabled[val] === true) {
                counter = 0;
                while (idx - dist1 >= wheel.min && disabled[v1] && counter < 100) {
                    counter++;
                    dist1++;
                    v1 = getValue(wheel, idx - dist1);
                }

                counter = 0;
                while (idx + dist2 < wheel.max && disabled[v2] && counter < 100) {
                    counter++;
                    dist2++;
                    v2 = getValue(wheel, idx + dist2);
                }

                // If we have direction (+/- or mouse wheel), the distance does not count
                if (((dist2 < dist1 && dist2 && dir !== 2) || !dist1 || (idx - dist1 < 0) || dir == 1) && !disabled[v2]) {
                    val = v2;
                } else {
                    val = v1;
                }
            }

            return val;
        }

        function scrollToPos(time, index, dir, manual, tap) {
            var diff,
                idx,
                offset,
                ret,
                isVisible = that._isVisible;

            isValidating = true;
            ret = s.validate.call(el, {
                values: tempWheelArray.slice(0),
                index: index,
                direction: dir
            }, that) || {};
            isValidating = false;

            if (ret.valid) {
                that._tempWheelArray = tempWheelArray = ret.valid.slice(0);
            }

            trigger('onValidated');

            $.each(wheels, function (i, wheel) {
                if (isVisible) {
                    // Enable all items
                    wheel._$markup.find('.mbsc-sc-itm-inv').removeClass('mbsc-sc-itm-inv mbsc-btn-d');
                }
                wheel._disabled = {};

                // Disable invalid items
                if (ret.disabled && ret.disabled[i]) {
                    $.each(ret.disabled[i], function (j, v) {
                        wheel._disabled[v] = true;
                        if (isVisible) {
                            wheel._$markup.find('.mbsc-sc-itm[data-val="' + v + '"]').addClass('mbsc-sc-itm-inv mbsc-btn-d');
                        }
                    });
                }

                // Get closest valid value
                tempWheelArray[i] = wheel.multiple ? tempWheelArray[i] : getValid(i, tempWheelArray[i], dir);

                if (isVisible) {
                    if (!wheel.multiple || index === undefined) {
                        wheel._$markup
                            .find('.mbsc-sc-itm-sel')
                            .removeClass(selectedClass)
                            .removeAttr('aria-selected');
                    }

                    if (wheel.multiple) {
                        // Add selected styling to selected elements in case of multiselect
                        if (index === undefined) {
                            for (var v in that._tempSelected[i]) {
                                wheel._$markup
                                    .find('.mbsc-sc-itm[data-val="' + v + '"]')
                                    .addClass(selectedClass)
                                    .attr('aria-selected', 'true');
                            }
                        }
                    } else {
                        // Mark element as aria selected
                        wheel._$markup
                            .find('.mbsc-sc-itm[data-val="' + tempWheelArray[i] + '"]')
                            .addClass('mbsc-sc-itm-sel')
                            .attr('aria-selected', 'true');
                    }

                    // Get index of valid value
                    idx = getIndex(wheel, tempWheelArray[i]);

                    diff = idx - wheel._index + wheel._batch;

                    if (Math.abs(diff) > 2 * batchSize + 1) {
                        offset = diff + (2 * batchSize + 1) * (diff > 0 ? -1 : 1);
                        wheel._offset += offset;
                        wheel._margin -= offset * itemHeight;
                        wheel._refresh();
                    }

                    wheel._index = idx + wheel._batch;

                    // Scroll to valid value
                    wheel._scroller.scroll(-(idx - wheel._offset + wheel._batch) * itemHeight, (index === i || index === undefined) ? time : animTime, tap);
                }
            });

            // Get formatted value
            that._tempValue = s.formatValue(tempWheelArray, that);

            if (isVisible) {
                // Update header text
                that._header.html(formatHeader(that._tempValue));
            }

            // If in live mode, set and fill value on every move
            if (that.live) {
                that._hasValue = manual || that._hasValue;
                setValue(manual, manual, 0, true);
                if (manual) {
                    trigger('onSet', {
                        valueText: that._value
                    });
                }
            }

            if (manual) {
                trigger('onChange', {
                    valueText: that._tempValue
                });
            }
        }

        function setWheelValue(wheel, i, idx, time, dir, tap) {
            // Get the value at the given index
            var value = getValue(wheel, idx);

            if (value !== undefined) {
                tempWheelArray[i] = value;

                // In case of circular wheels calculate the offset of the current batch
                wheel._batch = wheel._array ? Math.floor(idx / wheel._length) * wheel._length : 0;

                setTimeout(function () {
                    scrollToPos(time, i, dir, true, tap);
                }, 10);
            }
        }

        function setValue(fill, change, time, noscroll, temp) {
            if (!noscroll) {
                scrollToPos(time);
            } else {
                that._tempValue = s.formatValue(that._tempWheelArray, that);
            }

            if (!temp) {
                that._wheelArray = tempWheelArray.slice(0);
                that._value = that._hasValue ? that._tempValue : null;
                that._selected = extend(true, {}, that._tempSelected);
            }

            if (fill) {
                if (that._isInput) {
                    $elm.val(that._hasValue ? that._tempValue : '');
                }

                trigger('onFill', {
                    valueText: that._hasValue ? that._tempValue : '',
                    change: change
                });

                if (change) {
                    that._preventChange = true;
                    $elm.trigger('change');
                }
            }
        }

        // Call the parent constructor
        classes.Frame.call(this, el, settings, true);

        // Public functions

        /**
         * Sets the value of the scroller.
         * @param {Array} val - New value.
         * @param {Boolean} [fill=false] - Set the value of the associated input element.
         * @param {Boolean} [change=false] - Trigger change on the input element.
         * @param {Boolean} [temp=false] - If true, then only set the temporary value (only scroll there but not set the value).
         * @param {Number} [time=0] - Animation time in milliseconds.
         */
        that.setVal = that._setVal = function (val, fill, change, temp, time) {
            that._hasValue = val !== null && val !== undefined;
            that._tempWheelArray = tempWheelArray = $.isArray(val) ? val.slice(0) : s.parseValue.call(el, val, that) || [];
            setValue(fill, change === undefined ? fill : change, time, false, temp);
        };

        /**
         * Returns the selected value.
         */
        that.getVal = that._getVal = function (temp) {
            var val = that._hasValue || temp ? that[temp ? '_tempValue' : '_value'] : null;
            return util.isNumeric(val) ? +val : val;
        };

        /*
         * Sets the wheel values (passed as an array).
         */
        that.setArrayVal = that.setVal;

        /*
         * Returns the selected wheel values as an array.
         */
        that.getArrayVal = function (temp) {
            return temp ? that._tempWheelArray : that._wheelArray;
        };

        that.changeWheel = function (whls, time, manual) {
            var i,
                w;

            $.each(whls, function (key, wheel) {
                w = wheelsMap[key];
                i = w._nr;
                // Check if wheel exists
                if (w) {
                    extend(w, wheel);

                    initWheel(w, i, true);

                    if (that._isVisible) {
                        if (scroll3d) {
                            w._$3d.html(generateItems(w, i, w._first + batchSize - batchSize3d + 1, w._last - batchSize + batchSize3d, true));
                        }

                        w._$scroller
                            .html(generateItems(w, i, w._first, w._last))
                            .css('margin-top', w._margin + 'px');

                        w._refresh(isValidating);
                    }
                }
            });

            if (that._isVisible && !isValidating) {
                that.position();
            }

            if (!isValidating) {
                scrollToPos(time, undefined, undefined, manual);
            }
        };

        /**
         * Returns the closest valid value.
         */
        that.getValidValue = getValid;

        // Protected overrides

        that._generateContent = function () {
            var lbl,
                html = '',
                style = scroll3d ? pref + 'transform: translateZ(' + (itemHeight * s.rows / 2 + 3) + 'px);' : '',
                highlight = '<div class="mbsc-sc-whl-l" style="' + style + 'height:' + itemHeight + 'px;margin-top:-' + (itemHeight / 2 + (s.selectedLineBorder || 0)) + 'px;"></div>',
                l = 0;

            $.each(s.wheels, function (i, wg) {
                html += '<div class="mbsc-w-p mbsc-sc-whl-gr-c' + (s.showLabel ? ' mbsc-sc-lbl-v' : '') + '">' + highlight +
                    '<div class="mbsc-sc-whl-gr' +
                    (scroll3d ? ' mbsc-sc-whl-gr-3d' : '') +
                    (showScrollArrows ? ' mbsc-sc-cp' : '') + '">';

                $.each(wg, function (j, w) { // Wheels

                    that._tempSelected[l] = extend({}, that._selected[l]);

                    // TODO: this should be done on initialization, not on show
                    wheels[l] = initWheel(w, l);

                    lbl = w.label !== undefined ? w.label : j;

                    html += '<div class="mbsc-sc-whl-w ' + (w.cssClass || '') + (w.multiple ? ' mbsc-sc-whl-multi' : '') + '" style="' +
                        (s.width ? ('width:' + (s.width[l] || s.width) + 'px;') :
                            (s.minWidth ? ('min-width:' + (s.minWidth[l] || s.minWidth) + 'px;') : '') +
                            (s.maxWidth ? ('max-width:' + (s.maxWidth[l] || s.maxWidth) + 'px;') : '')) + '">' +
                        '<div class="mbsc-sc-whl-o" style="' + style + '"></div>' + highlight +
                        '<div tabindex="0" aria-live="off" aria-label="' + lbl + '"' + (w.multiple ? ' aria-multiselectable="true"' : '') + ' role="listbox" data-index="' + l + '" class="mbsc-sc-whl"' + ' style="' +
                        'height:' + (s.rows * itemHeight * (scroll3d ? 1.1 : 1)) + 'px;">' +
                        (showScrollArrows ?
                            '<div data-index="' + l + '" data-dir="inc" class="mbsc-sc-btn mbsc-sc-btn-plus ' + (s.btnPlusClass || '') + '" style="height:' + itemHeight + 'px;line-height:' + itemHeight + 'px;"></div>' + // + button
                            '<div data-index="' + l + '" data-dir="dec" class="mbsc-sc-btn mbsc-sc-btn-minus ' + (s.btnMinusClass || '') + '" style="height:' + itemHeight + 'px;line-height:' + itemHeight + 'px;"></div>' : '') + // - button
                        '<div class="mbsc-sc-lbl">' + lbl + '</div>' + // Wheel label
                        '<div class="mbsc-sc-whl-c"' +
                        ' style="height:' + itemHeight3d + 'px;margin-top:-' + (itemHeight3d / 2 + 1) + 'px;' + style + '">' +
                        '<div class="mbsc-sc-whl-sc" style="top:' + ((itemHeight3d - itemHeight) / 2) + 'px;">';

                    // Create wheel values
                    html += generateItems(w, l, w._first, w._last) +
                        '</div></div>';

                    if (scroll3d) {
                        html += '<div class="mbsc-sc-whl-3d" style="height:' + itemHeight + 'px;margin-top:-' + (itemHeight / 2) + 'px;">';
                        html += generateItems(w, l, w._first + batchSize - batchSize3d + 1, w._last - batchSize + batchSize3d, true);
                        html += '</div>';
                    }

                    html += '</div></div>';

                    l++;
                });

                html += '</div></div>';
            });

            return html;
        };

        that._attachEvents = function ($markup) {
            $('.mbsc-sc-btn', $markup)
                .on('touchstart mousedown', onBtnStart)
                .on('touchmove', onBtnMove)
                .on('touchend touchcancel', onBtnEnd);

            $('.mbsc-sc-whl', $markup)
                .on('keydown', onKeyDown)
                .on('keyup', onKeyUp);
        };

        that._detachEvents = function ($m) {
            $('.mbsc-sc-whl', $m).mobiscroll('destroy');
        };

        that._markupReady = function ($m) {
            $markup = $m;

            $('.mbsc-sc-whl', $markup).each(function (i) {
                var idx,
                    $wh = $(this),
                    wheel = wheels[i],
                    maxScroll = -(wheel.min - wheel._offset + (wheel.multiple && !scroll3d ? Math.floor(s.rows / 2) : 0)) * itemHeight,
                    minScroll = Math.min(maxScroll, -(wheel.max - wheel._offset - (wheel.multiple && !scroll3d ? Math.floor(s.rows / 2) : 0)) * itemHeight);

                wheel._$markup = $wh;
                wheel._$scroller = $('.mbsc-sc-whl-sc', this);
                wheel._$3d = $('.mbsc-sc-whl-3d', this);

                wheel._scroller = new ms.classes.ScrollView(this, {
                    mousewheel: s.mousewheel,
                    moveElement: wheel._$scroller,
                    initialPos: (wheel._first - wheel._index) * itemHeight,
                    contSize: 0,
                    snap: itemHeight,
                    minScroll: minScroll,
                    maxScroll: maxScroll,
                    maxSnapScroll: batchSize,
                    prevDef: true,
                    stopProp: true,
                    timeUnit: 3,
                    easing: 'cubic-bezier(0.190, 1.000, 0.220, 1.000)',
                    sync: function (pos, time, easing) {
                        if (scroll3d) {
                            wheel._$3d[0].style[pr + 'Transition'] = time ? pref + 'transform ' + Math.round(time) + 'ms ' + easing : '';
                            wheel._$3d[0].style[pr + 'Transform'] = 'rotateX(' + ((-pos / itemHeight) * scroll3dAngle) + 'deg)';
                        }
                    },
                    onStart: function (ev, inst) {
                        inst.settings.readonly = isReadOnly(i);
                    },
                    onGestureStart: function () {
                        $wh.addClass('mbsc-sc-whl-a mbsc-sc-whl-anim');

                        trigger('onWheelGestureStart', {
                            index: i
                        });
                    },
                    onGestureEnd: function (ev) {
                        var dir = ev.direction == 90 ? 1 : 2,
                            time = ev.duration,
                            pos = ev.destinationY;

                        idx = Math.round(-pos / itemHeight) + wheel._offset;

                        setWheelValue(wheel, i, idx, time, dir);
                    },
                    onAnimationStart: function () {
                        $wh.addClass('mbsc-sc-whl-anim');
                    },
                    onAnimationEnd: function () {
                        $wh.removeClass('mbsc-sc-whl-a mbsc-sc-whl-anim');

                        trigger('onWheelAnimationEnd', {
                            index: i
                        });

                        wheel._$3d.find('.mbsc-sc-itm-del').remove();
                    },
                    onMove: function (ev) {
                        infinite(wheel, i, ev.posY);
                    },
                    onBtnTap: function (ev) {
                        var $item = $(ev.target),
                            idx = +$item.attr('data-index');

                        // Select item on tap
                        if (toggleItem(i, $item)) {
                            // Don't scroll, but trigger validation
                            idx = wheel._current;
                        }

                        if (trigger('onItemTap', {
                                target: $item[0],
                                selected: $item.hasClass('mbsc-itm-sel')
                            }) !== false) {
                            setWheelValue(wheel, i, idx, animTime, true, true);

                            if (that.live && !wheel.multiple && (s.setOnTap === true || s.setOnTap[i])) {
                                setTimeout(function () {
                                    that.select();
                                }, 200);
                            }
                        }
                    }
                });
            });

            scrollToPos();
        };

        that._fillValue = function () {
            that._hasValue = true;
            setValue(true, true, 0, true);
        };

        that._clearValue = function () {
            $('.mbsc-sc-whl-multi .mbsc-sc-itm-sel', $markup)
                .removeClass(selectedClass)
                .removeAttr('aria-selected');
        };

        that._readValue = function () {
            var v = $elm.val() || '',
                l = 0;

            if (v !== '') {
                that._hasValue = true;
            }

            that._tempWheelArray = tempWheelArray = that._hasValue && that._wheelArray ?
                that._wheelArray.slice(0) :
                s.parseValue.call(el, v, that) || [];

            that._tempSelected = extend(true, {}, that._selected);

            $.each(s.wheels, function (i, wg) {
                $.each(wg, function (j, w) { // Wheels
                    wheels[l] = initWheel(w, l);
                    l++;
                });
            });

            setValue(false, false, 0, true);

            trigger('onRead');
        };

        that.__processSettings = function () {
            s = that.settings;
            s.cssClass = (s.cssClass || '') + ' mbsc-sc';
            trigger = that.trigger;
            lines = s.multiline;
            selectedClass = 'mbsc-sc-itm-sel mbsc-ic mbsc-ic-' + s.checkIcon;
            wheels = [];
            wheelsMap = {};
        };

        that.__init = function () {
            showScrollArrows = s.showScrollArrows;
            scroll3d = s.scroll3d && !force2D && !showScrollArrows;
            itemHeight = s.height;
            itemHeight3d = scroll3d ? Math.round((itemHeight - (itemHeight * s.rows / 2 + 3) * 0.03) / 2) * 2 : itemHeight;
            batchSize3d = Math.round(s.rows * 1.8);
            scroll3dAngle = 360 / (batchSize3d * 2);

            that._isLiquid = (s.layout || ((/top|bottom/.test(s.display) && s.wheels.length == 1) || s.display == 'inline' ? 'liquid' : '')) === 'liquid';

            // Ensure a minimum number of 3 items if clickpick buttons present
            if (showScrollArrows) {
                s.rows = Math.max(3, s.rows);
            }
        };

        that._getItemValue = getItemValue;

        // Properties
        that._tempSelected = {};
        that._selected = {};

        // Constructor
        if (!inherit) {
            that.init(settings);
        }
    };

    // Extend defaults
    classes.Scroller.prototype = {
        _hasDef: true,
        _hasTheme: true,
        _hasLang: true,
        _hasPreset: true,
        _class: 'scroller',
        _defaults: extend({}, classes.Frame.prototype._defaults, {
            // Options
            minWidth: 80,
            height: 40,
            rows: 3,
            multiline: 1,
            delay: 300,
            readonly: false,
            showLabel: true,
            setOnTap: false,
            wheels: [],
            preset: '',
            speedUnit: 0.0012,
            timeUnit: 0.08,
            checkIcon: 'checkmark',
            validate: function () {},
            formatValue: function (d) {
                return d.join(' ');
            },
            parseValue: function (value, inst) {
                var val = [],
                    ret = [],
                    i = 0,
                    found,
                    data;

                if (value !== null && value !== undefined) {
                    val = (value + '').split(' ');
                }

                $.each(inst.settings.wheels, function (j, wg) {
                    $.each(wg, function (k, w) {
                        data = w.data;
                        // Default to first wheel value if not found
                        found = inst._getItemValue(data[0]);
                        $.each(data, function (l, item) {
                            // Don't do strict comparison
                            if (val[i] == inst._getItemValue(item)) {
                                found = inst._getItemValue(item);
                                return false;
                            }
                        });
                        ret.push(found);
                        i++;
                    });
                });
                return ret;
            }
        })
    };

})(window, document);
// Chinese
(function () {
    mobiscroll.i18n.zh = {
        // Core
        setText: '确定',
        cancelText: '取消',
        clearText: '明确',
        selectedText: '{count} 选',
        // Datetime component
        dateFormat: 'yy/mm/dd',
        dayNames: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],
        dayNamesShort: ['日', '一', '二', '三', '四', '五', '六'],
        dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
        dayText: '日',
        hourText: '时',
        minuteText: '分',
        monthNames: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        monthNamesShort: ['一', '二', '三', '四', '五', '六', '七', '八', '九', '十', '十一', '十二'],
        monthText: '月',
        secText: '秒',
        timeFormat: 'HH:ii',
        yearText: '年',
        nowText: '当前',
        pmText: '下午',
        amText: '上午',
        todayText: '今天',
        // Calendar component
        dateText: '日',
        timeText: '时间',
        calendarText: '日历',
        closeText: '关闭',
        // Daterange component
        fromText: '开始时间',
        toText: '结束时间',
        // Measurement components
        wholeText: '合计',
        fractionText: '分数',
        unitText: '单位',
        // Time / Timespan component
        labels: ['年', '月', '日', '小时', '分钟', '秒', ''],
        labelsShort: ['年', '月', '日', '点', '分', '秒', ''],
        // Timer component
        startText: '开始',
        stopText: '停止',
        resetText: '重置',
        lapText: '圈',
        hideText: '隐藏',
        // Listview
        backText: '背部',
        undoText: '复原',
        // Form
        offText: '关闭',
        onText: '开启',
        // Numpad
        decimalSeparator: ',',
        thousandsSeparator: ' '
    };
})();
