import math
from itertools import product
from typing import List, Tuple, Optional


def find_dice_combination(
        target_mean: float,
        target_cv_percent: float,
        max_dice: int = 100,
        max_sides: int = -1,
        include_modifier: bool = False,
        exactly: bool = True
) -> List[Tuple[str, float, float, float]]:
    """
    Находит комбинации кубиков, приближающиеся к заданным параметрам.

    Args:
        target_mean: Целевое математическое ожидание
        target_cv_percent: Целевой коэффициент вариации в процентах (σ/μ * 100%)
        max_dice: Максимальное количество кубиков для перебора
        max_sides: Максимальное количество граней кубика
        include_modifier: Включать ли модификаторы (+N/-N)
        exactly: Точное значение

    Returns:
        Список кортежей (нотация, среднее, CV%, ошибка_среднего, ошибка_CV)
        отсортированный по суммарной ошибке
    """
    if max_sides == -1:
        max_sides = target_mean*2-1
    target_std = target_mean * (target_cv_percent / 100)

    results = []

    # Перебираем количество кубиков и грани
    for num_dice in range(1, max_dice + 1):
        for sides in range(2, max_sides + 1):

            if include_modifier:
                # С модификатором
                # Вычисляем необходимый модификатор для достижения target_mean
                # μ = n * (s+1)/2 + m
                # m = μ - n * (s+1)/2
                base_mean = num_dice * (sides + 1) / 2
                modifier = target_mean - base_mean

                # Модификатор должен быть целым числом
                mod_int = int(round(modifier))

                # Перебираем модификаторы вокруг вычисленного значения
                for mod_offset in [-2, -1, 0, 1, 2]:
                    mod = mod_int + mod_offset

                    # Итоговое среднее с модификатором
                    actual_mean = base_mean + mod

                    # Стандартное отклонение (не зависит от модификатора)
                    # σ = sqrt(n * (s² - 1) / 12)
                    variance = num_dice * (sides ** 2 - 1) / 12
                    actual_std = math.sqrt(variance)
                    actual_cv = (actual_std / actual_mean * 100) if actual_mean != 0 else 0

                    # Вычисляем ошибки
                    mean_error = abs(actual_mean - target_mean) / target_mean * 100
                    cv_error = abs(actual_cv - target_cv_percent)

                    # Общая ошибка (взвешенная)
                    total_error = 0.7 * mean_error + 0.3 * cv_error

                    # Формируем нотацию
                    if mod == 0:
                        notation = f"{num_dice}d{sides}"
                    else:
                        sign = "+" if mod > 0 else ""
                        notation = f"{num_dice}d{sides}{sign}{mod}"
                    if not exactly or actual_mean == target_mean:
                        results.append((
                            notation,
                            actual_mean,
                            actual_cv,
                            mean_error,
                            cv_error,
                            total_error
                        ))
            else:
                # Без модификатора
                actual_mean = num_dice * (sides + 1) / 2

                # Пропускаем если среднее слишком далеко от цели
                if abs(actual_mean - target_mean) > target_mean * 0.5:
                    continue

                # Стандартное отклонение
                variance = num_dice * (sides ** 2 - 1) / 12
                actual_std = math.sqrt(variance)
                actual_cv = (actual_std / actual_mean * 100) if actual_mean != 0 else 0

                # Ошибки
                mean_error = abs(actual_mean - target_mean) / target_mean * 100
                cv_error = abs(actual_cv - target_cv_percent)
                total_error = 0.7 * mean_error + 0.3 * cv_error
                if not exactly or actual_mean == target_mean:
                    results.append((
                        f"{num_dice}d{sides}_{actual_mean:.0f}",
                        actual_mean,
                        actual_cv,
                        mean_error,
                        cv_error,
                        total_error
                    ))

    # Сортируем по суммарной ошибке
    results.sort(key=lambda x: x[5])

    return results[:10]  # Возвращаем 10 лучших вариантов


