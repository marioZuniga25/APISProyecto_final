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

 
  // Referencias para las instancias de los gráficos
  const topAdminsChartRef = useRef(null);
  const usuariosPorRolChartRef = useRef(null);

  useEffect(() => {
    const fetchEstadisticas = async () => {
      try {
        const estadisticasResponse = await axios.get('http://localhost:5000/api/salas/estadisticas');
        setEstadisticas(estadisticasResponse.data);
      } catch (error) {
        console.error('Error fetching estadísticas:', error);
      }
    };

    const fetchTopAdmins = async () => {
      try {
        const topAdminsResponse = await axios.get('http://localhost:5000/api/admins/top_mensajes');
        setTopAdmins(topAdminsResponse.data);
        renderTopAdminsChart(topAdminsResponse.data);
      } catch (error) {
        console.error('Error fetching top admins:', error);
      }
    };

    const fetchUsuariosPorRol = async () => {
      try {
        const usuariosResponse = await axios.get('http://localhost:5000/api/usuarios/roles');
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
    <div className="promotion-card">
      <FontAwesomeIcon icon={faCheckCircle} className="promotion-icon" />
      <div>
        <h3>Promociones Activas</h3>
        <p>{promociones.activas.length}</p>
      </div>
    </div>
    <div className="promotion-card">
      <FontAwesomeIcon icon={faExclamationCircle} className="promotion-icon" />
      <div>
        <h3>Promociones Finalizadas</h3>
        <p>{promociones.finalizadas.length}</p>
      </div>
    </div>
  </div>
</section>


    </div>
  );
};

export default Dashboard;
