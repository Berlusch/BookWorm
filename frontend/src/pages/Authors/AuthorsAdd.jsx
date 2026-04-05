import { useState } from "react";
import { observer } from "mobx-react";
import { useNavigate } from "react-router-dom";
import AuthorStore from "../../stores/AuthorStore";
import { RouteNames } from "../../common/constants";

const AuthorsAdd = observer(() => {
    const navigate = useNavigate();

    const [form, setForm] = useState({
        firstName: "",
        lastName: "",
        birthYear: "",
        deathYear: "",
        biography: "",
        nationalLiterature: ""
    });

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async () => {
        const author = {
            ...form,
            birthYear: form.birthYear ? parseInt(form.birthYear) : null,
            deathYear: form.deathYear ? parseInt(form.deathYear) : null,
        };
        await AuthorStore.addAuthor(author);
        if (!AuthorStore.addStatus.error) {
            navigate(RouteNames.AUTHORS_LIST);
        }
    };

    return (
        <div className="container mt-4">
            <h2>Add Author</h2>

            {AuthorStore.addStatus.message && (
                <div className={`alert ${AuthorStore.addStatus.error ? "alert-danger" : "alert-success"}`}>
                    {AuthorStore.addStatus.message}
                </div>
            )}

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

export default AuthorsAdd;