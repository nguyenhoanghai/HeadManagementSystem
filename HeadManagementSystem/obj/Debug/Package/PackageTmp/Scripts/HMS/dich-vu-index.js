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
HMS.namespace('DichVu');
HMS.DichVu = function () {
    var Global = {
        UrlAction: {
            Delete: '/DichVu/Delete',
            Gets: '/DichVu/Gets',
            Save: '/DichVu/Save'
        },
        Data: {
            Id: 0,
            table: null,
            selectedObj: { Id: 0 },
            trans: [],
            DichVuXe: []
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Delete = function (Id) {
        swal({
            title: "Bạn có chắc muốn xóa?",
            text: "Chú ý tất cả dữ liệu liên quan đến dịch vụ này cũng sẽ bị mất theo.",
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
        Global.Data.selectedObj = $.map(Global.Data.DichVuXe, function (item, i) {
            if (item.Id == Id)
                return item;
        })[0];
        $('#loai-cv').val(Global.Data.selectedObj.LoaiCVId);
        $('#congviec').val(Global.Data.selectedObj.CVId);
        $('#giamua').val(Global.Data.selectedObj.GiaMua);
        $('#tgxl').val(HHMM( Global.Data.selectedObj.TGXL));
        $('#note').val(Global.Data.selectedObj.Note); 
        RefreshControls();
    }

    this.Init = function () {
        //  InitConfirmBox();
        RegisterEvent();
        Gets();
        $('#btSave').hide();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));
    }

    var RegisterEvent = function () {
        $('#btSave').click(function () {
            Save();
        });

        $('#btCancel').click(function () {
            $('#loai-cv').val('');
            $('#congviec').val('');
            $('#giamua').val(0);
            $('#tgxl').val('00:00');
            $('#note').val(''); 
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
                    $('#dichvu-table').empty();
                    Global.Data.table = null;
                    Global.Data.DichVuXe.length = 0;
                }
                Global.Data.DichVuXe = objs;
                InitTable(objs);
            }
        });
    }

    function InitTable(Objs) {
        Global.Data.table = $('#dichvu-table').DataTable({
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
                { "width": "100px", "orderable": true, "data": "LoaiCV", "title": "Loại công việc" },
                { "width": "100px", "data": "MaCV", "title": "mã công việc" },
                { "data": "TenCV", "title": "Tên công việc", 'width': 'calc(100% - 210px)' },
                { "data": "TGXL", "title": "Thời gian xử lý", 'width': '100px', render: function (data, type, row) { return (data != null ? HHMM(data) : ''); } },
                { "data": "GiaMua", "title": "Giá dịch vụ", 'width': '100px', render: function (data, type, row) { return (data != null ? parseCurency(data,'vnđ') : ''); } },
                { "data": "Note", "title": "Ghi chú", 'width': 'calc(100% - 210px)' },
                {
                    'className': 'table-edit-delete-col',
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return `<i class='fa fa-edit col-blue fa-lg pointer' onClick="Edit(${full.Id})"></i> <i class='fa fa-trash col-red fa-lg pointer' onClick="Delete(${full.Id})"></i>`;
                    }
                }]
        });
        $('#dichvu-table_wrapper #dichvu-table_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    }

    function Save() {
        if (IsValid()) {
            var transObj = {
                Id: Global.Data.selectedObj.Id,
                LoaiCVId: $('#loai-cv').val(),
                CVId: $('#congviec').val(),
                GiaMua: $('#giamua').val(),
                strTGXL: $('#tgxl').val(),
                Note: $('#note').val()
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
                    //window.location.href = '/DichVu/Index';
                    else
                        swal("Lỗi nhập liệu", response.sms);
                }
            });
        }
    }

    function IsValid() {
        $('.err-loaicv,.err-congviec,.err-tgxl,.err-giamua').empty();
        var error = 0;
        if ($('#loai-cv').val() == '') {
            $('.err-loaicv').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        if ($('#congviec').val() == '') {
            $('.err-congviec').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        if ($('#tgxl').val() == '') {
            $('.err-tgxl').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        if ($('#giamua').val() == '') {
            $('.err-giamua').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        return error > 0 ? false : true;
    }
}
