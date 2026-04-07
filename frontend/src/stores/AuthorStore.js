import { makeAutoObservable, runInAction } from "mobx";
import AuthorService from "../common/Services/AuthorService";

class AuthorStore {
    authors = [];
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

    sorting = { orderBy: "lastName", descending: false };

    constructor() {
        makeAutoObservable(this);
    }

    setSearchTerm(term) {
        this.searchTerm = term;
        this.currentPage = 1;
        this.fetchAuthors();
    }

    setPage(page) {
        this.currentPage = page;
        this.fetchAuthors();
    }

    setSorting(columnKey) {
        if (this.sorting.orderBy === columnKey) {
            this.sorting.descending = !this.sorting.descending;
        } else {
            this.sorting.orderBy = columnKey;
            this.sorting.descending = false;
        }
        this.fetchAuthors();
    }

    async fetchAuthors() {
        this.loading = true;
        try {
            const response = await AuthorService.getAuthorsPFS({
                pageNumber: this.currentPage,
                pageSize: this.pageSize,
                orderBy: this.sorting.orderBy,
                descending: this.sorting.descending,
                filterProperty: this.searchTerm ? "LastName" : "",
                filter: this.searchTerm || ""
            });

            runInAction(() => {
            this.authors = response.items ?? [];
            this.hasNextPage = this.currentPage < (response.totalPages ?? 1);
            this.loading = false;
        });
        } catch (error) {
            runInAction(() => {
                this.error = "Error fetching authors.";
                this.loading = false;
            });
            console.error(error);
        }
    }

    async addAuthor(author) {
        try {
            const result = await AuthorService.add(author);
            runInAction(() => {
                this.addStatus = result;
            });
            await this.fetchAuthors();
        } catch (error) {
            runInAction(() => {
                this.addStatus = { error: true, message: "Problem adding author." };
            });
        }
    }

    async editAuthor(id, author) {
        try {
            const result = await AuthorService.edit(id, author);
            await this.fetchAuthors();
            return result;
        } catch (error) {
            return { error: true, message: "Error updating author." };
        }
    }

    async deleteAuthor(id) {
        try {
            const result = await AuthorService.remove(id);
            await this.fetchAuthors();
            return result;
        } catch (error) {
            return { error: true, message: "Error deleting author." };
        }
    }

    async getAuthorById(id) {
        try {
            const result = await AuthorService.getById(id);
            return result;
        } catch (error) {
            return { error: true, message: "Error fetching author." };
        }
    }
}

export default new AuthorStore();