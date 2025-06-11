# FoodDeliveryApp Frontend Assets

## Overview

This directory contains the frontend assets for the FoodDeliveryApp, including JavaScript, CSS, images, and other static files. The code has been consolidated and optimized for better performance and maintainability.

## Structure

- `css/` - Contains all CSS files
  - `app.css` - The main consolidated CSS file with all styles
  - (Other CSS files are kept for backward compatibility but are no longer needed)

- `js/` - Contains all JavaScript files
  - `app.min.js` - The main consolidated JavaScript file with all functionality
  - (Other JS files are kept for backward compatibility but are no longer needed)

- `images/` - Contains all image assets
- `lib/` - Contains third-party libraries

## CSS Features

The consolidated CSS (`app.css`) includes:

1. CSS variables for consistent theming
2. Dark mode support
3. Responsive design for all screen sizes
4. Component styles (cards, buttons, forms, etc.)
5. Utility classes for common styling needs
6. Animations and transitions
7. Optimized for performance

## JavaScript Features

The consolidated JavaScript (`app.min.js`) includes:

1. Theme management (light/dark mode)
2. Navigation and UI components
3. Animations and transitions
4. Cart functionality
5. Search and filtering
6. Restaurant-specific features
7. Form validation and image previews
8. Toast notifications

## How to Use

In your layout or view files, include the consolidated files:

```html
<!-- Include the consolidated CSS -->
<link rel="stylesheet" href="~/css/app.css" asp-append-version="true" />

<!-- Include the consolidated JavaScript -->
<script src="~/js/app.min.js" asp-append-version="true"></script>
```

## Maintenance

When adding new functionality:

1. Add it to the appropriate section in `app.min.js`
2. Add any new styles to `app.css`
3. Avoid creating new JavaScript or CSS files to maintain the consolidated approach

## Benefits

- **Performance**: Fewer HTTP requests and smaller overall file size
- **Maintainability**: Single source of truth for styles and behaviors
- **Consistency**: Standardized patterns and naming conventions
- **Extensibility**: Clear organization makes it easier to add new features
