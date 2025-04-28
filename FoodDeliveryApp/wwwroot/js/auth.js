$(document).ready(function () {
    // Theme Toggle
    const $themeToggle = $('#themeToggle');
    const $html = $('html');
    const currentTheme = localStorage.getItem('theme') || 'light';
    $html.attr('data-bs-theme', currentTheme);
    updateThemeIcons(currentTheme);

    $themeToggle.on('click', function () {
        const newTheme = $html.attr('data-bs-theme') === 'light' ? 'dark' : 'light';
        $html.attr('data-bs-theme', newTheme);
        localStorage.setItem('theme', newTheme);
        updateThemeIcons(newTheme);
    });

    function updateThemeIcons(theme) {
        if (theme === 'dark') {
            $('.light-icon').addClass('d-none');
            $('.dark-icon').removeClass('d-none');
        } else {
            $('.light-icon').removeClass('d-none');
            $('.dark-icon').addClass('d-none');
        }
    }

    // Welcome Message
    if (!localStorage.getItem('welcomeDismissed')) {
        $('.welcome-message').removeClass('d-none');
        $('.welcome-message .btn-close').on('click', function () {
            localStorage.setItem('welcomeDismissed', 'true');
        });
    } else {
        $('.welcome-message').remove();
    }

    // Password Toggle
    $('.password-toggle').on('click', function () {
        const $toggle = $(this);
        const $input = $toggle.siblings('input');
        const type = $input.attr('type') === 'password' ? 'text' : 'password';
        $input.attr('type', type);
        $toggle.find('i').toggleClass('bi-eye bi-eye-slash');
        $toggle.attr('aria-label', type === 'password' ? 'Show password' : 'Hide password');
    });

    // Password Strength
    $('input[type="password"]').on('input', function () {
        const $input = $(this);
        const $strength = $input.siblings('.password-strength');
        const value = $input.val();
        let strength = 'weak';
        if (value.length >= 8 && /[A-Z]/.test(value) && /[0-9]/.test(value)) {
            strength = 'strong';
        } else if (value.length >= 6) {
            strength = 'medium';
        }
        $strength.removeClass('strength-weak strength-medium strength-strong').addClass(`strength-${strength}`);
    });

    // Form Validation
    $('form').on('submit', function (e) {
        const $form = $(this);
        $form.find('.is-invalid').removeClass('is-invalid');
        $form.find('.invalid-feedback').remove();

        let isValid = true;
        $form.find('input[required], select[required]').each(function () {
            const $input = $(this);
            if (!$input.val().trim()) {
                isValid = false;
                $input.addClass('is-invalid');
                $input.after('<div class="invalid-feedback">This field is required.</div>');
                gsap.from($input.siblings('.invalid-feedback'), { opacity: 0, y: 10, duration: 0.3 });
            }
        });

        const $email = $form.find('input[type="email"]');
        if ($email.length && $email.val().trim() && !isValidEmail($email.val())) {
            isValid = false;
            $email.addClass('is-invalid');
            $email.after('<div class="invalid-feedback">Please enter a valid email address.</div>');
            gsap.from($email.siblings('.invalid-feedback'), { opacity: 0, y: 10, duration: 0.3 });
        }

        if (!isValid) {
            e.preventDefault();
            $('[aria-live="polite"]').attr('aria-atomic', 'true').attr('aria-relevant', 'additions');
            return;
        }

        // Show loading spinner
        $form.find('.form-loading').removeClass('d-none');
        $form.find('button[type="submit"]').prop('disabled', true).html('<i class="bi bi-arrow-repeat spin me-1"></i> Submitting...');
    });

    function isValidEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    // OAuth Button Animation
    $('.oauth-btn').on('mouseenter', function () {
        gsap.to(this, { scale: 1.05, duration: 0.3, ease: 'power2.out' });
    }).on('mouseleave', function () {
        gsap.to(this, { scale: 1, duration: 0.3, ease: 'power2.out' });
    }).on('click', function () {
        const $btn = $(this);
        $btn.prop('disabled', true).html($btn.data('loading-text'));
        gsap.to($btn, { opacity: 0.7, duration: 0.2 });
    });

    // GSAP Animations
    gsap.from('.logo img', {
        opacity: 0,
        y: -20,
        duration: 0.8,
        ease: 'power2.out'
    });
    gsap.from('.auth-form-wrapper', {
        opacity: 0,
        y: 30,
        duration: 0.8,
        delay: 0.2,
        ease: 'power2.out'
    });
    gsap.from('.alert', {
        opacity: 0,
        y: 10,
        duration: 0.5,
        ease: 'power2.out',
        stagger: 0.1
    });
});