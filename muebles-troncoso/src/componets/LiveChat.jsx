import { useState, useEffect } from 'react';
import socket from '../socket';
import axios from 'axios';
import Swal from 'sweetalert2';
import '../css/LiveChat.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch, faComments, faPaperPlane } from '@fortawesome/free-solid-svg-icons';

const LiveChat = () => {
  const [currentUser, setCurrentUser] = useState(null);
  const [selectedRoom, setSelectedRoom] = useState(null);
  const [message, setMessage] = useState('');
  const [messages, setMessages] = useState([]);
  const [salas, setSalas] = useState([]);
  const [selectedRoomName, setSelectedRoomName] = useState('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    // Obtener el usuario autenticado
    axios.get('http://localhost:5000/current_user', { withCredentials: true })
      .then(response => {
        setCurrentUser(response.data);
        if (response.data.role === 1) {
          axios.get('http://localhost:5000/api/salas/activos', { withCredentials: true })
            .then(res => {
              setSalas(res.data);
            })
            .catch(err => console.error("Error al obtener las salas activas:", err));
        }
      })
      .catch(error => {
        console.error("Error al obtener el usuario autenticado:", error);
      });

    socket.on('message', (msg) => {
      setMessages((prevMessages) => [...prevMessages, msg]);
    });

    socket.on('fetch_messages', (historicalMessages) => {
      setMessages(historicalMessages);
    });

    return () => {
      socket.off('message');
      socket.off('fetch_messages');
    };
  }, []);

  const handleRoomChange = (roomId, roomName) => {
    const room = roomName;  
    setSelectedRoom(roomId);
    setSelectedRoomName(roomName);
    setMessages([]);
    socket.emit('join', { user_id: currentUser.user_id, room });
    socket.emit('fetch_messages', { user_id: currentUser.user_id, room });
  };

  const handleSendMessage = () => {
    if (message.trim()) {
      const messageData = {
        user_id: currentUser.user_id,
        room: selectedRoomName,
        text: message,
        usuario: currentUser.username
      };
      socket.emit('message', messageData);
      setMessage('');
    }
  };

  const handleMarkAsAttended = () => {
    Swal.fire({
      title: '¿Estás seguro?',
      text: "¿Quieres marcar esta sala como atendida y borrar los mensajes?",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, marcar como atendida'
    }).then((result) => {
      if (result.isConfirmed) {
        // Llamar a la API para finalizar la sala y eliminar los mensajes
        axios.post('http://localhost:5000/api/salas/finalizar', 
          { sala_id: selectedRoom }, 
          { withCredentials: true }
        )
          .then(() => {
            Swal.fire('¡Marcado!', 'La sala ha sido marcada como atendida.', 'success');
            // Actualizar la lista de salas para reflejar el cambio
            setSalas(salas.filter(sala => sala._id !== selectedRoom));
            setSelectedRoom(null);
            setMessages([]);
          })
          .catch(err => {
            console.error("Error al marcar la sala como atendida:", err);
            Swal.fire('Error', 'No se pudo marcar la sala como atendida.', 'error');
          });
      }
    });
  };

  const filteredSalas = salas.filter(sala =>
    sala.sala.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="live-chat">
      <div className="chat-sidebar">
        <h3><FontAwesomeIcon icon={faComments} /> Salas de Chat</h3>
        {currentUser && currentUser.role === 1 && (
          <>
            <div className="room-search">
              <FontAwesomeIcon icon={faSearch} />
              <input
                type="text"
                placeholder="Buscar sala..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
            <div className="room-cards">
              {filteredSalas.map((sala) => (
                <div 
                key={sala._id} 
                className={`room-card ${selectedRoom === sala._id ? "active" : ""}`}
                onClick={() => handleRoomChange(sala._id, sala.sala)}
                >
                  {sala.sala}
                </div>
              ))}
            </div>
          </>
        )}
      </div>
      <div className="chat-container">
  {selectedRoom ? (
    <>
      <div className="chat-header">
        <h3>{selectedRoomName}</h3>
        <button className="btn-attend" onClick={handleMarkAsAttended}  >
          Marcar como atendida
        </button>
      </div>
      <div className="chat-messages">
        {messages.map((msg, index) => (
          <div key={index} className={`message ${msg.user_id === currentUser.user_id ? "sent" : "received"}`}>
            <div className="message-content">
              <span>{msg.user_id === currentUser.user_id ? "Tú" : msg.usuario }: </span> 
              {msg.texto || msg.text} 
            </div>
            <span className={`message-time ${msg.user_id === currentUser.user_id ? "sent-time" : "received-time"}`}>
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
        <button onClick={handleSendMessage} className="send-button">
          <FontAwesomeIcon icon={faPaperPlane} />
        </button>
      </div>
    </>
  ) : (
    <p className="select-room">Selecciona una sala para comenzar a chatear</p>
  )}
</div>

    </div>
  );
};

export default LiveChat;
