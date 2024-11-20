# chat_app/config.py
import os

MONGO_URI = os.getenv("MONGO_URI", "mongodb+srv://admin:admin@muebles-troncoso.kqxfz.mongodb.net/?retryWrites=true&w=majority&appName=muebles-troncoso")

SECRET_KEY = os.getenv("SECRET_KEY", "mysecret")
