// Layout-specific JavaScript

// Sticky Header
class StickyHeader {
    constructor() {
        this.header = document.querySelector('.header');
        this.lastScroll = 0;
        this.init();
    }

    init() {
        if (!this.header) return;

        window.addEventListener('scroll', () => this.handleScroll());
        this.handleScroll(); // Initial check
    }

    handleScroll() {
        const currentScroll = window.pageYOffset;
        const headerHeight = this.header.offsetHeight;

        if (currentScroll <= 0) {
            this.header.classList.remove('header-scrolled');
            return;
        }

        if (currentScroll > this.lastScroll && currentScroll > headerHeight) {
            // Scrolling down
            this.header.classList.add('header-hidden');
            this.header.classList.remove('header-scrolled');
        } else {
            // Scrolling up
            this.header.classList.remove('header-hidden');
            this.header.classList.add('header-scrolled');
        }

        this.lastScroll = currentScroll;
    }
}

// Sidebar Management
class SidebarManager {
    constructor() {
        this.sidebar = document.querySelector('.sidebar');
        this.toggle = document.querySelector('.sidebar-toggle');
        this.overlay = document.querySelector('.sidebar-overlay');
        this.init();
    }

    init() {
        if (!this.sidebar || !this.toggle) return;

        this.toggle.addEventListener('click', () => this.toggleSidebar());
        
        if (this.overlay) {
            this.overlay.addEventListener('click', () => this.closeSidebar());
        }

        // Handle window resize
        window.addEventListener('resize', () => this.handleResize());
        this.handleResize(); // Initial check
    }

    toggleSidebar() {
        this.sidebar.classList.toggle('active');
        if (this.overlay) {
            this.overlay.classList.toggle('active');
        }
        document.body.classList.toggle('sidebar-open');
    }

    closeSidebar() {
        this.sidebar.classList.remove('active');
        if (this.overlay) {
            this.overlay.classList.remove('active');
        }
        document.body.classList.remove('sidebar-open');
    }

    handleResize() {
        if (window.innerWidth > 1024) {
            this.closeSidebar();
        }
    }
}

// Scroll to Top
class ScrollToTop {
    constructor() {
        this.button = document.querySelector('.scroll-to-top');
        this.init();
    }

    init() {
        if (!this.button) return;

        window.addEventListener('scroll', () => this.handleScroll());
        this.button.addEventListener('click', (e) => {
            e.preventDefault();
            this.scrollToTop();
        });
    }

    handleScroll() {
        if (window.pageYOffset > 300) {
            this.button.classList.add('show');
        } else {
            this.button.classList.remove('show');
        }
    }

    scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }
}

// Footer Management
class FooterManager {
    constructor() {
        this.footer = document.querySelector('.footer');
        this.init();
    }

    init() {
        if (!this.footer) return;

        this.setupFooterLinks();
        this.setupNewsletterForm();
    }

    setupFooterLinks() {
        const footerLinks = this.footer.querySelectorAll('.footer-links a');
        footerLinks.forEach(link => {
            link.addEventListener('click', (e) => {
                e.preventDefault();
                const href = link.getAttribute('href');
                if (href.startsWith('#')) {
                    const target = document.querySelector(href);
                    if (target) {
                        target.scrollIntoView({ behavior: 'smooth' });
                    }
                } else {
                    window.location.href = href;
                }
            });
        });
    }

    setupNewsletterForm() {
        const form = this.footer.querySelector('.newsletter-form');
        if (!form) return;

        form.addEventListener('submit', (e) => {
            e.preventDefault();
            const email = form.querySelector('input[type="email"]').value;
            // Handle newsletter subscription
            console.log('Newsletter subscription:', email);
        });
    }
}

// Breadcrumb Management
class BreadcrumbManager {
    constructor() {
        this.breadcrumb = document.querySelector('.breadcrumb');
        this.init();
    }

    init() {
        if (!this.breadcrumb) return;

        this.updateBreadcrumb();
        window.addEventListener('popstate', () => this.updateBreadcrumb());
    }

    updateBreadcrumb() {
        const path = window.location.pathname;
        const segments = path.split('/').filter(Boolean);
        let html = '<li class="breadcrumb-item"><a href="/">Home</a></li>';

        let currentPath = '';
        segments.forEach((segment, index) => {
            currentPath += `/${segment}`;
            const isLast = index === segments.length - 1;
            const text = segment.charAt(0).toUpperCase() + segment.slice(1).replace(/-/g, ' ');
            
            if (isLast) {
                html += `<li class="breadcrumb-item active">${text}</li>`;
            } else {
                html += `<li class="breadcrumb-item"><a href="${currentPath}">${text}</a></li>`;
            }
        });

        this.breadcrumb.innerHTML = html;
    }
}

// Initialize layout components when DOM is loaded
document.addEventListener('DOMContentLoaded', () => {
    // Initialize sticky header
    const stickyHeader = new StickyHeader();

    // Initialize sidebar
    const sidebarManager = new SidebarManager();

    // Initialize scroll to top
    const scrollToTop = new ScrollToTop();

    // Initialize footer
    const footerManager = new FooterManager();

    // Initialize breadcrumbs
    const breadcrumbManager = new BreadcrumbManager();
}); 