import random
import re
import matplotlib.pyplot as plt
import numpy as np
from collections import Counter
from typing import Tuple, List
import argparse

class DiceSimulator:
    """Класс для симуляции бросков кубиков в нотации D&D"""

    def __init__(self, notation: str):
        """
        Инициализация с нотацией кубиков.

        Args:
            notation: Строка в формате 'NdM[+/-K]', например '3d6', '2d10+5', '1d20-2'
        """
        self.notation = notation
        self.num_dice, self.dice_sides, self.modifier = self._parse_notation(notation)

    def _parse_notation(self, notation: str) -> Tuple[int, int, int]:
        """Парсит нотацию кубиков."""
        pattern = r'^(\d+)d(\d+)([+-]\d+)?$'
        match = re.match(pattern, notation)

        if not match:
            raise ValueError(f"Некорректная нотация: {notation}. Используйте формат 'NdM[+/-K]'")

        num_dice = int(match.group(1))
        dice_sides = int(match.group(2))
        modifier = int(match.group(3)) if match.group(3) else 0

        if num_dice <= 0:
            raise ValueError(f"Количество кубиков должно быть положительным: {num_dice}")
        if dice_sides <= 0:
            raise ValueError(f"Количество граней должно быть положительным: {dice_sides}")

        return num_dice, dice_sides, modifier

    def roll_once(self) -> int:
        """Совершает один бросок."""
        total = sum(random.randint(1, self.dice_sides) for _ in range(self.num_dice))
        return total + self.modifier

    def simulate(self, num_simulations: int = 100000) -> List[int]:
        """Выполняет заданное количество симуляций."""
        return [self.roll_once() for _ in range(num_simulations)]

    def calculate_statistics(self, results: List[int]) -> dict:
        """Рассчитывает статистику результатов."""
        results_array = np.array(results)

        return {
            'min': np.min(results_array),
            'max': np.max(results_array),
            'mean': np.mean(results_array),
            'median': np.median(results_array),
            'std': np.std(results_array),
            'variance': np.var(results_array),
            'mode': Counter(results).most_common(1)[0][0],
            'expected_value': self.calculate_expected_value(),
            'total_simulations': len(results)
        }

    def calculate_expected_value(self) -> float:
        """Рассчитывает математическое ожидание."""
        # Матожидание для одного кубика: (1 + dice_sides) / 2
        single_dice_expected = (1 + self.dice_sides) / 2
        return self.num_dice * single_dice_expected + self.modifier

    def plot_results(self, results: List[int], save_path: str = None):
        """Строит графики результатов."""
        stats = self.calculate_statistics(results)

        # Создаем фигуру с несколькими subplots
        fig = plt.figure(figsize=(15, 10))
        fig.suptitle(f'Симуляция бросков: {self.notation}\n'
                    f'{stats["total_simulations"]:,} симуляций',
                    fontsize=16, fontweight='bold')

        # 1. Гистограмма распределения
        ax1 = plt.subplot(2, 2, 1)
        min_result = stats['min']
        max_result = stats['max']
        bins = range(min_result, max_result + 2)

        n, bins, patches = ax1.hist(results, bins=bins, alpha=0.7,
                                   color='skyblue', edgecolor='black',
                                   density=True, rwidth=0.8)

        # Добавляем теоретическое распределение (для кубиков без модификатора)
        if self.modifier == 0 and self.num_dice <= 10:
            self._add_theoretical_distribution(ax1, min_result, max_result)

        ax1.set_xlabel('Результат броска')
        ax1.set_ylabel('Вероятность')
        ax1.set_title(f'Распределение результатов\n'
                     f'Мин: {min_result}, Макс: {max_result}')
        ax1.grid(True, alpha=0.3)

        # 2. Box plot
        ax2 = plt.subplot(2, 2, 2)
        ax2.boxplot(results, vert=True, patch_artist=True,
                   boxprops=dict(facecolor='lightgreen', alpha=0.7))
        ax2.set_title('Box Plot распределения')
        ax2.set_ylabel('Результат броска')
        ax2.grid(True, alpha=0.3)

        # 3. Круговая диаграмма самых частых результатов
        ax3 = plt.subplot(2, 2, 3)
        result_counts = Counter(results)
        top_n = min(10, len(result_counts))
        top_results = result_counts.most_common(top_n)

        labels = [f'{val}' for val, count in top_results]
        sizes = [count for val, count in top_results]
        colors = plt.cm.Set3(np.linspace(0, 1, top_n))

        ax3.pie(sizes, labels=labels, colors=colors, autopct='%1.1f%%',
               startangle=90)
        ax3.set_title(f'Топ-{top_n} наиболее частых результатов')
        ax3.axis('equal')

        # 4. Статистическая информация
        ax4 = plt.subplot(2, 2, 4)
        ax4.axis('off')

        stats_text = (
            f'Статистика для {self.notation}:\n\n'
            f'• Количество симуляций: {stats["total_simulations"]:,}\n'
            f'• Минимальный результат: {stats["min"]}\n'
            f'• Максимальный результат: {stats["max"]}\n'
            f'• Среднее значение: {stats["mean"]:.3f}\n'
            f'• Медиана: {stats["median"]:.1f}\n'
            f'• Стандартное отклонение: {stats["std"]:.3f}\n'
            f'• Стандартное отклонение: {stats["std"]*100/stats["expected_value"]:.3f} %\n'
            f'• Наиболее частый результат: {stats["mode"]}\n'
            f'• Математическое ожидание: {stats["expected_value"]:.3f}\n\n'
            f'Теоретический диапазон:\n'
            f'• Минимум: {self.num_dice * 1 + self.modifier}\n'
            f'• Максимум: {self.num_dice * self.dice_sides + self.modifier}\n'
            f'• Всего возможных значений: '
            f'{self.num_dice * self.dice_sides - self.num_dice + 1}'
        )

        ax4.text(0.1, 0.95, stats_text, fontsize=11,
                verticalalignment='top',
                bbox=dict(boxstyle='round', facecolor='wheat', alpha=0.5))

        plt.tight_layout()

        if save_path:
            plt.savefig(save_path, dpi=150, bbox_inches='tight')
            print(f"График сохранен как: {save_path}")

        plt.show()

        return stats

    def _add_theoretical_distribution(self, ax, min_val: int, max_val: int):
        """Добавляет теоретическое распределение (для маленького числа кубиков)."""
        if self.num_dice == 1:
            # Для одного кубика - равномерное распределение
            x = range(min_val, max_val + 1)
            y = [1/self.dice_sides] * len(x)
            ax.plot(x, y, 'r--', linewidth=2, label='Теоретическое')
            ax.legend()

    def run_simulation(self, num_simulations: int = 100000,
                      save_path: str = None) -> dict:
        """Запускает полную симуляцию и возвращает статистику."""
        print(f"\n{'='*60}")
        print(f"Начинаем симуляцию: {self.notation}")
        print(f"Количество симуляций: {num_simulations:,}")
        print(f"{'='*60}")

        # Выполняем симуляции
        results = self.simulate(num_simulations)

        # Рассчитываем статистику
        stats = self.calculate_statistics(results)

        # Выводим базовую статистику
        print(f"\nРезультаты для {self.notation}:")
        print(f"• Минимальный результат: {stats['min']}")
        print(f"• Максимум: {stats['max']}")
        print(f"• Среднее: {stats['mean']:.3f}")
        print(f"• Медиана: {stats['median']:.1f}")
        print(f"• Стандартное отклонение: {stats['std']:.3f}")
        print(f"• Наиболее частый результат: {stats['mode']}")
        print(f"• Математическое ожидание: {stats['expected_value']:.3f}")

        # Строим графики
        stats = self.plot_results(results, save_path)

        return stats


