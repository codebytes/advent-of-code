# https://adventofcode.com/2024/day/1

list1 = []
list2 = []

with open("../input.txt", "r") as file:
    for line in file:
        num1, num2 = map(int, line.split())
        list1.append(num1)
        list2.append(num2)

list1.sort()
list2.sort()

result = sum(abs(a - b) for a, b in zip(list1, list2))
print(f"Part 1: {result}")

result2 = sum(item * list2.count(item) for item in list1)
print(f"Part 2: {result2}")
