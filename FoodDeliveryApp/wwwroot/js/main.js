// Main JavaScript file

// Toast Notifications
class ToastManager {
    constructor() {
        this.container = document.querySelector('.toast-container');
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
            document.body.appendChild(this.container);
        }
    }

    show(message, type = 'info', duration = 5000) {
        const toast = document.createElement('div');
        toast.className = `toast show`;
        toast.setAttribute('role', 'alert');
        toast.setAttribute('aria-live', 'assertive');
        toast.setAttribute('aria-atomic', 'true');

        const icon = this.getIcon(type);
        const bgClass = this.getBackgroundClass(type);

        toast.innerHTML = `
            <div class="toast-header ${bgClass} text-white">
                <i class="${icon} me-2"></i>
                <strong class="me-auto">${type.charAt(0).toUpperCase() + type.slice(1)}</strong>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
            <div class="toast-body">
                ${message}
            </div>
        `;

        this.container.appendChild(toast);

        // Auto-hide after duration
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, duration);

        // Add click handler to close button
        const closeButton = toast.querySelector('.btn-close');
        if (closeButton) {
            closeButton.addEventListener('click', () => {
                toast.classList.remove('show');
                setTimeout(() => toast.remove(), 300);
            });
        }
    }

    getIcon(type) {
        switch (type) {
            case 'success':
                return 'fas fa-check-circle';
            case 'error':
                return 'fas fa-exclamation-circle';
            case 'warning':
                return 'fas fa-exclamation-triangle';
            default:
                return 'fas fa-info-circle';
        }
    }

    getBackgroundClass(type) {
        switch (type) {
            case 'success':
                return 'bg-success';
            case 'error':
                return 'bg-danger';
            case 'warning':
                return 'bg-warning';
            default:
                return 'bg-info';
        }
    }
}

// Form Validation
class FormValidator {
    constructor(form) {
        this.form = form;
        this.init();
    }

    init() {
        if (!this.form) return;

        this.form.addEventListener('submit', (e) => this.handleSubmit(e));
        this.form.querySelectorAll('input, select, textarea').forEach(input => {
            input.addEventListener('blur', () => this.validateField(input));
            input.addEventListener('input', () => this.validateField(input));
        });
    }

    handleSubmit(e) {
        if (!this.validateForm()) {
            e.preventDefault();
            const toast = new ToastManager();
            toast.show('Please correct the errors in the form.', 'error');
        }
    }

    validateForm() {
        let isValid = true;
        this.form.querySelectorAll('input, select, textarea').forEach(input => {
            if (!this.validateField(input)) {
                isValid = false;
            }
        });
        return isValid;
    }

    validateField(input) {
        const value = input.value.trim();
        let isValid = true;
        let errorMessage = '';

        // Required field validation
        if (input.hasAttribute('required') && !value) {
            isValid = false;
            errorMessage = 'This field is required.';
        }

        // Email validation
        if (input.type === 'email' && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                isValid = false;
                errorMessage = 'Please enter a valid email address.';
            }
        }

        // Password validation
        if (input.type === 'password' && value) {
            if (value.length < 8) {
                isValid = false;
                errorMessage = 'Password must be at least 8 characters long.';
            }
        }

        // Update UI
        this.updateFieldValidation(input, isValid, errorMessage);
        return isValid;
    }

    updateFieldValidation(input, isValid, errorMessage) {
        const formGroup = input.closest('.form-group') || input.closest('.mb-3');
        if (!formGroup) return;

        const errorElement = formGroup.querySelector('.text-danger');
        
        if (!isValid) {
            input.classList.add('is-invalid');
            if (!errorElement) {
                const error = document.createElement('div');
                error.className = 'text-danger mt-1';
                error.textContent = errorMessage;
                formGroup.appendChild(error);
            } else {
                errorElement.textContent = errorMessage;
            }
        } else {
            input.classList.remove('is-invalid');
            if (errorElement) {
                errorElement.remove();
            }
        }
    }
}

// Image Lazy Loading
class LazyLoader {
    constructor() {
        this.images = document.querySelectorAll('img[data-src]');
        this.init();
    }

    init() {
        if (!('IntersectionObserver' in window)) {
            this.loadAllImages();
            return;
        }

        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    this.loadImage(entry.target);
                    observer.unobserve(entry.target);
                }
            });
        });

        this.images.forEach(image => imageObserver.observe(image));
    }

    loadImage(image) {
        const src = image.getAttribute('data-src');
        if (!src) return;

        image.src = src;
        image.removeAttribute('data-src');
        image.classList.add('loaded');
    }

    loadAllImages() {
        this.images.forEach(image => this.loadImage(image));
    }
}

// Theme Switcher
class ThemeSwitcher {
    constructor() {
        this.themeToggle = document.querySelector('.theme-toggle');
        this.init();
    }

    init() {
        if (!this.themeToggle) return;

        // Check for saved theme preference
        const savedTheme = localStorage.getItem('theme');
        if (savedTheme) {
            document.documentElement.setAttribute('data-bs-theme', savedTheme);
        }

        this.themeToggle.addEventListener('click', () => this.toggleTheme());
    }

    toggleTheme() {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        document.documentElement.setAttribute('data-bs-theme', newTheme);
        localStorage.setItem('theme', newTheme);

        // Update icon
        const icon = this.themeToggle.querySelector('i');
        if (icon) {
            icon.className = newTheme === 'dark' ? 'fas fa-sun' : 'fas fa-moon';
        }
    }
}

// Initialize components when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Initialize toast manager
    window.toastManager = new ToastManager();

    // Initialize form validation
    document.querySelectorAll('form').forEach(form => {
        new FormValidator(form);
    });

    // Initialize lazy loading
    new LazyLoader();

    // Initialize theme switcher
    new ThemeSwitcher();
}); 