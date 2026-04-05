import { useEffect } from "react";
import { observer } from "mobx-react";
import LanguageStore from "../../stores/LanguageStore";

const LanguagesList = observer(() => {
    useEffect(() => {
        LanguageStore.fetchLanguages();
    }, []);

    return (
        <div className="container mt-4">
            <h2>Languages</h2>

            {LanguageStore.loading && <p>Loading...</p>}
            {LanguageStore.error && <p className="text-danger">{LanguageStore.error}</p>}

            <table className="table table-striped">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                    </tr>
                </thead>
                <tbody>
                    {LanguageStore.languages.map(lang => (
                        <tr key={lang.id}>
                            <td>{lang.id}</td>
                            <td>{lang.name}</td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <div>
                <button
                    className="btn btn-secondary me-2"
                    disabled={LanguageStore.currentPage === 1}
                    onClick={() => LanguageStore.setPage(LanguageStore.currentPage - 1)}
                >
                    Previous
                </button>
                <button
                    className="btn btn-secondary"
                    disabled={!LanguageStore.hasNextPage}
                    onClick={() => LanguageStore.setPage(LanguageStore.currentPage + 1)}
                >
                    Next
                </button>
            </div>
        </div>
    );
});

export default LanguagesList;