def get_best_dice_combination(
        target_mean: float,
        target_cv_percent: float,
        precision: float = 5.0
) -> Optional[Tuple[str, float, float]]:
    """
    Находит лучшую комбинацию кубиков с заданной точностью.

    Args:
        target_mean: Целевое математическое ожидание
        target_cv_percent: Целевой коэффициент вариации в %
        precision: Максимально допустимая ошибка в %

    Returns:
        Кортеж (нотация, фактическое_среднее, фактический_CV%) или None
    """
    candidates = find_dice_combination(target_mean, target_cv_percent)

    for notation, actual_mean, actual_cv, mean_error, cv_error, total_error in candidates:
        if mean_error <= precision and cv_error <= precision:
            return notation, actual_mean, actual_cv

    return None


def analyze_dice_combo(notation: str) -> dict:
    """Анализирует параметры заданной комбинации кубиков."""
    import re

    pattern = r'^(\d+)d(\d+)([+-]\d+)?$'
    match = re.match(pattern, notation)

    if not match:
        raise ValueError(f"Некорректная нотация: {notation}")

    num_dice = int(match.group(1))
    sides = int(match.group(2))
    modifier = int(match.group(3)) if match.group(3) else 0

    # Математическое ожидание
    base_mean = num_dice * (sides + 1) / 2
    mean = base_mean + modifier

    # Стандартное отклонение
    variance = num_dice * (sides ** 2 - 1) / 12
    std = math.sqrt(variance)

    # Коэффициент вариации
    cv = (std / mean * 100) if mean != 0 else 0

    # Теоретический диапазон
    min_val = num_dice * 1 + modifier
    max_val = num_dice * sides + modifier

    return {
        'notation': notation,
        'num_dice': num_dice,
        'sides': sides,
        'modifier': modifier,
        'mean': mean,
        'std': std,
        'cv_percent': cv,
        'min': min_val,
        'max': max_val,
        'range_size': max_val - min_val + 1
    }


def interactive_finder():
    """Интерактивный поиск комбинации кубиков."""
    print("=" * 60)
    print("Поиск комбинации кубиков по параметрам")
    print("=" * 60)

    while True:
        try:
            print("\nВведите параметры (или 'q' для выхода):")

            target_mean = input("Математическое ожидание (например, 15): ")
            if target_mean.lower() == 'q':
                break
            target_mean = float(target_mean)

            target_cv = input("Коэффициент вариации в % (например, 30): ")
            if target_cv.lower() == 'q':
                break
            target_cv = float(target_cv)

            print(f"\nИщем комбинации для μ={target_mean}, CV={target_cv}%")

            # Ищем лучшие варианты
            candidates = find_dice_combination(target_mean, target_cv, max_dice=15, max_sides=20)

            if not candidates:
                print("Не найдено подходящих комбинаций!")
                continue

            print("\nТоп-10 лучших комбинаций:")
            print("-" * 80)
            print(
                f"{'Нотация':<12} {'Среднее':<10} {'CV%':<8} {'Ошибка μ%':<10} {'Ошибка CV':<10} {'Сумм. ошибка':<12}")
            print("-" * 80)

            for i, (notation, mean, cv, mean_err, cv_err, total_err) in enumerate(candidates[:10], 1):
                print(
                    f"{i:2}. {notation:<10} {mean:<10.2f} {cv:<8.2f} {mean_err:<10.2f} {cv_err:<10.2f} {total_err:<12.2f}")

            # Проверяем, есть ли точное совпадение
            best = get_best_dice_combination(target_mean, target_cv, precision=5.0)

            if best:
                notation, actual_mean, actual_cv = best
                print(f"\n✓ Найдено хорошее совпадение: {notation}")
                print(f"  Фактическое среднее: {actual_mean:.2f}")
                print(f"  Фактический CV: {actual_cv:.2f}%")
            else:
                print("\n⚠ Точного совпадения не найдено. Рассмотрите ближайшие варианты выше.")

            # Анализ выбранной комбинации
            while True:
                choice = input("\nВведите номер комбинации для детального анализа (или Enter для продолжения): ")
                if not choice:
                    break

                try:
                    idx = int(choice) - 1
                    if 0 <= idx < len(candidates):
                        notation = candidates[idx][0]
                        analysis = analyze_dice_combo(notation)

                        print(f"\nДетальный анализ: {notation}")
                        print("-" * 40)
                        print(f"Количество кубиков: {analysis['num_dice']}")
                        print(f"Грани кубиков: d{analysis['sides']}")
                        print(f"Модификатор: {analysis['modifier']:+}")
                        print(f"Диапазон: {analysis['min']} - {analysis['max']}")
                        print(f"Всего возможных значений: {analysis['range_size']}")
                        print(f"Математическое ожидание: {analysis['mean']:.2f}")
                        print(f"Стандартное отклонение: {analysis['std']:.2f}")
                        print(f"Коэффициент вариации: {analysis['cv_percent']:.2f}%")

                        # Правило 68-95-99.7
                        print(f"\nОжидаемое распределение (правило 68-95-99.7):")
                        print(
                            f"68% результатов в диапазоне: {analysis['mean'] - analysis['std']:.1f} - {analysis['mean'] + analysis['std']:.1f}")
                        print(
                            f"95% результатов в диапазоне: {analysis['mean'] - 2 * analysis['std']:.1f} - {analysis['mean'] + 2 * analysis['std']:.1f}")
                        print(
                            f"99.7% результатов в диапазоне: {analysis['mean'] - 3 * analysis['std']:.1f} - {analysis['mean'] + 3 * analysis['std']:.1f}")
                    else:
                        print("Некорректный номер!")
                except ValueError:
                    print("Введите число!")

        except ValueError as e:
            print(f"Ошибка ввода: {e}")
        except KeyboardInterrupt:
            print("\nВыход...")
            break


