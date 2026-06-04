from functools import reduce

min_lambda = lambda lst: reduce(lambda x, y: x if x < y else y, lst)

numbers = [34, 12, 5, 67, 23, 1, 89]
print(min_lambda(numbers))