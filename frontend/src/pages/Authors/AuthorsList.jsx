import { useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import AuthorStore from "../../stores/AuthorStore";
import { RouteNames } from "../../common/constants";

const AuthorsList = observer(() => {
    const navigate = useNavigate();

    useEffect(() => {
        AuthorStore.fetchAuthors();
    }, []);

    const handleDelete = async (id) => {
    if (window.confirm("Are you sure you want to delete this author?")) {
        const result = await AuthorStore.deleteAuthor(id);
        if (result.error) {
            alert(result.message);
        }
    }
};

    return (
        <div className="container mt-4">
            <div className="d-flex justify-content-between align-items-center mb-3">
                <h2>Authors</h2>
                <button
                    className="btn btn-primary"
                    onClick={() => navigate(RouteNames.AUTHORS_ADD)}
                >
                    Add Author
                </button>
            </div>

            <input
                type="text"
                className="form-control mb-3"
                placeholder="Search by last name..."
                value={AuthorStore.searchTerm}
                onChange={(e) => AuthorStore.setSearchTerm(e.target.value)}
            />

            {AuthorStore.loading && <p>Loading...</p>}
            {AuthorStore.error && <p className="text-danger">{AuthorStore.error}</p>}

            <table className="table table-striped">
                <thead>
                    <tr>
                        <th onClick={() => AuthorStore.setSorting("firstName")} style={{ cursor: "pointer" }}>First Name</th>
                        <th onClick={() => AuthorStore.setSorting("lastName")} style={{ cursor: "pointer" }}>Last Name</th>
                        <th>Birth Year</th>
                        <th>Death Year</th>
                        <th>National Literature</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {AuthorStore.authors.map(author => (
                        <tr key={author.id}>
                            <td>{author.firstName}</td>
                            <td>{author.lastName}</td>
                            <td>{author.birthYear ?? "-"}</td>
                            <td>{author.deathYear ?? "-"}</td>
                            <td>{author.nationalLiterature}</td>
                            <td>
                                <button
                                    className="btn btn-sm btn-warning me-2"
                                    onClick={() => navigate(RouteNames.AUTHORS_EDIT.replace(":id", author.id))}
                                >
                                    Edit
                                </button>
                                <button
                                    className="btn btn-sm btn-danger"
                                    onClick={() => handleDelete(author.id)}
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
                    disabled={AuthorStore.currentPage === 1}
                    onClick={() => AuthorStore.setPage(AuthorStore.currentPage - 1)}
                >
                    Previous
                </button>
                <button
                    className="btn btn-secondary"
                    disabled={!AuthorStore.hasNextPage}
                    onClick={() => AuthorStore.setPage(AuthorStore.currentPage + 1)}
                >
                    Next
                </button>
            </div>
        </div>
    );
});

export default AuthorsList;