/**
 * FoodFast - Authentication UI
 * Handles the authentication form interactions
 */

document.addEventListener('DOMContentLoaded', function () {
    // Password visibility toggle
    setupPasswordToggles();

    // Form validation enhancement
    setupFormValidation();

    // Auto-dismiss alerts after 5 seconds
    setupAlertDismissal();

    // Smooth animation for alerts
    setupAlertAnimations();
});

/**
 * Sets up password visibility toggles
 */
function setupPasswordToggles() {
    const passwordFields = document.querySelectorAll('.password-field');

    passwordFields.forEach(field => {
        const input = field.querySelector('input[type="password"]');
        const toggleBtn = field.querySelector('.password-toggle');

        if (!input || !toggleBtn) return;

        toggleBtn.addEventListener('click', () => {
            // Change input type
            const type = input.getAttribute('type') === 'password' ? 'text' : 'password';
            input.setAttribute('type', type);

            // Toggle icon
            const showIcon = toggleBtn.querySelector('.bi-eye');
            const hideIcon = toggleBtn.querySelector('.bi-eye-slash');

            if (showIcon && hideIcon) {
                showIcon.classList.toggle('d-none');
                hideIcon.classList.toggle('d-none');
            }
        });
    });
}

/**
 * Sets up enhanced form validation
 */
function setupFormValidation() {
    const forms = document.querySelectorAll('.needs-validation');

    Array.from(forms).forEach(form => {
        // Add input validation feedback as user types
        const inputs = form.querySelectorAll('input, select, textarea');
        inputs.forEach(input => {
            input.addEventListener('blur', () => {
                if (input.checkValidity()) {
                    input.classList.remove('is-invalid');
                    input.classList.add('is-valid');
                } else {
                    input.classList.remove('is-valid');
                    input.classList.add('is-invalid');
                }
            });

            // Remove validation classes when user starts typing again
            input.addEventListener('input', () => {
                input.classList.remove('is-invalid', 'is-valid');
            });
        });

        // Handle form submission
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();

                // Find the first invalid field and focus it
                const invalidInput = form.querySelector('.form-control:invalid');
                if (invalidInput) {
                    invalidInput.focus();
                }
            }

            form.classList.add('was-validated');
        });
    });
}

/**
 * Sets up auto-dismissal for alerts
 */
function setupAlertDismissal() {
    const alerts = document.querySelectorAll('.alert-dismissible');

    alerts.forEach(alert => {
        // Auto-dismiss after 5 seconds
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
}

/**
 * Sets up animations for alerts
 */
function setupAlertAnimations() {
    const alerts = document.querySelectorAll('.alert');

    // Apply entrance animation
    alerts.forEach(alert => {
        // Ensure the alert starts visible (in case CSS has it hidden)
        alert.style.display = 'block';
        alert.style.opacity = '0';
        alert.style.transform = 'translateY(-20px)';
        alert.style.transition = 'opacity 0.3s ease, transform 0.3s ease';

        // Trigger animation on next frame
        setTimeout(() => {
            alert.style.opacity = '1';
            alert.style.transform = 'translateY(0)';
        }, 10);
    });
}

/**
 * Adds a password strength meter to password fields
 * @param {string} inputId - The ID of the password input element
 * @param {string} meterId - The ID of the meter element
 */
function setupPasswordStrengthMeter(inputId, meterId) {
    const passwordInput = document.getElementById(inputId);
    const strengthMeter = document.getElementById(meterId);
    const strengthText = document.getElementById(meterId + 'Text');

    if (!passwordInput || !strengthMeter) return;

    passwordInput.addEventListener('input', () => {
        const password = passwordInput.value;
        const strength = calculatePasswordStrength(password);

        // Update meter width
        strengthMeter.style.width = strength.score * 25 + '%';

        // Update meter color
        strengthMeter.className = 'password-strength-meter-bar';
        if (strength.score === 0) {
            strengthMeter.classList.add('bg-danger');
        } else if (strength.score <= 2) {
            strengthMeter.classList.add('bg-warning');
        } else if (strength.score === 3) {
            strengthMeter.classList.add('bg-info');
        } else {
            strengthMeter.classList.add('bg-success');
        }

        // Update text if available
        if (strengthText) {
            strengthText.textContent = strength.feedback;
        }
    });
}

/**
 * Calculates password strength score (0-4) and provides feedback
 * @param {string} password - The password to evaluate
 * @returns {Object} Object containing score and feedback
 */
function calculatePasswordStrength(password) {
    // Empty password
    if (!password) {
        return { score: 0, feedback: 'No password' };
    }

    let score = 0;

    // Length check
    if (password.length > 8) score++;
    if (password.length > 12) score++;

    // Character variety checks
    if (/[a-z]/.test(password) && /[A-Z]/.test(password)) score++;
    if (/[0-9]/.test(password)) score++;
    if (/[^a-zA-Z0-9]/.test(password)) score++;

    // Common patterns decrease score
    if (/^(password|12345|qwerty)/i.test(password)) score = Math.max(1, score - 2);

    // Define feedback based on score
    let feedback;
    switch (score) {
        case 0:
            feedback = 'Very weak';
            break;
        case 1:
            feedback = 'Weak';
            break;
        case 2:
            feedback = 'Fair';
            break;
        case 3:
            feedback = 'Good';
            break;
        case 4:
        case 5:
            feedback = 'Strong';
            break;
        default:
            feedback = 'Invalid';
    }

    return { score: Math.min(4, score), feedback };
}

/**
 * Sets up social login button animations
 */
function setupSocialButtons() {
    const socialButtons = document.querySelectorAll('.btn-social');

    socialButtons.forEach(button => {
        button.addEventListener('mouseenter', () => {
            button.classList.add('pulse-animation');
        });

        button.addEventListener('mouseleave', () => {
            setTimeout(() => {
                button.classList.remove('pulse-animation');
            }, 300);
        });
    });
}