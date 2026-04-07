import { HttpService } from "./HttpService";

async function getBookTitlesPFS({ pageNumber = 1, pageSize = 5, orderBy = "Id", descending = false, filterProperty = "", filter = "" } = {}) {
    const response = await HttpService.get('/BookTitle', {
        params: { pageNumber, pageSize, orderBy, descending, filterProperty, filter }
    });
    return response.data;
}

async function getAll() {
    const response = await HttpService.get('/BookTitle', {
        params: { pageNumber: 1, pageSize: 1000 }
    });
    return response.data.items ?? [];
}

async function getById(id) {
    try {
        const response = await HttpService.get('/BookTitle/' + id);
        return { error: false, data: response.data };
    } catch (error) {
        return { error: true, message: 'Fetching by ID failed' };
    }
}

async function add(bookTitle) {
    try {
        await HttpService.post('/BookTitle', bookTitle);
        return { error: false, message: 'Book title added successfully' };
    } catch (error) {
        return { error: true, message: 'Problem adding book title' };
    }
}

async function edit(id, bookTitle) {
    try {
        await HttpService.put('/BookTitle/' + id, bookTitle);
        return { error: false, message: 'Book title updated successfully' };
    } catch (error) {
        return { error: true, message: 'Editing failed' };
    }
}

async function remove(id) {
    try {
        await HttpService.delete('/BookTitle/' + id);
        return { error: false, message: 'Book title removed successfully' };
    } catch (error) {
        const message = error.response?.data?.message || 'Operation failed';
        return { error: true, message };
    }
}

export default {
    getBookTitlesPFS,
    getAll,
    getById,
    add,
    edit,
    remove
}