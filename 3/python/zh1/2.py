lst = [33,22,33,21,33,44,33,11,11,2,1]
s = {33,22,45,47,42,52}

diff = list(set(lst) - s)
print(diff)
union = list(set(lst) | s)
print(union)
inter = list(set(lst) & s)
print(inter)