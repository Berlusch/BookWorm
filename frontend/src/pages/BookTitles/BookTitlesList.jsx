import { useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import BookTitleStore from "../../stores/BookTitleStore";
import { RouteNames } from "../../common/constants";

const BookTitlesList = observer(() => {
    const navigate = useNavigate();

    useEffect(() => {
        BookTitleStore.fetchBookTitles();
    }, []);

    const handleDelete = async (id) => {
        if (window.confirm("Are you sure you want to delete this book title?")) {
            const result = await BookTitleStore.deleteBookTitle(id);
            if (result.error) {
                alert(result.message);
            }
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Book Titles</h2>
                <button
                    className="btn btn-primary"
                    onClick={() => navigate(RouteNames.BOOK_TITLES_ADD)}
                >
                    Add Book Title
                </button>
            </div>

            <input
                type="text"
                className="form-control mb-3"
                placeholder="Search by title..."
                value={BookTitleStore.searchTerm}
                onChange={(e) => BookTitleStore.setSearchTerm(e.target.value)}
            />

            {BookTitleStore.loading && <p>Loading...</p>}
            {BookTitleStore.error && <p className="text-danger">{BookTitleStore.error}</p>}

            <table className="table table-striped">
                <thead>
                    <tr>
                        <th onClick={() => BookTitleStore.setSorting("title")} style={{ cursor: "pointer" }}>Title</th>
                        <th>Subtitle</th>
                        <th>Author</th>
                        <th>Language</th>
                        <th>Genres</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {BookTitleStore.bookTitles.map(book => (
                        <tr key={book.id}>
                            <td>{book.title}</td>
                            <td>{book.subtitle ?? "-"}</td>
                            <td>{book.authorName}</td>
                            <td>{book.languageName}</td>
                            <td>{book.genres?.join(", ")}</td>
                            <td>
                                <button
                                    className="btn btn-sm btn-warning me-2"
                                    onClick={() => navigate(RouteNames.BOOK_TITLES_EDIT.replace(":id", book.id))}
                                >
                                    Edit
                                </button>
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDelete(book.id)}
                                >
                                    Delete
                                </button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <div>
                <button
                    className="btn btn-secondary me-2"
                    disabled={BookTitleStore.currentPage === 1}
                    onClick={() => BookTitleStore.setPage(BookTitleStore.currentPage - 1)}
                >
                    Previous
                </button>
                <button
                    className="btn btn-secondary"
                    disabled={!BookTitleStore.hasNextPage}
                    onClick={() => BookTitleStore.setPage(BookTitleStore.currentPage + 1)}
                >
                    Next
                </button>
            </div>
        </div>
    );
});

export default BookTitlesList;