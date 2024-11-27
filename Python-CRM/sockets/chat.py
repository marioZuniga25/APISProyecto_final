from flask_socketio import emit, join_room
from datetime import datetime
from bson import ObjectId
from models.models import mongo_db

def get_user_role_and_token(user_id):
    """Busca el rol y el session_token de un usuario usando el user_id en MongoDB."""
    user_data = mongo_db.tokens.find_one({"user_id": int(user_id)}, {"role": 1, "session_token": 1})
    if user_data:
        return user_data.get("role", 0), user_data.get("session_token")
    return None, None

def register_socketio_events(socketio):
    @socketio.on('join')
    def on_join(data):
        user_id = data.get('user_id')
        room = data.get('room')


        if not room or not user_id:
            emit('error', {'message': 'Falta el ID de usuario o la sala'})
            return

        user_role, session_token = get_user_role_and_token(user_id)

        if user_role is None:
            emit('error', {'message': 'Usuario no encontrado o sin permisos'})
            return

        # Comprobar si la sala existe en la base de datos
        sala_existente = mongo_db.mensajes.find_one({"sala": room})

        if sala_existente:
            join_room(room)
            emit('status', f'User {user_id} has entered the room {room}', to=room)
            print(f"Usuario {user_id} se unió a la sala {room}")
        else:
            emit('error', {'message': 'La sala no existe'})
            print(f"Error: La sala {room} no existe.")

    @socketio.on('fetch_messages')
    def handle_fetch_messages(data):
        user_id = data.get('user_id')
        room = data.get('room')
        print(room)
        print('ROMMM DEL FETCH')
        
        if not user_id:
            emit('error', {'message': 'Falta el ID de usuario'})
            return

        user_role, session_token = get_user_role_and_token(user_id)
        if user_role is None:
            emit('error', {'message': 'Usuario no encontrado o sin permisos'})
            return

        # Comprobar si la sala existe en la base de datos
        sala_existente = mongo_db.mensajes.find_one({"sala": room})

        if sala_existente:
            # Si la sala existe, se busca el historial de mensajes
            messages = sala_existente.get("comentarios", [])
            emit('fetch_messages', messages)
            print(f"Mensajes históricos enviados para la sala {room}")
            
            # Aquí puedes unirte a la sala solo si existe
            join_room(room)
            emit('status', f'User {user_id} has entered the room {room}', to=room)
            print(f"Usuario {user_id} se unió a la sala {room}")
        else:
            # Si la sala no existe, puedes optar por crearla o manejarlo como un error
            emit('error', {'message': 'La sala no existe'})
            print(f"Error: La sala {room} no existe.")



    @socketio.on('message')
    def handle_message(data):
        user_id = data.get('user_id')
        if not user_id:
            emit('error', {'message': 'Falta el ID de usuario'})
            return

        user_role, session_token = get_user_role_and_token(user_id)
        if user_role is None:
            emit('error', {'message': 'Usuario no encontrado o sin permisos'})
            return

        room = data.get('room', f"Sala de {data.get('usuario', 'Anónimo')}")

        # Comprobar si la sala existe en la base de datos
        sala_existente = mongo_db.mensajes.find_one({"sala": room})

        if not sala_existente:
            # Crear la sala si no existe
            mongo_db.mensajes.insert_one({"sala": room, "comentarios": []})
            print(f"Creada nueva sala: {room}")

        # Verificar permisos para enviar mensajes
        if user_role == 1 or room == f"Sala de {data.get('usuario', 'Anónimo')}":
            # Construir el mensaje
            message = {
                '_id': str(ObjectId()),
                'user_id': user_id,
                'usuario': data.get('usuario', 'Anónimo'),
                'texto': data.get('text', ''),
                'timestamp': datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                'deleted': False,
                'edited': False,
                'sala_nombre': room
            }

            # Almacenar el mensaje en MongoDB
            mongo_db.mensajes.update_one(
                {'sala': room},
                {'$push': {'comentarios': message}},
                upsert=True
            )

            # Emitir el mensaje en tiempo real
            emit('message', message, to=room)
            print(f"Mensaje almacenado y emitido en la sala {room}: {message}")

            # Emitir el historial completo de mensajes después de la creación de la sala
            sala_actualizada = mongo_db.mensajes.find_one({"sala": room})
            mensajes_actualizados = sala_actualizada.get("comentarios", [])
            join_room(room)  # Unir al cliente a la sala si aún no está
            emit('fetch_messages', mensajes_actualizados, to=room)
        else:
            emit('error', {'message': 'No tienes permisos para enviar mensajes en esta sala'})
            print("Error: Usuario intentó enviar un mensaje sin permisos.")