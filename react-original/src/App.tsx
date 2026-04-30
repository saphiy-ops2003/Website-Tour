import { useState, useEffect } from 'react';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import Home from './pages/Home';
import Tours from './pages/Tours';
import Deals from './pages/Deals';
import Register from './pages/Register';
import Login from './pages/Login';
import Checkout from './pages/Checkout';

function App() {
  const [view, setView] = useState('home');

  // Scroll to top on view change
  useEffect(() => {
    window.scrollTo(0, 0);
  }, [view]);

  const renderView = () => {
    switch (view) {
      case 'home':
        return <Home setView={setView} />;
      case 'about':
        return <Home setView={setView} />; // Reusing home for this demo as it has about sections
      case 'tours':
        return <Tours />;
      case 'deals':
        return <Deals setView={setView} />;
      case 'register':
        return <Register setView={setView} />;
      case 'login':
        return <Login setView={setView} />;
      case 'checkout':
        return <Checkout setView={setView} />;
      default:
        return <Home setView={setView} />;
    }
  };

  const showNavFooter = !['login', 'register'].includes(view);

  return (
    <div className="min-h-screen flex flex-col selection:bg-primary-light selection:text-primary-dark">
      {showNavFooter && <Navbar currentView={view} setView={setView} />}
      <main className="flex-grow">
        {renderView()}
      </main>
      {showNavFooter && <Footer />}
    </div>
  );
}

export default App;
