import { useEffect, useState } from 'react';
import '../css/SeguimientoPedido.css';
import servicioPedidos from '../services/promocionesService';
import axios from 'axios';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faShoppingCart, faTruck, faCheckCircle } from '@fortawesome/free-solid-svg-icons';


const SeguimientoPedidos = () => {
    const [pedidos, setPedidos] = useState([]);
    const [pedidoSeleccionado, setPedidoSeleccionado] = useState(null);
    const [rangoEntrega, setRangoEntrega] = useState("");
  
    useEffect(() => {
      const obtenerPedidosUsuario = async () => {
        try {
          const response = await axios.get('http://localhost:5000/current_user', {
            withCredentials: true,
          });
          const idUsuario = response.data.user_id;
  
          if (idUsuario) {
            const data = await servicioPedidos.getPedidosByUsuario(idUsuario);
            setPedidos(data);
          }
        } catch (error) {
          console.error('Error al obtener los pedidos o usuario autenticado:', error);
        }
      };
  
      obtenerPedidosUsuario();
    }, []);
  
    useEffect(() => {
      if (pedidoSeleccionado) {
        const { estatus, fechaVenta } = pedidoSeleccionado;
  
        if (estatus === "Pedido" || estatus === "Enviado") {
          const fechaInicial = new Date(fechaVenta);
          const rango = calcularRangoEntrega(fechaInicial, 5, 15);
          setRangoEntrega(rango);
        } else {
          setRangoEntrega("");
        }
      }
    }, [pedidoSeleccionado]);
  
    const handleSelectPedido = (pedido) => {
      setPedidoSeleccionado({ ...pedido });
    };
  
    const getEstatusClass = (estatus) => {
      switch (estatus) {
        case 'Pedido':
          return 1;
        case 'Enviado':
          return 2;
        case 'Entregado':
          return 3;
        default:
          return 0;
      }
    };

    const renderProductos = (productos) => {
      return productos.map((producto, index) => (
        <div key={index}>
          <strong>{producto.nombreProducto}</strong><br />
          Precio: ${producto.precioUnitario} | Cantidad: {producto.cantidad} | Total: ${producto.precioUnitario * producto.cantidad}
        </div>
      ));
    };
  
    const calcularRangoEntrega = (fechaInicial, diasMin, diasMax) => {
      const sumarDiasHabiles = (fecha, dias) => {
        let contador = 0;
        while (contador < dias) {
          fecha.setDate(fecha.getDate() + 1);
          if (fecha.getDay() !== 0 && fecha.getDay() !== 6) {
            contador++;
          }
        }
        return new Date(fecha);
      };
      const inicio = sumarDiasHabiles(new Date(fechaInicial), diasMin);
      const fin = sumarDiasHabiles(new Date(fechaInicial), diasMax);
      const opciones = { day: "2-digit", month: "short" };
      const inicioStr = inicio.toLocaleDateString("es-MX", opciones);
      const finStr = fin.toLocaleDateString("es-MX", opciones);
      return `${inicioStr} - ${finStr}`;
    };
    return (
      <div className="bodys">
        <div className="seguimiento-pedido">
          <div className="details">
          <div className="estatus-entrega">
            {pedidoSeleccionado?.estatus === "Pedido" || pedidoSeleccionado?.estatus === "Enviado" ? (
              <p>Entrega: 5 - 15 días hábiles ({rangoEntrega})</p>
            ) : pedidoSeleccionado?.estatus === "Entregado" ? (
              <p>Entregado</p>
            ) : null}
          </div>
            <div className="track">
              <ul id="progress" className="text-tracer">
                <li className={pedidoSeleccionado?.estatus === 'Pedido' || getEstatusClass(pedidoSeleccionado?.estatus) >= 1 ? 'active' : ''}></li>
                <li className={pedidoSeleccionado?.estatus === 'Enviado' || getEstatusClass(pedidoSeleccionado?.estatus) >= 2 ? 'active' : ''}></li>
                <li className={pedidoSeleccionado?.estatus === 'Entregado' || getEstatusClass(pedidoSeleccionado?.estatus) >= 3 ? 'active' : ''}></li>
              </ul>
            </div>

            <div className="lists">
              <div className="list">
                <FontAwesomeIcon icon={faShoppingCart} color="purple" />
                <p> Pedido</p>
              </div>
              <div className="list">
                <FontAwesomeIcon icon={faTruck} color="purple" />
                <p> Enviado</p>
              </div>
              <div className="list">
                <FontAwesomeIcon icon={faCheckCircle} color="purple" />
                <p> Entregado</p>
              </div>
            </div>
          </div>
        </div>
  
        {/* Contenedor de tabla con desplazamiento */}
        <div className="table-container">
          <table className="table table-hover">
            <thead>
              <tr>
                <th className="d-none">ID</th>
                <th>Producto</th>
                <th>Total Compra</th>
                <th>Fecha</th>
                <th>Estatus</th>
              </tr>
            </thead>
            <tbody>
              {pedidos.length > 0 ? (
                pedidos.map((pedido, index) => (
                  <tr key={index} className={pedidoSeleccionado?.idPedido === pedido.idPedido ? 'seleccionado' : ''} onClick={() => handleSelectPedido(pedido)}>
                    <td className="d-none">{pedido.idPedido}</td>
                    <td>{renderProductos(pedido.productos)}</td>
                    <td>${pedido.productos.reduce((total, producto) => total + producto.precioUnitario * producto.cantidad, 0)} </td>
                    <td>{new Date(pedido.fechaVenta).toLocaleDateString()}</td>
                    <td>{pedido.estatus}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7">No hay pedidos disponibles</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    );
};
  
export default SeguimientoPedidos;
