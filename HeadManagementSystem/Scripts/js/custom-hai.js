function RefreshControls() {  
    $('select').selectpicker('refresh')
    var controls = $('.form-control');
    $.each(controls, function (i, ctr) {
        if (!$(ctr).hasClass('fake-input')) {
            var parent = $(ctr).parent();
            if (parent.hasClass('form-line')) {
                if ($(ctr).get(0).tagName == 'DIV') {
                    if ($($(ctr).find('select')[0]).val() == '')
                        parent.removeClass('focused');
                    else
                        parent.addClass('focused');
                }
                else {
                    if ($(ctr).attr('type') == 'file')
                        parent.addClass('focused');
                    else {
                        if ($(ctr).val() == '')
                            parent.removeClass('focused');
                        else
                            parent.addClass('focused');
                    }
                }
            }
        }
    });
}
function parseCurency(number, currency) {
    return ( number.toFixed(1).replace(/\d(?=(\d{3})+\.)/g, '$&,')+' '+currency);
}
function ddMMyyyy(jsonDate) {
    return moment(jsonDate).format("MM/DD/YYYY");
}
function HHMM(jsonDate) {
    return moment(jsonDate).format("HH:mm");
}