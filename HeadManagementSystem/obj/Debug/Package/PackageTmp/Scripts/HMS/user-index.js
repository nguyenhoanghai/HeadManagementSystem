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
HMS.namespace('User');
HMS.User = function () {
    var Global = {
        UrlAction: {
            Delete: '/User/Delete',
            Gets: '/User/Gets',
            Save: '/User/Save'
        },
        Data: {
            Id: 0,
            table: null,
            selectedObj: { Id: 0 },
            trans: [],
            UserXe: []
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
            text: "Chú ý tất cả dữ liệu liên quan đến nhân viên này cũng sẽ bị mất theo.",
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
        Global.Data.selectedObj = $.map(Global.Data.UserXe, function (item, i) {
            if (item.Id == Id)
                return item;
        })[0];
        $('#user-type').val(Global.Data.selectedObj.LoaiNV);
        $('#code').val(Global.Data.selectedObj.Ma);
        $('#name').val(Global.Data.selectedObj.Ten);
        $('#phone').val(Global.Data.selectedObj.DienThoai);
        $('#address').val(Global.Data.selectedObj.DiaChi);
        $('#note').val(Global.Data.selectedObj.Note);
        $('#acc').prop('disabled', true);
        $('#pass').prop('disabled', true);
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
            $('#user-type').val('');
            $('#code').val('');
            $('#phone').val('');
            $('#acc').val('');
            $('#pass').val('');
            $('#address').val('');
            $('#btSave').hide();
            $('#btAdd').show();
            $('#acc').prop('disabled',false);
            $('#pass').prop('disabled', false);
            Global.Data.selectedObj = { Id: 0 };
            RefreshControls();
            $('.err-name').empty();
        });

        $('#btAdd').click(function () {
            Save();
        });

        $("#user-type").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var inter = setInterval(function () {
                    clearInterval(inter);
                    RefreshControls();
                }, 500); 
            });

        $('#btnhap-excel').click(() => { $('#file-mau').click() })
        $("#file-mau").change(function () {
            var data = new FormData();
            var files = $("#file-mau").get(0).files;
            if (files.length > 0) {
                data.append("file", files[0]);
            }
            $.ajax({
                url: "/user/InsertExcel",
                type: "POST",
                processData: false,
                contentType: false,
                data: data,
                success: function (res) {
                    if (res == '0')
                        swal("Thông báo", "Nhập nhân viên từ file excel thất bại. Vui lòng tải file mẫu và thử lại.!",'error'); 
                    else
                        Gets();
                },
                error: function (er) {
                    swal("Thông báo", "Nhập nhân viên từ file excel thất bại. Vui lòng tải file mẫu và thử lại.!", 'error');
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
                    $('#User-table').empty();
                    Global.Data.table = null;
                    Global.Data.UserXe.length = 0;
                }
                Global.Data.UserXe = objs;
                InitTable(objs);
            }
        });
    }

    function InitTable(Objs) {
        Global.Data.table = $('#user-table').DataTable({
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
                    "orderable": true,
                    "width": "100px",
                    "data": "strLoaiNV",
                    "title": "Loại nhân viên"
                },
                { "data": "Ma", "title": "Mã nhân viên", 'width': '100px' },
                { "data": "Ten", "title": "Họ tên", 'width': '200px' },
                { "data": "DienThoai", "title": "điện thoại", 'width': '100px' },
                { "data": "DiaChi", "title": "địa chỉ", 'width': '300px' },
                { "data": "Note", "title": "Ghi chú", 'width': 'calc(100% - 800px)', className: 'tb-user-note' },
                {
                    'className': 'table-edit-delete-col',
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return `<i class='fa fa-edit col-blue fa-lg pointer' onClick="Edit(${full.Id})"></i> <i class='fa fa-trash col-red fa-lg pointer' onClick="Delete(${full.Id})"></i>`;
                    }
                }]
        });
        $('#user-table_wrapper #user-table_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    }

    function Save() {
        if (IsValid()) {
            var transObj = {
                Id: Global.Data.selectedObj.Id,
                LoaiNV: $('#user-type').val(),
                Ma: $('#code').val(),
                Ten: $('#name').val(),
                DienThoai: $('#phone').val(),
                DiaChi: $('#address').val(),
                Note: $('#note').val(),
                TaiKhoan: $('#acc').val(),
                MatKhau: $('#pass').val()
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
                    //window.location.href = '/User/Index';
                    else
                        swal("Lỗi nhập liệu", response.sms);
                }
            });
        }
    }

    function IsValid() {
        $('.err-user-type,.err-code,.err-name').empty();
        var error = 0;
        if ($('#user-type').val() == '') {
            $('.err-user-type').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        
        if ($('#name').val() == '') {
            $('.err-name').append('<span class="err-msg">Trường không được để trống.</span>');
            error++;
        }
        return error > 0 ? false : true;
    }
}
