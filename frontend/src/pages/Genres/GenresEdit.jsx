import { useState, useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate, useParams } from "react-router-dom";
import GenreStore from "../../stores/GenreStore";
import { RouteNames } from "../../common/constants";

const GenresEdit = observer(() => {
    const navigate = useNavigate();
    const { id } = useParams();

    const [form, setForm] = useState({
        name: "",
        description: ""
    });

    useEffect(() => {
        const loadGenre = async () => {
            const result = await GenreStore.getGenreById(id);
            if (!result.error) {
                const g = result.data;
                setForm({
                    name: g.name ?? "",
                    description: g.description ?? ""
                });
            }
        };
        loadGenre();
    }, [id]);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        const result = await GenreStore.editGenre(id, form);
        if (!result.error) {
            navigate(RouteNames.GENRES_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Edit Genre</h2>

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

export default GenresEdit;