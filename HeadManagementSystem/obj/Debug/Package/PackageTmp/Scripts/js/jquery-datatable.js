$(function () {
    $('.basic-data-table').DataTable({ 
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
    $('#js-customer-index_wrapper #js-customer-index_filter').append($('<label><div class="input-daterange input-group m-b-0" id="bs_datepicker_range_container"><div class="form-line"><input type="text" class="form-control" placeholder="Từ ngày"></div><span class="input-group-addon"></span><div class="form-line"><input type="text" class="form-control" placeholder="Đến ngày"></div><button class="btn btn-primary btn-table-search-range">Tìm theo ngày</button></div></label>'));    
    $('#js-customer-index_wrapper #js-customer-index_filter').append($('<label><i class="fa fa-file-excel-o fa-lg col-red pointer" title="xuất excel" aria-hidden="true"></i></label>'));
   
  
    //Exportable table
    $('.js-exportable').DataTable({
        dom: 'Bfrtip',
        responsive: true,
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });

    //Bootstrap datepicker plugin
    $('#bs_datepicker_container input').datepicker({
        autoclose: true,
        container: '#bs_datepicker_container'
    });

    $('#bs_datepicker_component_container').datepicker({
        autoclose: true,
        container: '#bs_datepicker_component_container'
    });
    //
    $('#bs_datepicker_range_container').datepicker({
        autoclose: true,
        container: '#bs_datepicker_range_container'
    });
});