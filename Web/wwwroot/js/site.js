// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// نمایش نوتیفیکیشن
function showNotification(message, type = 'success') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} notification`;
    notification.textContent = message;
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.remove();
    }, 3000);
}

// نمایش لودینگ
function showLoading() {
    const loading = document.createElement('div');
    loading.className = 'loading';
    loading.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
    document.body.appendChild(loading);
}

function hideLoading() {
    const loading = document.querySelector('.loading');
    if (loading) {
        loading.remove();
    }
}

// مدیریت جداول
function initDataTable(tableId) {
    const table = document.getElementById(tableId);
    if (table) {
        // اضافه کردن قابلیت جستجو
        const searchInput = document.createElement('input');
        searchInput.type = 'text';
        searchInput.className = 'form-control mb-3';
        searchInput.placeholder = 'جستجو...';
        table.parentNode.insertBefore(searchInput, table);
        
        searchInput.addEventListener('input', (e) => {
            const searchText = e.target.value.toLowerCase();
            const rows = table.getElementsByTagName('tr');
            
            for (let i = 1; i < rows.length; i++) {
                const row = rows[i];
                const cells = row.getElementsByTagName('td');
                let found = false;
                
                for (let j = 0; j < cells.length; j++) {
                    if (cells[j].textContent.toLowerCase().indexOf(searchText) > -1) {
                        found = true;
                        break;
                    }
                }
                
                row.style.display = found ? '' : 'none';
            }
        });
    }
}

// مدیریت چارت‌ها
function initChart(chartId, type, data, options = {}) {
    const ctx = document.getElementById(chartId).getContext('2d');
    return new Chart(ctx, {
        type: type,
        data: data,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            ...options
        }
    });
}

// مدیریت فرم‌ها
function initFormValidation(formId) {
    const form = document.getElementById(formId);
    if (form) {
        form.addEventListener('submit', (e) => {
            e.preventDefault();
            const inputs = form.querySelectorAll('input, select, textarea');
            let isValid = true;
            
            inputs.forEach(input => {
                if (input.hasAttribute('required') && !input.value) {
                    isValid = false;
                    input.classList.add('is-invalid');
                } else {
                    input.classList.remove('is-invalid');
                }
            });
            
            if (isValid) {
                form.submit();
            }
        });
    }
}

// مدیریت دراپ‌داون‌ها
function initDropdowns() {
    const dropdowns = document.querySelectorAll('.dropdown-toggle');
    dropdowns.forEach(dropdown => {
        dropdown.addEventListener('click', (e) => {
            e.preventDefault();
            const menu = dropdown.nextElementSibling;
            menu.classList.toggle('show');
        });
    });
}

// مدیریت مودال‌ها
function showModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('show');
        modal.style.display = 'block';
    }
}

function hideModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('show');
        modal.style.display = 'none';
    }
}

// مدیریت منوهای موبایل
function initMobileMenu() {
    const menuButton = document.querySelector('.navbar-toggler');
    const menu = document.querySelector('.navbar-collapse');
    
    if (menuButton && menu) {
        menuButton.addEventListener('click', () => {
            menu.classList.toggle('show');
        });
    }
}

// مقداردهی اولیه
document.addEventListener('DOMContentLoaded', () => {
    initDataTable('dataTable');
    initFormValidation('mainForm');
    initDropdowns();
    initMobileMenu();
    
    // اضافه کردن انیمیشن به کارت‌ها
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.classList.add('fade-in');
    });
});
