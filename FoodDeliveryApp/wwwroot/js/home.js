document.addEventListener('DOMContentLoaded', () => {
    // Predictive search
    const searchInput = document.querySelector('.search-input');
    const searchSuggestions = document.querySelector('.search-suggestions');
    const popularSearches = @Json.Serialize(Model.PopularSearches);

    searchInput.addEventListener('input', () => {
        const query = searchInput.value.toLowerCase();
        const suggestions = popularSearches.filter(s => s.toLowerCase().includes(query));
        searchSuggestions.innerHTML = suggestions.length ?
            suggestions.map(s => `<div class="p-2 hover:bg-gray-100 dark:hover:bg-gray-700 cursor-pointer" data-suggestion="${s}">${s}</div>`).join('') :
            '<div class="p-2 text-gray-500">No suggestions</div>';
        searchSuggestions.classList.toggle('hidden', !query || !suggestions.length);
        gsap.from(searchSuggestions.children, { y: 10, opacity: 0, stagger: 0.1, duration: 0.3 });
    });

    searchSuggestions.addEventListener('click', (e) => {
        if (e.target.dataset.suggestion) {
            searchInput.value = e.target.dataset.suggestion;
            searchSuggestions.classList.add('hidden');
        }
    });

    // 3D tilt effect for cards
    VanillaTilt.init(document.querySelectorAll('.category-card, .dish-card, .restaurant-card, .promotion-card'), {
        max: 20,
        speed: 600,
        glare: true,
        'max-glare': 0.4,
        perspective: 1000
    });

    // Add to cart
    document.querySelectorAll('.add-to-cart').forEach(button => {
        button.addEventListener('click', async () => {
            const dishId = button.getAttribute('data-dish-id');
            try {
                const response = await fetch('/api/cart/add', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ dishId })
                });
                if (response.ok) {
                    const data = await response.json();
                    updateCartCount(data.cartCount);
                    showToast('Item added to cart!', 'success');
                } else {
                    showToast('Failed to add item to cart.', 'danger');
                }
            } catch (error) {
                showToast('An error occurred.', 'danger');
            }
        });
    });

    // Newsletter form
    const newsletterForm = document.querySelector('.newsletter-form');
    if (newsletterForm) {
        newsletterForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const emailInput = newsletterForm.querySelector('input[type="email"]');
            const email = emailInput.value;

            try {
                const response = await fetch('/api/newsletter/subscribe', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ email })
                });
                if (response.ok) {
                    showToast('Subscribed successfully!', 'success');
                    gsap.to(emailInput, { value: '', duration: 0.5, ease: 'power2.out' });
                } else {
                    showToast('Subscription failed. Please try again.', 'danger');
                }
            } catch (error) {
                showToast('An error occurred.', 'danger');
            }
        });
    });

// Load more dishes
const loadMoreDishes = document.getElementById('load-more-dishes');
if (loadMoreDishes) {
    let dishOffset = 6;
    loadMoreDishes.addEventListener('click', async () => {
        try {
            const response = await fetch(`/api/dishes?offset=${dishOffset}&limit=6`);
            if (response.ok) {
                const dishes = await response.json();
                const container = document.getElementById('dishes-container');
                dishes.forEach(dish => {
                    const dishHtml = `
                            <div class="dish-card group relative rounded-xl shadow-lg overflow-hidden bg-white/10 dark:bg-gray-800/10 backdrop-blur-lg" data-dish-id="${dish.id}" data-aos="fade-up">
                                <img src="${dish.imageUrl || 'https://source.unsplash.com/400x300/?' + dish.name}" alt="${dish.name}" class="w-full h-48 object-cover lazy-load" loading="lazy">
                                <div class="p-4">
                                    <div class="flex justify-between items-start mb-2">
                                        <h5 class="text-lg font-semibold">${dish.name}</h5>
                                        <span class="bg-primary text-white text-sm px-3 py-1 rounded-full">$${dish.price.toFixed(2)}</span>
                                    </div>
                                    <p class="text-gray-600 dark:text-gray-300 text-sm mb-3">${dish.description}</p>
                                    <div class="flex justify-between items-center">
                                        <div>
                                            <p class="text-gray-500 dark:text-gray-400 text-sm">From ${dish.restaurantName}</p>
                                            <div class="flex items-center text-yellow-500">
                                                ${Array(Math.floor(dish.rating)).fill('<i class="bi bi-star-fill"></i>').join('')}
                                                ${dish.rating % 1 !== 0 ? '<i class="bi bi-star-half"></i>' : ''}
                                            </div>
                                        </div>
                                        ${dish.isAvailable ? `<button class="add-to-cart btn bg-primary text-white hover:bg-primary-dark" data-dish-id="${dish.id}" aria-label="Add ${dish.name} to cart"><i class="bi bi-cart-plus"></i> Add</button>` : '<span class="text-gray-500 dark:text-gray-400 text-sm">Unavailable</span>'}
                                    </div>
                                </div>
                            </div>`;
                    container.insertAdjacentHTML('beforeend', dishHtml);
                });
                dishOffset += 6;
                if (dishOffset >= @Model.FeaturedDishes.Count) {
                    loadMoreDishes.classList.add('hidden');
                }
                AOS.refresh();
                VanillaTilt.init(document.querySelectorAll('.dish-card'), {
                    max: 20,
                    speed: 600,
                    glare: true,
                    'max-glare': 0.4
                });
            }
        } catch (error) {
            showToast('Error loading more dishes.', 'danger');
        }
    });
}

