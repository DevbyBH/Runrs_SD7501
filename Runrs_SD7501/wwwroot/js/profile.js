(function () {
    'use strict';
    function initProfilePictureUpload() {
        const profileImageInput = document.getElementById('profileImageInput');
        if (profileImageInput) {
            profileImageInput.addEventListener('change', handleProfileImageChange);
        }
    }
    function handleProfileImageChange(event) {
        const file = event.target.files[0];
        if (!file) return;
        const reader = new FileReader();
        reader.onload = function (e) {
            Swal.fire({
                title: "Change Profile Picture?",
                html: `<img src="${e.target.result}" style="width: 150px; height: 150px; object-fit: cover; margin: 10px 0;">`,
                showCancelButton: true,
                confirmButtonColor: "#38793a",
                cancelButtonColor: "#444",
                confirmButtonText: "Yes, upload it!",
                cancelButtonText: "Cancel",
                background: "#1a1a1a",
                color: "#ffffff"
            }).then((result) => {
                if (result.isConfirmed) {
                    const form = document.getElementById('uploadProfilePicForm');
                    if (form) {
                        form.submit();
                    }
                } else {
                    event.target.value = '';
                }
            });
        };
        reader.readAsDataURL(file);
    }

    window.confirmRemovePicture = function (userId) {
        Swal.fire({
            title: "Remove Profile Picture?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#ca3b3b",
            cancelButtonColor: "#444",
            confirmButtonText: "Yes, remove it!",
            cancelButtonText: "Cancel",
            background: "#1a1a1a",
            color: "#ffffff"
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: "Removing...",
                    allowOutsideClick: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });
                fetch(`/Profile/RemoveProfilePicture/${userId}`, {
                    method: 'POST',
                    headers: {
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
                    }
                })
                    .then(response => {
                        if (response.redirected) {
                            window.location.href = response.url;
                        } else {
                            return response.json();
                        }
                    })
                    .then(data => {
                        if (data && !data.success) {
                            Swal.fire({
                                title: "Error!",
                                text: data.message || "Failed to remove profile picture.",
                                icon: "error",
                                background: "#1a1a1a",
                                color: "#ffffff"
                            });
                        }
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        const form = document.createElement('form');
                        form.method = 'POST';
                        form.action = `/Profile/RemoveProfilePicture/${userId}`;
                        document.body.appendChild(form);
                        form.submit();
                    });
            }
        });
    };


    function uploadProfilePictureAJAX(formElement) {
        const formData = new FormData(formElement);

        fetch(formElement.action, {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    const profilePicContainer = document.querySelector('.profile-picture-container img');
                    if (profilePicContainer && data.imageUrl) {
                        profilePicContainer.src = data.imageUrl;
                    }
                    Swal.fire({
                        title: "Updated!",
                        text: "Profile picture updated successfully.",
                        icon: "success",
                        background: "#1a1a1a",
                        color: "#ffffff",
                        timer: 2000,
                        showConfirmButton: false
                    });
                } else {
                    Swal.fire({
                        title: "Error!",
                        text: data.message || "Failed to upload profile picture.",
                        icon: "error",
                        background: "#1a1a1a",
                        color: "#ffffff"
                    });
                }
            })
            .catch(error => {
                console.error('Error:', error);
                Swal.fire({
                    title: "Error!",
                    text: "Something went wrong. Please try again.",
                    icon: "error",
                    background: "#1a1a1a",
                    color: "#ffffff"
                });
            });
    }
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initProfilePictureUpload);
    } else {
        initProfilePictureUpload();
    }
})();