if (typeof HMS == 'undefined' || !HMS) {
    var HMS = {};
}

HMS.namespace = function () {
    var a = arguments,
        o = null,
        i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = ('' + a[i]).split('.');
        o = HMS;
        for (j = (d[0] == 'HMS') ? 1 : 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
}
HMS.namespace('SQLConnect');
HMS.SQLConnect = function () {
    var Global = {
        UrlAction: {
            getDatabases: '/SQLConnect/GetDatabases',
            save: '/SQLConnect/Save'
        },
        Data: {
        },
        Element: {
        },
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
    }

    var RegisterEvent = function () {
        $('#save-sql').click(function () { Save(); });

        $('#ck_authen').change(function () {
            if ($('#ck_authen').is(':checked')) {
                $('#lg').prop("disabled", true);
                $('#ps').prop("disabled", true);
                GetDatabases();
            }
            else {
                $('#lg').prop("disabled", false);
                $('#ps').prop("disabled", false);
            }
        });

        $('#refresh-sql').click(function () { GetDatabases(); });
    }

    function GetDatabases() {
        $.ajax({
            url: Global.UrlAction.getDatabases,
            type: 'POST',
            data: JSON.stringify({ 'sname': $('#sname').val(), 'lg': $('#lg').val(), 'ps': $('#ps').val(), 'authen': $('#ck_authen').is(':checked') }),
            contentType: 'application/json charset=utf-8',
            // beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (data) {
                // $('.progress').addClass('hide');
                $('#database').selectpicker('destroy');
                $('#database').empty();
                if (data != null) {
                    $.each(data, function (i, item) {
                        $('#database').append('<option value="' + item.Code + '">' + item.Code + '</option>');
                    });
                }
                $('#database').selectpicker();
            }
        });
    }

    function Save() {
        $.ajax({
            url: Global.UrlAction.save,
            type: 'POST',
            data: JSON.stringify({ 'sname': $('#sname').val(), 'lg': $('#lg').val(), 'ps': $('#ps').val(), 'authen': $('#ck_authen').is(':checked'), 'dataName': $('#database').val() }),
            contentType: 'application/json charset=utf-8',
            // beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (sms) {
                alert(sms);
            }
        });
    }
}
