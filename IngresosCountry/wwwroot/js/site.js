// =============================================
// Ingresos Country - Site JavaScript
// =============================================

$(document).ready(function () {

    // Auto-dismiss alerts after 5 seconds
    setTimeout(function () {
        $('.alert-dismissible').fadeOut('slow');
    }, 5000);

    // Active nav link highlighting
    var currentPath = window.location.pathname.toLowerCase();
    $('.navbar-nav .nav-link').each(function () {
        var href = $(this).attr('href');
        if (href && currentPath.startsWith(href.toLowerCase()) && href !== '/') {
            $(this).addClass('active');
        } else if (href === '/' && currentPath === '/') {
            $(this).addClass('active');
        }
    });

    // Confirm delete actions
    $('[data-confirm]').click(function (e) {
        if (!confirm($(this).data('confirm'))) {
            e.preventDefault();
        }
    });

    // Auto-focus first input in forms
    $('form:first input:visible:first').focus();

    // Table row click to navigate (if data-href is set)
    $('tr[data-href]').css('cursor', 'pointer').click(function () {
        window.location = $(this).data('href');
    });

    // Print button functionality
    $('#btnPrint').click(function () {
        window.print();
    });

    // Real-time clock display
    function updateClock() {
        var now = new Date();
        var timeStr = now.toLocaleTimeString('es-GT', { hour: '2-digit', minute: '2-digit', second: '2-digit' });
        $('#reloj').text(timeStr);
    }

    if ($('#reloj').length) {
        setInterval(updateClock, 1000);
        updateClock();
    }

    // Tooltip initialization
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
});