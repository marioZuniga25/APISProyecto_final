// src/App.jsx

import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './componets/Navbar';
import Dashboard from './componets/Dashboard';
import LiveChat from './componets/LiveChat';
import Settings from './componets/Settings';
import UserChat from './componets/UserChat';

function App() {
  return (
    <Router>
      <Navbar />
      <div className="content">
        <Routes>
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/live-chat" element={<LiveChat />} />
          <Route path="/settings" element={<Settings />} />
          <Route path="/User" element={<UserChat />} />

        </Routes>
      </div>
    </Router>
  );
}

export default App;
