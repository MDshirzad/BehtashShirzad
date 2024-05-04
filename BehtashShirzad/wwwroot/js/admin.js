// script.js
document.addEventListener('DOMContentLoaded', function () {
    const sidebarLinks = document.querySelectorAll('.sidebar a');

    sidebarLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            document.querySelector('.main-content header h2').textContent = this.textContent;
        });
    });
});