# Примеры использования
if __name__ == "__main__":

    # Пример 1: Ваш пример F(15, 10.575) = 10d2
    print("Пример 1: F(15, 10.575%)")
    target_mean = 15
    target_cv = 10.575  # В процентах!

    # Стандартное отклонение = 15 * 10.575% = 1.58625
    target_std = target_mean * (target_cv / 100)

    print(f"Целевые параметры:")
    print(f"• Математическое ожидание (μ): {target_mean}")
    print(f"• Коэффициент вариации (CV): {target_cv}%")
    print(f"• Стандартное отклонение (σ): {target_std:.3f}")

    candidates = find_dice_combination(target_mean, target_cv)

    print("\nЛучшие комбинации:")
    for i, (notation, mean, cv, mean_err, cv_err, total_err) in enumerate(candidates[:5], 1):
        print(f"{i}. {notation}: μ={mean:.2f}, CV={cv:.2f}% "
              f"(ошибка μ: {mean_err:.1f}%, CV: {cv_err:.1f})")

    # Проверим 10d2
    analysis_10d2 = analyze_dice_combo("10d2")
    print(f"\nАнализ 10d2:")
    print(f"• Среднее: {analysis_10d2['mean']} (ожидалось: {target_mean})")
    print(f"• CV: {analysis_10d2['cv_percent']:.3f}% (ожидалось: {target_cv}%)")
    print(f"• Стандартное отклонение: {analysis_10d2['std']:.3f} (ожидалось: {target_std:.3f})")

    print("\n" + "=" * 60)

    # Пример 2: Типичные D&D параметры
    print("\nПример 2: Поиск для типичных D&D параметров")

    examples = [
        ("Высокий урон, низкая вариативность", 35, 15),  # как 10d6
        ("Средний урон, средняя вариативность", 10.5, 28),  # как 3d6
        ("Атака, высокая вариативность", 10.5, 55),  # как 1d20
        ("Стабильный урон оружия", 7, 35),  # как 2d6
    ]

    for name, mean, cv in examples:
        print(f"\n{name} (μ={mean}, CV={cv}%):")
        best = get_best_dice_combination(mean, cv, precision=2.0)

        if best:
            notation, actual_mean, actual_cv = best
            print(f"  Найдено: {notation}")
            print(f"  Фактические значения: μ={actual_mean:.2f}, CV={actual_cv:.2f}%")
        else:
            print("  Точное совпадение не найдено")

            # Покажем ближайшие варианты
            candidates = find_dice_combination(mean, cv, max_dice=10)
            if candidates:
                notation, actual_mean, actual_cv, _, _, _ = candidates[0]
                print(f"  Ближайший вариант: {notation} (μ={actual_mean:.2f}, CV={actual_cv:.2f}%)")

    print("\n" + "=" * 60)

    # Запуск интерактивного режима
    interactive_finder()