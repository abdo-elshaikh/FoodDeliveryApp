// Core Application JavaScript

// Theme Management
class ThemeManager {
    constructor() {
        this.theme = localStorage.getItem('theme') || 'light';
        this.init();
    }

    init() {
        document.documentElement.setAttribute('data-theme', this.theme);
        this.setupThemeSwitcher();
    }

    setupThemeSwitcher() {
        const themeSwitcher = document.querySelector('.theme-switcher');
        if (!themeSwitcher) return;

        const button = themeSwitcher.querySelector('.theme-switcher-button');
        const menu = themeSwitcher.querySelector('.theme-switcher-menu');
        const options = menu.querySelectorAll('.theme-option');

        button.addEventListener('click', () => {
            menu.classList.toggle('active');
        });

        options.forEach(option => {
            option.addEventListener('click', () => {
                const newTheme = option.dataset.theme;
                this.setTheme(newTheme);
                menu.classList.remove('active');
            });
        });

        // Close menu when clicking outside
        document.addEventListener('click', (e) => {
            if (!themeSwitcher.contains(e.target)) {
                menu.classList.remove('active');
            }
        });
    }

    setTheme(theme) {
        this.theme = theme;
        document.documentElement.setAttribute('data-theme', theme);
        localStorage.setItem('theme', theme);
    }
}

// Navigation Management
class NavigationManager {
    constructor() {
        this.init();
    }

    init() {
        this.setupMobileNav();
        this.setupActiveLinks();
    }

    setupMobileNav() {
        const toggle = document.querySelector('.mobile-nav-toggle');
        const nav = document.querySelector('.nav');

        if (!toggle || !nav) return;

        toggle.addEventListener('click', () => {
            nav.classList.toggle('active');
            toggle.setAttribute('aria-expanded', 
                toggle.getAttribute('aria-expanded') === 'true' ? 'false' : 'true'
            );
        });
    }

    setupActiveLinks() {
        const currentPath = window.location.pathname;
        const navLinks = document.querySelectorAll('.nav-link');

        navLinks.forEach(link => {
            if (link.getAttribute('href') === currentPath) {
                link.classList.add('active');
            }
        });
    }
}

// Form Validation
class FormValidator {
    constructor(form) {
        this.form = form;
        this.init();
    }

    init() {
        this.form.addEventListener('submit', (e) => {
            if (!this.validate()) {
                e.preventDefault();
            }
        });

        // Real-time validation
        this.form.querySelectorAll('input, select, textarea').forEach(field => {
            field.addEventListener('blur', () => this.validateField(field));
            field.addEventListener('input', () => this.validateField(field));
        });
    }

    validate() {
        let isValid = true;
        this.form.querySelectorAll('input, select, textarea').forEach(field => {
            if (!this.validateField(field)) {
                isValid = false;
            }
        });
        return isValid;
    }

    validateField(field) {
        const value = field.value.trim();
        let isValid = true;
        let errorMessage = '';

        // Required field validation
        if (field.hasAttribute('required') && !value) {
            isValid = false;
            errorMessage = 'This field is required';
        }

        // Email validation
        if (field.type === 'email' && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                isValid = false;
                errorMessage = 'Please enter a valid email address';
            }
        }

        // Password validation
        if (field.type === 'password' && value) {
            if (value.length < 8) {
                isValid = false;
                errorMessage = 'Password must be at least 8 characters long';
            }
        }

        // Update field state
        this.updateFieldState(field, isValid, errorMessage);
        return isValid;
    }

    updateFieldState(field, isValid, errorMessage) {
        const formGroup = field.closest('.form-group');
        if (!formGroup) return;

        const errorElement = formGroup.querySelector('.invalid-feedback') || 
            document.createElement('div');

        if (!isValid) {
            field.classList.add('is-invalid');
            errorElement.className = 'invalid-feedback';
            errorElement.textContent = errorMessage;
            if (!formGroup.contains(errorElement)) {
                formGroup.appendChild(errorElement);
            }
        } else {
            field.classList.remove('is-invalid');
            errorElement.remove();
        }
    }
}

// Modal Management
class ModalManager {
    constructor() {
        this.init();
    }

    init() {
        document.querySelectorAll('[data-toggle="modal"]').forEach(trigger => {
            trigger.addEventListener('click', (e) => {
                e.preventDefault();
                const targetId = trigger.dataset.target;
                const modal = document.querySelector(targetId);
                if (modal) {
                    this.openModal(modal);
                }
            });
        });

        document.querySelectorAll('.modal').forEach(modal => {
            const closeButtons = modal.querySelectorAll('[data-dismiss="modal"]');
            closeButtons.forEach(button => {
                button.addEventListener('click', () => this.closeModal(modal));
            });

            modal.addEventListener('click', (e) => {
                if (e.target === modal) {
                    this.closeModal(modal);
                }
            });
        });
    }

    openModal(modal) {
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }

    closeModal(modal) {
        modal.classList.remove('active');
        document.body.style.overflow = '';
    }
}

// Toast Notifications
class ToastManager {
    constructor() {
        this.container = this.createContainer();
    }

    createContainer() {
        const container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
        return container;
    }

    show(message, type = 'info', duration = 3000) {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type}`;
        toast.textContent = message;

        this.container.appendChild(toast);

        // Trigger animation
        setTimeout(() => toast.classList.add('show'), 10);

        // Auto remove
        setTimeout(() => {
            toast.classList.remove('show');
            setTimeout(() => toast.remove(), 300);
        }, duration);
    }
}

// Initialize all managers when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Initialize theme manager
    const themeManager = new ThemeManager();

    // Initialize navigation manager
    const navigationManager = new NavigationManager();

    // Initialize modal manager
    const modalManager = new ModalManager();

    // Initialize toast manager
    const toastManager = new ToastManager();

    // Initialize form validation
    document.querySelectorAll('form').forEach(form => {
        new FormValidator(form);
    });

    // Global error handling
    window.addEventListener('error', (e) => {
        console.error('Global error:', e.error);
        toastManager.show('An error occurred. Please try again.', 'error');
    });

    // Handle AJAX errors
    document.addEventListener('ajaxError', (e) => {
        console.error('AJAX error:', e.detail);
        toastManager.show('Failed to complete the request. Please try again.', 'error');
    });
}); 