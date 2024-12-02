with open("../input.txt") as f:
    reports = [list(map(int, line.split())) for line in f]

def is_safe(levels):
    increasing = levels[1] > levels[0]
    return all(
        1 <= abs(diff) <= 3 and (diff >= 0 if increasing else diff <= 0)
        for diff in (levels[i] - levels[i - 1] for i in range(1, len(levels)))
    )

def is_safe_with_dampener(levels):
    return any(
        is_safe(levels[:i] + levels[i+1:])
        for i in range(len(levels))
    )

# Part 1: Count safe reports without the Problem Dampener
safe_reports_part1 = sum(is_safe(levels) for levels in reports)
print(f"Part 1: {safe_reports_part1}")

# Part 2: Count safe reports with the Problem Dampener

safe_reports_part2 = sum(
    is_safe(levels) or is_safe_with_dampener(levels)
    for levels in reports
)
print(f"Part 2: {safe_reports_part2}")
