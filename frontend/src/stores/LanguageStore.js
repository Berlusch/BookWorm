import { makeAutoObservable, runInAction } from "mobx";
import LanguageService from "../common/Services/LanguageService";

class LanguageStore {
    allLanguages = [];
    languages = [];
    currentPage = 1;
    pageSize = 5;
    hasNextPage = false;
    loading = false;
    error = null;

    constructor() {
        makeAutoObservable(this);
    }

    setPage(page) {
        this.currentPage = page;
        this.fetchLanguages();
    }

    async fetchAllLanguages() {
    try {
        const response = await LanguageService.getLanguagesPFS({
            pageNumber: 1,
            pageSize: 9999,
            orderBy: "name",
            descending: false,
            filterProperty: "",
            filter: ""
        });
        runInAction(() => {
            this.allLanguages = response.items ?? [];
        });
    } catch (error) {
        console.error("Error fetching all languages:", error);
    }
}

    async fetchLanguages() {
        this.loading = true;
        try {
            const response = await LanguageService.getLanguagesPFS({
                pageNumber: this.currentPage,
                pageSize: this.pageSize
            });

            runInAction(() => {
            this.languages = response.items ?? [];
            this.hasNextPage = this.currentPage < (response.totalPages ?? 1);
            this.loading = false;
        });
        } catch (error) {
            runInAction(() => {
                this.error = "Error fetching languages.";
                this.loading = false;
            });
            console.error(error);
        }
    }
}

export default new LanguageStore();