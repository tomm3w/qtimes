(function () {
    var days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    var months_short = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    Date.prototype.getMonthName = function () {
        return months[this.getMonth()];
    };
    Date.prototype.getShortMonthName = function () {
        return months_short[this.getMonth()];
    };
    Date.prototype.getDayName = function () {
        return days[this.getDay()];
    };
})();

function getFormatedDate(messageDateTime) {
    var date = new Date(messageDateTime);

    var month = (date.getMonth() + 1);
    var day = date.getDate();
    return date.getDayName() + ', ' + date.getMonthName() + " " + (day < 10 ? '0' + day : day) + ", " + date.getFullYear();
}


function getFormatedTime(messageDateTime) {
    var date = new Date(messageDateTime);
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}

function linkify(text) {
    if (text) {
        text = text.replace(
            /((https?\:\/\/)|(www\.))(\S+)(\w{2,4})(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/gi,
            function (url) {
                var full_url = url;
                if (!full_url.match('^https?:\/\/')) {
                    full_url = 'http://' + full_url;
                }
                return '<a target="_blank" href="' + full_url + '">' + url + '</a>';
            }
        );
    }
    return text;
}

(function ($) {
    if (typeof jQuery.tmpl !== "undefined") {
        $.extend(jQuery.tmpl.tag, {
            "for": {
                _default: { $2: "var i=1;i<=1;i++" },
                open: 'for ($2){',
                close: '};'
            }
        });
    }
})(jQuery);

(function ($) {
    $.fn.extend({
        collapsiblePanel: function (options) {
            var settings = $.extend({
                // These are the defaults.
                collapsed: false
            }, options);
            $(this).each(function () {
                var indicator = $(this).find('.ui-expander').first();
                var header = $(this).find('.ui-widget-header').first();
                var content = $(this).find('.ui-widget-content').first();
                if (settings.collapsed) {
                    content.hide();
                    indicator.removeClass('ui-icon-triangle-1-e ui-icon-triangle-1-s').addClass('ui-icon-triangle-1-e');
                } else {
                    indicator.removeClass('ui-icon-triangle-1-e ui-icon-triangle-1-s').addClass('ui-icon-triangle-1-s');
                }

                header.click(function () {
                    content.slideToggle(500, function () {
                        console.log(content.is(':visible'));
                        if (!content.is(':visible')) {
                            indicator.removeClass('ui-icon-triangle-1-e ui-icon-triangle-1-s').addClass('ui-icon-triangle-1-e');
                        } else {
                            indicator.removeClass('ui-icon-triangle-1-e ui-icon-triangle-1-s').addClass('ui-icon-triangle-1-s');
                        }
                    });
                });
            });
        }
    });
})(jQuery);
if (typeof ko !== "undefined") {
    //wrapper to an observable that requires accept/cancel
    ko.protectedObservable = function (initialValue) {
        //private variables
        var _actualValue = ko.observable(initialValue),
            _tempValue = initialValue;
        var _nonchangableactualvalue = ko.observable(initialValue);
        //computed observable that we will return
        var result = ko.computed({
            //always return the actual value
            read: function () {
                return _actualValue();
            },
            //stored in a temporary spot until commit
            write: function (newValue) {
                _tempValue = newValue;
            }
        });

        //if different, commit temp value
        result.commit = function () {
            if (_tempValue !== _actualValue()) {
                _actualValue(_tempValue);
            }
        };

        //force subscribers to take original
        result.reset = function () {
            _actualValue.valueHasMutated();
            _tempValue = _actualValue();   //reset temp value
        };

        result.resethard = function () {
            _actualValue(_nonchangableactualvalue());
            _tempValue = _actualValue();
        }

        return result;
    };
}

function serialize(element) {
    return $("input, select", element).serialize();
}

function serializeArray(element) {
    var a = $("input, select, textarea", element).serializeArray();
    var o = {};

    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

function showErrorOnElement(jsondata, element) {
    if (jsondata.status == "error") {
        if (jsondata.ErrorMessage) {
            element.html(jsondata.ErrorMessage);
            element.show();
        }
    }
}

function showErrorAlertMessageFromServerResponse(jsondata) {
    if (jsondata.status == "error") {
        if (jsondata.ErrorMessage) {
            alert(jsondata.ErrorMessage);
        }
    }
}

function showErrorMessage(jsondata) {
    if (jsondata.status == "badrequest") {
        if (jsondata.message) {
            //alert(jsondata.message);
            $.notify(jsondata.message, { position: "top center" });
        }
    }
}

function NotifyErrorMessage(msg) {
    $.notify(msg, { position: 'top center', className: 'error' });
}

function NotifySavedMessage(msg) {
    $.notify(msg, { position: 'top center', className: 'success' });
}

function ShowErrorMessage(xhr, status, error) {
    if (xhr.responseJSON) {
        if (xhr.responseJSON.ExceptionMessage)
            $.notify(xhr.responseJSON.ExceptionMessage, "error");
        else
            $.notify(xhr.responseJSON.Message, "error");
    }
    else
        $.notify(status, "error");
}

function fillFormWithData(formelement, data) {
    if (data) {
        for (var i in data) {
            var value = data[i];
            $("[name='" + i + "']:not([type='radio']):not([type='checkbox'])", formelement).val(value);
            $("input[name='" + i + "'][type='radio'][value='" + value + "']", formelement).attr('checked', 'checked');
            $("input[name='" + i + "'][type='checkbox'][value='" + value + "']", formelement).attr('checked', 'checked');
            if ($.isArray(value)) {
                var elements = $("input[name='" + i + "']", formelement);
                elements.prop({ 'checked': false });
                for (var val in value) {
                    elements.eq(value[val]).prop({ 'checked': true });
                }
            }
        }

    }
    else {
        $("input:not([type='radio']):not([type='checkbox']):not([type='button'])", formelement).val(null);
        $("input[type='radio']", formelement).prop({ 'checked': false });
        $("input[type='checkbox']", formelement).attr({ 'checked': false });
        $("textarea", formelement).val(null)
    }
}
function clearElementValues(element) {
    $("input, select", element).not("[type='button']").not("[type='checkbox']").not("[type='radio']").val('');
}
function deepCopy(src, dest) {
    var name,
        value,
        isArray,
        toString = Object.prototype.toString;

    // If no `dest`, create one
    if (!dest) {
        isArray = toString.call(src) === "[object Array]";
        if (isArray) {
            dest = [];
            dest.length = src.length;
        }
        else { // You could have lots of checks here for other types of objects
            dest = {};
        }
    }

    // Loop through the props
    for (name in src) {
        // If you don't want to copy inherited properties, add a `hasOwnProperty` check here
        // In our case, we only do that for arrays, but it depends on your needs
        if (!isArray || src.hasOwnProperty(name)) {
            value = src[name];
            if (typeof value === "object") {
                // Recurse
                value = deepCopy(value);
            }
            dest[name] = value;
        }
    }

    return dest;
}

function unique(array) {
    return $.grep(array, function (el, index) {
        return index == $.inArray(el, array);
    });
}

function isExistsArrayInArray(mainarray, valuearray, propname) {
    var isProp = typeof propname !== 'undefined';
    var result = $.grep(mainarray, function (e) {
        return isExistsValueInArray(valuearray, e, propname);
    });
    return result.length > 0;
}
function isExistsValueInArray(arr, findvalue, propname) {
    var isProp = typeof propname !== 'undefined';
    var result = $.grep(arr, function (e) {
        return (isProp ? (e[propname] == findvalue[propname]) : e == findvalue);
    });
    return result.length > 0;
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

(function ($) {
    $.fn.serializeObjectEx = function () {

        var self = this,
            json = {},
            push_counters = {},
            patterns = {
                "validate": /^[a-zA-Z][a-zA-Z0-9_]*(?:\[(?:\d*|[a-zA-Z0-9_]+)\])*$/,
                "key": /[a-zA-Z0-9_]+|(?=\[\])/g,
                "push": /^$/,
                "fixed": /^\d+$/,
                "named": /^[a-zA-Z0-9_]+$/
            };


        this.build = function (base, key, value) {
            base[key] = value;
            return base;
        };

        this.push_counter = function (key) {
            if (push_counters[key] === undefined) {
                push_counters[key] = 0;
            }
            return push_counters[key]++;
        };

        $.each(serializeArray(this), function () {

            // skip invalid keys
            if (!patterns.validate.test(this.name)) {
                return;
            }

            var k,
                keys = this.name.match(patterns.key),
                merge = this.value,
                reverse_key = this.name;

            while ((k = keys.pop()) !== undefined) {

                // adjust reverse_key
                reverse_key = reverse_key.replace(new RegExp("\\[" + k + "\\]$"), '');

                // push
                if (k.match(patterns.push)) {
                    merge = self.build([], self.push_counter(reverse_key), merge);
                }

                // fixed
                else if (k.match(patterns.fixed)) {
                    merge = self.build([], k, merge);
                }

                // named
                else if (k.match(patterns.named)) {
                    merge = self.build({}, k, merge);
                }
            }

            json = $.extend(true, json, merge);
        });

        return json;
    };
})(jQuery);
