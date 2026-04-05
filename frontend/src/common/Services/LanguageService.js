import { HttpService } from "./HttpService";

async function getLanguagesPFS({ pageNumber = 1, pageSize = 5, orderBy = "Id", descending = false, filterProperty = "", filter = "" } = {}) {
    const response = await HttpService.get('/Language', {
        params: { pageNumber, pageSize, orderBy, descending, filterProperty, filter }
    });
    return response.data;
}

async function getById(id) {
    try {
        const response = await HttpService.get('/Language/' + id);
        return { error: false, data: response.data };
    } catch (error) {
        return { error: true, message: 'Fetching by ID failed' };
    }
}

export default {
    getLanguagesPFS,
    getById
}