// Custom JavaScript for FoodDeliveryApp

document.addEventListener('DOMContentLoaded', function () {
    // Notification Bar Dismiss
    const notificationBar = document.getElementById('notificationBar');
    const notificationCloseBtn = document.getElementById('notificationCloseBtn');

    if (notificationCloseBtn && notificationBar) {
        notificationCloseBtn.addEventListener('click', () => {
            notificationBar.style.display = 'none';
            // Store the preference in localStorage
            localStorage.setItem('notificationClosed', 'true');
        });
        
        // Check if notification was previously closed
        if (localStorage.getItem('notificationClosed') === 'true') {
            notificationBar.style.display = 'none';
        }
    }

    // Tap to Top Button
    const tapToTopBtn = document.getElementById('tapToTopBtn');

    window.addEventListener('scroll', () => {
        if (window.scrollY > 300) {
            tapToTopBtn.classList.add('show');
        } else {
            tapToTopBtn.classList.remove('show');
        }
    });

    tapToTopBtn.addEventListener('click', () => {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
    
    // Navbar Scroll Effect
    const navbar = document.querySelector('.navbar');
    let lastScrollTop = 0;
    
    window.addEventListener('scroll', () => {
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        
        if (scrollTop > 100) {
            navbar.classList.add('navbar-scrolled');
        } else {
            navbar.classList.remove('navbar-scrolled');
        }
        
        // Hide/show navbar on scroll direction
        if (scrollTop > lastScrollTop && scrollTop > 300) {
            // Scrolling down
            navbar.classList.add('navbar-hidden');
        } else {
            // Scrolling up
            navbar.classList.remove('navbar-hidden');
        }
        
        lastScrollTop = scrollTop;
    });
    
    // Dark Mode Toggle
    const darkModeToggle = document.getElementById('darkModeToggle');
    const htmlElement = document.documentElement;
    
    // Check for saved theme preference or respect OS preference
    const savedTheme = localStorage.getItem('theme');
    const prefersDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
    
    // Set initial theme
    if (savedTheme === 'dark' || (!savedTheme && prefersDarkMode)) {
        htmlElement.classList.add('dark-mode');
        updateDarkModeIcon(true);
    }
    
    // Toggle dark mode
    darkModeToggle.addEventListener('click', () => {
        const isDarkMode = htmlElement.classList.toggle('dark-mode');
        localStorage.setItem('theme', isDarkMode ? 'dark' : 'light');
        updateDarkModeIcon(isDarkMode);
    });
    
    function updateDarkModeIcon(isDarkMode) {
        const icon = darkModeToggle.querySelector('i');
        if (isDarkMode) {
            icon.classList.remove('fa-moon');
            icon.classList.add('fa-sun');
        } else {
            icon.classList.remove('fa-sun');
            icon.classList.add('fa-moon');
        }
    }
    
    // Add lazy loading to images
    const images = document.querySelectorAll('img[data-src]');
    
    if ('IntersectionObserver' in window) {
        const imageObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    
                    // Create a new image to preload
                    const newImg = new Image();
                    newImg.src = img.dataset.src;
                    
                    // When the image is loaded, update the visible image
                    newImg.onload = function() {
                        img.src = img.dataset.src;
                        img.classList.add('fade-in');
                        img.removeAttribute('data-src');
                    };
                    
                    imageObserver.unobserve(img);
                }
            });
        });
        
        images.forEach(img => {
            imageObserver.observe(img);
        });
    } else {
        // Fallback for browsers that don't support IntersectionObserver
        images.forEach(img => {
            img.src = img.dataset.src;
            img.removeAttribute('data-src');
        });
    }
    
    // Add animation to elements when they come into view
    const animatedElements = document.querySelectorAll('.animate-on-scroll');
    
    if ('IntersectionObserver' in window) {
        const animationObserver = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const element = entry.target;
                    const animation = element.dataset.animation || 'fade-in';
                    const delay = element.dataset.delay || 0;
                    
                    setTimeout(() => {
                        element.classList.add(animation);
                        element.classList.remove('animate-on-scroll');
                    }, delay);
                    
                    animationObserver.unobserve(element);
                }
            });
        }, {
            threshold: 0.1
        });
        
        animatedElements.forEach(element => {
            animationObserver.observe(element);
        });
    } else {
        // Fallback for browsers that don't support IntersectionObserver
        animatedElements.forEach(element => {
            element.classList.add('fade-in');
            element.classList.remove('animate-on-scroll');
        });
    }
    
    // Handle form submissions with loading state
    const forms = document.querySelectorAll('form:not([data-no-loading])');
    
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitButton = form.querySelector('button[type="submit"]');
            
            if (submitButton && !form.classList.contains('loading')) {
                // Save original button text
                const originalText = submitButton.innerHTML;
                
                // Add loading state
                form.classList.add('loading');
                submitButton.disabled = true;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span> Loading...';
                
                // If it's not an AJAX form, restore after a delay (for demo purposes)
                if (!form.hasAttribute('data-ajax')) {
                    setTimeout(() => {
                        form.classList.remove('loading');
                        submitButton.disabled = false;
                        submitButton.innerHTML = originalText;
                    }, 2000);
                }
            }
        });
    });
});
