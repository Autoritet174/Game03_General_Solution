from pymongo import MongoClient
import bson
import uuid

# Строка подключения
uri = "mongodb://localhost:27017/"

# Создаём клиент
client = MongoClient(uri, uuidRepresentation='standard')

# Выбираем базу данных и коллекцию (они создадутся, если не существуют)
db = client['userData']
collection = db['heroes']

# Используем стандартное пространство имён (например, DNS)
namespace = uuid.NAMESPACE_DNS  # или NAMESPACE_URL, NAMESPACE_OID и др.

# Генерируем UUID5 (рекомендуется)
user_uuid = uuid.UUID("113ae534-2310-40e3-a895-f3747ea976ca")
hero_warrior_uuid = uuid.UUID("58e860c7-3819-4f5b-bdff-f36850411498")
hero_huntress_uuid = uuid.UUID("64ec68ff-ee4d-4204-a830-5f992f64fae9")
hero_orc_with_axes_uuid = uuid.UUID("b582d3e8-d673-462f-80ad-f59b0deb1373")
hero_hammerman_uuid = uuid.UUID("c5c4aaa5-a4d3-4f4b-ac8c-26d77f6ea198")
hero_rogue_uuid = uuid.UUID("eb93675d-d05f-43ad-857f-e47d43152e43")

# Данные для вставки
for i in range(7):
    data = {
        "o": user_uuid,
        "h": hero_rogue_uuid
    }
    result = collection.insert_one(data)

print(f"выполнено")

# Закрываем соединение
client.close()
