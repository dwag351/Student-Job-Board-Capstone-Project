var tableData;
//set sweet-alert.js 
var Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
});
$(function () {
  //load table when first load the page
    LoadCompanyTable();
})
$(document).on("click", "#search-btn", function (e) {
    //click search button will reload the table
    LoadCompanyTable();
})
function GetQuery() {
    //get search conditions.
    var query = {
        categoryId: $("#category").val(),
        companyId: $("#company").val(), 
        jobStatusId: $("#job-status").val(),
        workTypeId: $("#work-type").val(),
        contractTypeId: $("#contract-type").val(),
        payTypeId: $("#pay-type").val()
    }
    return query;
   
}
function LoadCompanyTable() {
    var query = GetQuery();

    $.ajax({
        type: "get",
        url: "/api/job",
        data:query,
        contentType: "application/json",
        success: function (res) {
            tableData = res;
            $("#job-list").bootstrapTable('destroy').bootstrapTable({
                striped: true,
                search: true,//open table search feature
                pagination: true,//open pagination
                showFullscreen: true,//show full screen button
                showColumns: true, // show select column button 
                pageSize: 10, //default page size
                pageList: [10, 20, 50, 100],
                toggle: true,
                idField: 'id',
                data:res,
                sortable: true,
                  /// 1:pending
                    /// 2:approved
                    /// 3:rejected
                    /// 4:expired
                    /// 5:hired
                    /// 6:closed
                columns:
                    [
                        {
                            field: 'jobTitle',
                            title: 'Job Title',
                            sortable: true,
                            align: "center",
                            width: "15",
                            widthUnit:"%"

                        },{
                        field: 'companyName',
                        title: 'Company',
                        sortable: true,
                            align: "center",
                            width: "10",
                            widthUnit: "%"

                    },
                        {
                            field: 'jobStatusId',
                            title: 'Status',
                            sortable: true,
                            align: "center",
                            formatter: function (value, row) {
                                console.log(GetStatus(value))
                                return GetStatus(value)
                            },
                        },
                        {
                            field: 'category',
                            title: 'Category',
                            sortable: true,
                            align: "center",

                        },
                        {
                            field: 'submitDate',
                            title: 'Submit Date',
                            sortable: true,
                            align: "center",
                            formatter: function (value, row) {
                                var result = moment(value, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY HH:mm:SS");
                                if (value == "0001-01-01T00:00:00") {

                                    result = "";
                                }
                                return result
                            },

                        },
                        {
                            field: 'approvalDate',
                            title: 'Approval Date',
                            sortable: true,
                            align: "center",
                            formatter: function (value, row) {
                                var result = moment(value, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY HH:mm:SS");
                                if (value == "0001-01-01T00:00:00") {

                                    result = "";
                                }
                                if (row.jobStatusId==2) {
                                    result += `<a class="btn ml-2 btn-danger btn-sm test-btn" data-toggle="modal" data-target="#TestModal" data-id=${row.jobId} data-date=${value}><i class="fas fa-exclamation"></i ></a>`
                                }
                                
                                return result;
                            },

                        },
                        {
                            field: 'category',
                            title: 'Category',
                            sortable: true,
                            align: "center",
                            visible: false,

                        },
                        {
                            field: 'location',
                            title: 'Location',
                            sortable: true,
                            align: "center",

                        },
                        {
                            field: 'payType',
                            title: 'Pay Type',
                            sortable: true,
                            align: "center",

                        },
                        {
                            field: 'workType',
                            title: 'Work Type',
                            sortable: true,
                            align: "center",

                        },
                        {
                            field: 'contractType',
                            title: 'Contract Type',
                            sortable: true,
                            align: "center",

                        },
                        {
                            field: 'startDate',
                            title: 'Start Date',
                            visible: false,
                            sortable: true,
                            align: "center",
                            formatter: function (value, row) {
                                var result = moment(value, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY");
                                if (value == "0001-01-01T00:00:00") {

                                    result = "";
                                }

                                return result;
                            },

                        },
                        {
                            field: 'closingDate',
                            title: 'Closing Date',
                            //visible: false,
                            sortable: true,
                            align: "center",
                            formatter: function (value, row) {
                                var result = moment(value, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY");
                                if (value == "0001-01-01T00:00:00") {
                                    result = "";
                                }
                                return result;
                            },

                        },
                        {
                            field: 'contactPositionTitle',
                            title: 'Contact Position Title',
                            sortable: true,
                            align: "center",
                            visible: false,

                        },
                        {
                            field: 'contactName',
                            title: 'Contact Name',
                            sortable: true,
                            align: "center",
                            visible: false,

                        },
                        {
                            field: 'contactEmail',
                            title: 'Contact Email',
                            sortable: true,
                            align: "center",
                            visible: false,

                        },
                        {
                            field: 'contactPhone',
                            title: 'Contact Phone',
                            sortable: true,
                            align: "center",
                            visible: false,

                        },

                        {
                            field: 'roleDescription',
                            title: 'Role Description',
                            sortable: true,
                            visible: false,
                          

                        },
                       
                        {
                            field: 'jobId',
                            title: 'Detail',
                         
                         
                            formatter: function (value, row,index) {
                                var result = `<button class='btn btn-primary btn-sm view-btn m-1' data-index=${index} data-toggle='modal' data-target='#view-modal'>View</button>`
                        
                                return result;
                            },
                        },

                        {
                            field: 'jobId',
                            title: 'Action',
                            width: "15",
                            widthUnit: "%",
                            formatter: function (value, row, index) {
                                var result = ""
                                if (row.jobStatusId == 1) {

                                    result += `<button class='btn btn-success btn-sm approve-btn m-1' data-job-id=${value} >Approve</button>`
                                    result += `<button class='btn btn-danger btn-sm reject-btn m-1' data-job-id=${value} >Reject</button>`
                                }
                                return result;
                            },
                        },
                    ]
            });
        }
    })
   
}
$(document).on("click", ".view-btn", function (e) {
    //show job ad detail
    var index = $(this).data("index");
    var row = tableData[index];
    UpdateModal(row);

})
$(document).on("click", ".approve-btn", function (e) {
    e.preventDefault();
    //approve the job, change pending to approved
    var jobId = $(this).data("job-id");

    Swal.fire({
        title: "Are you sure you want to approve this AD?",
        text: "Click approve button will publish this ad.",
        type: "success",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: "#28a745",
        confirmButtonText: "Approve",
        closeOnConfirm: false,
        html: false
    }).then((res) => {
        if (res.isConfirmed) {
            $.ajax({
                type: "post",
                url: "/api/job/approve",
                data: { jobId: jobId },
                success: function (res) {

                    LoadCompanyTable();
                    Swal.fire('Saved!', '', 'success')
                }
            })
        }

    })
    

})
$(document).on("click", ".reject-btn", function (e) {
 
          //reject job ad , change pending to rejected
       
            var jobId = $(this).data("job-id");

            Swal.fire({
                title: "Are you sure you want to reject this AD?",
                text: "Click reject button will take down this ad.",
                type: "warning",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: "#cc3f44",
                confirmButtonText: "Reject",
                closeOnConfirm: false,
                html: false
            }).then((res) => {
                if (res.isConfirmed) {
                    $.ajax({
                        type: "post",
                        url: "/api/job/reject",
                        data: { jobId: jobId },
                        success: function (res) {
                          
                            LoadCompanyTable();
                            Swal.fire('Saved!', '', 'success')
                        }
                    })
                }
            })
      


})

function UpdateModal(row) {
    //update job detail
    $("#d-position").html(row.jobTitle);
    $("#d-company").html(row.companyName);
    $("#d-role-description").html(row.roleDescription);
    $("#d-key-skills").html(row.keySkills);
    $("#d-contactor-name").html(row.contactName);
    $("#d-contactor-title").html(row.contactPositionTitle);
    $("#d-contactor-email").html(row.contactEmail);
    $("#d-contactor-phone").html(row.contactPhone);
    $("#d-work-type").html(row.workType);
    $("#d-pay-type").html(row.payType);
    $("#d-contract-type").html(row.contractType);
    $("#d-location").html(row.location);
    $("#d-start-date").html(moment(row.startDate, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY"));
    $("#d-close-date").html(moment(row.closingDate, "YYYY-MM-DD hh:mm:SS A").format("DD/MM/YYYY"));

}



function GetStatus(jobStatusId) {
    //show status badge
    var className = "";
    if (jobStatusId == 1)//pending
    {
        className =`<span class="badge badge-primary" >Pending</span>`;
    }
    else if (jobStatusId == 2)//approved
    {
 
        className = `<span class="badge badge-success" >Approved</span>`;
    }
    else if (jobStatusId == 3)//rejected
    {

        className = `<span class="badge badge-danger" >Rejected</span>`;
    }
    else if (jobStatusId == 4)//expired
    {
        
        className = `<span class="badge badge-secondary" >Expired</span>`;
    }
    else if (jobStatusId == 5)//hired
    {
       
        className = `<span class="badge badge-warning" >Hired</span>`;
    }
    else if (jobStatusId == 6)//closed
    {
       
        className = `<span class="badge badge-dark" >Closed</span>`;
    }
    return className;
}



//change approval date 

$(document).on("click", ".test-btn", function (e) {
    
    var approvalDate = $(this).data("date");
    var jobId = $(this).data("id");
    console.log(approvalDate,jobId)
    $("#approval-date").val(moment(approvalDate, "YYYY-MM-DD HH:mm:SS").format("DD/MM/YYYY"));
    $("#job-id").val(jobId);
})
$(document).on("click", ".test-save-btn", function (e) {

    e.preventDefault(); 
    var approvalDate = $("#approval-date").val();
    var jobId = $("#job-id").val();
    $.ajax({
        url: "/api/job/approvaldate",
        type: "post",
        data: { jobId: jobId, approvalDate: approvalDate },
        success: function (res) {
            LoadCompanyTable();
            $('#TestModal').modal('hide');
            Toast.fire({
                icon: 'success',
                title: res
            })
        },
        error: function (requestObject, error, errorThrown) {
            console.log(requestObject, error, errorThrown)
            Toast.fire({
                icon: 'error',
                title: requestObject.responseText
            })
        }
    })
})

$('.datepicker').datepicker({
    format: "dd/mm/yyyy", //nz format
    //odayBtn: "linked",//show today link
    autoclose: true,
    orientation: 'bottom', // show calendar under the input
    autocomplete: 'off'
});