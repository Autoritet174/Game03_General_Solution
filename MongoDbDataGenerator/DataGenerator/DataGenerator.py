import random

from pymongo import MongoClient
import bson
import uuid
import random


def Generate():
    # Строка подключения
    uri = "mongodb://localhost:27017/"

    # Создаём клиент
    client = MongoClient(uri, uuidRepresentation='standard')

    # Выбираем базу данных и коллекцию (они создадутся, если не существуют)
    db = client['userData']

    # Используем стандартное пространство имён (например, DNS)
    # namespace = uuid.NAMESPACE_DNS  # или NAMESPACE_URL, NAMESPACE_OID и др.

    user_uuid = uuid.UUID("113ae534-2310-40e3-a895-f3747ea976ca")

    # r = random.Random
    # random_number = random.randint(0, 100)

    mode = 1
    if mode == 1:
        collection = db['heroes']
        # # Данные для вставки
        for i in range(50):
            data = {
                "owner_id": user_uuid,
                "hero_id": random.randint(1, 5),
                "health": random.randint(0, 100),
                "attack": random.randint(0, 100),
                "speed": random.randint(0, 100),
                "strength": random.randint(0, 100),
                "agility": random.randint(0, 100),
                "intelligence": random.randint(0, 100)
            }
            result = collection.insert_one(data)

        # Обновляем имя поля 'speed' на 'haste'
        result = collection.update_many({"speed": {"$exists": True}}, {"$rename": {"speed": "haste"}})

    if mode == 2:
        collection = db['heroes']

    # Закрываем соединение
    client.close()
    print(f"выполнено")
