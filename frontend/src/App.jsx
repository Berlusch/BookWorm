import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import AuthorsList from './pages/Authors/AuthorsList';
import { RouteNames } from './common/constants';
import AuthorsAdd from './pages/Authors/AuthorsAdd';
import AuthorsEdit from './pages/Authors/AuthorsEdit';
import LanguagesList from './pages/Languages/LanguagesList';
import BookQuotesList from './pages/BookQuotes/BookQuotesList';
import BookQuotesAdd from './pages/BookQuotes/BookQuotesAdd';
import BookQuotesEdit from './pages/BookQuotes/BookQuotesEdit';
import BookTitlesList from './pages/BookTitles/BookTitlesList';
import BookTitlesAdd from './pages/BookTitles/BookTitlesAdd';
import BookTitlesEdit from './pages/BookTitles/BookTitlesEdit';
import GenresEdit from './pages/Genres/GenresEdit';
import GenresList from './pages/Genres/GenresList';
import GenresAdd from './pages/Genres/GenresAdd';


function App() {
  return (
    <Router>
      <Routes>
        <Route element={<MainLayout />}>

          <Route path="/" element={<Home />} />

          <Route path={RouteNames.AUTHORS_LIST} element={<AuthorsList />} />
          <Route path={RouteNames.AUTHORS_ADD} element={<AuthorsAdd />} />
          <Route path={RouteNames.AUTHORS_EDIT} element={<AuthorsEdit/>} />

          <Route path={RouteNames.BOOK_QUOTES_LIST} element={<BookQuotesList />} />
          <Route path={RouteNames.BOOK_QUOTES_ADD} element={<BookQuotesAdd />} />
          <Route path={RouteNames.BOOK_QUOTES_EDIT} element={<BookQuotesEdit/>} />

          <Route path={RouteNames.BOOK_TITLES_LIST} element={<BookTitlesList />} />
          <Route path={RouteNames.BOOK_TITLES_ADD} element={<BookTitlesAdd />} />
          <Route path={RouteNames.BOOK_TITLES_EDIT} element={<BookTitlesEdit/>} />

          <Route path={RouteNames.GENRES_LIST} element={<GenresList />} />
          <Route path={RouteNames.GENRES_ADD} element={<GenresAdd  />} />
          <Route path={RouteNames.GENRES_EDIT} element={<GenresEdit/>} />

          <Route path={RouteNames.LANGUAGES_LIST} element={<LanguagesList/>} />
          
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
