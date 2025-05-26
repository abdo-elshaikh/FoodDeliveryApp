document.addEventListener('DOMContentLoaded', () => {
    // Initialize AOS
    AOS.init({
        duration: 1200,
        once: true,
        offset: 150
    });

    // Theme toggle
    const themeToggle = document.getElementById('theme-toggle');
    const themeIcon = themeToggle.querySelector('i');
    const htmlElement = document.documentElement;
    const savedTheme = localStorage.getItem('theme') || 'light';

    htmlElement.classList.toggle('dark', savedTheme === 'dark');
    themeIcon.className = `bi bi-${savedTheme === 'dark' ? 'moon' : 'sun'}-fill text-lg`;

    themeToggle.addEventListener('click', () => {
        const isDark = htmlElement.classList.toggle('dark');
        const newTheme = isDark ? 'dark' : 'light';
        themeIcon.className = `bi bi-${newTheme === 'dark' ? 'moon' : 'sun'}-fill text-lg`;
        localStorage.setItem('theme', newTheme);
        gsap.to(htmlElement, { backgroundColor: isDark ? '#111827' : '#F9FAFB', duration: 0.5 });
    });

    // Navbar toggle
    const navbarToggle = document.getElementById('navbar-toggle');
    const navbarMenu = document.getElementById('navbar-mobile-menu');
    navbarToggle.addEventListener('click', () => {
        navbarMenu.classList.toggle('hidden');
        gsap.fromTo(navbarMenu, { height: 0, opacity: 0 }, { height: 'auto', opacity: 1, duration: 0.4, ease: 'power2.out' });
    });

    // Navbar scroll behavior
    const navbar = document.getElementById('navbar');
    window.addEventListener('scroll', () => {
        navbar.classList.toggle('navbar-scrolled', window.scrollY > 50);
    });

    // Back to top button
    const backToTop = document.getElementById('back-to-top');
    window.addEventListener('scroll', () => {
        backToTop.classList.toggle('hidden', window.scrollY <= 100);
    });

    backToTop.addEventListener('click', (e) => {
        e.preventDefault();
        gsap.to(window, { scrollTo: 0, duration: 1.2, ease: 'power3.inOut' });
    });

    // Cart preview
    const cartItemsContainer = document.getElementById('cart-items');
    async function updateCartPreview() {
        try {
            const response = await fetch('/api/cart', { method: 'GET' });
            if (response.ok) {
                const cart = await response.json();
                cartItemsContainer.innerHTML = cart.items.length ?
                    cart.items.map(item => `<p>${item.name} - $${item.price.toFixed(2)}</p>`).join('') :
                    'Cart is empty';
                updateCartCount(cart.items.length);
            }
        } catch (error) {
            cartItemsContainer.innerHTML = 'Error loading cart';
        }
    }
    updateCartPreview();
});

// Update cart count
function updateCartCount(count) {
    const cartCount = document.querySelector('.cart-count');
    cartCount.textContent = count;
    cartCount.classList.toggle('hidden', count === 0);
    gsap.from(cartCount, { scale: 1.5, duration: 0.4, ease: 'elastic.out(1, 0.3)' });
}

// Show toast message
function showToast(message, type = 'info') {
    const toastContainer = document.getElementById('toast-container');
    const toast = document.createElement('div');
    toast.className = `toast bg-${type === 'success' ? 'green' : type === 'danger' ? 'red' : 'blue'}-500/90 text-white p-4 rounded-lg shadow-xl backdrop-blur-lg`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    toast.innerHTML = `
        <div class="flex items-center">
            <div class="flex-1">${message}</div>
            <button class="text-white hover:text-gray-200" data-dismiss="toast" aria-label="Close">×</button>
        </div>
    `;

    toastContainer.appendChild(toast);
    gsap.from(toast, { x: 100, opacity: 0, duration: 0.6, ease: 'power2.out' });
    setTimeout(() => {
        gsap.to(toast, { x: 100, opacity: 0, duration: 0.6, onComplete: () => toast.remove() });
    }, 4000);

    toast.querySelector('[data-dismiss="toast"]').addEventListener('click', () => {
        gsap.to(toast, { x: 100, opacity: 0, duration: 0.6, onComplete: () => toast.remove() });
    });
}