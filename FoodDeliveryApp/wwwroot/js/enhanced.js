// Enhanced Interactivity
document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS
    AOS.init({
        duration: 800,
        easing: 'ease-in-out',
        once: true
    });

    // Smooth Scroll
    const smoothScrollLinks = document.querySelectorAll('.smooth-scroll');
    smoothScrollLinks.forEach(link => {
        link.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href').substring(1);
            const target = document.getElementById(targetId);
            if (target) {
                target.scrollIntoView({ behavior: 'smooth' });
            }
            // Highlight the active link
            smoothScrollLinks.forEach(link =>
                link.classList.remove('active'));
            this.classList.add('active');
            // Close the modal if it's open
            const modal = document.getElementById('modal');
            if (modal && modal.classList.contains('active')) {
                modal.classList.remove('active');
            }
        });
    });


    // Parallax Effect
    window.addEventListener('scroll', function () {
        const parallaxElements = document.querySelectorAll('.parallax');
        parallaxElements.forEach(element => {
            const speed = element.dataset.speed || 0.5;
            const yPos = -(window.pageYOffset * speed);
            element.style.transform = `translateY(${yPos}px)`;
        });
    });

    // Lazy Loading Images
    const lazyImages = document.querySelectorAll('img[data-src]');
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.removeAttribute('data-src');
                observer.unobserve(img);
            }
        });
    });

    lazyImages.forEach(img => imageObserver.observe(img));

    // Enhanced Form Validation
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            if (!form.checkValidity()) {
                e.preventDefault();
                e.stopPropagation();

                // Show validation messages
                const invalidInputs = form.querySelectorAll(':invalid');
                invalidInputs.forEach(input => {
                    const feedback = input.nextElementSibling;
                    if (feedback && feedback.classList.contains('invalid-feedback')) {
                        feedback.style.display = 'block';
                    }
                });
            }
            form.classList.add('was-validated');
        });
    });

    // Enhanced Toast Notifications
    window.showToast = function (message, type = 'success') {
        const toast = document.createElement('div');
        toast.className = `toast toast-${type} animate-fade-in`;
        toast.innerHTML = `
            <div class="toast-content">
                <i class="fas fa-${type === 'success' ? 'check-circle' : 'exclamation-circle'}"></i>
                <span>${message}</span>
            </div>
        `;
        document.querySelector('.toast-container').appendChild(toast);

        setTimeout(() => {
            toast.classList.add('toast-exit');
            setTimeout(() => toast.remove(), 300);
        }, 3000);
    };

    // Enhanced Search
    const searchInput = document.querySelector('.search-box input');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function () {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                const query = this.value.trim();
                if (query.length >= 2) {
                    // Implement search functionality
                    console.log('Searching for:', query);
                }
            }, 300);
        });
    }

    // Enhanced Cart Animation
    const cartButtons = document.querySelectorAll('.add-to-cart');
    cartButtons.forEach(button => {
        button.addEventListener('click', function () {
            const cart = document.querySelector('.cart-icon');
            const buttonRect = this.getBoundingClientRect();
            const cartRect = cart.getBoundingClientRect();

            const flyingItem = document.createElement('div');
            flyingItem.className = 'flying-item';
            flyingItem.style.cssText = `
                position: fixed;
                top: ${buttonRect.top}px;
                left: ${buttonRect.left}px;
                width: 20px;
                height: 20px;
                background: var(--primary-color);
                border-radius: 50%;
                z-index: 1000;
            `;

            document.body.appendChild(flyingItem);

            const animation = flyingItem.animate([
                { transform: 'scale(1)', opacity: 1 },
                { transform: 'scale(0.5)', opacity: 0.5, offset: 0.5 },
                { transform: 'scale(0.1)', opacity: 0, offset: 1 }
            ], {
                duration: 1000,
                easing: 'ease-in-out'
            });

            animation.onfinish = () => {
                flyingItem.remove();
                updateCartCount();
            };
        });
    });

    // Update Cart Count
    function updateCartCount() {
        const cartCount = document.querySelector('.cart-count');
        if (cartCount) {
            const currentCount = parseInt(cartCount.textContent);
            cartCount.textContent = currentCount + 1;
            cartCount.classList.add('animate-bounce');
            setTimeout(() => cartCount.classList.remove('animate-bounce'), 1000);
        }
    }

    // Enhanced Image Gallery
    const galleryImages = document.querySelectorAll('.gallery-image');
    galleryImages.forEach(image => {
        image.addEventListener('click', function () {
            const modal = document.createElement('div');
            modal.className = 'image-modal';
            modal.innerHTML = `
                <div class="modal-content">
                    <img src="${this.src}" alt="${this.alt}">
                    <button class="close-modal">&times;</button>
                </div>
            `;
            document.body.appendChild(modal);

            modal.querySelector('.close-modal').addEventListener('click', () => {
                modal.classList.add('fade-out');
                setTimeout(() => modal.remove(), 300);
            });
        });
    });

    // Enhanced Scroll Progress
    const progressBar = document.createElement('div');
    progressBar.className = 'scroll-progress';
    document.body.appendChild(progressBar);

    window.addEventListener('scroll', () => {
        const windowHeight = document.documentElement.scrollHeight - document.documentElement.clientHeight;
        const progress = (window.scrollY / windowHeight) * 100;
        progressBar.style.width = `${progress}%`;
    });

    // Enhanced Theme Toggle
    const themeToggle = document.getElementById('themeToggle');
    if (themeToggle) {
        themeToggle.addEventListener('click', function () {
            const currentTheme = document.documentElement.getAttribute('data-bs-theme');
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            document.documentElement.setAttribute('data-bs-theme', newTheme);
            localStorage.setItem('theme', newTheme);

            // Animate theme toggle
            this.classList.add('rotate');
            setTimeout(() => this.classList.remove('rotate'), 500);
        });
    }

    // Load saved theme
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme) {
        document.documentElement.setAttribute('data-bs-theme', savedTheme);
    }
}); 