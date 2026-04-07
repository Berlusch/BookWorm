import { makeAutoObservable, runInAction } from "mobx";
import BookQuoteService from "../common/Services/BookQuoteService";

class BookQuoteStore {
    quotes = [];
    searchTerm = "";
    currentPage = 1;
    pageSize = 5;
    hasNextPage = false;
    loading = false;
    error = null;

    addStatus = {
        error: false,
        message: ""
    };

    sorting = { orderBy: "id", descending: false };

    constructor() {
        makeAutoObservable(this);
    }

    setSearchTerm(term) {
        this.searchTerm = term;
        this.currentPage = 1;
        this.fetchQuotes();
    }

    setPage(page) {
        this.currentPage = page;
        this.fetchQuotes();
    }

    setSorting(columnKey) {
        if (this.sorting.orderBy === columnKey) {
            this.sorting.descending = !this.sorting.descending;
        } else {
            this.sorting.orderBy = columnKey;
            this.sorting.descending = false;
        }
        this.fetchQuotes();
    }

    async fetchQuotes() {
        this.loading = true;
        try {
            const response = await BookQuoteService.getBookQuotesPFS({
                pageNumber: this.currentPage,
                pageSize: this.pageSize,
                orderBy: this.sorting.orderBy,
                descending: this.sorting.descending,
                filterProperty: this.searchTerm ? "Text" : "",
                filter: this.searchTerm || ""
            });

            runInAction(() => {
                this.quotes = response.items ?? [];
                this.hasNextPage = this.currentPage < (response.totalPages ?? 1);
                this.loading = false;
            });
        } catch (error) {
            runInAction(() => {
                this.error = "Error fetching quotes.";
                this.loading = false;
            });
            console.error(error);
        }
    }

    async addQuote(quote) {
        try {
            const result = await BookQuoteService.add(quote);
            runInAction(() => {
                this.addStatus = { error: false, message: "" };
            });
            await this.fetchQuotes();
            return result;
        } catch (error) {
            runInAction(() => {
                this.addStatus = { error: true, message: "Problem adding quote." };
            });
        }
    }

    async editQuote(id, quote) {
        try {
            const result = await BookQuoteService.edit(id, quote);
            await this.fetchQuotes();
            return result;
        } catch (error) {
            return { error: true, message: "Error updating quote." };
        }
    }

    async deleteQuote(id) {
        try {
            await BookQuoteService.remove(id);
            await this.fetchQuotes();
            return { error: false };
        } catch (error) {
            return { error: true, message: "Error deleting quote." };
        }
    }

    async getQuoteById(id) {
        try {
            const result = await BookQuoteService.getById(id);
            return result;
        } catch (error) {
            return { error: true, message: "Error fetching quote." };
        }
    }
}

export default new BookQuoteStore();