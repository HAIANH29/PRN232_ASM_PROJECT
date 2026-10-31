// Submits the candidate review forms with fetch so the page does not reload.
//   data-fetch="item"    approve/reject one candidate: update that card in place.
//   data-fetch="section" bulk actions: re-render the header and the candidate list from the server.
(function () {
    var toastRegion = null;

    function showToast(message, succeeded) {
        if (!toastRegion) {
            toastRegion = document.createElement('div');
            toastRegion.className = 'toast-region';
            toastRegion.setAttribute('aria-live', 'polite');
            document.body.appendChild(toastRegion);
        }

        var toast = document.createElement('div');
        toast.className = 'site-alert ' + (succeeded ? 'success' : 'error');
        toast.setAttribute('role', 'alert');
        var icon = document.createElement('span');
        icon.className = 'icon';
        icon.setAttribute('aria-hidden', 'true');
        icon.textContent = succeeded ? 'check_circle' : 'error';
        var text = document.createElement('span');
        text.className = 'site-alert-text';
        text.textContent = message;
        toast.appendChild(icon);
        toast.appendChild(text);
        toastRegion.appendChild(toast);
        setTimeout(function () { toast.remove(); }, 6000);
    }

    function setBusy(form, busy) {
        form.querySelectorAll('button').forEach(function (button) {
            if (busy) {
                button.dataset.wasDisabled = button.disabled ? '1' : '';
                button.disabled = true;
            } else {
                button.disabled = button.dataset.wasDisabled === '1';
            }
        });
    }

    function updateItem(form, result) {
        var card = form.closest('.content-item');
        if (!card) {
            return;
        }

        var chip = card.querySelector('.js-status-chip');
        if (chip && result.status) {
            chip.textContent = result.statusLabel || result.status;
            chip.classList.remove('warning', 'success', 'danger');
            chip.classList.add(result.status === 'Approved' ? 'success' : 'danger');
        }

        var actions = card.querySelector('.row-actions');
        if (actions) {
            actions.remove();
        }
    }

    function refreshSections() {
        return fetch(window.location.href, { headers: { 'X-Requested-With': 'fetch' } })
            .then(function (response) { return response.text(); })
            .then(function (html) {
                var doc = new DOMParser().parseFromString(html, 'text/html');
                ['book-source-head', 'candidates-section'].forEach(function (id) {
                    var fresh = doc.getElementById(id);
                    var current = document.getElementById(id);
                    if (fresh && current) {
                        current.replaceWith(fresh);
                    }
                });
            });
    }

    // Bulk buttons are only useful while pending candidates remain.
    function syncBulkButtons() {
        var pending = document.querySelectorAll('#candidates-section .row-actions').length;
        var approveAll = document.querySelector('form[action*="ApproveAllCandidates"] button');
        if (approveAll && pending === 0) {
            approveAll.disabled = true;
        }
    }

    // Generate runs for a while on the server: show a small popup with a spinning ring and the
    // percentage of this run, polled from the server until the request finishes.
    function startProgress(form) {
        var url = form.getAttribute('data-progress-url');
        if (!url) {
            return function () {};
        }

        var radius = 34;
        var circumference = 2 * Math.PI * radius;
        var overlay = document.createElement('div');
        overlay.className = 'run-overlay';
        overlay.innerHTML =
            '<div class="run-card" role="status" aria-live="polite">' +
            '<div class="run-ring">' +
            '<svg viewBox="0 0 80 80" aria-hidden="true">' +
            '<circle class="run-track" cx="40" cy="40" r="' + radius + '"></circle>' +
            '<circle class="run-value" cx="40" cy="40" r="' + radius + '" stroke-dasharray="' + circumference + '" stroke-dashoffset="' + circumference + '"></circle>' +
            '</svg>' +
            '<span class="run-spinner"></span>' +
            '<span class="run-percent">0%</span>' +
            '</div>' +
            '<div class="run-label"></div>' +
            '<div class="run-detail"></div>' +
            '</div>';
        overlay.querySelector('.run-label').textContent = form.getAttribute('data-progress-label') || '';
        document.body.appendChild(overlay);

        var value = overlay.querySelector('.run-value');
        var percentText = overlay.querySelector('.run-percent');
        var detail = overlay.querySelector('.run-detail');
        var stopped = false;

        function render(percent, processed, total) {
            value.style.strokeDashoffset = circumference * (1 - percent / 100);
            percentText.textContent = percent + '%';
            detail.textContent = total > 0 ? processed + ' / ' + total : '';
        }

        function poll() {
            fetch(url, { headers: { 'X-Requested-With': 'fetch' } })
                .then(function (response) { return response.json(); })
                .then(function (run) {
                    if (!stopped && run.running) {
                        render(run.percent, run.processed, run.total);
                    }
                })
                .catch(function () {});
        }

        var timer = setInterval(poll, 1000);
        return function () {
            stopped = true;
            clearInterval(timer);
            overlay.remove();
        };
    }

    document.addEventListener('submit', function (event) {
        var form = event.target;
        var mode = form.getAttribute && form.getAttribute('data-fetch');
        // An inline onsubmit confirm() that was declined has already cancelled the event.
        if (!mode || event.defaultPrevented) {
            return;
        }

        event.preventDefault();
        setBusy(form, true);
        var stopProgress = startProgress(form);

        fetch(form.action, {
            method: 'POST',
            body: new FormData(form),
            headers: { 'X-Requested-With': 'fetch' }
        })
            .then(function (response) { return response.json(); })
            .then(function (result) {
                stopProgress();
                showToast(result.message, result.succeeded);
                if (!result.succeeded) {
                    setBusy(form, false);
                    return null;
                }

                if (mode === 'item') {
                    updateItem(form, result);
                    syncBulkButtons();
                    return null;
                }

                return refreshSections();
            })
            .catch(function () {
                stopProgress();
                showToast(document.documentElement.lang === 'vi' ? 'Yêu cầu thất bại. Vui lòng thử lại.' : 'The request failed. Please try again.', false);
                setBusy(form, false);
            });
    });
})();
