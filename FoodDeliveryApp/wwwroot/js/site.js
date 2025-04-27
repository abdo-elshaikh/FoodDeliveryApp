document.addEventListener('DOMContentLoaded', () => {
    // Theme toggle
    const themeToggle = document.createElement('button');
    themeToggle.className = 'btn btn-outline-secondary position-fixed bottom-0 end-0 m-3';
    themeToggle.innerHTML = '<i class="bi bi-moon-stars-fill"></i>';
    document.body.appendChild(themeToggle);

    themeToggle.addEventListener('click', () => {
        const isLight = document.documentElement.getAttribute('data-bs-theme') === 'light';
        document.documentElement.setAttribute('data-bs-theme', isLight ? 'dark' : 'light');
        themeToggle.innerHTML = `<i class="bi bi-${isLight ? 'sun-fill' : 'moon-stars-fill'}"></i>`;
    });

    // Smooth scroll for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', (e) => {
            e.preventDefault();
            document.querySelector(anchor.getAttribute('href'))?.scrollIntoView({
                behavior: 'smooth'
            });
        });
    });

    // Navbar shrink on scroll
    const header = document.getElementById('mainHeader');
    const handleScroll = () => {
        if (window.scrollY > 50) {
            header.classList.add('navbar-shrink');
        } else {
            header.classList.remove('navbar-shrink');
        }
    };

    window.addEventListener('scroll', handleScroll);
});
