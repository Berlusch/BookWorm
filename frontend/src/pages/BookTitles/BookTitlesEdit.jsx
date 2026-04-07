import { useState, useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate, useParams } from "react-router-dom";
import { Typeahead } from "react-bootstrap-typeahead";
import "react-bootstrap-typeahead/css/Typeahead.css";
import BookTitleStore from "../../stores/BookTitleStore";
import LanguageStore from "../../stores/LanguageStore";
import GenreStore from "../../stores/GenreStore";
import AuthorService from "../../common/Services/AuthorService";
import { RouteNames } from "../../common/constants";

const BookTitlesEdit = observer(() => {
    const navigate = useNavigate();
    const { id } = useParams();

    const [form, setForm] = useState({
        title: "",
        subtitle: "",
        authorId: null,
        languageId: "",
        genreIds: []
    });

    const [authors, setAuthors] = useState([]);
    const [selectedAuthor, setSelectedAuthor] = useState([]);
    const [authorSearch, setAuthorSearch] = useState("");

    useEffect(() => {
        LanguageStore.fetchAllLanguages();
        GenreStore.fetchAllGenres();

        const loadBookTitle = async () => {
            const result = await BookTitleStore.getBookTitleById(id);
            if (!result.error) {
                const b = result.data;
                setForm({
                    title: b.title ?? "",
                    subtitle: b.subtitle ?? "",
                    authorId: b.authorId,
                    languageId: b.languageId?.toString() ?? "",
                    genreIds: b.genreIds ?? []
                });
                setSelectedAuthor([{ id: b.authorId, label: b.authorName }]);
            }
        };
        loadBookTitle();
    }, [id]);

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
        if (!form.authorId || !form.languageId) {
            alert("Please select an author and a language.");
            return;
        }
        const result = await BookTitleStore.editBookTitle(id, {
            title: form.title,
            subtitle: form.subtitle || null,
            authorId: form.authorId,
            languageId: parseInt(form.languageId),
            genreIds: form.genreIds
        });
        if (!result.error) {
            navigate(RouteNames.BOOK_TITLES_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Edit Book Title</h2>

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
                    selected={selectedAuthor}
                    onInputChange={(text) => setAuthorSearch(text)}
                    onChange={(selected) => {
                        setSelectedAuthor(selected);
                        setForm({ ...form, authorId: selected[0]?.id ?? null });
                    }}
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
                    {form.genreIds.map(gid => {
                        const genre = GenreStore.allGenres.find(g => g.id === gid);
                        return genre ? (
                            <span key={gid} className="badge bg-primary fs-6">
                                {genre.name}
                                <span
                                    className="ms-2"
                                    style={{ cursor: "pointer" }}
                                    onClick={() => setForm({ ...form, genreIds: form.genreIds.filter(id => id !== gid) })}
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

export default BookTitlesEdit;