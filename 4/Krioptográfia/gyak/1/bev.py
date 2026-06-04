for i in [1, 2, 3, 4, 5]:
    print(i)
print("-"*30)

for i in "Hello World\x01":
    print(i)
print("-"*30)

for i in range(10):
    print(i)
print("*"*30)

d = {"a" : 1, "b" : {}, 1 : "1"}

l = ["1", 1, 0b01]

print(d.items())
for k, v in d.items():
    print(k, v)
print(d[1])
d["New key"] = "New value"
print(d.keys())
print(d.values())

print("-"*30)
l.append("New element")
l += ["a", "b", "c"]
print(l)
print(l[6], l[-1])
print(l[2:5], l[:9], l[3:], l[::4])
print(l[::-1])
print("-"*30)

#   list comprehension
l2 = [i**2 for i in range(10)]
l3 = [i**3 for i in range(10) if i % 3 == 0]
print(l2)
print(l3)
print("-"*30)

# dictionary comprehension
d2 = {k: 2**k for k in range(10)}
d3 = {k: 2**k for k in range(10) if k % 3 == 0}
print(d2)
print(d3)
print("*"*30)

#   STRING
print("hello", 'world')
s = """
Ez
tobb sorban 
is lehet
"""

print(str(98765))
print(str(["-", "1", 1]))

list()
dict()
set([k for k in range(10)])

print("*+-"*30)
l4 = [3] * 5
l6 = [None] * 10
l4[3] = 9999
print(l4)
l6 = [_*3**_ for _ in range(5)]
l6[3] = 8765
print(l6)
print (f"{l6 = }")
print(f"{137:016b}")
print(f"{137:04x}")

if 4 % 2 == 0:
    print("4 is even")
else:
    print("4 is odd")

if 1:
    print("1")
else:
    print("---")

print(bool(0), bool(1), bool(-1), bool([]), bool([0]), bool(""), bool("0"), bool(None), bool({}))
print(1 and 0)
print(1 or 0)