import { useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import BookQuoteStore from "../../stores/BookQuoteStore";
import { RouteNames } from "../../common/constants";

const BookQuotesList = observer(() => {
    const navigate = useNavigate();

    useEffect(() => {
        BookQuoteStore.fetchQuotes();
    }, []);

    const handleDelete = async (id) => {
        if (window.confirm("Are you sure you want to delete this quote?")) {
            const result = await BookQuoteStore.deleteQuote(id);
            if (result.error) {
                alert(result.message);
            }
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Book Quotes</h2>
                <button
                    className="btn btn-primary"
                    onClick={() => navigate(RouteNames.BOOK_QUOTES_ADD)}
                >
                    Add Quote
                </button>
            </div>

            <input
                type="text"
                className="form-control mb-3"
                placeholder="Search by quote text..."
                value={BookQuoteStore.searchTerm}
                onChange={(e) => BookQuoteStore.setSearchTerm(e.target.value)}
            />

            {BookQuoteStore.loading && <p>Loading...</p>}
            {BookQuoteStore.error && <p className="text-danger">{BookQuoteStore.error}</p>}

            <table className="table table-striped">
                <thead>
                    <tr>
                        <th onClick={() => BookQuoteStore.setSorting("text")} style={{ cursor: "pointer" }}>Quote</th>
                        <th onClick={() => BookQuoteStore.setSorting("bookTitleName")} style={{ cursor: "pointer" }}>Book</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {BookQuoteStore.quotes.map(quote => (
                        <tr key={quote.id}>
                            <td>{quote.text}</td>
                            <td>{quote.bookTitleName}</td>
                            <td>
                                <button
                                    className="btn btn-sm btn-warning me-2"
                                    onClick={() => navigate(RouteNames.BOOK_QUOTES_EDIT.replace(":id", quote.id))}
                                >
                                    Edit
                                </button>
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDelete(quote.id)}
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
                    disabled={BookQuoteStore.currentPage === 1}
                    onClick={() => BookQuoteStore.setPage(BookQuoteStore.currentPage - 1)}
                >
                    Previous
                </button>
                <button
                    className="btn btn-secondary"
                    disabled={!BookQuoteStore.hasNextPage}
                    onClick={() => BookQuoteStore.setPage(BookQuoteStore.currentPage + 1)}
                >
                    Next
                </button>
            </div>
        </div>
    );
});

export default BookQuotesList;