// src/components/Ofertas.jsx
import { useEffect, useState } from 'react';

const Ofertas = () => {
    const [ofertas, setOfertas] = useState([]);

    useEffect(() => {
        const fetchOfertas = async () => {
            try {
                const response = await fetch('/api/promociones'); // Reemplaza con tu endpoint real
                const data = await response.json();
                setOfertas(data);
            } catch (error) {
                console.error('Error fetching ofertas:', error);
            }
        };

        fetchOfertas();
    }, []);

    return (
        <div className="ofertas-container">
            <h2>Productos en Oferta</h2>
            {ofertas.length > 0 ? (
                <ul className="ofertas-list">
                    {ofertas.map((oferta) => (
                        <li key={oferta.IdPromocion}>
                            <h3>{oferta.Codigo}</h3>
                            <p>Productos: {oferta.Productos.join(', ')}</p>
                            <p>Fecha de Inicio: {new Date(oferta.FechaCreacion).toLocaleString()}</p>
                            <p>Fecha de Expiración: {new Date(oferta.FechaExpiracion).toLocaleString()}</p>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No hay ofertas disponibles en este momento.</p>
            )}
        </div>
    );
};

export default Ofertas;
