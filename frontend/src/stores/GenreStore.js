import { makeAutoObservable, runInAction } from "mobx";
import GenreService from "../common/Services/GenreService";

class GenreStore {
    genres = [];
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

    sorting = { orderBy: "name", descending: false };

    constructor() {
        makeAutoObservable(this);
    }

    setSearchTerm(term) {
        this.searchTerm = term;
        this.currentPage = 1;
        this.fetchGenres();
    }

    setPage(page) {
        this.currentPage = page;
        this.fetchGenres();
    }

    setSorting(columnKey) {
        if (this.sorting.orderBy === columnKey) {
            this.sorting.descending = !this.sorting.descending;
        } else {
            this.sorting.orderBy = columnKey;
            this.sorting.descending = false;
        }
        this.fetchGenres();
    }

    async fetchGenres() {
        this.loading = true;
        try {
            const response = await GenreService.getGenresPFS({
                pageNumber: this.currentPage,
                pageSize: this.pageSize,
                orderBy: this.sorting.orderBy,
                descending: this.sorting.descending,
                filterProperty: this.searchTerm ? "name" : "",
                filter: this.searchTerm || ""
            });

            runInAction(() => {
            this.genres = response.items ?? [];
            this.hasNextPage = this.currentPage < (response.totalPages ?? 1);
            this.loading = false;
        });
        } catch (error) {
            runInAction(() => {
                this.error = "Error fetching Genres.";
                this.loading = false;
            });
            console.error(error);
        }
    }

    async addGenre(genre){
        try {
            const result = await GenreService.add(genre);
            runInAction(() => {
                this.addStatus = result;
            });
            await this.fetchGenres();
        } catch (error) {
            runInAction(() => {
                this.addStatus = { error: true, message: "Problem adding Genres" };
            });
        }
    }

    async editGenre(id, genre) {
        try {
            const result = await GenreService.edit(id, genre);
            await this.fetchGenres();
            return result;
        } catch (error) {
            return { error: true, message: "Error updating Genres" };
        }
    }

    async deleteGenre(id) {
        try {
            const result = await GenreService.remove(id);
            await this.fetchGenres();
            return result;
        } catch (error) {
            return { error: true, message: "Error deleting Genres" };
        }
    }

    async getGenreById(id) {
        try {
            const result = await GenreService.getById(id);
            return result;
        } catch (error) {
            return { error: true, message: "Error fetching Genres" };
        }
    }
}

export default new GenreStore();