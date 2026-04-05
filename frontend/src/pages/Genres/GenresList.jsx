import { useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import GenreStore from "../../stores/GenreStore";
import { RouteNames } from "../../common/constants";

const GenresList = observer(() => {
    const navigate = useNavigate();

    useEffect(() => {
        GenreStore.fetchGenres();
    }, []);

    const handleDelete = async (id) => {
        if (window.confirm("Are you sure you want to delete this genre?")) {
            const result = await GenreStore.deleteGenre(id);
            if (result.error) {
                alert(result.message);
            }
        }
    };

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Genres</h2>
                <button
                    className="btn btn-primary"
                    onClick={() => navigate(RouteNames.GENRES_ADD)}
                >
                    Add Genre
                </button>
            </div>

            <input
                type="text"
                className="form-control mb-3"
                placeholder="Search by name..."
                value={GenreStore.searchTerm}
                onChange={(e) => GenreStore.setSearchTerm(e.target.value)}
            />

            {GenreStore.loading && <p>Loading...</p>}
            {GenreStore.error && <p className="text-danger">{GenreStore.error}</p>}

            <table className="table table-striped">
                <thead>
                    <tr>
                        <th onClick={() => GenreStore.setSorting("name")} style={{ cursor: "pointer" }}>Name</th>
                        <th>Description</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {GenreStore.genres.map(genre => (
                        <tr key={genre.id}>
                            <td>{genre.name}</td>
                            <td>{genre.description}</td>
                            <td>
                                <button
                                    className="btn btn-sm btn-warning me-2"
                                    onClick={() => navigate(RouteNames.GENRES_EDIT.replace(":id", genre.id))}
                                >
                                    Edit
                                </button>
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDelete(genre.id)}
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
                    disabled={GenreStore.currentPage === 1}
                    onClick={() => GenreStore.setPage(GenreStore.currentPage - 1)}
                >
                    Previous
                </button>
                <button
                    className="btn btn-secondary"
                    disabled={!GenreStore.hasNextPage}
                    onClick={() => GenreStore.setPage(GenreStore.currentPage + 1)}
                >
                    Next
                </button>
            </div>
        </div>
    );
});

export default GenresList;