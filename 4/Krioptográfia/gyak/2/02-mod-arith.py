from sage.all import *

# -----------------------------------------------------------------------
# Find the class representative of x modulo m. If sym is True then the representative should be the one with the smallest absolute value.
def find_class_representative(x:int, m:int, sym=False) -> int:
    raise NotImplementedError

try:
    assert find_class_representative(1,5) == 1
    assert find_class_representative(1,5, sym=True) == 1
    assert find_class_representative(14,5) == 4
    assert find_class_representative(14,5, sym=True) == -1
    assert find_class_representative(-1,5) == 4
    assert find_class_representative(-1,5, sym=True) == -1
    assert find_class_representative(-6,5) == 4
    assert find_class_representative(-6,5, sym=True) == -1
    assert find_class_representative(-10,5) == 0
    assert find_class_representative(-10,5, sym=True) == 0
    print("find_class_representative works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Create a program that classify elements according to their representative (smallest non-negative).
def classify_elements(input_set:set[int]|list[int], m:int) -> dict[int, set[int]]:
    raise NotImplementedError

try:
    assert classify_elements([0, 1, 2, 3, 4, 5, 6, 7, 8], 2) == {0: {0, 2, 4, 6, 8}, 1: {1, 3, 5, 7}}
    assert classify_elements([0, 1, 2, 3, 4, 5, 6, 7, 8], 3) == {0: {0, 3, 6}, 1: {1, 4, 7}, 2: {2, 5, 8}}
    assert classify_elements([0, 1, 2, 3, 4, 5, 6, 7, 8], 4) == {0: {0, 4, 8}, 1: {1, 5}, 2: {2, 6}, 3: {3, 7}}
    assert classify_elements([0, 1, 2, 3, 4, 5, 6, 7, 8], 5) == {0: {0, 5}, 1: {1, 6}, 2: {2, 7}, 3: {3, 8}, 4: {4}}
    assert classify_elements([1, 7, 9, 10], 2) == {1: {1, 7, 9}, 0: {10}}
    assert classify_elements([1, 7, 9, 10], 3) == {1: {1, 7, 10}, 0: {9}}
    assert classify_elements([1, 7, 9, 10], 4) == {1: {1, 9}, 3: {7}, 2: {10}}
    assert classify_elements([1, 7, 9, 10], 5) == {1: {1}, 2: {7}, 4: {9}, 0: {10}}
    assert classify_elements([-4, 6, -10, 12, -123], 2) == {0: {-4, 6, -10, 12}, 1: {-123}}
    assert classify_elements([-4, 6, -10, 12, -123], 3) == {2: {-4, -10}, 0: {6, 12, -123}}
    assert classify_elements([-4, 6, -10, 12, -123], 4) == {0: {-4, 12}, 2: {6, -10}, 1: {-123}}
    assert classify_elements([-4, 6, -10, 12, -123], 5) == {1: {-4, 6}, 0: {-10}, 2: {12, -123}}
    print("classify_elements works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Write a program that calculates the modular inverse of a modulo m!
# If there is no such element then return with None.
def imod(a:int ,m:int) -> int|None:
    raise NotImplementedError

try:
    for m in range(2,10):
        for a in range(m):
            solution = None
            try:
                solution = inverse_mod(a,m)
            except ZeroDivisionError:
                pass
            my_solution = imod(a,m)
            assert imod(a,m) == solution, f'{a} {m} -> {my_solution=} {solution=}'
    print("inverse_mod works")
except NotImplementedError:
    pass


# -----------------------------------------------------------------------
# Write a program that calculates all bth root of a modulo m!
def mod_root(a:int, m:int, b:int = 2) -> set[int]:
    raise NotImplementedError

try:
    assert mod_root(a=1, m=7, b=2) == {1, 6}
    assert mod_root(a=3, m=7, b=2) == set()
    assert mod_root(a=0, m=11, b=2) == {0}
    assert mod_root(a=1, m=8, b=2) == {1, 3, 5, 7}
    assert mod_root(a=1, m=7, b=3) == {1, 2, 4}
    print("root works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Create a program that calculates all the powers of a modulo m!
def mod_powers(a:int, m:int) -> set[int]:
    raise NotImplementedError

try:
    assert mod_powers(2, 7) == {1, 2, 4}
    assert mod_powers(3, 7) == {1, 2, 3, 4, 5, 6}
    assert mod_powers(1, 13) == {1}
    assert mod_powers(0, 11) == {0}
    assert mod_powers(2, 8) == {0, 2, 4}
    assert mod_powers(2, 6) == {2, 4}
    print("mod_powers works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Write a program that calculates the multiplicative order of all elements modulo prime p.
def mul_orders(p:int) -> dict[int,int]:
    raise NotImplementedError

try:
    assert mul_orders(7) == {1: 1, 2: 3, 3: 6, 4: 3, 5: 6, 6: 2}
    assert mul_orders(2) == {1: 1}
    for o in mul_orders(11).values():
        assert 10 % o == 0
    for o in divisors(12):
        assert o in mul_orders(13).values()
    print("mul_orders works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Write a program that calculates classify the elements based on their multiplicative order modulo p.
def mul_ord_classification(p:int) -> dict[int,set[int]]:
    raise NotImplementedError

try:
    assert mul_ord_classification(7) == {1: {1}, 2: {6}, 3: {2, 4}, 6: {3, 5}}
    assert mul_ord_classification(2) == {1: {1}}
    for order in mul_ord_classification(11).keys():
        assert 10 % order == 0
    elements = set()
    for s in mul_ord_classification(13).values():
        elements |= s
    assert elements == set(range(1, 13))
    classes = mul_ord_classification(11)
    assert 10 in classes
    assert len(classes[10]) == 4  # φ(10) = 4
    print("mul_ord_classification works")
except NotImplementedError:
    pass

# -----------------------------------------------------------------------
# Write a solver for discrete logarithm using baby-step giant step!
def dlog_bf(a:int, b:int, m:int) -> int|None:
    raise NotImplementedError

try:
    assert dlog_bf(2, 8, 11) == 3
    assert dlog_bf(3, 1, 7) == 0
    assert dlog_bf(3, 5, 7) == 5
    assert dlog_bf(2, 3, 7) is None
    assert dlog_bf(5, 4, 13) is None
    assert dlog_bf(5, 8, 13) == 3
    assert dlog_bf(4, 1, 7) == 0
    print("dlog_bf works")
except NotImplementedError:
    pass


# -----------------------------------------------------------------------
# Write a solver for discrete logarithm using baby-step giant step!
def dlog_bsgs(a:int, b:int, m:int) -> int|None:
    raise NotImplementedError

try:
    assert dlog_bsgs(2, 8, 11) == 3
    assert dlog_bsgs(3, 1, 7) == 0
    assert dlog_bsgs(3, 5, 7) == 5
    assert dlog_bsgs(2, 3, 7) is None
    assert dlog_bsgs(5, 4, 13) is None
    assert dlog_bsgs(5, 8, 13) == 3
    assert dlog_bsgs(4, 1, 7) == 0
    print("dlog_bsgs works")
except NotImplementedError:
    pass