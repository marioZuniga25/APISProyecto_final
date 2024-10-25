import { useEffect, useState } from 'react';
import PromocionesService from '../services/PromocionesService';
import styles from "../css/Ofertas.module.css"; // Asegúrate de tener estilos para el banner

const Ofertas = () => {
  const [ofertas, setOfertas] = useState([]);
  const [countdown, setCountdown] = useState(60);
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const promocionesData = await PromocionesService.getPromocionesRandom();
        const productosData = await PromocionesService.getProductos();

        const ahora = new Date();
        const promocionesActivas = promocionesData.filter(promocion => {
          const fechaFin = new Date(promocion.fechaFin);
          return fechaFin > ahora; // Solo incluir si la fecha de fin es futura
        });

        const ultimaPromocion = promocionesActivas.length > 0 ? promocionesActivas[promocionesActivas.length - 1] : null;

        if (ultimaPromocion) {
          const productosEnPromocion = ultimaPromocion.productos.map(productoId => {
            const producto = productosData.find(p => p.idProducto === productoId);
            return {
              ...producto,
              precioConDescuento: (producto.precio * 0.9).toFixed(2), // Aplica un 10% de descuento
            };
          });
          setOfertas({ ...ultimaPromocion, productos: productosEnPromocion });
          setCountdown(60); // Reiniciar el contador al obtener nuevas ofertas
          setProgress(0); // Reiniciar el progreso al obtener nuevas ofertas
        }
      } catch (error) {
        console.error('Error fetching ofertas:', error);
      }
    };

    fetchData();

    const intervalId = setInterval(() => {
      fetchData();
    }, 60000); // Refrescar datos cada minuto

    const countdownInterval = setInterval(() => {
      setCountdown(prev => {
        if (prev > 0) {
          setProgress((prev - 1) * (100 / 60)); // Incrementa el progreso de acuerdo al tiempo restante
          return prev - 1;
        } else {
          clearInterval(countdownInterval); // Limpiar el intervalo si llega a cero
          return 0;
        }
      });
    }, 1000); // Actualizar cada segundo

    return () => {
      clearInterval(intervalId);
      clearInterval(countdownInterval);
    };
  }, []);

  return (
    <div className={styles.ofertasContainer}>
        <h2>Ofertas Relámpago</h2>
        {ofertas && ofertas.productos && ofertas.productos.length > 0 ? (
            <div className={styles.banner}>
                <div key={ofertas.idPromocionRandom} className={styles.ofertaCard}>
                    <div className={styles.productList}>
                        {ofertas.productos.map(producto => (
                            <div key={producto.idProducto} className={styles.productCard}>
                                <img src={producto.imagen} alt={producto.nombreProducto} className={styles.productImage} />
                                <div className={styles.productName}>{producto.nombreProducto}</div>
                                <div className={styles.productPrice}>Precio: ${producto.precio}</div>
                                <div className={styles.productDiscount}>Precio con Descuento: ${producto.precioConDescuento}</div>
                                <div className={styles.countdown}>
                                    <span>Quedan: {countdown} segundos</span>
                                </div>
                                <div className={styles.progressLoader}>
                                    <div className={styles.progress} style={{ width: `${progress}%` }}></div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        ) : (
            <p>No hay ofertas disponibles.</p>
        )}
    </div>
  );
};

export default Ofertas;
