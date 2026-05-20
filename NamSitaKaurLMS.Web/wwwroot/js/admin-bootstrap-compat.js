(function (window, document, $) {
    'use strict';

    if (!$ || !$.fn || !$.fn.modal) {
        return;
    }

    function ModalCompat(element) {
        this.element = element;
    }

    ModalCompat.prototype.show = function () {
        $(this.element).modal('show');
    };

    ModalCompat.prototype.hide = function () {
        $(this.element).modal('hide');
    };

    ModalCompat.prototype.dispose = function () {
        $(this.element).modal('dispose');
    };

    ModalCompat.getOrCreateInstance = function (element) {
        if (!element) return null;
        var instance = $(element).data('admin.modalCompat');
        if (!instance) {
            instance = new ModalCompat(element);
            $(element).data('admin.modalCompat', instance);
        }
        return instance;
    };

    ModalCompat.getInstance = function (element) {
        if (!element) return null;
        return $(element).data('admin.modalCompat') || null;
    };

    window.bootstrap = window.bootstrap || {};
    window.bootstrap.Modal = ModalCompat;

    document.addEventListener('click', function (event) {
        var dismissButton = event.target.closest('[data-bs-dismiss="modal"]');
        if (!dismissButton) return;

        event.preventDefault();
        var modal = dismissButton.closest('.modal');
        if (modal) {
            $(modal).modal('hide');
        }
    });
})(window, document, window.jQuery);
