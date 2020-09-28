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
HMS.namespace('Work');
HMS.Work = function () {
    var Global = {
        UrlAction: {
            Delete: '/Work/Delete',
            Gets: '/Work/Gets',
            Save: '/Work/Save'
        },
        Data: {
            Id: 0,
            table: null,
            selectedObj: { Id: 0 },
            trans: [],
            WorkXe: []
        } 
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Delete = function (Id) {
        swal({
            title: "Bạn có chắc muốn xóa?",
            text: "Chú ý tất cả dữ liệu liên quan đến công việc này cũng sẽ bị mất theo.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Xóa",
            closeOnConfirm: true
        }, function () {
            Delete(Id);
        });
    }

    this.Edit = function (Id) {
        $('#btSave').show();
        $('#btAdd').hide();
        Global.Data.selectedObj = $.map(Global.Data.WorkXe, function (item, i) {
            if (item.Id == Id)
                return item;
        })[0];
        $('#name').val(Global.Data.selectedObj.Name);
        $('#code').val(Global.Data.selectedObj.Code);
        RefreshControls();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));
    }

    this.Init = function () {
        //  InitConfirmBox();
        RegisterEvent();
        Gets();
        $('#btSave').hide();
    }

    var RegisterEvent = function () {
        $('#btSave').click(function () {
            Save();
        });

        $('#btCancel').click(function () {
            $('#name').val('');
            $('#code').val('');
            $('#btSave').hide();
            $('#btAdd').show();
            Global.Data.selectedObj = { Id: 0 };
            RefreshControls();
            $('.err-name,.err-code').empty();
        });

        $('#btAdd').click(function () {
            Save();
        });
    }

    function Delete(Id) {
        $.ajax({
            url: Global.UrlAction.Delete,
            type: 'POST',
            data: JSON.stringify({ 'Id': Id }),
            contentType: 'application/json charset=utf-8',
            // beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (response) {
                // $('.progress').addClass('hide');
                if (response.success) {
                    Gets();
                }
                else
                    swal("Lỗi", response.responseText, "error");
            }
        });
    }

    function Gets() {
        $.ajax({
            url: Global.UrlAction.Gets,
            type: 'POST',
            data: null,
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (objs) {
                $('.progress').addClass('hide');
                if (Global.Data.table != null) {
                    Global.Data.table.destroy();
                    $('#work-table').empty();
                    Global.Data.table = null;
                    Global.Data.WorkXe.length = 0;
                }
                Global.Data.WorkXe = objs;
                InitTable(objs);
            }
        });
    }

    function InitTable(Objs) {
        Global.Data.table = $('#work-table').DataTable({
            responsive: true, 
            "data": Objs,
            "dom": '<"top"<"col-sm-3 m-b-0 p-l-0"l><"col-sm-9 m-b-0"f><"clear">>rt<"bottom"<"col-sm-6 p-l-0"i><"col-sm-6 m-b-0"p><"clear">><"clear">',
            "oLanguage": {
                "sSearch": "Bộ lọc",
                "sLengthMenu": "Hiển thị _MENU_ dòng mỗi trang",
                "sInfo": "Hiển thị từ _START_ - _END_ trong _TOTAL_ dòng",
                'paginate': {
                    'previous': '<span class="prev-icon"></span>',
                    'next': '<span class="next-icon"></span>'
                }
            },
            "columns": [
                {
                    "width": "200px",
                    "orderable": true,
                    "data": "Code",
                    "title": "mã"
                },
                { "data": "Name", "title": "tên công việc", 'width': 'calc(100% - 210px)'  },
                {
                    'className': 'table-edit-delete-col', 
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return `<i class='fa fa-edit col-blue fa-lg pointer' onClick="Edit(${full.Id})"></i> <i class='fa fa-trash col-red fa-lg pointer' onClick="Delete(${full.Id})"></i>`;
                    }
                }]
        });
        $('#work-table_wrapper #work-table_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    }

    function Save() {
        if (IsValid()) {
            var transObj = {
                Id: Global.Data.selectedObj.Id,
                Code: $('#code').val(),
                Name: $('#name').val()
            }
            $.ajax({
                url: Global.UrlAction.Save,
                type: 'POST',
                data: JSON.stringify(transObj),
                contentType: 'application/json charset=utf-8',
                // beforeSend: function () { $('.progress').removeClass('hide'); },
                success: function (response) {
                    //  $('.progress').addClass('hide');
                    if (response.IsSuccess) {
                        Gets();
                        $('#btCancel').click();
                    }
                    //window.location.href = '/Work/Index';
                    else
                        swal("Lỗi nhập liệu", response.sms);
                }
            });
        }
    }

    function IsValid() {
        $('.err-name,.err-code').empty();
        var error = 0;
        if ($('#code').val() == '') {
            $('.err-code').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }  
        if ($('#name').val() == '') {
            $('.err-name').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }  
        return error > 0 ? false : true;
    }
}
