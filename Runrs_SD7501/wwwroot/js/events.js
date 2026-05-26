// Byron (22/05/2026) - Event modal and delete functionality
function loadEventModal(eventId) {
    fetch(`/Event/Details/${eventId}`)
        .then(response => response.text())
        .then(html => {
            const container = document.getElementById('eventModalContainer');
            container.innerHTML = html;
            container.querySelectorAll('script').forEach(oldScript => {
                const newScript = document.createElement('script');
                newScript.textContent = oldScript.textContent;
                document.body.appendChild(newScript);
                document.body.removeChild(newScript);
            });

            const modal = new bootstrap.Modal(
                document.getElementById('eventDetailsModal')
            );
            modal.show();
        })
        .catch(error => console.error('Error loading event details:', error));
}

function confirmDeleteEvent(eventId) {
    Swal.fire({
        title: "Delete Event?",
        text: "This action cannot be undone!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#FF7614",
        cancelButtonColor: "#444",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "Cancel",
        background: "#1a1a1a",
        color: "#ffffff"
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById('deleteEventForm').submit();
        }
    });
}

function confirmDeleteAnnouncement(announcementId) {
    Swal.fire({
        title: "Delete Announcement?",
        text: "This action cannot be undone!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#A12A67",
        cancelButtonColor: "#444",
        confirmButtonText: "Confirm Delete!",
        cancelButtonText: "Cancel",
        background: "#1a1a1a",
        color: "#ffffff"
    }).then((result) => {
        if (result.isConfirmed) {
            document.getElementById(`deleteAnnouncementForm-${announcementId}`).submit();
        }
    });
}

function getAntiForgeryToken() {
    return document.querySelector('input[name="__RequestVerificationToken"]')?.value;
}

document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.view-event-btn').forEach(button => {
        button.addEventListener('click', function () {
            const eventId = this.dataset.eventId;
            loadEventModal(eventId);
        });
    });
});