def plot_multiple_dice(notations: List[str], num_simulations: int = 100000):
    """Сравнивает несколько типов кубиков на одном графике."""
    plt.figure(figsize=(14, 8))

    colors = plt.cm.tab10(np.linspace(0, 1, len(notations)))

    for i, notation in enumerate(notations):
        try:
            simulator = DiceSimulator(notation)
            results = simulator.simulate(num_simulations)

            # Нормализованная гистограмма
            counts = Counter(results)
            x = sorted(counts.keys())
            y = [counts[val] / num_simulations for val in x]

            plt.plot(x, y, 'o-', color=colors[i], linewidth=2,
                    markersize=4, label=f'{notation}', alpha=0.7)

            # Добавляем математическое ожидание
            expected = simulator.calculate_expected_value()
            plt.axvline(x=expected, color=colors[i], linestyle='--',
                       alpha=0.5, linewidth=1)

        except ValueError as e:
            print(f"Ошибка для {notation}: {e}")

    plt.xlabel('Результат броска')
    plt.ylabel('Вероятность')
    plt.title(f'Сравнение распределений для разных кубиков\n'
              f'({num_simulations:,} симуляций каждый)')
    plt.grid(True, alpha=0.3)
    plt.legend()
    plt.tight_layout()
    plt.show()


def main():
    """Основная функция для запуска из командной строки."""
    parser = argparse.ArgumentParser(
        description='Симулятор бросков кубиков D&D',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Примеры использования:
  python dice_simulator.py 3d6
  python dice_simulator.py 2d10+5 --simulations 50000
  python dice_simulator.py 1d20 --save dice_plot.png
  python dice_simulator.py compare 1d6 2d6 3d6
        """
    )

    subparsers = parser.add_subparsers(dest='command', help='Команда')

    # Парсер для одиночной симуляции
    single_parser = subparsers.add_parser('single', help='Симуляция одного типа кубиков')
    single_parser.add_argument('notation', type=str, help='Нотация кубиков (например, 3d6)')
    single_parser.add_argument('-s', '--simulations', type=int, default=100000,
                              help='Количество симуляций (по умолчанию: 100000)')
    single_parser.add_argument('--save', type=str, help='Путь для сохранения графика')

    # Парсер для сравнения
    compare_parser = subparsers.add_parser('compare', help='Сравнение нескольких типов кубиков')
    compare_parser.add_argument('notations', type=str, nargs='+',
                               help='Нотации кубиков для сравнения')
    compare_parser.add_argument('-s', '--simulations', type=int, default=100000,
                               help='Количество симуляций (по умолчанию: 100000)')

    args = parser.parse_args()

    if args.command == 'single':
        try:
            simulator = DiceSimulator(args.notation)
            simulator.run_simulation(args.simulations, args.save)
        except ValueError as e:
            print(f"Ошибка: {e}")

    elif args.command == 'compare':
        plot_multiple_dice(args.notations, args.simulations)

    else:
        # Интерактивный режим
        print("Симулятор бросков кубиков D&D")
        print("=" * 40)

        while True:
            notation = input("\nВведите нотацию кубиков (например, '3d6') или 'q' для выхода: ")

            if notation.lower() == 'q':
                break

            try:
                num_simulations = int(input("Количество симуляций (по умолчанию 100000): ") or "100000")

                save_option = input("Сохранить график? (y/n): ").lower()
                save_path = None
                if save_option == 'y':
                    save_path = input("Имя файла (например, dice_plot.png): ") or "dice_plot.png"

                simulator = DiceSimulator(notation)
                simulator.run_simulation(num_simulations, save_path)

            except ValueError as e:
                print(f"Ошибка: {e}")
            except KeyboardInterrupt:
                print("\nВыход...")
                break

