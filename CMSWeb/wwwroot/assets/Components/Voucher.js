function Form(FormType, ID) {
    $.ajax({
        cache: false,
        url: '/Voucher/_form',
        data: { FormType: FormType, ID: ID },
        beforeSend: function () {

        },
        success: function (myData) {
            $('#data').empty().append(myData);
        },
        complete: function () {
            $('.date').daterangepicker({
                singleDatePicker: true,
                autoApply: true,
                showDropdowns: true,
                autoUpdateInput: false,
            }, function (start, end, label) {
                $('.date').val(start.format('YYYY-MM-DD'));
                fromdate = start.format('YYYY-MM-DD');
            });

            if (FormType == "Edit") {
                $('#DD_Year').val($('#txt_year').val());
            }

            $('#Form').submit(function (e) {
                e.preventDefault();

                var name = $('#txt_title').val();
                var amount = $('#txt_amount').val();
                var qty = $('#txt_qty').val();
                var date = $('#txt_date').val();
                var description = $('#txt_desc').val();
              
                $(".error").remove();

                if (name.length < 1) {
                    $('#txt_tittle').after('<span class="error">This field is required</span>');
                    return;
                }

                if (name.length > 100) {
                    $('#txt_tittle').after('<span class="error">Length is not valid, maximum 100 allowed</span>');
                    return;
                }


                if (amount.length < 1) {
                    $('#txt_amount').after('<span class="error">This field is required</span>');
                    return;
                }

                if (qty.length < 1) {
                    $('#txt_qty').after('<span class="error">This field is required</span>');
                    return;
                }

               
                if (date.length < 1) {
                    $('#txt_date').after('<span class="error">This field is required</span>');
                    return;
                }


                $.ajax({
                    cache: false,
                    url: '/Voucher/upsert',
                    type: "Post",
                    data: $(this).serialize(),
                    beforeSend: function () {

                        $('.btn').prop("disabled", true);

                    },
                    success: function (myData) {
                        if (myData == "Success") {
                            location.href = '../Voucher';
                        } else {
                            alert("Oops Fail!!!!");
                        }
                    },
                    complete: function () {
                       
                        $('.btn').prop("disabled", false);
                       
                    }
                });
            });


        }


    });

}


