// src/App.jsx
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './componets/Navbar';
import Dashboard from './componets/Dashboard';
import LiveChat from './componets/LiveChat';
import Promociones from './componets/Promociones';
import SeguimientoPedido from './componets/SegimientoPedido';
import 'bootstrap/dist/css/bootstrap.min.css';
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
        <Route path="/seguimientoPedido" element={<SeguimientoPedido />} />
      </Routes>
      </div>
    </Router>
  );
}

export default App;