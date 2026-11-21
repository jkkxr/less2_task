const apiUrl = '/api/categories';
const authToken = '';

let currentPage = 1;
const pageSize = 5;
let totalPages = 1;

function getHeaders() {
    const headers = { 'Content-Type': 'application/json' };
    if (authToken) {
        headers['Authorization'] = `Bearer ${authToken}`;
    }
    return headers;
}

function toggleFiltersPanel() {
    const panel = document.getElementById('filtersPanel');
    panel.classList.toggle('active');
}

async function loadCategories(page = 1) {
    currentPage = page;

    const nameFilter = document.getElementById('filterName').value.trim();
    const sortBy = document.getElementById('sortBy').value;
    const createdFrom = document.getElementById('filterCreatedFrom').value;
    const createdTo = document.getElementById('filterCreatedTo').value;

    let url = `${apiUrl}?page=${currentPage}&pageSize=${pageSize}`;

    if (sortBy) {
        url += `&orderBy=${encodeURIComponent(sortBy)}`;
    }
    if (nameFilter) {
        url += `&filter.name=${encodeURIComponent(nameFilter)}`;
    }
    if (createdFrom) {
        url += `&filter.createdFrom=${encodeURIComponent(createdFrom)}`;
    }
    if (createdTo) {
        url += `&filter.createdTo=${encodeURIComponent(createdTo)}`;
    }

    const response = await fetch(url, {
        headers: getHeaders()
    });

    if (!response.ok) {
        alert('Помилка завантаження категорій: ' + response.status);
        return;
    }

    const data = await response.json();
    const categories = data.items || [];
    currentPage = data.currentPage || page;
    totalPages = data.totalPages || 1;

    const tbody = document.getElementById('categoriesTable');
    tbody.innerHTML = '';

    if (categories.length === 0) {
        tbody.innerHTML = `<tr><td colspan="6" style="text-align: center; color: #777;">Нічого не знайдено</td></tr>`;
    } else {
        categories.forEach(cat => {
            const imgSrc = getCategoryImageSrc(cat);
            const imageCellHtml = imgSrc
                ? `<img src="${imgSrc}" alt="${cat.name}" class="category-thumb">`
                : `<span class="category-no-image">Немає</span>`;
            const imageBtnText = imgSrc ? 'Змінити зображення' : 'Встановити зображення';

            const safeName = String(cat.name).replace(/'/g, "\\'");
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${cat.id}</td>
                <td>${imageCellHtml}</td>
                <td>${cat.name}</td>
                <td>${new Date(cat.createdAt).toLocaleString()}</td>
                <td>${new Date(cat.updatedAt).toLocaleString()}</td>
                <td>
                    <button class="btn-image" onclick="triggerCategoryImageUpload(${cat.id})">${imageBtnText}</button>
                    <button class="btn-edit" onclick="editCategory(${cat.id}, '${safeName}')">Редагувати</button>
                    <button class="btn-delete" onclick="deleteCategory(${cat.id})">Видалити</button>
                </td>
            `;
            tbody.appendChild(tr);
        });
    }

    updatePaginationUI();
}

function applyFilters() {
    loadCategories(1); 
}

function resetFilters() {
    document.getElementById('filterName').value = '';
    document.getElementById('sortBy').value = '-CreatedAt';
    document.getElementById('filterCreatedFrom').value = '';
    document.getElementById('filterCreatedTo').value = '';
    loadCategories(1);
}

function updatePaginationUI() {
    const displayTotal = totalPages > 0 ? totalPages : 1;
    document.getElementById('pageInfo').textContent = `Сторінка ${currentPage} з ${displayTotal}`;
    document.getElementById('prevBtn').disabled = currentPage <= 1;
    document.getElementById('nextBtn').disabled = currentPage >= displayTotal;
}

function changePage(delta) {
    const newPage = currentPage + delta;
    if (newPage >= 1 && newPage <= totalPages) {
        loadCategories(newPage);
    }
}

async function addCategory() {
    const input = document.getElementById('categoryName');
    const name = input.value.trim();
    if (!name) {
        alert('Введіть назву категорії!');
        return;
    }

    const response = await fetch(apiUrl, {
        method: 'POST',
        headers: getHeaders(),
        body: JSON.stringify({ name: name })
    });

    if (response.ok) {
        input.value = '';
        loadCategories(currentPage);
    } else {
        alert('Помилка при додаванні: ' + response.status);
    }
}

async function editCategory(id, oldName) {
    const newName = prompt('Введіть нову назву категорії:', oldName);
    if (newName === null || newName.trim() === '' || newName === oldName) {
        return;
    }

    const response = await fetch(`${apiUrl}/${id}/name`, {
        method: 'PATCH',
        headers: getHeaders(),
        body: JSON.stringify({ name: newName.trim() })
    });

    if (response.ok) {
        loadCategories(currentPage);
    } else {
        alert('Помилка при редагуванні: ' + response.status);
    }
}

async function deleteCategory(id) {
    if (!confirm('Ви впевнені, що хочете видалити цю категорію?')) {
        return;
    }

    const response = await fetch(`${apiUrl}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });

    if (response.ok) {
        const rowsCount = document.getElementById('categoriesTable').children.length;
        const targetPage = (rowsCount === 1 && currentPage > 1) ? currentPage - 1 : currentPage;
        loadCategories(targetPage);
    } else {
        alert('Помилка при видаленні: ' + response.status);
    }
}

async function applyBackground() {
    const response = await fetch('/upload/background');
    if (!response.ok) return;

    const data = await response.json();
    const url = data.imageUrl || data.ImageUrl;
    if (url) {
        document.body.style.backgroundImage = `url('${url}')`;
    }
}

const uploadForm = document.getElementById('uploadForm');
if (uploadForm) {
    uploadForm.addEventListener('submit', async (e) => {
        e.preventDefault();

        const response = await fetch('/upload/background', {
            method: 'POST',
            body: new FormData(uploadForm)
        });

        if (response.ok) {
            await applyBackground();
        } else {
            alert('Помилка завантаження фону: ' + response.status);
        }
    });
}

let selectedCategoryId = null;

function getCategoryImageSrc(cat) {
    const img = cat.image || cat.Image;
    if (!img) return null;

    const filename = img.filename || img.Filename;
    if (filename) {
        return `/uploads/${filename}`;
    }

    return img.url || img.Url || img.imageUrl || img.ImageUrl || null;
}

function triggerCategoryImageUpload(categoryId) {
    selectedCategoryId = categoryId;
    const fileInput = document.getElementById('categoryImageInput');
    if (!fileInput) return;

    fileInput.value = '';
    fileInput.click();
}

const categoryImageInput = document.getElementById('categoryImageInput');
if (categoryImageInput) {
    categoryImageInput.addEventListener('change', async () => {
        const file = categoryImageInput.files[0];
        if (!file || !selectedCategoryId) return;

        const formData = new FormData();
        formData.append('file', file);

        const headers = {};
        if (authToken) {
            headers['Authorization'] = `Bearer ${authToken}`;
        }

        const response = await fetch(`${apiUrl}/${selectedCategoryId}/image`, {
            method: 'POST',
            headers: headers,
            body: formData
        });

        if (response.ok) {
            selectedCategoryId = null;
            await loadCategories(currentPage);
        } else {
            alert('Помилка завантаження зображення категорії: ' + response.status);
        }
    });
}

applyBackground();

loadCategories(1);