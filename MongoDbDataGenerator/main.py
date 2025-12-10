from DataGenerator.DataGenerator import Generate
from Dice.DiceFinder import find_dice_combination
from Dice.DiceSimulator import DiceSimulator

mode = 3
if mode == 1:
    simulator = DiceSimulator("10d199")
    stats = simulator.run_simulation(100000)

if mode == 2:
    candidates = find_dice_combination(15, 5, 20)
    for i in candidates:
        print(i)

if mode == 3:
    Generate()
