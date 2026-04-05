import { Outlet } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { RouteNames } from '../common/constants';

const MainLayout = () => {
  return (
    <div className="app">
      <header>
        <nav className="navbar navbar-expand-lg navbar-dark bg-dark px-3">
          <Link className="navbar-brand" to={RouteNames.HOME}>BookWorm</Link>
          <div className="collapse navbar-collapse">
            <ul className="navbar-nav">
              <li className="nav-item">
                <Link className="nav-link" to={RouteNames.AUTHORS_LIST}>Authors</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={RouteNames.GENRES_LIST}>Genres</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={RouteNames.LANGUAGES_LIST}>Languages</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={RouteNames.BOOK_TITLES_LIST}>Book Titles</Link>
              </li>
              <li className="nav-item">
                <Link className="nav-link" to={RouteNames.BOOK_QUOTES_LIST}>Book Quotes</Link>
              </li>
            </ul>
          </div>
        </nav>
      </header>

      <main className="main p-4">
        <Outlet />
        <div style={{ textAlign: 'center', paddingTop: '30px', fontSize: '12px' }}>
          &copy; Bernarda Lusch 2026
        </div>
      </main>
    </div>
  );
};

export default MainLayout;