// Load more restaurants
const loadMoreRestaurants = document.getElementById('load-more-restaurants');
if (loadMoreRestaurants) {
    let restaurantOffset = 6;
    loadMoreRestaurants.addEventListener('click', async () => {
        try {
            const response = await fetch(`/api/restaurants?offset=${restaurantOffset}&limit=6`);
            if (response.ok) {
                const restaurants = await response.json();
                const container = document.getElementById('restaurants-container');
                restaurants.forEach(restaurant => {
                    const restaurantHtml = `
                            <a href="/restaurants/${restaurant.id}" class="restaurant-card group relative rounded-xl shadow-lg overflow-hidden bg-white/10 dark:bg-gray-800/10 backdrop-blur-lg" data-restaurant-id="${restaurant.id}" data-aos="fade-up">
                                <img src="${restaurant.imageUrl || 'https://source.unsplash.com/400x300/?restaurant'}" alt="${restaurant.name}" class="w-full h-48 object-cover lazy-load" loading="lazy">
                                <div class="p-4">
                                    <h5 class="text-lg font-semibold mb-2">${restaurant.name}</h5>
                                    <p class="text-gray-600 dark:text-gray-300 text-sm mb-3">${restaurant.description}</p>
                                    <div class="flex justify-between items-center">
                                        <div>
                                            <p class="text-gray-500 dark:text-gray-400 text-sm">${restaurant.categoryName}</p>
                                            <div class="flex items-center text-yellow-500">
                                                ${Array(Math.floor(restaurant.rating)).fill('<i class="bi bi-star-fill"></i>').join('')}
                                                ${restaurant.rating % 1 !== 0 ? '<i class="bi bi-star-half"></i>' : ''}
                                                <span class="text-gray-500 dark:text-gray-400 text-sm ml-1">(${restaurant.reviewCount} reviews)</span>
                                            </div>
                                        </div>
                                        <p class="text-gray-500 dark:text-gray-400 text-sm">Delivery: $${restaurant.deliveryFee.toFixed(2)}</p>
                                    </div>
                                </div>
                            </a>`;
                    container.insertAdjacentHTML('beforeend', restaurantHtml);
                });
                restaurantOffset += 6;
                if (restaurantOffset >= @Model.TopRatedRestaurants.Count) {
                    loadMoreRestaurants.classList.add('hidden');
                }
                AOS.refresh();
                VanillaTilt.init(document.querySelectorAll('.restaurant-card'), {
                    max: 20,
                    speed: 600,
                    glare: true,
                    'max-glare': 0.4
                });
            }
        } catch (error) {
            showToast('Error loading more restaurants.', 'danger');
        }
    });
}

// Lazy load images
const lazyImages = document.querySelectorAll('.lazy-load');
const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.src = entry.target.dataset.src || entry.target.src;
            observer.unobserve(entry.target);
        }
    });
}, { rootMargin: '0px 0px 200px 0px' });

lazyImages.forEach(image => observer.observe(image));
});