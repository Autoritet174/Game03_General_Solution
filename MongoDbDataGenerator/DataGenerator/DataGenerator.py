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
        db['heroes'].drop()
        collection = db['heroes']
        # Данные для вставки
        for i in range(5000):
            data = {
                "OwnerId": user_uuid,
                "HeroId": random.randint(1, 5),
                "Health": random.randint(0, 100),
                "Attack": random.randint(0, 100),
                "Speed": random.randint(0, 100),
                "Str": random.randint(0, 100),
                "Agi": random.randint(0, 100),
                "Int": random.randint(0, 100)
            }
            result = collection.insert_one(data)

    if mode == 2:
        db['equipment'].drop()
        collection = db['equipment']
        for i in range(100):
            data = {
                "OwnerId": user_uuid,
                "HeroId": random.randint(1, 5),
                "EquipmentTypeId": 1,
                "SlotId": 0,  # 0 - в инвентаре, 1+ - в слотах героя
                "Attack": {
                    "C": random.randint(1, 5),
                    "S": random.randint(20, 50),
                    "M": random.randint(0, 5)
                },
                "Level": random.randint(1, 20),
                "SAP": random.randint(10, 20),
                "Str": random.randint(0, 100),
                "Agi": random.randint(0, 100),
                "Int": random.randint(0, 100)
            }
            result = collection.insert_one(data)

        stats = db.command("collstats", "equipment")
        print(f"Размер коллекции: {stats['size']} байт")

    # Закрываем соединение
    client.close()
    print(f"выполнено")
