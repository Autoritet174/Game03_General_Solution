from DataGenerator.DataGenerator import Generate
from Dice.DiceFinder import find_dice_combination
from Dice.DiceSimulator import DiceSimulator

mode = 1
if mode == 1:
    simulator = DiceSimulator("5d21")
    stats = simulator.run_simulation(100000)

if mode == 2:
    candidates = find_dice_combination((int)(50), 5)
    for i in candidates:
        print(i)

if mode == 3:
    Generate()
