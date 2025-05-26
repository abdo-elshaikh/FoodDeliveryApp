/**
 * FoodFast - Theme Management
 * Handles the light/dark theme toggle functionality
 */

document.addEventListener('DOMContentLoaded', function () {
    const themeToggleBtn = document.getElementById('themeToggle');
    if (!themeToggleBtn) return;

    // Function to set the theme
    const setTheme = (theme) => {
        document.documentElement.setAttribute('data-bs-theme', theme);
        localStorage.setItem('food-fast-theme', theme);
    };

    // Function to toggle the theme
    const toggleTheme = () => {
        const currentTheme = document.documentElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        setTheme(newTheme);
    };

    // Initialize theme based on user preference
    const initializeTheme = () => {
        const savedTheme = localStorage.getItem('food-fast-theme');
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;

        if (savedTheme) {
            setTheme(savedTheme);
        } else if (prefersDark) {
            setTheme('dark');
        } else {
            setTheme('light');
        }
    };

    // Add event listener to the theme toggle button
    themeToggleBtn.addEventListener('click', toggleTheme);

    // Initialize theme on page load
    initializeTheme();

    // Listen for system theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', (e) => {
        if (!localStorage.getItem('food-fast-theme')) {
            setTheme(e.matches ? 'dark' : 'light');
        }
    });
});