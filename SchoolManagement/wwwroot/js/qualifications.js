$(document).ready(function () {
    var table = $('#qualificationsTable').DataTable({
        ajax: {
            url: '/api/qualifications',
            dataSrc: ''
        },
        columns: [
            { data: 'course' },
            { data: 'university' },
            { data: 'startYear' },
            { data: 'endYear' },
            { data: 'percentage' },
            {
                data: null,
                className: "center",
                /*defaultContent: '<button class="edit-btn">Edit</button> <button class="delete-btn">Delete</button>'*/
                defaultContent: `
                    <form class="edit-form" method="get" action="/Students/QualificationsMV/Edit" style="display:inline;">
                        <input type="hidden" name="id" value="">
                        <button type="submit" class="edit-btn">Edit</button>
                    </form>
                    <form class="delete-form" method="post" action="/Students/QualificationsMV/Delete" style="display:inline;">
                        <input type="hidden" name="id" value="">
                        <button type="submit" class="delete-btn">Delete</button>
                    </form>
                `
            }
        ]
    });

    // Fill the hidden input with the qualification ID
    $('#qualificationsTable tbody').on('click', 'button.edit-btn', function () {
        var data = table.row($(this).parents('tr')).data();
        $(this).siblings('input[name="id"]').val(data.id);
    });

    $('#qualificationsTable tbody').on('click', 'button.delete-btn', function () {
        var data = table.row($(this).parents('tr')).data();
        $(this).siblings('input[name="id"]').val(data.id);
    });

    // Handle delete confirmation with SweetAlert2
    // delete button takes in id from datatable.
    $('#qualificationsTable tbody').on('click', 'button.delete-btn', function (e) {
        e.preventDefault();
        var data = table.row($(this).parents('tr')).data();
        var qualificationId = data.id;

        // SweetAlert2 confirmation dialog
        Swal.fire({
            title: 'Are you sure?',
            text: 'You will not be able to recover this qualification!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // User confirmed deletion, send AJAX request to delete the qualification
                $.ajax({
                    url: '/Students/QualificationsMV/Delete',
                    type: 'POST',
                    data: {
                        id: qualificationId,
                        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() // Include anti-forgery token
                    },
                    success: function (result) {
                        // Reload table data after successful deletion
                        table.ajax.reload();
                        // Show success message
                        Swal.fire(
                            'Deleted!',
                            'The qualification has been deleted.',
                            'success'
                        );
                    },
                    error: function (xhr, status, error) {
                        // error message if deletion fails
                        Swal.fire(
                            'Error!',
                            'Failed to delete the qualification.',
                            'error'
                        );
                    }
                });
            }
        });
    });


    // Open add qualification modal
    $('#addQualificationBtn').on('click', function () {
        $('#addQualificationModal').show();
    });

    // Close add qualification modal
    $('#closeAddModal').on('click', function () {
        $('#addQualificationModal').hide();
    });

    // Handle form submission for adding a qualification
    $('#addQualificationForm').submit(function (e) {
        e.preventDefault();

        var formData = $(this).serialize();

        $.ajax({
            url: '/api/qualifications',
            type: 'POST',
            contentType: 'application/x-www-form-urlencoded',
            data: formData,
            success: function (result) {
                table.ajax.reload();
                $('#addQualificationModal').hide();
            }
        });
    });


});
