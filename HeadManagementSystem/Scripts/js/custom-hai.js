function RefreshControls() {  
    $('select').selectpicker('refresh')
    var controls = $('.form-control');
    $.each(controls, function (i, ctr) {
        if (!$(ctr).hasClass('fake-input')) {
            var parent = $(ctr).parent();
            if (parent.hasClass('form-line')) {
                if ($(ctr).get(0).tagName == 'DIV') {
                    //if ($($(ctr).find('select')[0]).val() == '')
                    //    parent.removeClass('focused');
                    //else
                        parent.addClass('focused');
                }
                else {
                    if ($(ctr).attr('type') == 'file'  )
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

function createDatatable(tableId, searchByDate,excelbutton) {
    $('#' + tableId).DataTable({
        responsive: true,
        "dom": '<"top"<"col-sm-3 m-b-0 p-l-0"l><"col-sm-9 m-b-0"f><"clear">>rt<"bottom"<"col-sm-6 p-l-0"i><"col-sm-6 m-b-0"p><"clear">><"clear">',
        "oLanguage": {
            "sSearch": "Bộ lọc",
            "sLengthMenu": "Hiển thị _MENU_ dòng mỗi trang",
            "sInfo": "Hiển thị từ _START_ - _END_ trong _TOTAL_ dòng",
            'paginate': {
                'previous': '<span class="prev-icon"></span>',
                'next': '<span class="next-icon"></span>'
            }
        }
    });
    if (searchByDate)
    $('#' + tableId + '_wrapper #' + tableId +'_filter').append($('<label><div class="input-daterange input-group m-b-0" id="bs_datepicker_range_container"><div class="form-line"><input type="text" class="form-control from-date" placeholder="Từ ngày"></div><span class="input-group-addon"></span><div class="form-line"><input type="text" class="form-control to-date" placeholder="Đến ngày"></div><button class="btn btn-primary btn-table-search-range">Tìm theo ngày</button></div></label>'));
    if (excelbutton)
    $('#' + tableId + '_wrapper #' + tableId + '_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));

}