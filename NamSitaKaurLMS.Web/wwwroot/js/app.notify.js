window.NamsitaKaurLMSNotify = {
    show: function (message, type) {
        let container = document.querySelector("#appNotifyContainer");

        if (!container) {
            container = document.createElement("div");
            container.id = "appNotifyContainer";
            container.className = "toast-container position-fixed top-0 end-0 p-3";
            container.style.zIndex = "9999";
            document.body.appendChild(container);
        }
        const toastId = "toast_" + Date.now();
        const html = `
            <div id="${toastId}" class="toast align-items-center text-bg-${type} border-0 mb-2" role="alert">
                <div class="d-flex">
                    <div class="toast-body">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto"
                            data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        container.insertAdjacentHTML("beforeend", html);

        const toastElement = document.getElementById(toastId);

        const toast = new bootstrap.Toast(toastElement, {
            delay: 2000
        });

        toast.show();

        toastElement.addEventListener("hidden.bs.toast", function () {
            toastElement.remove();
        });
    },

    success: function (message) {
        this.show(message, "success");
    },

    error: function (message) {
        this.show(message, "danger");
    },

    warning: function (message) {
        this.show(message, "warning");
    },

    info: function (message) {
        this.show(message, "info");
    }
};