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
HMS.namespace('MemberExtract');
HMS.MemberExtract = function () {
    var Global = {
        UrlAction: {
            Get: '/Members/SearchMemberExtract',
            exportActionMembers: '/Members/exportActionMembersCSV',
            exportMaillingLabelDOC: '/Members/exportMaillingLabelDOC',
            exportMemberCardExtendedCSV: '/Members/exportMemberCardExtendedCSV'
        },
        Data: {
            table: null,
            members: []
        }
    }
    this.GetGlobal = function () {
        return Global;
    }

    this.Init = function () {
        RegisterEvent();
        $('#cbExtract').change();
    }

    this.DrawTable = function () {
        switch (parseInt($('#cbExtract').val())) {
            case 1: DrawTable_Mailing_labels(Global.Data.members); break;
            case 2: DrawTable_ActionMember(Global.Data.members); break;
            case 3: DrawTable_MemberCardExtended(Global.Data.members); break;
            case 4: DrawTable_Eligibility(Global.Data.members); break;
            case 5: DrawTable_OutOfWork(Global.Data.members); break;
            case 6: DrawTable_T_shirt(Global.Data.members); break;
            case 7: DrawTable_MissingForms(Global.Data.members); break;
        }
        ReDrawFilterAndLengthBoxForGrid(Global.Data.table);
        $('.table-responsive').show();
    }

    var RegisterEvent = function () {
        $('#btnSearch').click(function () {
            window.location.href = '/Members/MemberExtract?extractType=' + $('#cbExtract').val();
            //  GetData();
        });

        $('#btnExport').click(function () {
            switch (parseInt($('#cbExtract').val())) {
                case 1: window.location.href = Global.UrlAction.exportMaillingLabelDOC; break;
                case 2: window.location.href = Global.UrlAction.exportActionMembers; break;
                case 3: window.location.href = Global.UrlAction.exportMemberCardExtendedCSV; break;
            }
        });

        $('#cbExtract').change(function () {
            $('#btnExport').addClass('hide');

            if ($('#cbExtract').val() == '1') {
                $('#btnExport').removeClass('hide');
                $('#btnExport').html('Export (.Docx)');
            }
            if ($('#cbExtract').val() == '2') {
                $('#btnExport').removeClass('hide');
                $('#btnExport').html('Export (.csv)');
            }
            if ($('#cbExtract').val() == '3') {
                $('#btnExport').removeClass('hide');
                $('#btnExport').html('Export (.csv)');
            }
        });
    }

    function GetData() {
        $.ajax({
            url: Global.UrlAction.Get,
            type: 'POST',
            data: JSON.stringify({ 'extractType': parseInt($('#cbExtract').val()) }),
            contentType: 'application/json charset=utf-8',
            beforeSend: function () { $('.progress').removeClass('hide'); },
            success: function (objs) {
                $('.progress').addClass('hide');
                if (Global.Data.table != null) {
                    Global.Data.table.destroy();
                    $('#tbMembers').empty();
                    Global.Data.table = null;
                }
                switch (parseInt($('#cbExtract').val())) {
                    case 1: DrawTable_Mailing_labels(JSON.parse(objs)); break;
                    case 2: DrawTable_ActionMember(JSON.parse(objs)); break;
                    case 3: DrawTable_MemberCardExtended(JSON.parse(objs)); break;
                    case 4: DrawTable_Eligibility(JSON.parse(objs)); break;
                    case 5: DrawTable_OutOfWork(JSON.parse(objs)); break;
                    case 6: DrawTable_T_shirt(JSON.parse(objs)); break;
                    case 7: DrawTable_MissingForms(JSON.parse(objs)); break;
                }
                ReDrawFilterAndLengthBoxForGrid(Global.Data.table);
                $('.table-responsive').show();
            }
        });
    }


    function DrawTable_Mailing_labels(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "order": [[0, "asc"]],
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "columns": [
                {
                    "orderable": true,
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                { "data": "strLocalUnion", "title": "Local Union" },
                { "data": "MailingAddressLine1", "title": "Mailing Address 1" },
                { "data": "MailingAddressLine2", "title": "Mailing Address 2" },
                { "data": "MailingAddressCity", "title": "City" },
                { "data": "MailingAddressState", "title": "State" },
                { "data": "MailingAddressZip", "title": "Zip Code" },
                {
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return ` <a   href="/Members/Details/${full.Id}">Details</a> `;
                    }
                }]
        });
    }

    function DrawTable_MemberCardExtended(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "order": [[0, "asc"]],
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "columns": [
                {
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                { "data": "MailingAddressLine1", "title": "Mailing Address 1" },
                { "data": "MailingAddressLine2", "title": "Mailing Address 2" },
                { "data": "MailingAddressCity", "title": "Mailing Address City" },
                { "data": "MailingAddressState", "title": "Mailing Address State" },
                { "data": "MailingAddressZip", "title": "Mailing Address Zip" },
                { "data": "CellPhone", "title": "Cell Phone" },
                {
                    "data": "BirthDate",
                    "title": "Birth Date",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return data ? moment(data).format("MM/DD/YYYY") : '';
                        }
                        else return '';
                    }
                },
                { 'data': 'strClassification', 'title': 'Classification' },
                {
                    "data": "InitiationDate",
                    "title": "Initiation Date",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return moment(data).format("MM/DD/YYYY");
                        } else return '';
                    }
                },
                {
                    "data": "PaidThrough",
                    "title": "Paid Through",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return data ? moment(data).format("MM/DD/YYYY") : '';
                        }
                        else return '';
                    }
                },
                { 'data': 'Str_MemberStatus', 'title': 'Status' },
                {
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return ` <a href="/Members/Details/${full.Id}">Details</a> `;
                    }
                }]
        });
    }

    function DrawTable_Eligibility(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "order": [[0, "asc"]],
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "columns": [
                { "data": "strLocalUnion", "title": "Local Union" },
                {
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                {
                    "data": "SSN", "title": "SSN",
                    //render: function (data, type, row) {
                    //    if (data != null && data != '' && data.length > 4)
                    //        return data.substring(data.length - 4);
                    //    else
                    //        return data;
                    //}
                },
                { "data": "strClassification", "title": "Class" },
                { "data": "CellPhone", "title": "Mobile Phone" },
                {
                    "data": "PaidThrough",
                    "title": "Paid Through",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return data ? moment(data).format("MM/DD/YYYY") : '';
                        }
                        else return '';
                    }
                },
                {
                    "data": "InitiationDate",
                    "title": "Initiation Date",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return moment(data).format("MM/DD/YYYY");
                        } else return '';
                    }
                },
                { "data": "Island", "title": "Island" },
                {
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return ` <a   href="/Members/Details/${full.Id}">Details</a> `;
                    }
                }]
        });
    }

    function DrawTable_OutOfWork(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            // "order": [[3, "desc"]],
            "ordering": true,
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "order": [[3, "desc"]],
            "columns": [
                {
                    "orderable": true,
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                { "orderable": true, "data": "CellPhone", "title": "Mobile Phone" },
                {
                    "orderable": true,
                    "data": "NotifiedUnionDate", "title": "Notified Union Date",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return data ? moment(data).format("MM/DD/YYYY") : '';
                        }
                        else return '';
                    }
                },
                {
                    "orderable": true,
                    "data": "LastEmployedDate", "title": "Last Employed Date",
                    render: function (data, type, row) {
                        if (data) {
                            if (type === "sort" || type === "type") {
                                return data;
                            }
                            return data ? moment(data).format("MM/DD/YYYY") : '';
                        }
                        else return '';
                    }
                },
                {
                    "orderable": false,
                    "render": function (data, type, full, meta) {
                        return ` <a   href="/Members/Details/${full.Id}">Details</a> `;
                    }
                }]
        });
    }

    function DrawTable_T_shirt(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true, // this is for disable filter (search box)
            "orderMulti": true, // for disable multiple column at once
            "ordering": true,
            //"paging": false,
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "order": [[0, "asc"]],
            //"paging": false,
            //"info": false,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "columns": [
                { "data": "Island", "title": "Island" },
                { "data": "S", "title": "S" },
                { "data": "M", "title": "M" },
                { "data": "L", "title": "L" },
                { "data": "U", "title": "U" },
                { "data": "XL", "title": "XL" },
                { "data": "XXL", "title": "XXL" },
                { "data": "XXXL", "title": "3XL" },
                { "data": "XXXXL", "title": "4XL" }
            ],
            "drawCallback": function (settings) {
                $('#tbMembers  tr:last').addClass('bold');
            }
        });
    }

    function DrawTable_MissingForms(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true,
            "orderMulti": true,
            "ordering": true,
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "order": [[0, "asc"]],
            "oLanguage": {
                "sSearch": "Filter"
            },
            "columns": [
                {
                    "orderable": false,
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                { "data": "Str_MemberStatus", "title": "Status" },
                { "data": "MemberApplication", "title": "Application" },
                { "data": "BeneficiaryCard", "title": "Beneficiary Card" },
                { "data": "BeckPolicy", "title": "Beck Policy" },
            ]
        });
    }

    function DrawTable_ActionMember(response) {
        Global.Data.table = $("#tbMembers").DataTable({
            "filter": true,
            "orderMulti": true,
            "ordering": true,
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            "pageLength": 25,
            "pagingType": "input",
            "data": response,
            "responsive": true,
            "oLanguage": {
                "sSearch": "Filter"
            },
            "order": [[0, "asc"]],
            "columns": [
                {
                    "orderable": true,
                    "data": "LastName",
                    "title": "Name",
                    render: function (data, type, row) {
                        return row.LastName + " " + row.FirstName;
                    }
                },
                {
                    "data": "SSN", "title": "Last 4 of Social #",
                    //render: function (data, type, row) {
                    //    if (data != null && data != '' && data.length > 4)
                    //        return data.substring(data.length - 4);
                    //    else
                    //        return data;
                    //}
                },
                { "data": "strClassification", "title": "Class" },
                { "data": "MailingAddressLine1", "title": "Mailing Address" },
                { "data": "MailingAddressCity", "title": "City" },
                { "data": "MailingAddressState", "title": "State" },
                { "data": "MailingAddressZip", "title": "Zip Code" },
                { "data": "CellPhone", "title": "Mobile Phone Number" },
            ]
        });
    }

}

