import { useState, useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import { Typeahead } from "react-bootstrap-typeahead";
import "react-bootstrap-typeahead/css/Typeahead.css";
import BookTitleStore from "../../stores/BookTitleStore";
import LanguageStore from "../../stores/LanguageStore";
import GenreStore from "../../stores/GenreStore";
import AuthorService from "../../common/Services/AuthorService";
import { RouteNames } from "../../common/constants";

const BookTitlesAdd = observer(() => {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        title: "",
        subtitle: "",
        authorId: null,
        languageId: "",
        genreIds: []
    });

    const [authors, setAuthors] = useState([]);
    const [authorSearch, setAuthorSearch] = useState("");

    useEffect(() => {
        LanguageStore.fetchAllLanguages();
        GenreStore.fetchAllGenres();
    }, []);

    useEffect(() => {
        const fetchAuthors = async () => {
            if (authorSearch.length < 2) return;
            const response = await AuthorService.getAuthorsPFS({
                filterProperty: "LastName",
                filter: authorSearch,
                pageSize: 10
            });
            setAuthors(response.items?.map(a => ({ id: a.id, label: `${a.firstName} ${a.lastName}` })) ?? []);
        };
        fetchAuthors();
    }, [authorSearch]);

    const handleSubmit = async () => {
        if (!form.title.trim() || !form.authorId || !form.languageId) {
            alert("Please fill in title, author and language.");
            return;
        }
        await BookTitleStore.addBookTitle({
            title: form.title,
            subtitle: form.subtitle || null,
            authorId: form.authorId,
            languageId: parseInt(form.languageId),
            genreIds: form.genreIds
        });
        if (!BookTitleStore.addStatus.error) {
            navigate(RouteNames.BOOK_TITLES_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Add Book Title</h2>

            {BookTitleStore.addStatus.message && (
                <div className={`alert ${BookTitleStore.addStatus.error ? "alert-danger" : "alert-success"}`}>
                    {BookTitleStore.addStatus.message}
                </div>
            )}

            <div className="mb-3">
                <label className="form-label">Title</label>
                <input
                    className="form-control"
                    value={form.title}
                    onChange={(e) => setForm({ ...form, title: e.target.value })}
                />
            </div>

            <div className="mb-3">
                <label className="form-label">Subtitle</label>
                <input
                    className="form-control"
                    value={form.subtitle}
                    onChange={(e) => setForm({ ...form, subtitle: e.target.value })}
                />
            </div>

            <div className="mb-3">
                <label className="form-label">Author</label>
                <Typeahead
                    id="author-typeahead"
                    options={authors}
                    onInputChange={(text) => setAuthorSearch(text)}
                    onChange={(selected) => setForm({ ...form, authorId: selected[0]?.id ?? null })}
                    placeholder="Type to search author..."
                />
            </div>

            <div className="mb-3">
                <label className="form-label">Language</label>
                <select
                    className="form-select"
                    value={form.languageId}
                    onChange={(e) => setForm({ ...form, languageId: e.target.value })}
                >
                    <option value="">-- Select a language --</option>
                    {LanguageStore.allLanguages.map(l => (
                        <option key={l.id} value={l.id}>{l.name}</option>
                    ))}
                </select>
            </div>

            <div className="mb-3">
                <label className="form-label">Genres</label>
                <select
                    className="form-select"
                    onChange={(e) => {
                        const genreId = parseInt(e.target.value);
                        if (genreId && !form.genreIds.includes(genreId)) {
                            setForm({ ...form, genreIds: [...form.genreIds, genreId] });
                        }
                        e.target.value = "";
                    }}
                >
                    <option value="">-- Add a genre --</option>
                    {GenreStore.allGenres
                        .filter(g => !form.genreIds.includes(g.id))
                        .map(g => (
                            <option key={g.id} value={g.id}>{g.name}</option>
                        ))}
                </select>

                <div className="d-flex flex-wrap gap-2 mt-2">
                    {form.genreIds.map(id => {
                        const genre = GenreStore.allGenres.find(g => g.id === id);
                        return genre ? (
                            <span key={id} className="badge bg-primary fs-6">
                                {genre.name}
                                <span
                                    className="ms-2"
                                    style={{ cursor: "pointer" }}
                                    onClick={() => setForm({ ...form, genreIds: form.genreIds.filter(gid => gid !== id) })}
                                >
                                    &times;
                                </span>
                            </span>
                        ) : null;
                    })}
                </div>
            </div>

            <button className="btn btn-primary me-2" onClick={handleSubmit}>Save</button>
            <button className="btn btn-secondary" onClick={() => navigate(RouteNames.BOOK_TITLES_LIST)}>Cancel</button>
        </div>
    );
});

export default BookTitlesAdd;