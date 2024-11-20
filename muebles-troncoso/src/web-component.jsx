import React from 'react';
import ReactDOM from 'react-dom/client';
import UserChat from './componets/UserChat';
import reactToWebComponent from 'react-to-webcomponent';

// Convertir el componente de React a un Web Component
const ChatUsuarioComponent = reactToWebComponent(UserChat, React, ReactDOM);

// Registrar el Web Component
customElements.define('user-chat', ChatUsuarioComponent);
