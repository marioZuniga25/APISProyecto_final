import { useState, useEffect } from 'react';
import socket from '../socket';
import axios from 'axios';
import '../css/UserChat.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPaperPlane  } from '@fortawesome/free-solid-svg-icons';

const UserChat = () => {
  const [currentUser, setCurrentUser] = useState(null);
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);
  const [userRoom, setUserRoom] = useState(null);

  useEffect(() => {
    // Obtener el usuario actual y unirse a la sala de chat
    axios.get('http://localhost:5000/current_user', { withCredentials: true })
      .then(response => {
        setCurrentUser(response.data);
        const room = `Sala de ${response.data.username}`;
        setUserRoom(room);
        socket.emit('join', { user_id: response.data.user_id, room });
        socket.emit('fetch_messages', { user_id: response.data.user_id, room });
      })
      .catch(error => {
        console.error("Error al obtener el usuario autenticado:", error);
      });

    // Escuchar mensajes históricos
    socket.on('fetch_messages', (historicalMessages) => {
      setMessages(historicalMessages);
    });

    // Escuchar nuevos mensajes en tiempo real
    socket.on('message', (msg) => {
      setMessages((prevMessages) => [...prevMessages, msg]);
    });

    // Limpiar los eventos del socket cuando el componente se desmonte
    return () => {
      socket.off('message');
      socket.off('fetch_messages');
    };
  }, []);

  const handleSendMessage = () => {
    if (message.trim() && currentUser && userRoom) {
      const messageData = {
        user_id: currentUser.user_id,
        room: userRoom,
        text: message,
        usuario: currentUser.username,
        timestamp: new Date().toISOString()
      };

      // Emitir el mensaje sin añadirlo manualmente al estado
      socket.emit('message', messageData);
      setMessage('');
    }
  };

  return (
    <div className="user-chat">
      <div className="chat-header">
        <h3>Chat con Soporte</h3>
      </div>
      <div className="chat-messages">
        {messages.map((msg, index) => (
          <div
            key={index}
            className={`messages ${msg.user_id === currentUser.user_id ? "sent" : "received"}`}
          >
            <div className="messages-content">
              <span className="messages-user">{msg.usuario}:</span>
              <span className="messages-text">{msg.texto || msg.text}</span>
            </div>
            <span className="messages-time">
              {new Date(msg.timestamp).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
            </span>
          </div>
        ))}
      </div>
      <div className="chat-input">
        <input
          type="text"
          placeholder="Escribe un mensaje..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
        />
        <button onClick={handleSendMessage}><FontAwesomeIcon icon={faPaperPlane} /></button>
      </div>
    </div>
  );
};

export default UserChat;
