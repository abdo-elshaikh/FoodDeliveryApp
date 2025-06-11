class ToastManager {
    constructor() {
        this.container = document.querySelector('.toast-container');
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.className = 'toast-container';
            document.body.appendChild(this.container);
        }
        this.toasts = [];
        this.maxToasts = 5;
    }

    show(message, type = 'info', options = {}) {
        const { title = null, duration = 4000, closeable = true } = options;
        // Remove oldest if max
        if (this.toasts.length >= this.maxToasts) {
            const oldest = this.toasts.shift();
            oldest.classList.add('hide');
            setTimeout(() => oldest.remove(), 300);
        }
        const toast = document.createElement('div');
        toast.className = `toast toast-${type} show`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');
        toast.innerHTML = `
            <div class="toast-header">
                <span class="toast-icon">${this.getIcon(type)}</span>
                <span class="toast-title">${title || this.getTitle(type)}</span>
                ${closeable ? '<button type="button" class="btn-close ms-2" aria-label="Close"></button>' : ''}
            </div>
            <div class="toast-body">${message}</div>
            <div class="toast-progress"></div>
        `;
        this.container.appendChild(toast);
        this.toasts.push(toast);
        // Progress bar
        const progress = toast.querySelector('.toast-progress');
        progress.style.transition = `width ${duration}ms linear`;
        setTimeout(() => progress.style.width = '0%', 10);
        // Remove after duration
        let timer = setTimeout(() => this.hide(toast), duration);
        // Pause on hover
        toast.addEventListener('mouseenter', () => {
            clearTimeout(timer);
            progress.style.transition = 'none';
        });
        toast.addEventListener('mouseleave', () => {
            progress.style.transition = `width ${duration}ms linear`;
            progress.style.width = '0%';
            timer = setTimeout(() => this.hide(toast), duration);
        });
        // Close button
        const closeBtn = toast.querySelector('.btn-close');
        if (closeBtn) {
            closeBtn.onclick = () => this.hide(toast);
        }
        // Keyboard dismiss
        toast.tabIndex = 0;
        toast.addEventListener('keydown', e => {
            if (e.key === 'Escape') this.hide(toast);
        });
        // Animation end cleanup
        toast.addEventListener('animationend', e => {
            if (toast.classList.contains('hide')) toast.remove();
        });
        // Focus for accessibility
        toast.focus();
    }

    hide(toast) {
        toast.classList.remove('show');
        toast.classList.add('hide');
        setTimeout(() => toast.remove(), 300);
        this.toasts = this.toasts.filter(t => t !== toast);
    }

    getIcon(type) {
        switch (type) {
            case 'success': return '<i class="fas fa-check-circle text-success"></i>';
            case 'error': return '<i class="fas fa-times-circle text-danger"></i>';
            case 'warning': return '<i class="fas fa-exclamation-triangle text-warning"></i>';
            case 'info': default: return '<i class="fas fa-info-circle text-info"></i>';
        }
    }

    getTitle(type) {
        switch (type) {
            case 'success': return 'Success';
            case 'error': return 'Error';
            case 'warning': return 'Warning';
            case 'info': default: return 'Info';
        }
    }
}

// Create global instance
window.toastManager = new ToastManager();

// Add global helper function
window.showToast = (msg, type = 'info', options = {}) => window.toastManager.show(msg, type, options);