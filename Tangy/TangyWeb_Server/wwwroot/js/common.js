window.showToastr = (type, message) => {
    if (type === 'success') {
        toastr.success(message, 'Operation Successful');        
    } else if (type === 'error') {
        toastr.error(message, 'Operation Failed');
    }
};

window.showSwal = (type, message) => {
    let alertTitle = "Success Notification!";
    let alertIcon = "success";

    if (type === "error") {
        alertTitle = "Error Notification!";
        alertIcon = "error";        
    } 

    swal.fire({
        title: alertTitle,
        text: message,
        icon: alertIcon
    });
};