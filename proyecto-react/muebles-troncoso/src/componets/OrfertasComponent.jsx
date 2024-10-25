import { useEffect, useState } from 'react';
import PromocionesService from '../services/PromocionesService';
import styles from "../css/Ofertas.module.css"; // Asegúrate de tener estilos para el banner

const Ofertas = () => {
  const [ofertas, setOfertas] = useState([]);
  const [countdown, setCountdown] = useState(60);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const promocionesData = await PromocionesService.getPromocionesRandom();
        const productosData = await PromocionesService.getProductos();

        // Filtrar promociones que estén activas
        const ahora = new Date();
        const promocionesActivas = promocionesData.filter(promocion => {
          const fechaFin = new Date(promocion.fechaFin);
          return fechaFin > ahora; // Solo incluir si la fecha de fin es futura
        });

        // Tomar solo la última promoción
        const ultimaPromocion = promocionesActivas.length > 0 ? promocionesActivas[promocionesActivas.length - 1] : null;

        // Empatar productos con promociones si existe
        if (ultimaPromocion) {
          const productosEnPromocion = ultimaPromocion.productos.map(productoId => {
            const producto = productosData.find(p => p.idProducto === productoId);
            return {
              ...producto,
              precioConDescuento: (producto.precio * 0.9).toFixed(2), // Aplica un 10% de descuento
            };
          });
          setOfertas({ ...ultimaPromocion, productos: productosEnPromocion });
        }
      } catch (error) {
        console.error('Error fetching ofertas:', error);
      }
    };

    fetchData();

    // Refrescar datos cada minuto
    const intervalId = setInterval(() => {
      fetchData();
      setCountdown(60); // Resetea el temporizador cada vez que se obtienen datos
    }, 60000); // 60000 ms = 1 minuto

    const countdownInterval = setInterval(() => {
      setCountdown(prev => (prev > 0 ? prev - 1 : 0)); // Decrementa el temporizador
    }, 1000); // 1000 ms = 1 segundo

    return () => {
      clearInterval(intervalId); // Limpiar el intervalo al desmontar el componente
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
                                    Quedan: {countdown} segundos
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
