#Dokumentációk segítségnek: https://docs.python.org/3/library/heapq.html
#                           https://docs.python.org/3/library/sys.html
#                           https://docs.python.org/3/library/os.html
#                           https://docs.python.org/3/library/random.html

import heapq
import sys
import os
import random

#1 Adott az alábbi listánk:
ex1 = [3, 3, 2, 1, 4, 3, 2, 1, 2, 2, 2, 1, 2, 3, 1, 3, 2, 2, 1, 1, 1, 1, 0, 0, 4, 2, 3]
# Írjuk ki a 2 leggyakrabban előforduló elemet!

print("1: ")

occurrences = {}
for num in ex1:
    if num in occurrences:
        occurrences[num] += 1
    else:
        occurrences[num] = 1
print(occurrences)
sorted_occ = sorted(occurrences.items(), key=lambda x: x[1], reverse=True)
print([key for key, _ in sorted_occ[:2]])

#2 A heapq modul definiálja nekünk a minimum prioritásos sort.
# Az alábbi listából tároljuk el az értékeket minimum prioritásos sorban,
# majd írassuk ki őket növekvő sorrendben!

print("2: ")

ex2 = [81, 49, 21, 421, 4, -4, 42, 77, 0]

heapq.heapify(ex2)  #helyben fogja átalakítani!
while ex2:          #ameddig nem üres
    print(heapq.heappop(ex2), end = " ")


#6 Írjuk meg a saját map, filter függvényünket!
# Írjuk meg úgy a saját all, any függvényünket, hogy várjanak egy predikátumot is (feltételt), 
# amit alkalmaznak az elemekre!

print("\n6: ")

nums = [1,2,3,4,5,6]

def mymap(func, iterable):
    for x in iterable:
        yield func(x)

print(list(map(lambda x : x**2, nums)))
print(list(mymap(lambda x : x**2, nums)))

def myfilter(func, iterable):
    for x in iterable:
        if func(x):
            yield x

print(list(filter(lambda x : x % 2 == 0, nums)))
print(list(myfilter(lambda x : x % 2 == 0, nums)))

def myall(predicate, iterable):
    for x in iterable:
        if not predicate(x):
            return False
    return True

bools = [True, False, 0,1]

print(all(bools))
print(myall(lambda x : x is True,bools))

def myany(predicate, iterable):
    for x in iterable:
        if predicate(x):
            return True
        
print(any(bools))
print(myany(lambda x : x is False, bools))

#7 Írjunk egy függvényt, ami véletlenszerűen kiválaszt egy elemet egy tetszőleges méretű listából!
# Használd a random modult!

print("\n7: ")

def get_rand_item(iterable):
    return random.choice(iterable)

lst = [1,2,3,4,5,6,7,8]
print(get_rand_item(lst))