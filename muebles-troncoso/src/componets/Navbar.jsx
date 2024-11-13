// src/components/Navbar.jsx

import { Link } from 'react-router-dom';
import '../css/Navbar.css'; // Archivo CSS para estilos específicos

const Navbar = () => {
  return (
    <nav className="navbar">
      <ul className="navbar-links">
        <li><Link to="/dashboard">Dashboard</Link></li>
        <li><Link to="/live-chat">Chat en Vivo</Link></li>
        <li><Link to="/settings">Configuración</Link></li>
      </ul>
    </nav>
  );
};

export default Navbar;
