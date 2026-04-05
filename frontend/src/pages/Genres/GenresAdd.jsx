import { useState } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import GenreStore from "../../stores/GenreStore";
import { RouteNames } from "../../common/constants";

const GenresAdd = observer(() => {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        name: "",
        description: ""
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        await GenreStore.addGenre(form);
        if (!GenreStore.addStatus.error) {
            navigate(RouteNames.GENRES_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Add Genre</h2>

            {GenreStore.addStatus.message && (
                <div className={`alert ${GenreStore.addStatus.error ? "alert-danger" : "alert-success"}`}>
                    {GenreStore.addStatus.message}
                </div>
            )}

            <div className="mb-3">
                <label className="form-label">Name</label>
                <input className="form-control" name="name" value={form.name} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">Description</label>
                <textarea className="form-control" name="description" rows={4} value={form.description} onChange={handleChange} />
            </div>

            <button className="btn btn-primary me-2" onClick={handleSubmit}>Save</button>
            <button className="btn btn-secondary" onClick={() => navigate(RouteNames.GENRES_LIST)}>Cancel</button>
        </div>
    );
});

export default GenresAdd;