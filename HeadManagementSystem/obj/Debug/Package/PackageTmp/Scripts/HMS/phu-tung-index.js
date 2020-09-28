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
HMS.namespace('PhuTung');
HMS.PhuTung = function () {
    var Global = {
        UrlAction: {
            Delete: '/PhuTung/Delete',
            Gets: '/PhuTung/Gets',
            Save: '/PhuTung/Save'
        },
        Data: {
            Id: 0,
            table: null,
            selectedObj: { Id: 0 },
            trans: [],
            PhuTungXe: []
        },
        Element: {
            tableId: 'tbPayments',
        },
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Delete = function (Id) {
        swal({
            title: "Bạn có chắc muốn xóa?",
            text: "Chú ý tất cả dữ liệu liên quan đến phụ tùng này cũng sẽ bị mất theo.",
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
        Global.Data.selectedObj = $.map(Global.Data.PhuTungXe, function (item, i) {
            if (item.Id == Id)
                return item;
        })[0];
        $('#soluong').val(Global.Data.selectedObj.SoLuong);
        $('#code').val(Global.Data.selectedObj.Ma);
        $('#name').val(Global.Data.selectedObj.Ten);
        $('#giamua').val(Global.Data.selectedObj.GiaMua);
        $('#giaban').val(Global.Data.selectedObj.GiaBan);
        $('#note').val(Global.Data.selectedObj.Note);
        RefreshControls();
    }

    this.Init = function () {
        RegisterEvent();
        Gets();
        $('#btSave').hide();
        RefreshControls();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));
    }

    var RegisterEvent = function () {
        $('#btSave').click(function () {
            Save();
        });

        $('#btCancel').click(function () {
            $('#name').val('');
            $('#note').val('');
            $('#giamua').val('');
            $('#code').val('');
            $('#giaban').val('');
            $('#soluong').val('');
            $('#btSave').hide();
            $('#btAdd').show();
            $('#acc').prop('disabled', false);
            $('#pass').prop('disabled', false);
            Global.Data.selectedObj = { Id: 0 };
            RefreshControls();
            $('.err-name,.err-code').empty();
        });

        $('#btAdd').click(function () {
            Save();
        });

        $('#btnhap-excel').click(() => { $('#file-mau').click() })
        $("#file-mau").change(function () {
            var data = new FormData();
            var files = $("#file-mau").get(0).files;
            if (files.length > 0) {
                data.append("file", files[0]);
            }
            $.ajax({
                url: "/phutung/InsertExcel",
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (res) {
                    if (res == '0')
                        $('.up-err').html('Nhập phụ tùng từ file excel thất bại. Vui lòng tải file mẫu và thử lại.');
                    else
                        Gets();
                },
                error: function (er) {
                    $('.up-err').html('Nhập phụ tùng từ file excel thất bại. Vui lòng tải file mẫu và thử lại.');
                }
            });
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
                    $('#phutung-table').empty();
                    Global.Data.table = null;
                    Global.Data.PhuTungXe.length = 0;
                }
                Global.Data.PhuTungXe = objs;
                InitTable(objs);
            }
        });
    }

    function InitTable(Objs) {
        Global.Data.table = $('#phutung-table').DataTable({
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
                { "orderable": true, "data": "Ma", "title": "Mã phụ tùng", 'width': '100px' },
                { "data": "Ten", "title": "Tên phụ tùng", 'width': '200px' },
                { "data": "SoLuong", "title": "Số lượng", 'width': '100px' },
                { "data": "GiaMua", "title": "giá mua", 'width': '100px' },
                { "data": "GiaBan", "title": "giá bán", 'width': '100px' },
                { "data": "Note", "title": "Ghi chú", 'width': 'calc(100% - 600px)', className: 'tb-PhuTung-note' },
                {
                     'width': '80px', className: 'tb-PhuTung-note', orderable: false,
                    render: (data, type, full, meta) => {
                        return `<a class="" href="/phutung/lichsu?id=${full.Id}"><i class="fa fa-print"></i> Xem lịch sử</a>`;
                    }
                },
                {
                    'className': 'table-edit-delete-col',
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return `<i class='fa fa-edit col-blue fa-lg pointer' onClick="Edit(${full.Id})"></i> <i class='fa fa-trash col-red fa-lg pointer' onClick="Delete(${full.Id})"></i>`;
                    }
                }]
        });
        $('#phutung-table_wrapper #phutung-table_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    }

    function Save() {
        if (IsValid()) {
            var transObj = {
                Id: Global.Data.selectedObj.Id,
                Ma: $('#code').val(),
                Ten: $('#name').val(),
                SoLuong: parseInt($('#soluong').val()),
                GiaMua: parseFloat($('#giamua').val()),
                Note: $('#note').val(),
                GiaBan: parseFloat($('#giaban').val()),
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
                    //window.location.href = '/PhuTung/Index';
                    else
                        swal("Lỗi nhập liệu", response.sms);
                }
            });
        }
    }

    function IsValid() {
        $('.err-code,.err-name').empty();
        var error = 0;
        if ($('#err-code').val() == '') {
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
