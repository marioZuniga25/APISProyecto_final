import { useEffect, useState } from 'react';
import promocionesService from '../services/promocionesService';
import '../css/Promociones.css';
import Swal from 'sweetalert2';

const Promociones = () => {
  const [productos, setProductos] = useState([]);
  const [selectedProducts, setSelectedProducts] = useState([]);
  const [descuento, setDescuento] = useState('');
  const [fechaInicio, setFechaInicio] = useState('');
  const [fechaFin, setFechaFin] = useState('');
  const [nombrePromocion, setNombrePromocion] = useState('');
  const [recipientType, setRecipientType] = useState("todos"); // Tipo de destinatarios para el correo
  const [isLoading, setIsLoading] = useState(false);

  // Función para cargar los productos que no están en promoción
  const fetchProductos = async () => {
    try {
      const productosData = await promocionesService.getProductos();
      const productosSinPromocion = productosData.filter(producto => producto.enPromocion === 0);
      setProductos(productosSinPromocion);
    } catch (error) {
      console.error('Error fetching productos:', error);
    }
  };

  useEffect(() => {
    fetchProductos();
  }, []);

  const handleProductSelect = (producto) => {
    if (!selectedProducts.some(p => p.idProducto === producto.idProducto)) {
      setSelectedProducts([...selectedProducts, producto]);
    }
  };

  const handleRemoveProduct = (productoId) => {
    setSelectedProducts(selectedProducts.filter(p => p.idProducto !== productoId));
  };

  // Nueva función para manejar el cambio en el campo de descuento con todas las validaciones
  const handleDescuentoChange = (e) => {
    let value = e.target.value;

    // Asegura que el valor sea un número, positivo y menor o igual a 100
    if (!isNaN(value)) {
      value = Math.abs(value); // Convierte a positivo
      if (value > 100) {
        value = 100; // Limita a 100
      }
      setDescuento(value);
    }
  };

  const handleSubmit = async () => {
    // Validación de campos antes de enviar
    if (!nombrePromocion || !descuento || !fechaInicio || !fechaFin || selectedProducts.length === 0 || !recipientType) {
      Swal.fire({
        icon: 'warning',
        title: 'Campos incompletos',
        text: 'Por favor, completa todos los campos y selecciona al menos un producto.',
      });
      return;
    }
  
    // Validación adicional para las fechas
    if (new Date(fechaInicio) > new Date(fechaFin)) {
      Swal.fire({
        icon: 'error',
        title: 'Fechas inválidas',
        text: 'La fecha de inicio no puede ser mayor que la fecha de fin.',
      });
      return;
    }
  
    const result = await Swal.fire({
      title: '¿Estás seguro?',
      text: "Se creará la promoción y una vez creada no se podrá editar ni eliminar, confirma los datos nuevamente.",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, crear promoción',
      cancelButtonText: 'Cancelar'
    });
    if (!result.isConfirmed) {
      return; // Detener si el usuario canceló
    }

    const promocion = {
      nombre: nombrePromocion,
      descuento: parseFloat(descuento),
      fechaInicio,
      fechaFin,
      productosIds: selectedProducts.map(p => p.idProducto),
    };
    setIsLoading(true);
  
    try {
      // Crear la promoción y obtener el ID
      const createdPromocion = await promocionesService.createPromocion(promocion);
      const promocionId = createdPromocion.idPromocion; // Capturar el ID de la promoción creada
      // Enviar correo de promoción usando el ID y recipientType
      await promocionesService.sendPromotionEmail(promocionId, recipientType);
      Swal.fire({
        icon: 'success',
        title: 'Promoción creada',
        text: 'La promoción se ha creado y los correos han sido enviados exitosamente.',
      });
      // Limpiar los campos después de la creación de la promoción y envío de correo
      setNombrePromocion('');
      setDescuento('');
      setFechaInicio('');
      setFechaFin('');
      setSelectedProducts([]);
      setRecipientType("todos");
  
      // Recargar productos para actualizar la lista de disponibles
      await fetchProductos();
  
    } catch (error) {
      console.error('Error creando la promoción y enviando correo:', error);
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Hubo un problema al crear la promoción o enviar el correo.',
      });
    }finally {
      setIsLoading(false); 
    }
  };
  
  return (
    <div className="promociones-container">
      {/* Carrusel de Productos */}
      <div className="carrusel-productos">
        <h2>Productos</h2>
        <div className="carrusel">
          {Array.isArray(productos) && productos.map((producto) => (
            <div 
              className="producto-card" 
              key={producto.idProducto} 
              onClick={() => handleProductSelect(producto)}
            >
              <img 
                src={producto.imagen} 
                alt={producto.nombreProducto} 
                className="producto-img" 
              />
              <h3>{producto.nombreProducto}</h3>
              <p>Precio: ${producto.precio}</p>
            </div>
          ))}
        </div>
      </div>

      <div className="formulario-promocion">
      <h2>Crear Nueva Promoción</h2>
      
      <div className="input-container">
        <input 
          type="text" 
          placeholder=" " 
          value={nombrePromocion}
          onChange={(e) => setNombrePromocion(e.target.value)} 
        />
        <label>Nombre de la promoción</label>
      </div>

      <div className="productos-seleccionados">
        <h3>Productos Seleccionados</h3>
        {selectedProducts.map(producto => (
          <div key={producto.idProducto} className="producto-seleccionado">
            <img src={producto.imagen} alt={producto.nombreProducto} />
            <p>{producto.nombreProducto}</p>
            <button className='botonEliminar' onClick={() => handleRemoveProduct(producto.idProducto)}>Eliminar</button>
          </div>
        ))}
      </div>

      <div className="input-container">
        <input 
          type="number" 
          placeholder=" " 
          value={descuento} 
          onChange={handleDescuentoChange}
          min="0"
          max="100"
          step="1"
        />
        <label>Descuento (%)</label>
      </div>

      <div className="input-container">
        <input 
          type="date" 
          placeholder=" " 
          value={fechaInicio} 
          onChange={(e) => setFechaInicio(e.target.value)} 
        />
        <label>Fecha de Inicio</label>
      </div>

      <div className="input-container">
        <input 
          type="date" 
          placeholder=" " 
          value={fechaFin} 
          onChange={(e) => setFechaFin(e.target.value)} 
        />
        <label>Fecha de Fin</label>
      </div>

      <div className="input-container">
        <select value={recipientType} onChange={(e) => setRecipientType(e.target.value)}>
          <option value="todos">Todos</option>
          <option value="frecuentes">Más Frecuentes</option>
          <option value="nuevos">Nuevos Usuarios</option>
        </select>
        <label>Enviar correo de promoción a:</label>
      </div>

      <button className='btnCrear' onClick={handleSubmit} disabled={isLoading}>
        {isLoading ? <div className="spinner"></div> : 'Crear Promoción y Enviar Correo'}
      </button>
    </div>
  </div>
);
};


export default Promociones;
