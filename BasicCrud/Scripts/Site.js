/* Azure Statistics
-------------------------------------------------- */
window.appInsights = window.appInsights || function (ai) {
    function f(t) { ai[t] = function () { var i = arguments; ai.queue.push(function () { ai[t].apply(ai, i) }) } }
    var t = document, r = "script", u = t.createElement(r), i; for (u.src = ai.url || "//az416426.vo.msecnd.net/scripts/a/ai.0.js", t.getElementsByTagName(r)[0].parentNode.appendChild(u), ai.cookie = t.cookie, ai.queue = [], i = ["Event", "Exception", "Metric", "PageView", "Trace"]; i.length;) f("track" + i.pop());
    return ai;
}({
    iKey: "84d98a19-5d47-48cb-8b1c-b1e316a56708"
});

appInsights.trackPageView();


/* Completed Check and Minus
-------------------------------------------------- */
$('.check, .minus').on('click', function () {
    var token = $('input[name="__RequestVerificationToken"]').val(),
        id = $(this).data('id'),
        value = $(this).data('value');

    if (value) {
        if ($(this).hasClass('fa-minus-square minus')) {
            $(this).removeClass('fa-minus-square minus').addClass('fa-check-square check');
            $(this).attr('data-value', true);
            value = true;
        } else {
            $(this).removeClass('fa-check-square check').addClass('fa-minus-square minus');
            $(this).attr('data-value', false);
            value = false;
        }
    } else {
        if ($(this).hasClass('fa-check-square check')) {
            $(this).removeClass('fa-check-square check').addClass('fa-minus-square minus');
            $(this).attr('data-value', false);
            value = false;
        } else {
            $(this).removeClass('fa-minus-square minus').addClass('fa-check-square check');
            $(this).attr('data-value', true);
            value = true;
        }
    }

    $.ajax({
        type: 'POST',
        url: '/Tasks/IsFinished',
        data: {
            __RequestVerificationToken: token,
            id: id,
            value: value
        }
    });
});


/* Sortable for Lists and Tasks Tables
-------------------------------------------------- */
$('#sortableTasks').sortable({
    axis: 'y',
    update: function () {
        var token = $('input[name="__RequestVerificationToken"]').val(),
            newTasks = $(this).sortable('toArray'),
            listId = $('input[name="listId"]').val();

        $.ajax({
            type: 'POST',
            url: '/Tasks/Reorder',
            data: {
                __RequestVerificationToken: token,
                newTasks: newTasks,
                listId: listId
            }
        });
    }
});

/* Scroll up arrow
-------------------------------------------------- */
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('.scrollup').fadeIn();
    } else {
        $('.scrollup').fadeOut();
    }
});

$('.scrollup').click(function () {
    $("html, body").animate({
        scrollTop: 0
    }, 600);
    return false;
});

/* Animations
-------------------------------------------------- */
$('a').popover();

$('#hide').on('click', function () {
    $('.check').parent().parent().parent().toggle();

    if ($.trim($(this).text()) === 'Hide Completed') {
        $(this).text('Show Completed');
    } else {
        $(this).text('Hide Completed');
    }
});

/* Bootstrap Form Validation
-------------------------------------------------- */
$('#list').bootstrapValidator({
    message: 'This value is not valid',
    fields: {
        Name: {
            validators: {
                notEmpty: {
                    message: 'Is required and cannot be empty'
                },
                remote: {
                    url: '/Lists/Validate/',
                    data: function (validator) {
                        return {
                            __RequestVerificationToken: validator.getFieldElements('__RequestVerificationToken').val(),
                            name: validator.getFieldElements('Name').val()
                        };
                    },
                    message: 'That name is not available'
                }
            }
        },
        Type: {
            validators: {
                notEmpty: {
                    message: 'Is required and cannot be empty'
                }
            }
        }
    }
});

$('#task').bootstrapValidator({
    message: 'This value is not valid',
    fields: {
        Name: {
            validators: {
                notEmpty: {
                    message: 'Is required and cannot be empty'
                }
            }
        },
        Quantity: {
            validators: {
                integer: {
                    message: 'The value is not an integer'
                }
            }
        }
    }
});