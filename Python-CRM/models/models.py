# chat_app/models/models.py
from pymongo import MongoClient
from config import MONGO_URI

# Crear un cliente y conectarse al servidor
client = MongoClient(MONGO_URI)
mongo_db = client['muebles-troncoso']  # Nombre de la base de datos

# Probar la conexión
try:
    client.admin.command('ping')
    print("Pinged your deployment. You successfully connected to MongoDB!")
except Exception as e:
    print(e)
