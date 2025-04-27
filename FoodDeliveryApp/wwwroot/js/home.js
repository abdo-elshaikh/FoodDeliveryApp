$(document).ready(function () {
    // Update cart count
    function updateCartCount() {
        $.ajax({
            url: '/Order/GetCartCount',
            method: 'GET',
            success: function (response) {
                $('.cart-count').text(response.count || 0);
            },
            error: function () {
                $('.cart-count').text(0);
            }
        });
    }
    updateCartCount();

    // Search suggestions with debouncing
    let searchTimeout;
    $('#search-input, #mainSearch').on('input', function () {
        clearTimeout(searchTimeout);
        const query = $(this).val().trim();
        if (query.length < 3) {
            $('#searchSuggestions').hide().empty();
            return;
        }
        searchTimeout = setTimeout(() => {
            $.ajax({
                url: '/Restaurants/SearchSuggestions',
                method: 'GET',
                data: { query: query },
                success: function (response) {
                    const $suggestions = $('#searchSuggestions').empty();
                    if (response.length === 0) {
                        $suggestions.append('<div class="dropdown-item text-muted">No results found</div>').show();
                        return;
                    }
                    response.forEach(item => {
                        $suggestions.append(
                            `<a class="dropdown-item" href="/Restaurants/Details/${item.id}">${item.name}</a>`
                        );
                    });
                    $suggestions.show();
                },
                error: function () {
                    $('#searchSuggestions').empty().append('<div class="dropdown-item text-muted">Error fetching suggestions</div>').show();
                }
            });
        }, 300);
    });

    // Hide suggestions on click outside
    $(document).on('click', function (e) {
        if (!$(e.target).closest('#searchSuggestions, #search-input, #mainSearch').length) {
            $('#searchSuggestions').hide();
        }
    });

    // Geolocation
    $('#geoLocateBtn').on('click', function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
                position => {
                    const { latitude, longitude } = position.coords;
                    $.ajax({
                        url: '/Home/UpdateLocation',
                        method: 'POST',
                        data: { latitude, longitude },
                        success: function (response) {
                            window.location.reload();
                        },
                        error: function () {
                            alert('Failed to update location.');
                        }
                    });
                },
                error => {
                    alert('Geolocation is not supported or permission denied.');
                }
            );
        } else {
            alert('Geolocation is not supported by your browser.');
        }
    });

    // Restaurant filters
    function applyFilters() {
        const category = $('#categoryFilter').val();
        const rating = parseFloat($('#ratingFilter').val()) || 0;
        const distance = parseFloat($('#distanceFilter').val()) || Infinity;
        const price = parseInt($('#priceFilter').val()) || Infinity;

        $('.restaurant-card').each(function () {
            const $card = $(this);
            const cardCategory = $card.data('category');
            const cardRating = parseFloat($card.data('rating'));
            const cardDistance = parseFloat($card.data('distance'));
            const cardPrice = parseInt($card.data('price'));

            const matchesCategory = !category || cardCategory === category;
            const matchesRating = cardRating >= rating;
            const matchesDistance = cardDistance <= distance;
            const matchesPrice = cardPrice <= price;

            if (matchesCategory && matchesRating && matchesDistance && matchesPrice) {
                $card.show();
            } else {
                $card.hide();
            }
        });
    }

    $('#applyFilters').on('click', applyFilters);
    $('.filter-select').on('change', applyFilters);

    // Toggle grid/list view
    let isGridView = true;
    $('#toggleViewBtn').on('click', function () {
        isGridView = !isGridView;
        $('#restaurantGrid').toggleClass('row-cols-1', !isGridView);
        $('.restaurant-item').toggleClass('flex-row', !isGridView);
        $(this).find('i').toggleClass('bi-grid bi-list');
    });

    // Initialize Swiper
    new Swiper('.dish-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
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
        navigation: {
            nextEl: '.category-swiper-next',
            prevEl: '.category-swiper-prev',
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
        pagination: {
            el: '.testimonial-pagination',
            clickable: true,
        },
        autoplay: {
            delay: 5000,
            disableOnInteraction: false,
        }
    });

    new Swiper('.offer-swiper', {
        slidesPerView: 1,
        spaceBetween: 20,
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

    // Copy offer code
    $('.copy-offer-code').on('click', function () {
        const code = $(this).data('code');
        navigator.clipboard.writeText(code).then(() => {
            $(this).text('Copied!').addClass('btn-success').removeClass('btn-primary');
            setTimeout(() => {
                $(this).text('Copy Code').removeClass('btn-success').addClass('btn-primary');
            }, 2000);
        }).catch(() => {
            alert('Failed to copy code.');
        });
    });

    // Newsletter form submission
    $('#newsletterForm').on('submit', function (e) {
        e.preventDefault();
        const email = $(this).find('input[name="email"]').val();
        $.ajax({
            url: $(this).attr('action'),
            method: 'POST',
            data: { email: email },
            success: function () {
                alert('Subscribed successfully!');
                $('#newsletterForm')[0].reset();
            },
            error: function () {
                alert('Failed to subscribe. Please try again.');
            }
        });
    });
});