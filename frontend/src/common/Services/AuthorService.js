import { HttpService } from "./HttpService";

async function getAuthorsPFS({ pageNumber = 1, pageSize = 5, orderBy = "Id", descending = false, filterProperty = "", filter = "" } = {}) {
    const response = await HttpService.get('/Author', {
        params: { pageNumber, pageSize, orderBy, descending, filterProperty, filter }
    });
    return response.data;
}

async function getById(id) {
    try {
        const response = await HttpService.get('/Author/' + id);
        return { error: false, data: response.data };
    } catch (error) {
        return { error: true, message: 'Fetching by ID failed' };
    }
}

async function add(author) {
    try {
        await HttpService.post('/Author', author);
        return { error: false, message: 'Author added successfully' };
    } catch (error) {
        return { error: true, message: 'Problem adding author' };
    }
}

async function edit(id, author) {
    try {
        await HttpService.put('/Author/' + id, author);
        return { error: false, message: 'Author updated successfully' };
    } catch (error) {
        return { error: true, message: 'Editing failed' };
    }
}

async function remove(id) {
    try {
        await HttpService.delete('/Author/' + id);
        return { error: false, message: 'Author removed successfully' };
    } catch (error) {
        const message = error.response?.data?.message || 'Operation failed';
        return { error: true, message };
    }
}

export default {
    getAuthorsPFS,
    getById,
    add,
    edit,
    remove
}