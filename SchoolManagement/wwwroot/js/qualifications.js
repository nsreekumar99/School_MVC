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

    // Handle delete qualification
    $('#qualificationsTable tbody').on('click', 'button.delete-btn', function () {
        var data = table.row($(this).parents('tr')).data();
        if (confirm("Are you sure you want to delete this qualification?")) {
            $.ajax({
                url: '/api/qualifications/' + data.id,
                type: 'DELETE',
                success: function (result) {
                    table.ajax.reload();
                }
            });
        }
    });
});
