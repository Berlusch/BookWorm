import { useState, useEffect } from "react";
import { observer } from "mobx-react";
import { useNavigate, useParams } from "react-router-dom";
import AuthorStore from "../../stores/AuthorStore";
import { RouteNames } from "../../common/constants";

const AuthorsEdit = observer(() => {
    const navigate = useNavigate();
    const { id } = useParams();

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        birthYear: "",
        deathYear: "",
        biography: "",
        nationalLiterature: ""
    });

    useEffect(() => {
        const loadAuthor = async () => {
            const result = await AuthorStore.getAuthorById(id);
            if (!result.error) {
                const a = result.data;
                setForm({
                    firstName: a.firstName ?? "",
                    lastName: a.lastName ?? "",
                    birthYear: a.birthYear ?? "",
                    deathYear: a.deathYear ?? "",
                    biography: a.biography ?? "",
                    nationalLiterature: a.nationalLiterature ?? ""
                });
            }
        };
        loadAuthor();
    }, [id]);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        const author = {
            ...form,
            birthYear: form.birthYear ? parseInt(form.birthYear) : null,
            deathYear: form.deathYear ? parseInt(form.deathYear) : null,
        };
        const result = await AuthorStore.editAuthor(id, author);
        if (!result.error) {
            navigate(RouteNames.AUTHORS_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Edit Author</h2>

            <div className="mb-3">
                <label className="form-label">First Name</label>
                <input className="form-control" name="firstName" value={form.firstName} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">Last Name</label>
                <input className="form-control" name="lastName" value={form.lastName} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">Birth Year</label>
                <input className="form-control" name="birthYear" type="number" value={form.birthYear} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">Death Year</label>
                <input className="form-control" name="deathYear" type="number" value={form.deathYear} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">National Literature</label>
                <input className="form-control" name="nationalLiterature" value={form.nationalLiterature} onChange={handleChange} />
            </div>
            <div className="mb-3">
                <label className="form-label">Biography</label>
                <textarea className="form-control" name="biography" rows={4} value={form.biography} onChange={handleChange} />
            </div>

            <button className="btn btn-primary me-2" onClick={handleSubmit}>Save</button>
            <button className="btn btn-secondary" onClick={() => navigate(RouteNames.AUTHORS_LIST)}>Cancel</button>
        </div>
    );
});

export default AuthorsEdit;