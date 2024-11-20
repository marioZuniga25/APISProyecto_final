// src/App.jsx

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './componets/Navbar';
import Dashboard from './componets/Dashboard';
import LiveChat from './componets/LiveChat';
import Promociones from './componets/Promociones';
function App() {
  return (
    <Router basename="/crm">
      <Navbar />
      <div className="content">
      <Routes>
        <Route index element={<Dashboard />} /> {/* Ruta predeterminada */}
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/live-chat" element={<LiveChat />} />
        <Route path="/promociones" element={<Promociones />} />
      </Routes>
      </div>
    </Router>
  );
}

export default App;
