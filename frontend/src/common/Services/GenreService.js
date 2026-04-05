import { HttpService } from "./HttpService";

async function getGenresPFS({ pageNumber = 1, pageSize = 5, orderBy = "Id", descending = false, filterProperty = "", filter = "" } = {}) {
    const response = await HttpService.get('/Genre', {
        params: { pageNumber, pageSize, orderBy, descending, filterProperty, filter }
    });
    return response.data;
}

async function getById(id) {
    try {
        const response = await HttpService.get('/Genre/' + id);
        return { error: false, data: response.data };
    } catch (error) {
        return { error: true, message: 'Fetching by ID failed' };
    }
}

async function add(genre) {
    try {
        await HttpService.post('/Genre', genre);
        return { error: false, message: 'Genre added successfully' };
    } catch (error) {
        return { error: true, message: 'Problem adding Genre' };
    }
}

async function edit(id, genre) {
    try {
        await HttpService.put('/Genre/' + id, genre);
        return { error: false, message: 'Genre updated successfully' };
    } catch (error) {
        return { error: true, message: 'Editing failed' };
    }
}

async function remove(id) {
    try {
        await HttpService.delete('/Genre/' + id);
        return { error: false, message: 'Genre removed successfully' };
    } catch (error) {
        const message = error.response?.data?.message || 'Operation failed';
        return { error: true, message };
    }
}

export default {
    getGenresPFS,
    getById,
    add,
    edit,
    remove
}