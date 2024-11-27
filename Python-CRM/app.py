from flask import Flask, request, jsonify, session
from flask_socketio import SocketIO
from flask_login import current_user, login_user, UserMixin, LoginManager
from flask_cors import CORS
from models.models import mongo_db
from bson import ObjectId
import uuid
from apscheduler.schedulers.background import BackgroundScheduler
app = Flask(__name__)
app.config.from_object('config')
app.secret_key = 'integrador'  # Necesario para usar sesiones con Flask

# Configuración de CORS para Angular y React
CORS(app, resources={r"/*": {"origins": ["http://localhost:4200", "http://localhost:5173"]}}, supports_credentials=True)

# Configuración de Flask-SocketIO
socketio = SocketIO(app, cors_allowed_origins=["http://localhost:4200", "http://localhost:5173"], max_http_buffer_size=1e8, async_mode="eventlet")

# Configuración de Flask-Login
login_manager = LoginManager()
login_manager.init_app(app)

# Clase de usuario para Flask-Login
class User(UserMixin):
    def __init__(self, id, nombre):
        self.id = id
        self.nombre = nombre

# user_loader para Flask-Login
@login_manager.user_loader
def load_user(user_id):
    # Cargar el usuario desde la sesión
    username = session.get("username", "Usuario")  # Usa "Usuario" como predeterminado si no está en la sesión
    return User(id=user_id, nombre=username)

@app.route('/login', methods=['POST'])
def login():
    data = request.get_json()
    user_id = data.get('user_id')
    username = data.get('username')
    role = data.get('rol')
    print(f"Usuario {user_id} y {username} y el rol es: {role}")
    
    if user_id and username:
        user = User(id=user_id, nombre=username)
        login_user(user)  # Autentica al usuario con Flask-Login
        session["username"] = username  # Guarda el nombre en la sesión
        session["user_id"] = user_id    # Guarda el ID en la sesión
        session["role"] = role
        # Generar token de sesión único
        session_token = str(uuid.uuid4())
        session["session_token"] = session_token

        # Almacena el token y rol en MongoDB
        mongo_db.tokens.update_one(
            {"user_id": user_id},
            {"$set": {"user_id": user_id, "session_token": session_token, "role": role}},
            upsert=True
        )

        return jsonify({"status": "success", "message": "User session created and room setup complete.", "session_token": session_token}), 200
    else:
        return jsonify({"status": "error", "message": "Missing user_id or username"}), 400


@app.route('/current_user', methods=['GET'])
def get_current_user():
    if current_user.is_authenticated:
        print(f"Usuario autenticado: {current_user.nombre}")
        return jsonify({
            "user_id": current_user.id,
            "username": current_user.nombre,
            "role": session.get("role")
        }), 200
    else:
        return jsonify({"error": "Usuario no autenticado"}), 401

@app.route('/api/salas', methods=['GET'])
def obtener_salas():
    try:
        salas = mongo_db.mensajes.find({}, {"sala": 1})
        lista_salas = [{"_id": str(sala["_id"]), "sala": sala["sala"]} for sala in salas]  # Convertir ObjectId a string
        print("Salas obtenidas:", lista_salas)  # Depuración para verificar las salas
        return jsonify(lista_salas), 200
    except Exception as e:
        print("Error al obtener las salas:", str(e))
        return jsonify({"error": "No se pudieron obtener las salas"}), 500

from sockets.chat import register_socketio_events
register_socketio_events(socketio)

@app.route('/api/salas/activos', methods=['GET'])
def listar_salas_activas():
    try:
        # Filtrar las salas que no están atendidas
        salas = mongo_db.mensajes.find({"atendida": {"$ne": True}})
        lista_salas = []
        for sala in salas:
            comentarios = sala.get("comentarios", [])
            lista_salas.append({
                "_id": str(sala["_id"]),
                "sala": sala["sala"],
                "comentarios": comentarios
            })
        return jsonify(lista_salas), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500


# Endpoint para finalizar una sala y eliminar los mensajes
@app.route('/api/salas/finalizar', methods=['POST'])
def finalizar_sala():
    data = request.get_json()
    sala_id = data.get('sala_id')

    try:
        result = mongo_db.mensajes.update_one(
            {"_id": ObjectId(sala_id)},
            {"$set": {"comentarios": [], "atendida": True}}
        )
        
        if result.modified_count > 0:
            return jsonify({"status": "success", "message": "Sala finalizada y mensajes eliminados"}), 200
        else:
            return jsonify({"status": "error", "message": "No se encontró la sala o ya estaba vacía"}), 404
    except Exception as e:
        return jsonify({"error": str(e)}), 500

