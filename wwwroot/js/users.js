
$.validator.setDefaults({
    submitHandler: function () {
        
        $('#new-user-form').get(0).submit();//submit form
    }
});

$('#new-user-form').validate({
    rules: {
       name: {
            required: true,
            remote: {
                url: "/api/adminuser/checkname",
                type: "post",
                data: {
                    name: function () {
                        return $("#new-name-input").val().toLowerCase();
                    }
                }
            }

        },
        email: {
            required: true,
            email: true,
            remote: {
                url: "/api/adminuser/checkemail" ,
                type: "post",
                data: {
                    email: function () {
                        return $("#new-email-input").val();
                    }
                }
            }

        },
        password: {
            required: true,
            minlength: 8
        },
  
    },
    messages: {
        name: {
            required: "Please enter a name",
          
            remote: "User name Already Exists"
        },
        email: {
            required: "Please enter a email address",
            email: "Please enter a vaild email address",
            remote:"Email Already Exists"
        },
        password: {
            required: "Please provide a password",
            minlength: "Your password must be at least 5 characters long"
        },
     
    },
    errorElement: 'span',
    errorPlacement: function (error, element) {
        error.addClass('invalid-feedback').addClass('offset-4');
        element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
        $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).removeClass('is-invalid');
    }
});



//delete admin user
$(document).on("click", ".delete-btn", function () {

    Swal.fire({
        title: "Are you sure you want to delete this user?",
        
        type: "warning",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: "#cc3f44",
        confirmButtonText: "Delete",
        closeOnConfirm: false,
        html: false
    }).then((res) => {
        if (res.isConfirmed) {
            var adminUserId = $(this).data("id");
            $.ajax({
                type: "post",
                url: "/api/adminuser/delete",
                data: { adminUserId: adminUserId },
                success: function (res) {

                    if (res == "success") {
                        Swal.fire('Saved!', '', 'success').then(() => location.reload())
                    }
                    
                }
            })
        }
    })
})


