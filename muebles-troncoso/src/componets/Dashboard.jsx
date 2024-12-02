import { useEffect, useState, useRef } from 'react';
import axios from 'axios';
import '../css/Dashboard.css';
import { Chart } from 'chart.js/auto';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComments, faCheckCircle, faExclamationCircle, faChartBar, faBell } from '@fortawesome/free-solid-svg-icons';
import PromocionesService from '../services/PromocionesService';
const Dashboard = () => {
  const [estadisticas, setEstadisticas] = useState({});
  const [topAdmins, setTopAdmins] = useState([]);
  const [usuariosPorRol, setUsuariosPorRol] = useState([]);
  const [alertas, setAlertas] = useState([]);
  const [promociones, setPromociones] = useState({ activas: [], finalizadas: [] });
  const [selectedPromo, setSelectedPromo] = useState(null); // Promoción seleccionada
  const [showModal, setShowModal] = useState(false); // Controla la visibilidad del modal


  // Referencias para las instancias de los gráficos
  const topAdminsChartRef = useRef(null);
  const usuariosPorRolChartRef = useRef(null);

  useEffect(() => {
    const fetchEstadisticas = async () => {
      try {
        const estadisticasResponse = await axios.get('http://localhost:5001/api/salas/estadisticas');
        setEstadisticas(estadisticasResponse.data);
      } catch (error) {
        console.error('Error fetching estadísticas:', error);
      }
    };

    const fetchTopAdmins = async () => {
      try {
        const topAdminsResponse = await axios.get('http://localhost:5001/api/admins/top_mensajes');
        setTopAdmins(topAdminsResponse.data);
        renderTopAdminsChart(topAdminsResponse.data);
      } catch (error) {
        console.error('Error fetching top admins:', error);
      }
    };

    const fetchUsuariosPorRol = async () => {
      try {
        const usuariosResponse = await axios.get('http://localhost:5001/api/usuarios/roles');
        setUsuariosPorRol(usuariosResponse.data);
        renderUsuariosPorRolChart(usuariosResponse.data);
      } catch (error) {
        console.error('Error fetching usuarios por rol:', error);
      }
    };

    const fetchAlertas = async () => {
      try {
        // Obtener las alertas
        const alertasResponse = await axios.get('http://localhost:5194/api/Dashboard/Alertas');
        const alertas = alertasResponse.data.inventarioBajo;

        // Obtener los productos
        const productosResponse = await PromocionesService.getProductos(); // Usar la función ya definida para obtener productos
        const productos = productosResponse;

        // Anidar las imágenes a las alertas según el nombre del producto
        const alertasConImagenes = alertas.map((alerta) => {
          const producto = productos.find(
            (producto) => producto.nombreProducto === alerta.nombreProducto
          );
          return {
            ...alerta,
            imagen: producto ? producto.imagen : null, // Añadir la imagen si existe
          };
        });
        console.log(alertasConImagenes)
        setAlertas(alertasConImagenes);
      } catch (error) {
        console.error('Error fetching alertas:', error);
      }
    };


    const fetchPromociones = async () => {
      try {
        const promocionesResponse = await axios.get('http://localhost:5194/api/Dashboard/PromocionesActivasYResultados');
        setPromociones(promocionesResponse.data);
        console.log("HOLIIII")
        console.log(promocionesResponse.data)
      } catch (error) {
        console.error('Error fetching promociones:', error);
      }
    };

    fetchEstadisticas();
    fetchTopAdmins();
    fetchUsuariosPorRol();
    fetchAlertas();
    fetchPromociones();
  }, []);

  const handleVerDetalle = async (idPromocion) => {
    try {
      // Llamada al endpoint principal
      const response = await axios.get(`http://localhost:5194/api/Promociones/${idPromocion}`);
      const promoData = response.data;

      // Llamada al endpoint adicional para el porcentaje de descuento y precio final
      const detalleResponse = await axios.get(
        `http://localhost:5194/api/Promociones/GetDetallePromocion`,
        {
          params: { idPromocion },
        }
      );
      const detallesPromocion = detalleResponse.data;

      // Combinar los datos y guardar en el estado
      setSelectedPromo({
        ...promoData,
        detalles: promoData.detalles.map((detalle) => {
          const detalleExtra = detallesPromocion.find(
            (d) => d.idProducto === detalle.producto.idProducto
          );
          return {
            ...detalle,
            porcentajeDescuento: detalleExtra?.porcentajeDescuento || 0,
            precioFinal: detalleExtra?.precioFinal || detalle.producto.precio,
          };
        }),
      });
      setShowModal(true); // Muestra el modal
    } catch (error) {
      console.error('Error fetching detalles de la promoción:', error);
    }
  };


  const handleCloseModal = () => {
    setShowModal(false);
    setSelectedPromo(null); // Resetea la promoción seleccionada
  };


  const renderTopAdminsChart = (data) => {
    const ctx = document.getElementById('topAdminsChart').getContext('2d');
    if (topAdminsChartRef.current) {
      topAdminsChartRef.current.destroy(); // Destruye el gráfico existente
    }
    topAdminsChartRef.current = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: data.map((admin) => admin.admin),
        datasets: [
          {
            label: 'Mensajes Atendidos',
            data: data.map((admin) => admin.totalMensajes),
            backgroundColor: '#36A2EB',
          },
        ],
      },
      options: {
        responsive: true,
        plugins: {
          legend: { display: false },
        },
        scales: {
          x: { title: { display: true, text: 'Administradores' } },
          y: { title: { display: true, text: 'Mensajes Atendidos' } },
        },
      },
    });
  };

  const renderUsuariosPorRolChart = (data) => {
    const ctx = document.getElementById('usuariosPorRolChart').getContext('2d');
    if (usuariosPorRolChartRef.current) {
      usuariosPorRolChartRef.current.destroy(); // Destruye el gráfico existente
    }
    usuariosPorRolChartRef.current = new Chart(ctx, {
      type: 'pie',
      data: {
        labels: data.map((d) => d.role === 0 ? 'Usuarios' : 'Administradores'),
        datasets: [
          {
            data: data.map((d) => d.cantidad),
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'],
          },
        ],
      },
    });
  };

  return (
    <div className="dashboard-container">
      <section className="stats-section">
        <h2>Estadísticas de Salas</h2>
        <div className="stats-grid">
          <div className="stat-card">
            <FontAwesomeIcon icon={faChartBar} className="stat-icon" />
            <h3>Total Salas</h3>
            <p>{estadisticas.total_salas}</p>
          </div>
          <div className="stat-card">
            <FontAwesomeIcon icon={faCheckCircle} className="stat-icon" />
            <h3>Salas Atendidas</h3>
            <p>{estadisticas.atendidas}</p>
          </div>
          <div className="stat-card">
            <FontAwesomeIcon icon={faExclamationCircle} className="stat-icon" />
            <h3>Salas No Atendidas</h3>
            <p>{estadisticas.no_atendidas}</p>
          </div>
          <div className="stat-card">
            <FontAwesomeIcon icon={faComments} className="stat-icon" />
            <h3>Promedio de Comentarios</h3>
            <p>{estadisticas.promedio_comentarios}</p>
          </div>
        </div>
      </section>

      <section className="charts-section">
        <h2>Gráficos</h2>
        <div className="charts-grid">
          <div className="chart-container">
            <h3>Administradores con Más Mensajes</h3>
            <canvas id="topAdminsChart"></canvas> {/* Cambiado a topAdminsChart */}
          </div>
          <div className="chart-container">
            <h3>Usuarios por Rol</h3>
            <canvas id="usuariosPorRolChart"></canvas>
          </div>
        </div>
      </section>

      <section className="alerts-section">
        <h2>
          <FontAwesomeIcon icon={faBell} style={{ color: '#e74c3c' }} /> Alertas de Inventario
        </h2>
        <ul>
          {alertas.map((producto, index) => (
            <li key={index} className="alert-item">
              <img
                src={producto.imagen}
                alt={producto.nombreProducto}
                className="alert-img"
              />
              <div className="alert-content">
                <strong>{producto.nombreProducto}</strong>
                <span style={{ color: '#e74c3c' }}> ({producto.stock} en stock)</span>
              </div>
            </li>
          ))}
        </ul>
      </section>


      <section className="promotions-section">
        <h2>
          <FontAwesomeIcon icon={faChartBar} style={{ color: '#36A2EB' }} /> Promociones Activas y Finalizadas
        </h2>
        <div className="promotions-grid">
          {/* Promociones Activas */}
          <div className="promotion-card">
            <div>

              <h3> <FontAwesomeIcon icon={faCheckCircle} className="promotion-icon" /> Promociones Activas</h3>
              <p>{promociones.activas.length}</p>
              <ul className="promotion-details">
                {promociones.activas.map((promo, index) => (
                  <li key={index}>
                    <hr />
                    <p><strong>Nombre de promoción</strong>: {promo.nombre}</p>
                    <br /> <strong>Fecha Fin</strong>: {new Date(promo.fechaFin).toLocaleDateString()}
                    <br />
                    <br />
                    <button className='btn btn-primary' onClick={() => handleVerDetalle(promo.idPromocion)} > Ver detalle</button>
                    <hr />
                  </li>
                ))}
              </ul>
            </div>
          </div>

          {/* Promociones Finalizadas */}
          <div className="promotion-card">
            <div>

              <h3> <FontAwesomeIcon icon={faExclamationCircle} className="promotion-icon" /> Promociones Finalizadas</h3>
              <p>{promociones.finalizadas.length}</p>
              <ul className="promotion-details">
                {promociones.finalizadas.map((promo, index) => (
                  <li key={index}>
                    <hr />
                    <p><strong>Nombre de promoción</strong>:  {promo.nombre}</p>
                    <strong>Fecha Fin</strong>: {new Date(promo.fechaFin).toLocaleDateString()}
                    <br />
                    <br />
                    <button className='btn btn-primary' onClick={() => handleVerDetalle(promo.idPromocion)} > Ver detalle</button>
                    <hr />
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </div>
      </section>

      {showModal && (
        <div className="modal-overlay">
          <div className="modal-container">
            <div className="modal-header">
              <h2><strong>{selectedPromo ? selectedPromo.nombre : ''}</strong></h2>
              <button className="btn-close" onClick={handleCloseModal}>
                ×
              </button>
            </div>
            <div className="modal-body">
              {selectedPromo ? (
                <div>
                  <p>
                    <strong>Nombre:</strong> {selectedPromo.nombre}
                  </p>
                  <p>
                    <strong>Fecha Inicio:</strong>{' '}
                    {new Date(selectedPromo.fechaInicio).toLocaleDateString()}
                  </p>
                  <p>
                    <strong>Fecha Fin:</strong>{' '}
                    {new Date(selectedPromo.fechaFin).toLocaleDateString()}
                  </p>
                  <h5 className='text-center'>Productos en Promoción</h5>
                  <ul>
                    {selectedPromo.detalles.map((detalle) => (
                      <p key={detalle.idDetallePromocion}>
                        <p>
                          <strong>{detalle.producto.nombreProducto}</strong>
                        </p>
                        <p>
                          <strong>Descripción:</strong> {detalle.producto.descripcion}
                        </p>
                        <p>
                          <strong>Precio:</strong> ${detalle.producto.precio}
                        </p>
                        <p>
                          <strong>Porcentaje Descuento:</strong> {detalle.porcentajeDescuento}%
                        </p>
                        <p>
                          <strong>Precio Final:</strong> ${detalle.precioFinal}
                        </p>
                        <p>
                          <strong>Stock:</strong> {detalle.producto.stock}
                        </p>
                        <img src={detalle.producto.imagen} alt="Imagen de producto" className='img-detalle-producto' />

                      </p>
                    ))}
                  </ul>
                </div>
              ) : (
                <p>Cargando detalles...</p>
              )}
            </div>
            <div className="modal-footer">
              <button className="btn btn-secondary" onClick={handleCloseModal}>
                Cerrar
              </button>
            </div>
          </div>
        </div>
      )}

    </div>
  );
};
export default Dashboard;

