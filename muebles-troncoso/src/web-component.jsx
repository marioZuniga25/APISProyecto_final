import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import reactToWebComponent from 'react-to-webcomponent';

// Convertir el componente de React a un Web Component
const AppComponent = reactToWebComponent(App, React, ReactDOM);

// Registrar el Web Component

customElements.define('app-component', AppComponent);
