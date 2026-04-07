import { HttpService } from "./HttpService";

const ENDPOINT = "/BookQuote";

const BookQuoteService = {
    getBookQuotesPFS: async ({ pageNumber, pageSize, orderBy, descending, filterProperty, filter }) => {
        const response = await HttpService.get(ENDPOINT, {
            params: { pageNumber, pageSize, orderBy, descending, filterProperty, filter }
        });
        return response.data;
    },

    getById: async (id) => {
        const response = await HttpService.get(`${ENDPOINT}/${id}`);
        return response.data;
    },

    add: async (bookQuote) => {
        const response = await HttpService.post(ENDPOINT, bookQuote);
        return response.data;
    },

    edit: async (id, bookQuote) => {
        const response = await HttpService.put(`${ENDPOINT}/${id}`, bookQuote);
        return response.data;
    },

    remove: async (id) => {
        await HttpService.delete(`${ENDPOINT}/${id}`);
        return { error: false, message: "Deleted successfully." };
    }
};

export default BookQuoteService;