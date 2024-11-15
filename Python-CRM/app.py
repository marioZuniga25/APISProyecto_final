from flask import Flask, request, jsonify, session
from flask_socketio import SocketIO
from flask_login import current_user, login_user, UserMixin, LoginManager
from flask_cors import CORS
from models.models import mongo_db
from bson import ObjectId
import uuid
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

if __name__ == '__main__':
    socketio.run(app, host='0.0.0.0', port=5001, debug=False)
