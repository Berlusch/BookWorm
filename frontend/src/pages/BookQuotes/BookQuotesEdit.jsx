import { useEffect, useState } from "react";
import { observer } from "mobx-react";
import { useNavigate, useParams } from "react-router-dom";
import BookQuoteStore from "../../stores/BookQuoteStore";
import BookTitleStore from "../../stores/BookTitleStore";
import { RouteNames } from "../../common/constants";

const BookQuoteEdit = observer(() => {
    const navigate = useNavigate();
    const { id } = useParams();

    const [text, setText] = useState("");
    const [bookTitleId, setBookTitleId] = useState("");

    useEffect(() => {
        BookTitleStore.fetchAllBookTitles();

        const loadQuote = async () => {
            const quote = await BookQuoteStore.getQuoteById(parseInt(id));
            if (quote?.error) {
                alert(quote.message);
                navigate(RouteNames.BOOK_QUOTES);
            } else {
                setText(quote.text);
                setBookTitleId(quote.bookTitleId.toString());
            }
        };

        loadQuote();
    }, [id]);

    const handleSubmit = async () => {
        if (!text.trim() || !bookTitleId) {
            alert("Please fill in all fields.");
            return;
        }

        const result = await BookQuoteStore.editQuote(parseInt(id), {
            text,
            bookTitleId: parseInt(bookTitleId)
        });

        if (result?.error) {
            alert(result.message);
        } else {
            navigate(RouteNames.BOOK_QUOTES);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Edit Quote</h2>

            <div className="mb-3">
                <label className="form-label">Quote Text</label>
                <textarea
                    className="form-control"
                    rows={4}
                    value={text}
                    onChange={(e) => setText(e.target.value)}
                    placeholder="Enter quote..."
                />
            </div>

            <div className="mb-3">
                <label className="form-label">Book</label>
                <select
                    className="form-select"
                    value={bookTitleId}
                    onChange={(e) => setBookTitleId(e.target.value)}
                >
                    <option value="">-- Select a book --</option>
                    {BookTitleStore.allBookTitles.map(book => (
                        <option key={book.id} value={book.id}>
                            {book.title}
                        </option>
                    ))}
                </select>
            </div>

            <button className="btn btn-primary me-2" onClick={handleSubmit}>
                Save
            </button>
            <button className="btn btn-secondary" onClick={() => navigate(RouteNames.BOOK_QUOTES_LIST)}>
                Cancel
            </button>
        </div>
    );
});

export default BookQuoteEdit;