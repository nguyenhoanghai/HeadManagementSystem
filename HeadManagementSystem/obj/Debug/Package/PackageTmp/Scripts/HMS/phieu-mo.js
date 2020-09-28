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
HMS.namespace('PhieuMo');
HMS.PhieuMo = function () {
    var Global = {
        UrlAction: {
        },
        Data: {
            data: []
        }
    }
    this.GetGlobal = function () {
        return Global;
    }


    this.Init = function () {
        RegisterEvent();
        InitTable();
    }

    var RegisterEvent = function () {

    }

    function InitTable( ) {
        Global.Data.table = $('#table-phieu-mo').DataTable({
            responsive: true,
            "data": Global.Data.data,
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
            "order": [[9, "desc"]],
            "columns": [
                {
                    "data": "Index", "title": "Số phiếu", 'width': '50px',
                    render: (data, type, full, meta) => {
                        return `<div>
                                    <div><a href="/Receipt/edit?id=${full.Id}">${full.SoPhieu}</a></div>
                                    <button class="btn-primary"><i class="fa fa-print"></i> In phiếu</button>
                                </div>`;
                    }
                }, 
                {
                    "data": "MaKH", "title": "Khách hàng", 'width': '50px', "orderable": false,
                    render: (data, type, full, meta) => {
                        return `<div>
                                    <div><a href="/khachhang/detail?id=${full.KHId}">${full.MaKH}</a></div>
                                    <div>${full.KHTen}</div>
                                    <div>${full.KHDThoai}</div>
                                </div>`;
                    }
                },
                {
                    "data": "Model", "title": "Thông tin xe", 'width': '50px', "orderable": false,
                    render: (data, type, full, meta) => {
                        return `<div>
                                    <div>${full.Model}</div>
                                    <div>${full.SoKhung}</div>
                                    <div>${full.SoMay}</div>
                                    <div>${full.BienSo}</div>
                                </div>`;
                    }
                },
                {
                    "data": "TongGiaNhap", "title": "Giá nhập", 'width': '30px',
                    render: (data, type, full, meta) => {
                        return (data ? data : '');
                    }
                },
                { "data": "SLPhuTung", "title": "Số lượng phụ tùng", 'width': '30px' },
                { "data": "TongGiaBan", "title": "Tổng tiền phụ tùng", 'width': '70px' },
                { "data": "TienCong", "title": "Tiền công", 'width': '70px' },
                { "data": "TongTien", "title": "Tổng tiền", 'width': '70px' },
                { "data": "Yeucau", "title": "Yêu cầu khách hàng", 'width': '70px',"orderable": false, },
                {
                    "orderable": true,"data": "Ngay", "title": "Ngày tạo phiếu", 'width': '30px',
                    render: (data, type, full, meta) => {
                        return (data ? moment(data).format('DD/MM/YYYY hh:mm') : '');
                    }
                } ]
        });
        $('#table-phieu-mo_wrapper #table-phieu-mo_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
    } 
}
