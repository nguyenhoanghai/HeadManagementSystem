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
HMS.namespace('BanHangEdit');
HMS.BanHangEdit = function () {
    var Global = {
        UrlAction: {
            
        },
        Data: {
            TVId: 0, 
            modelId: 0,
            typeId: 0,
            gia: 0,
            discount : 0
        }
    }
    this.GetGlobal = function () {
        return Global;
    }
      
    this.Init = function () {
        $('.tuvanid').val(Global.Data.TVId);
        $('.modelid').val(Global.Data.modelId);
        $('.WorkTypeId').val(Global.Data.typeId);
        $('.giaban').val(Global.Data.gia);
        $('.chietkhau').val(Global.Data.discount);

        RegisterEvent();
        
        RefreshControls();
        //Textarea auto growth
        autosize($('textarea.auto-growth'));

        var $demoMaskedInput = $('.masked-input');
        $demoMaskedInput.find('.date').inputmask('dd/mm/yyyy', { placeholder: '__/__/____' });
    }

    var RegisterEvent = function () {    
        $('.chietkhau,.giaban').change(() => { tinhChietKhau(); });

        $('.btnSave').click(function () {
            $('input,select').prop('disabled', false);
          $('#frmReceipt').submit();
        }); 

        $('.chietkhau').change();
    }

    tinhChietKhau = () => {
        if ($('.chietkhau').val() != '' && $('.giaban').val() != '') {
            var ck = parseFloat($('.chietkhau').val());
            var gb = parseFloat($('.giaban').val());
            if (ck > 0) {
                var giack = (gb * ck) / 100;
                $('.thanhtien').html((gb - giack).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,') + ' vnđ')
            }
            else
                $('.thanhtien').html((gb).toFixed(0).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,') + ' vnđ')
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
