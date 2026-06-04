from sage.all import *




# --------------------------------------------------------------------------
# Count the characters of a text
def char_count(s: str) -> dict:
    raise NotImplementedError

try:
    assert char_count("abc") == {"a": 1, "b": 1, "c": 1}
    assert char_count("aabbc") == {"a": 2, "b": 2, "c": 1}
    assert char_count("") == {}
    s = "a a!?"
    assert char_count(s) == { "a": 2, " ": 1, "!": 1, "?": 1, }
    assert char_count("Aa") == {"A": 1, "a": 1}
    s = "árvíztűrő"
    assert char_count(s) == { "á": 1, "r": 1, "v": 1, "í": 1, "z": 1, "t": 1, "ű": 1, "r": 1, "ő": 1, }
    s = "1122$$"
    assert char_count(s) == { "1": 2, "2": 2, "$": 2, }
    s = "hello world"
    assert char_count(s)["l"] == 3
    assert char_count(s)["o"] == 2
except NotImplementedError:
    pass

# --------------------------------------------------------------------------
# Write a program that calculates the kth power of a number
def my_pow(a:int|float, k:int) -> int|float:
    raise NotImplementedError

try:
    for a,k in zip(range(10), range(2, 10)):
        assert my_pow(a, k) == a**k
except NotImplementedError:
    pass
# --------------------------------------------------------------------------
# Write a program that calculates the greatest common divisor of two numbers (+ binary)
def my_gcd(a: int, b: int) -> int:
    raise NotImplementedError

try:
    for i,j in zip(range(1,20), range(1,20)):
        assert my_gcd(i, j) == gcd(i,j)
except NotImplementedError:
    pass
# --------------------------------------------------------------------------
# Create a generator for the Fibonacci numbers (use yield)
def fib_nums():
    yield 0
    raise NotImplementedError

try:
    F = fib_nums()
    A = [next(F) for _ in range(10)]
    assert len(A) == 10
    assert A == [fibonacci(i) for i in range(len(A))]
except NotImplementedError:
    pass

# --------------------------------------------------------------------------
# Create a class that encode and decodes a string with Huffman-coding
class Huffman:
    def __init__(self, base = 2):
        self.__base = base
    def __repr__(self):
        return f'Huffman encoder (base ={self.__base})'
    def encode(self, data: str) -> tuple[str,DiGraph]:
        pass
    def decode(self, data: str, tree: DiGraph) -> str:
        pass

