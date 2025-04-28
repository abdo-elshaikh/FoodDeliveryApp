$(document).ready(function () {
    // Update cart count with error handling
    function updateCartCount() {
        $.ajax({
            url: '/Order/GetCartCount',
            method: 'GET',
            success: function (response) {
                const count = response.count || 0;
                $('.cart-count').text(count).attr('aria-label', `Cart contains ${count} items`);
                if (count > 0) {
                    $('.cart-count').addClass('badge bg-primary rounded-pill').removeClass('bg-secondary');
                } else {
                    $('.cart-count').addClass('badge bg-secondary rounded-pill').removeClass('bg-primary');
                }
            },
            error: function () {
                $('.cart-count').text(0).attr('aria-label', 'Cart is empty').addClass('badge bg-secondary rounded-pill');
            }
        });
    }
    updateCartCount();

    // Search suggestions with debouncing and loading state
    let searchTimeout;
    const $searchInput = $('#search-input, #mainSearch');
    const $suggestions = $('#searchSuggestions');
    $searchInput.on('input', function () {
        clearTimeout(searchTimeout);
        const query = $(this).val().trim();
        $suggestions.removeClass('border border-primary');

        if (query.length < 3) {
            $suggestions.hide().empty();
            return;
        }

        searchTimeout = setTimeout(() => {
            $suggestions.html('<div class="dropdown-item text-muted"><i class="bi bi-hourglass-split me-2"></i>Loading...</div>').show();
            $.ajax({
                url: '/Restaurants/SearchSuggestions',
                method: 'GET',
                data: { query: query },
                success: function (response) {
                    $suggestions.empty().addClass('border border-primary');
                    if (response.length === 0) {
                        $suggestions.append('<div class="dropdown-item text-muted"><i class="bi bi-info-circle me-2"></i>No results found</div>').show();
                        return;
                    }
                    response.forEach(item => {
                        $suggestions.append(
                            `<a class="dropdown-item d-flex align-items-center" href="/Restaurants/Details/${item.id}">
                                <i class="bi bi-shop me-2 text-primary"></i>
                                <span>${item.name}</span>
                            </a>`
                        );
                    });
                    $suggestions.show();
                },
                error: function () {
                    $suggestions.empty().append('<div class="dropdown-item text-muted"><i class="bi bi-exclamation-triangle me-2"></i>Error fetching suggestions</div>').show();
                }
            });
        }, 300);
    });

    // Hide suggestions on click outside or ESC key
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#searchSuggestions, #search-input, #mainSearch').length) {
            $suggestions.hide().removeClass('border border-primary');
        }
    });
    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') {
            $suggestions.hide().removeClass('border border-primary');
        }
    });

    // Geolocation with visual feedback
    $('#geoLocateBtn').on('click', function () {
        const $btn = $(this);
        const originalText = $btn.html();
        if (navigator.geolocation) {
            $btn.html('<i class="bi bi-arrow-repeat spin me-1"></i> Locating...').prop('disabled', true);
            navigator.geolocation.getCurrentPosition(
                position => {
                    const { latitude, longitude } = position.coords;
                    $.ajax({
                        url: '/Home/UpdateLocation',
                        method: 'POST',
                        data: { latitude, longitude },
                        success: function () {
                            window.location.reload();
                        },
                        error: function () {
                            $btn.html(originalText).prop('disabled', false);
                            showToast('Failed to update location.', 'error');
                        }
                    });
                },
                error => {
                    $btn.html(originalText).prop('disabled', false);
                    showToast('Geolocation not supported or permission denied.', 'error');
                }
            );
        } else {
            showToast('Geolocation is not supported by your browser.', 'error');
        }
    });

    // Restaurant filters with smooth transitions
    function applyFilters() {
        const category = $('#categoryFilter').val();
        const rating = parseFloat($('#ratingFilter').val()) || 0;
        const distance = parseFloat($('#distanceFilter').val()) || Infinity;
        const price = parseInt($('#priceFilter').val()) || Infinity;

        $('.restaurant-card').each(function () {
            const $card = $(this);
            const cardCategory = $card.data('category')?.toString();
            const cardRating = parseFloat($card.data('rating')) || 0;
            const cardDistance = parseFloat($card.data('distance')) || Infinity;
            const cardPrice = parseInt($card.data('price')) || Infinity;

            const matchesCategory = !category || cardCategory === category;
            const matchesRating = cardRating >= rating;
            const matchesDistance = cardDistance <= distance;
            const matchesPrice = cardPrice <= price;

            if (matchesCategory && matchesRating && matchesDistance && matchesPrice) {
                $card.fadeIn(300).css('display', 'block');
            } else {
                $card.fadeOut(300);
            }
        });
    }

    $('#applyFilters').on('click', function () {
        $(this).addClass('btn-pulse').text('Filtering...');
        setTimeout(() => {
            $(this).removeClass('btn-pulse').text('Apply Filters');
            applyFilters();
        }, 500);
    });
    $('.filter-select').on('change', applyFilters);

    // Toggle grid/list view with animation
    let isGridView = true;
    $('#toggleViewBtn').on('click', function () {
        isGridView = !isGridView;
        const $grid = $('#restaurantGrid');
        const $icon = $(this).find('i');
        $grid.addClass('transition-all');
        $grid.toggleClass('row-cols-1 row-cols-md-2 row-cols-lg-3', !isGridView);
        $('.restaurant-item').toggleClass('flex-row align-items-center', !isGridView);
        $icon.toggleClass('bi-grid bi-list');
        setTimeout(() => $grid.removeClass('transition-all'), 300);
    });

    // Initialize Swiper with lazy loading
    new Swiper('.dish-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
        lazy: true,
        navigation: {
            nextEl: '.dish-swiper-next',
            prevEl: '.dish-swiper-prev',
        },
        breakpoints: {
            576: { slidesPerView: 2 },
            768: { slidesPerView: 3 },
            992: { slidesPerView: 4 }
        }
    });

    new Swiper('.category-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
        lazy: true,
        navigation: {
            nextEl: '.category-swiper-prev',
            prevEl: '.category-swiper-next',
        },
        breakpoints: {
            576: { slidesPerView: 2 },
            768: { slidesPerView: 3 },
            992: { slidesPerView: 4 }
        }
    });

    new Swiper('.testimonial-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
        lazy: true,
        pagination: {
            el: '.testimonial-pagination',
            clickable: true,
            bulletClass: 'swiper-pagination-bullet bg-primary',
        },
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        }
    });

    new Swiper('.offer-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
        lazy: true,
        navigation: {
            nextEl: '.offer-swiper-next',
            prevEl: '.offer-swiper-prev',
        },
        breakpoints: {
            576: { slidesPerView: 2 },
            768: { slidesPerView: 3 },
            992: { slidesPerView: 4 }
        }
    });

    // Copy offer code with toast feedback
    $('.copy-offer-code').on('click', function () {
        const $btn = $(this);
        const code = $btn.data('code');
        navigator.clipboard.writeText(code).then(() => {
            $btn.html('<i class="bi bi-check-circle me-1"></i> Copied!').addClass('btn-success').removeClass('btn-primary');
            showToast(`Offer code "${code}" copied!`, 'success');
            setTimeout(() => {
                $btn.html('<i class="bi bi-clipboard me-1"></i> Copy Code').removeClass('btn-success').addClass('btn-primary');
            }, 2000);
        }).catch(() => {
            showToast('Failed to copy code.', 'error');
        });
    });

    // Newsletter form submission
    $('#newsletterForm').on('submit', function (e) {
        e.preventDefault();
        const $form = $(this);
        const email = $form.find('input[name="email"]').val();
        $form.find('button').prop('disabled', true).html('<i class="bi bi-arrow-repeat spin me-1"></i> Subscribing...');
        $.ajax({
            url: $form.attr('action'),
            method: 'POST',
            data: { email: email },
            success: function () {
                showToast('Subscribed successfully!', 'success');
                $form[0].reset();
                $form.find('button').prop('disabled', false).html('Subscribe');
            },
            error: function () {
                showToast('Failed to subscribe. Please try again.', 'error');
                $form.find('button').prop('disabled', false).html('Subscribe');
            }
        });
    });

    // Toast notification utility
    function showToast(message, type = 'success') {
        const toastId = `toast-${Date.now()}`;
        const bgClass = type === 'success' ? 'bg-success text-white' : 'bg-danger text-white';
        const toastHtml = `
            <div id="${toastId}" class="toast position-fixed top-0 end-0 m-3" role="alert" aria-live="assertive" aria-atomic="true" style="z-index: 1050;">
                <div class="toast-header ${bgClass}">
                    <i class="bi ${type === 'success' ? 'bi-check-circle' : 'bi-exclamation-triangle'} me-2"></i>
                    <strong class="me-auto">${type === 'success' ? 'Success' : 'Error'}</strong>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
                <div class="toast-body bg-white">${message}</div>
            </div>
        `;
        $('body').append(toastHtml);
        const $toast = $(`#${toastId}`);
        $toast.toast({ delay: 3000 }).toast('show');
        $toast.on('hidden.bs.toast', function () {
            $(this).remove();
        });
    }

    // Add hover effects for cards
    $('.restaurant-card, .dish-card, .offer-card, .testimonial-card, .how-it-works-card, .category-card').on('mouseenter', function () {
        $(this).addClass('shadow-lg').css('transform', 'translateY(-5px)');
    }).on('mouseleave', function () {
        $(this).removeClass('shadow-lg').css('transform', 'translateY(0)');
    });

    // Smooth scroll for anchor links
    $('a[href*="#"]').on('click', function (e) {
        if (this.hash !== '') {
            e.preventDefault();
            const hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800);
        }
    });
});