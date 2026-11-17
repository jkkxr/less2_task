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

async function loadCategories(page = 1) {
    const response = await fetch(`${apiUrl}?page=${page}&pageSize=${pageSize}`, {
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

    categories.forEach(cat => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td>${cat.id}</td>
            <td>${cat.name}</td>
            <td>${new Date(cat.createdAt).toLocaleString()}</td>
            <td>${new Date(cat.updatedAt).toLocaleString()}</td>
            <td>
                <button class="btn-edit" onclick="editCategory(${cat.id}, '${cat.name}')">Редагувати</button>
                <button class="btn-delete" onclick="deleteCategory(${cat.id})">Видалити</button>
            </td>
        `;
        tbody.appendChild(tr);
    });

    updatePaginationUI();
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

loadCategories(1);