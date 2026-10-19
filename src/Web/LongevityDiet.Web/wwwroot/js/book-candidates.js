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

    document.addEventListener('submit', function (event) {
        var form = event.target;
        var mode = form.getAttribute && form.getAttribute('data-fetch');
        // An inline onsubmit confirm() that was declined has already cancelled the event.
        if (!mode || event.defaultPrevented) {
            return;
        }

        event.preventDefault();
        setBusy(form, true);

        fetch(form.action, {
            method: 'POST',
            body: new FormData(form),
            headers: { 'X-Requested-With': 'fetch' }
        })
            .then(function (response) { return response.json(); })
            .then(function (result) {
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
                showToast(document.documentElement.lang === 'vi' ? 'Yêu cầu thất bại. Vui lòng thử lại.' : 'The request failed. Please try again.', false);
                setBusy(form, false);
            });
    });
})();
