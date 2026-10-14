// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Tap-to-add suggestion chips for a comma-separated text input.
document.querySelectorAll('[data-chip-target]').forEach(function (row) {
    var input = document.getElementById(row.getAttribute('data-chip-target'));
    if (!input) { return; }
    row.querySelectorAll('[data-chip]').forEach(function (chip) {
        chip.addEventListener('click', function () {
            var value = chip.getAttribute('data-chip');
            var tags = input.value.split(',').map(function (t) { return t.trim(); }).filter(Boolean);
            if (!tags.some(function (t) { return t.toLowerCase() === value.toLowerCase(); })) {
                tags.push(value);
            }
            input.value = tags.join(', ');
            input.focus();
        });
    });
});

// Reload a waiting page until the background work is done.
var autoRefresh = document.querySelector('[data-auto-refresh]');
if (autoRefresh) {
    setTimeout(function () { window.location.reload(); }, parseInt(autoRefresh.getAttribute('data-auto-refresh'), 10) || 3000);
}
