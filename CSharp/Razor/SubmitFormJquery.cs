$(document).ready(function () {

        $('#AddNewKrAcModalSaveBtn').click(function () {
            $('#frmNewKrAc').submit();
        });

        $("#frmNewKrAc").on("submit", function (event) {

            var $this = $(this);

            event.preventDefault();

            $('#AddNewKrAcModalSaveBtn').attr('disabled', 'disabled');

            var dataToPost = $(this).serialize();

            $.ajax({
                type: $this.attr('method'),
                url: $this.attr('action'),
                data: dataToPost,
                success: function (response) {

                    if (response) {
                        if (response.Success === true) {


                            if (response.WasInserted === true) {
                                // notify success
                                toastr.options.closeButton = true;
                                toastr.options.newestOnTop = true;
                                toastr.options.progressBar = true;

                                toastr.success('New assesment criteria saved successfully', 'Success');

                                toastr.options.onHidden = function () {
                                    // auto close the modal
                                    $('#AddNewKrAcModal').modal('toggle');
                                }
                            } else {
                                // notify failure
                                toastr.options.closeButton = true;
                                toastr.options.newestOnTop = true;
                                toastr.options.progressBar = true;

                                toastr.error(response.Message, 'Failure');
                            }
                        }
                        else if (response.Success === false) {

                            // if there exists a failure message, it should be rendered within the error partial...
                            if (response.Message) {
                                // notify failure
                                toastr.options.closeButton = true;
                                toastr.options.newestOnTop = true;
                                toastr.options.progressBar = true;

                                toastr.error(response.Message, 'Failure');
                            } else {
                                // notify failure
                                toastr.options.closeButton = true;
                                toastr.options.newestOnTop = true;
                                toastr.options.progressBar = true;

                                toastr.error('Changes not saved', 'Failure');
                            }
                        }
                    }

                    return false;
                },
                error: function (response) {
                    console.log('Warning: unable to save changes on submittal of form. ' + response);
                    toastr.error('Changes not saved', 'Failure');
                },
                complete: function () {
                    // $('#AddNewKrAcModalSaveBtn').removeAttr('disabled');
                    $('AddNewKrAcModalCancelBtn').prop('value', 'Close');
                }
            });
        });