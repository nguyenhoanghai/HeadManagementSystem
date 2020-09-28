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
HMS.namespace('ReceiptEdit');
HMS.ReceiptEdit = function () {
    var Global = {
        UrlAction: {
            Delete: '/Receipt/Delete',
            Gets: '/Receipt/Gets',
            Save: '/Receipt/Save',
            Find: '/Khachhang/Get',
        },
        Data: {
            Id: 0,
            table: null,
            selectedObj: { Id: 0 },
            trans: [],
            ReceiptXe: [],
            _DKId: 0,
            _SXId: 0,
            _TNId: 0,
            _TNganId: 0,
            _KTCuoiId: 0,
            _works: [],
            _dichvus: [],
            _phutungs: [],
            _dsPhuTungs: []
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Delete = function (Id) {
        swal({
            title: "Bạn có chắc muốn xóa?",
            text: "Chú ý tất cả dữ liệu liên quan đến khách hàng này cũng sẽ bị mất theo.",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Xóa",
            closeOnConfirm: true
        }, function () {
            Delete(Id);
        });
    }

    this.Init = function () {
        RegisterEvent();

        if (Global.Data._works.length > 0) {
            $.each(Global.Data._works, (i, item) => {
                $('#works').append('<option value="' + item.Code + '"></option>')
                $('#works-name').append('<option value="' + item.Name + '"></option>')
            });
        }
        if (Global.Data._phutungs.length > 0) {
            $.each(Global.Data._phutungs, (i, item) => {
                $('#phutungs').append('<option value="' + item.Code + '"></option>')
                $('#phutungs-name').append('<option value="' + item.Name + '"></option>')
            });
        }
        if (Global.Data._dichvus.length == 0)
            AddNew();

        if (Global.Data._dsPhuTungs.length == 0)
            AddNewPT();

        TinhTong();

        var $demoMaskedInput = $('.masked-input');
        $demoMaskedInput.find('.date').inputmask('dd/mm/yyyy', { placeholder: '__/__/____' });

        $('#dangky').val(Global.Data._DKId);
        $('#suaxe').val(Global.Data._SXId);
        $('#tiepnhan').val(Global.Data._TNId);
        $('#thungan').val(Global.Data._TNganId);
        $('#ktcuoi').val(Global.Data._KTCuoiId);

        RefreshControls();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));
    }

    var RegisterEvent = function () {
        $('#btSave').click(function () {
            // $('#frmReceipt').append('<input type="hidden" name ="TaoKH" id="TaoKH" value="1" />');
            if (Global.Data._dichvus.length > 0) {
                var index = 0;
                $.each(Global.Data._dichvus, (i, item) => {
                    if (item.DVId != null && item.DVId > 0) {
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].DVId" value="' + item.DVId + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].DVCode" value="' + item.DVCode + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].DVName" value="' + item.DVName + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].GiaBan" value="' + item.GiaBan + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].CKhau" value="' + item.CKhau + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="DichVus[' + index + '].GiaCK" value="' + item.GiaCK + '" />');
                        index++;
                    }
                })
            }
            if (Global.Data._dsPhuTungs.length > 0) {
                var index = 0;
                $.each(Global.Data._dsPhuTungs, (i, item) => {
                    if (item.PTId != null && item.PTId > 0) {
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].PTId" value="' + item.PTId + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].PTCode" value="' + item.PTCode + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].PTName" value="' + item.PTName + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].SoLuong" value="' + item.SoLuong + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].GiaBan" value="' + item.GiaBan + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].CKhau" value="' + item.CKhau + '" />');
                        $('#frmReceipt').append('<input type="hidden" name ="PhuTungs[' + index + '].GiaCK" value="' + item.GiaCK + '" />');
                        index++;
                    }
                })
            }
            $('#frmReceipt').append("<input type='hidden' name ='jsonDichVus' value='" + JSON.stringify(Global.Data._dichvus) + "' />");
            $('#frmReceipt').append("<input type='hidden' name ='jsonPhuTungs' value=\'" + JSON.stringify(Global.Data._dsPhuTungs) + "' />");
            $('#frmReceipt').submit();
            // Save();
        });

        $('#btClose').click(() => { window.location.href = '/Receipt/Close?id=' + $('#Id').val(); });

        $('#btAdd').click(function () {
            $('#frmReceipt').append('<input type="hidden" name ="TaoKH" id="TaoKH" value="1" />');
            $('#frmReceipt').submit();
        });

        $("#Receipt-type").on("changed.bs.select",
            function (e, clickedIndex, newValue, oldValue) {
                var inter = setInterval(function () {
                    clearInterval(inter);
                    RefreshControls();
                }, 500);
            });

        $('.btn-find').click(() => { window.location.href = ('/Receipt/Create?ma=' + $('#code').val()); });
    }

    _tr = () => { return '<tr></tr>' }
    _td = () => { return '<td></td>' }
    _input = (value, list, name, index, className) => { return '<input class="' + className + '" type="text" value="' + value + '" list="' + list + '" name="' + name + '" id="dv_' + index + '" />' }
    _delete = () => { return '<i class="fa fa-trash col-red fa-lg pointer" ></i>' }

    DrawTable = () => {
        if (Global.Data._dichvus.length > 0) {
            var tb = $('.tb-dichvu tbody');
            tb.empty();
            $.each(Global.Data._dichvus, (i, item) => {
                var tr = $(_tr());
                var td = $(_td());
                var code = $(_input(item.DVCode, 'works', '', ('code_' + i), 'i-code pointer'));
                code.change(() => { findCode(i, $(code).val()); });
                code.focus(() => { $(code).select(); })
                td.append(code);
                tr.append(td);

                td = $(_td());
                var name = $(_input(item.DVName, 'works-name', '', ('name_' + i), 'i-name pointer'));
                name.change(() => { findName(i, $(name).val()); })
                name.focus(() => { $(name).select(); })
                td.append(name);
                tr.append(td);

                td = $(_td());
                td.text(item.GiaBan);
                tr.append(td);

                td = $(_td());
                var ck = $(_input(item.CKhau, '', '', ('ck_' + i), 'i-number pointer'));
                ck.change(() => { Global.Data._dichvus[i].CKhau = $(ck).val(); TinhTong(); })
                td.append(ck);
                tr.append(td);

                td = $(_td());
                td.text(item.GiaCK);
                td.addClass('col-red bold');
                tr.append(td);

                td = $(_td());
                var btndelete = $(_delete());
                btndelete.click(() => { Global.Data._dichvus.splice(i, 1); TinhTong(); })
                td.append(btndelete);
                tr.append(td);

                tb.append(tr);
            });
        }
    }
    findName = (index, value) => {
        var obj = Global.Data._works.filter(x => x.Name == value)[0];
        if (obj) {
            Global.Data._dichvus[index].DVId = obj.Id;
            Global.Data._dichvus[index].DVName = value;
            Global.Data._dichvus[index].DVCode = obj.Code;
            Global.Data._dichvus[index].GiaBan = obj._double1;
        }
        TinhTong();
    }
    findCode = (index, value) => {
        var obj = Global.Data._works.filter(x => x.Code == value)[0];
        if (obj) {
            Global.Data._dichvus[index].DVId = obj.Id;
            Global.Data._dichvus[index].DVCode = value;
            Global.Data._dichvus[index].DVName = obj.Name;
            Global.Data._dichvus[index].GiaBan = obj._double1;
        }
        TinhTong();
    }
    AddNew = () => { Global.Data._dichvus.push({ DVId: null, DVCode: '', DVName: '', GiaBan: 0, CKhau: 0, GiaCK: 0 }); }

    DrawTablePhuTung = () => {
        if (Global.Data._dsPhuTungs.length > 0) {
            var tb = $('.tb-phutung tbody');
            tb.empty();
            $.each(Global.Data._dsPhuTungs, (i, item) => {
                var tr = $(_tr());
                var td = $(_td());
                var code = $(_input(item.PTCode, 'phutungs', '', ('pt_code_' + i), 'i-code pointer'));
                code.change(() => { findCode_PT(i, $(code).val()); });
                code.focus(() => { $(code).select(); })
                td.append(code);
                tr.append(td);

                td = $(_td());
                var name = $(_input(item.PTName, 'phutungs-name', '', ('pt_name_' + i), 'i-name pointer'));
                name.change(() => { findName_PT(i, $(name).val()); })
                name.focus(() => { $(name).select(); })
                td.append(name);
                tr.append(td);

                td = $(_td());
                var sl = $(_input(item.SoLuong, '', '', ('pt_sl_' + i), 'i-number pointer'));
                sl.change(() => {
                    var num = parseInt($(sl).val());
                    if (num <= 0)
                        num = 1;
                    Global.Data._dsPhuTungs[i].SoLuong = num;
                    TinhTong();
                });
                td.append(sl);
                tr.append(td);

                td = $(_td());
                td.text(item.GiaBan);
                tr.append(td);

                td = $(_td());
                var ck = $(_input(item.CKhau, '', '', ('pt_ck_' + i), 'i-number pointer'));
                ck.change(() => { Global.Data._dsPhuTungs[i].CKhau = $(ck).val(); TinhTong(); })
                td.append(ck);
                tr.append(td);

                td = $(_td());
                td.text(item.GiaCK);
                td.addClass('col-red bold');
                tr.append(td);

                td = $(_td());
                var btndelete = $(_delete());
                btndelete.click(() => { Global.Data._dsPhuTungs.splice(i, 1); TinhTong(); })
                td.append(btndelete);
                tr.append(td);

                tb.append(tr);
            });
        }
    }
    AddNewPT = () => { Global.Data._dsPhuTungs.push({ PTId: null, PTCode: '', PTName: '', SoLuong: 1, GiaBan: 0, CKhau: 0, GiaCK: 0 }); }
    findCode_PT = (index, value) => {
        var obj = Global.Data._phutungs.filter(x => x.Code == value)[0];
        if (obj) {
            if (obj.Data > 0) {
                Global.Data._dsPhuTungs[index].PTId = obj.Id;
                Global.Data._dsPhuTungs[index].PTCode = value;
                Global.Data._dsPhuTungs[index].PTName = obj.Name;
                Global.Data._dsPhuTungs[index].GiaBan = obj._double1;
            }
            else
                swal("Lỗi nhập liệu", "Số lượng tồn kho đã hết. Vui lòng nhập thêm phụ tùng.", "error");
        }
        TinhTong();
    }
    findName_PT = (index, value) => {
        var obj = Global.Data._phutungs.filter(x => x.Name == value)[0];
        if (obj) {
            if (obj.Data > 0) {
                Global.Data._dsPhuTungs[index].PTId = obj.Id;
                Global.Data._dsPhuTungs[index].PTCode = obj.Code;
                Global.Data._dsPhuTungs[index].PTName = value;
                Global.Data._dsPhuTungs[index].GiaBan = obj._double1;
            }
            else
                swal("Lỗi nhập liệu", "Số lượng tồn kho đã hết. Vui lòng nhập thêm phụ tùng.", "error");
        }
        TinhTong();
    }

    TinhTong = () => {
        var total = 0;
        var totalPT = 0;
        $.each(Global.Data._dichvus, (i, item) => {
            var gia = item.GiaBan;
            var ck = item.CKhau;
            item.GiaCK = gia - ((gia * ck) / 100);
            total += item.GiaCK;
        });

        $.each(Global.Data._dsPhuTungs, (i, item) => {
            var gia = item.GiaBan;
            var ck = item.CKhau;
            item.GiaCK = (gia - ((gia * ck) / 100)) * item.SoLuong;
            totalPT += item.GiaCK;
        });
        $('#total_dv').html(total);
        $('#total_pt').html(totalPT);
        $('#total').html(total + totalPT);
        if (Global.Data._dichvus[Global.Data._dichvus.length - 1].DVId)
            AddNew();

        if (Global.Data._dsPhuTungs[Global.Data._dsPhuTungs.length - 1].PTId)
            AddNewPT();
        DrawTable();
        DrawTablePhuTung();
        // console.log(Global.Data._dichvus);
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
                    $('#kh-table').empty();
                    Global.Data.table = null;
                    Global.Data.ReceiptXe.length = 0;
                }
                Global.Data.ReceiptXe = objs;
                InitTable(objs);
            }
        });
    }

    function InitTable(Objs) {
        Global.Data.table = $('#kh-table').DataTable({
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
                { "orderable": true, "data": "Ma", "title": "Mã khách hàng", 'width': '100px' },
                { "data": "Ten", "title": "tên khách hàng", 'width': '150px' },
                { "data": "NSinh", "title": "ngày sinh", 'width': '50px', render: (data, type, full, meta) => { return (data != null ? ddMMyyyy(data) : '') } },
                { "data": "GTinh", "title": "giới tính", 'width': '30px', render: (data, type, full, meta) => { return (data ? '<i class="fa fa-male col-blue fa-2x"></i>' : '<i class="fa fa-female col-pink fa-2x"></i>') } },
                { "data": "DThoai", "title": "điện thoại", 'width': '70px' },
                { "data": "DChi", "title": "địa chỉ", 'width': '300px' },
                { "data": "Note", "title": "Ghi chú", 'width': 'calc(100% - 700px)', className: 'tb-Receipt-note' },
                {
                    'className': 'table-edit-delete-col',
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return `<i class='fa fa-edit col-blue fa-lg pointer' onClick="Edit(${full.Id})"></i> <i class='fa fa-trash col-red fa-lg pointer' onClick="Delete(${full.Id})"></i>`;
                    }
                }]
        });
        $('#kh-table_wrapper #kh-table_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    }

    function Save() {
        if (IsValid()) {
            var transObj = {
                Id: Global.Data.selectedObj.Id,
                Ma: $('#code').val(),
                Ten: $('#name').val(),
                NSinh: moment($('#ngaysinh').val(), ''),
                GTinh: $('#gioitinh').val() == '0' ? false : true,
                DThoai: $('#phone').val(),
                DChi: $('#address').val(),
                TPho: $('#tinh-thanhpho').val(),
                Huyen: $('#quan-huyen').val(),
                Phuong: $('#phuong').val(),
                JobId: $('#jobid').val(),
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
                    //window.location.href = '/Receipt/Index';
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

    findKH = () => {
        if ($('#code').val() != '') {
            $.ajax({
                url: Global.UrlAction.Find,
                type: 'POST',
                data: JSON.stringify({ 'data': $('#code').val() }),
                contentType: 'application/json charset=utf-8',
                // beforeSend: function () { $('.progress').removeClass('hide'); },
                success: function (obj) {
                    if (obj)
                        Bind(obj);
                    else
                        swal("Thông báo", "Không tìn thấy khách hàng trong hệ thống.!");
                }
            });
        }
    }

    function Bind(obj) {
        $('#tinh-thanhpho').val(obj.TPho);
        Global.Data.HuyenValue = obj.Huyen;
        $('#ngaysinh').val(moment(obj.NSinh).format('DD/MM/YYYY'));
        $('#name').val(obj.Ten);
        $('#gioitinh').val(obj.GTinh ? '1' : '0');
        $('#phone').val(obj.DThoai);
        $('#address').val(obj.DChi);
        $('#phuong').val(obj.Phuong);
        $('#note').val(obj.Note);
        $('#jobid').val(obj.JobId);
        $('#code').val(obj.Ma);
        $('#tinh-thanhpho').change();
        RefreshControls();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));
    }

    SavePhieu = () => {
        if (IsValid()) {
            var transObj = {
                SoPhieu: $('#sophieu').val(),
                MaKH: $('#code').val(),
                ModelId: $('#model').val(),
                Ngay: moment($('#ngaytao').val(), ''),
                SoKhung: $('#sokhung').val(),
                SoMay: $('#somay').val(),
                BienSo: $('#bienso').val(),
                Km: parseInt($('#km').val()),

                yeucau: parseFloat($('#giaban').val()),
                nhanxet: parseFloat($('#chietkhau').val()),
                trangthai: parseFloat($('#chietkhau').val()),
                Note: $('#note').val()
            }
            $.ajax({
                url: Global.UrlAction.SavePhieu,
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
                    //window.location.href = '/BanHang/Index';
                    else
                        swal("Lỗi nhập liệu", response.sms);
                }
            });
        }
    }
}
