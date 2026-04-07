import { makeAutoObservable, runInAction } from "mobx";
import BookTitleService from "../common/Services/BookTitleService";

class BookTitleStore {
    allBookTitles = [];
    bookTitles = [];
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

    sorting = { orderBy: "title", descending: false };

    constructor() {
        makeAutoObservable(this);
    }

    setSearchTerm(term) {
        this.searchTerm = term;
        this.currentPage = 1;
        this.fetchBookTitles();
    }

    setPage(page) {
        this.currentPage = page;
        this.fetchBookTitles();
    }

    setSorting(columnKey) {
        if (this.sorting.orderBy === columnKey) {
            this.sorting.descending = !this.sorting.descending;
        } else {
            this.sorting.orderBy = columnKey;
            this.sorting.descending = false;
        }
        this.fetchBookTitles();
    }

    async fetchAllBookTitles() {
    try {
        const response = await BookTitleService.getBookTitlesPFS({
            pageNumber: 1,
            pageSize: 9999,
            orderBy: "title",
            descending: false,
            filterProperty: "",
            filter: ""
        });

        runInAction(() => {
            this.allBookTitles = response.items ?? [];
        });
    } catch (error) {
        console.error("Error fetching all book titles:", error);
    }
}

    async fetchBookTitles() {
        this.loading = true;
        try {
            const response = await BookTitleService.getBookTitlesPFS({
                pageNumber: this.currentPage,
                pageSize: this.pageSize,
                orderBy: this.sorting.orderBy,
                descending: this.sorting.descending,
                filterProperty: this.searchTerm ? "Title" : "",
                filter: this.searchTerm || ""
            });

            runInAction(() => {
                this.bookTitles = response.items ?? [];
                this.hasNextPage = this.currentPage < (response.totalPages ?? 1);
                this.loading = false;
            });
        } catch (error) {
            runInAction(() => {
                this.error = "Error fetching book titles.";
                this.loading = false;
            });
            console.error(error);
        }
    }

    async addBookTitle(bookTitle) {
        try {
            const result = await BookTitleService.add(bookTitle);
            runInAction(() => {
                this.addStatus = result;
            });
            await this.fetchBookTitles();
        } catch (error) {
            runInAction(() => {
                this.addStatus = { error: true, message: "Problem adding book title." };
            });
        }
    }

    async editBookTitle(id, bookTitle) {
        try {
            const result = await BookTitleService.edit(id, bookTitle);
            await this.fetchBookTitles();
            return result;
        } catch (error) {
            return { error: true, message: "Error updating book title." };
        }
    }

    async deleteBookTitle(id) {
        try {
            const result = await BookTitleService.remove(id);
            await this.fetchBookTitles();
            return result;
        } catch (error) {
            return { error: true, message: "Error deleting book title." };
        }
    }

    async getBookTitleById(id) {
        try {
            const result = await BookTitleService.getById(id);
            return result;
        } catch (error) {
            return { error: true, message: "Error fetching book title." };
        }
    }
}

export default new BookTitleStore();