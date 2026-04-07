import { useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import BookQuoteStore from "../../stores/BookQuoteStore";
import BookTitleStore from "../../stores/BookTitleStore";
import { RouteNames } from "../../common/constants";

const BookQuoteAdd = observer(() => {
    const navigate = useNavigate();

    const [text, setText] = useState("");
    const [bookTitleId, setBookTitleId] = useState("");

    useEffect(() => {
        BookTitleStore.fetchAllBookTitles();
    }, []);

    const handleSubmit = async () => {
        if (!text.trim() || !bookTitleId) {
            alert("Please fill in all fields.");
            return;
        }

        const result = await BookQuoteStore.addQuote({
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
            <h2>Add Quote</h2>

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

export default BookQuoteAdd;