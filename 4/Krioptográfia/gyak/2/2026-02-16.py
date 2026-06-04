def mymod(a, b, m):
    ...


def oszt(a: int, m: int) -> int:
    return a % m

print(oszt(-9, 10))
print(oszt(1, 10))

def oszt_dict(l:list[int], m: int) -> dict[int, list[int]]:
    # d = {}
    # for i in l:
    #     k = oszt(i, m)
    #     if k not in d:
    #         d[k] = []
    #     d[k].append(i)
    # return d
    return {k: [i for i in l if oszt(i, m) == k] for k in range(m)}

print(oszt_dict([1, 6, 8, 15], 5))