@app.route('/api/salas/estadisticas', methods=['GET'])
def obtener_estadisticas():
    try:
        total_salas = mongo_db.mensajes.count_documents({})
        atendidas = mongo_db.mensajes.count_documents({"atendida": True})
        no_atendidas = total_salas - atendidas

        # Calcula el promedio de comentarios por sala
        salas = mongo_db.mensajes.find()
        total_comentarios = sum(len(sala.get("comentarios", [])) for sala in salas)
        promedio_comentarios = total_comentarios / total_salas if total_salas > 0 else 0

        estadisticas = {
            "total_salas": total_salas,
            "atendidas": atendidas,
            "no_atendidas": no_atendidas,
            "promedio_comentarios": round(promedio_comentarios, 2)
        }
        
        return jsonify(estadisticas), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500

# Endpoint para las salas más comentadas
@app.route('/api/admins/top_mensajes', methods=['GET'])
def obtener_admins_top_mensajes():
    try:
        admins_top_mensajes = mongo_db.mensajes.aggregate([
            # Desanidar el array de comentarios
            {"$unwind": "$comentarios"},
            # Asegurarnos de que sala_nombre sea un string válido
            {
                "$addFields": {
                    "sala_nombre_str": {
                        "$cond": {
                            "if": {"$not": {"$isArray": "$comentarios.sala_nombre"}},  # Validar que no sea array
                            "then": {"$ifNull": ["$comentarios.sala_nombre", ""]},  # Reemplazar valores nulos con string vacío
                            "else": "$comentarios.sala_nombre"
                        }
                    }
                }
            },
            # Filtrar mensajes donde el usuario no es el dueño de la sala
            {
                "$match": {
                    "$expr": {
                        "$ne": [
                            "$comentarios.usuario", 
                            {"$substr": ["$sala_nombre_str", 8, {"$strLenCP": "$sala_nombre_str"}]}  # Extraer el nombre después de "Sala de "
                        ]
                    }
                }
            },
            # Agrupar por administrador (usuario que no es dueño de la sala)
            {
                "$group": {
                    "_id": "$comentarios.usuario",  # Agrupar por el usuario (administrador)
                    "totalMensajes": {"$sum": 1},  # Contar mensajes atendidos
                    "salasAtendidas": {"$addToSet": "$comentarios.sala_nombre"}  # Listar salas atendidas
                }
            },
            # Ordenar por la cantidad de mensajes enviados
            {"$sort": {"totalMensajes": -1}},
            # Limitar a los top 5
            {"$limit": 5}
        ])

        # Convertir el cursor en una lista de diccionarios
        top_admins = [
            {
                "admin": admin["_id"],
                "totalMensajes": admin["totalMensajes"],
                "salasAtendidas": list(admin["salasAtendidas"])
            }
            for admin in admins_top_mensajes
        ]

        return jsonify(top_admins), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500





# Endpoint para usuarios por rol
@app.route('/api/usuarios/roles', methods=['GET'])
def usuarios_por_rol():
    try:
        roles = mongo_db.tokens.aggregate([
            {"$group": {"_id": "$role", "cantidad": {"$sum": 1}}}
        ])
        usuarios_roles = [{"role": rol["_id"], "cantidad": rol["cantidad"]} for rol in roles]
        return jsonify(usuarios_roles), 200
    except Exception as e:
        return jsonify({"error": str(e)}), 500


def limpiar_mensajes_antiguos():
    """Elimina mensajes de salas atendidas que tienen más de 30 días."""
    fecha_limite = datetime.now() - timedelta(days=7)
    mongo_db.mensajes.update_many(
        {"atendida": True, "comentarios.timestamp": {"$lt": fecha_limite.strftime('%Y-%m-%d %H:%M:%S')}},
        {"$pull": {"comentarios": {"timestamp": {"$lt": fecha_limite.strftime('%Y-%m-%d %H:%M:%S')}}}}
    )
    print("Mensajes antiguos eliminados.")

# Configurar el scheduler para ejecutar cada 24 horas
scheduler = BackgroundScheduler()
scheduler.add_job(limpiar_mensajes_antiguos, 'interval', days=1)
scheduler.start()

if __name__ == '__main__':
    try:
        socketio.run(app, host='0.0.0.0', port=5001, debug=False)
    except (KeyboardInterrupt, SystemExit):
        scheduler.shutdown()