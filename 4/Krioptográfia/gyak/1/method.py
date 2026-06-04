SUM = tuple[int, int, int, int]
def kripto(a : int, b : int, c : int) -> SUM:
    return (a + b + c, a, b, c)

kripto(a=1, b=2, c=3)

def kripto2(*args):
    for arg in args:
        print(arg)

print(kripto2(*[i for i in range(10)], 1, 2, 7, 3, 6))