import React from 'react';
import ReactDOM from 'react-dom/client';
import seguimiento from './componets/SegimientoPedido';
import reactToWebComponent from 'react-to-webcomponent';

// Convertir el componente de React a un Web Component
const seguimientoPedido = reactToWebComponent(seguimiento, React, ReactDOM);

// Registrar el Web Component
customElements.define('seguimiento-pedido', seguimientoPedido);
