from string import ascii_lowercase
import random

import matplotlib.pyplot as plt

def inf_seq():
    """Végtelen sorozatot a `yield` kulcsszóval tudunk csinálni."""
    n = 0
    while True:
        yield n
        n += 1

seq = inf_seq()
print(seq)
print(next(seq))
print(next(seq))
print(inf_seq())

def lcg(a, b, m, seed):
    """Egy Linear Congruential Generator pszeudomrandomszám generálására"""
    while True:
        seed = (a * seed + b) % m
        yield seed


def plot_2d_spectral_test(seq):
    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111)
    ax.set_xlabel('x')
    ax.set_ylabel('y')
    ax.scatter(seq[:-1], seq[1:], marker='o')
    return fig, ax


def plot_3d_spectral_test(seq, elev, azim):
    fig = plt.figure(figsize=(10, 10))
    ax = fig.add_subplot(111, projection='3d')
    ax.scatter(seq[:-2], seq[1:-1], seq[2:], marker='o')
    ax.set_xlabel('x')
    ax.set_ylabel('y')
    ax.set_zlabel('z')
    ax.view_init(elev=elev, azim=azim)
    return fig, ax


#################################################
#        Törjük fel az LCG generátorokat!       #
#################################################
def gen_key(rng, length):
    k = []
    for _ in range(length // 2):
        i = str(next(rng)).zfill(4)
        k.append(i[:2])
        k.append(i[2:])
    return k


def encode_pt(pt):
    return [str(ascii_lowercase.find(i)).zfill(2) for i in pt]

def encrypt_pt(key, plaintext):
    ct = []
    for k, c in zip(key, plaintext):
        a = (int(k[0]) + int(c[0])) % 10
        b = (int(k[1]) + int(c[1])) % 10
        ct.append(f'{a}{b}')
    return ct

def decrypt_ct(key, ciphertext):
    pt = []
    for k, c in zip(key, ciphertext):
        a = (int(c[0]) - int(k[0])) % 10
        b = (int(c[1]) - int(k[1])) % 10
        pt.append(f'{a}{b}')
    return pt

rng = lcg(4381, 7364, 8397, 2134)
plaintext = encode_pt('lcgrandomnumbergeneratorsaregenerallynotsecure')
secret_key = gen_key(rng, len(plaintext))
ciphertext = encrypt_pt(secret_key, plaintext)
print("Helyes?", encode_pt(plaintext) == decrypt_ct(secret_key, ciphertext))

print(f"PT : {'  '.join(i for i in 'lcgrandomnumbergeneratorsaregenerallynotsecure')}")
print(f"PT : {' '.join(i for i in plaintext)}")
print(f"KEY: {' '.join(i for i in secret_key)}")
print('-' * 145)
print(f"CT : {' '.join(i for i in ciphertext)}")

print("A támadás:")
possible_keys = []
for c, p in zip(ciphertext[28:37], encode_pt('generally')):
    a = (int(c[0]) - int(p[0])) % 10
    b = (int(c[1]) - int(p[1])) % 10
    possible_keys.append(f'{a}{b}')
print(possible_keys)


#####################################################
#         Linear Feedback Shift Register            #
#####################################################
from functools import reduce
from operator import xor

def lfsr(seed: list[int], coeffs: list[int]) -> int:
    state = seed
    while True:
        yield state[-1]
        t = reduce(xor, [i * j for i, j in zip(state, coeffs)])
        state = [t] + state[:-1]

rng = lfsr([0, 1, 0, 1], [0, 1, 0, 1])
print([next(rng) for _ in range(20)])

# LFSR törése:
seq = [0, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 1, 0, 1, 0, 1, 1, 1, 1]

from sage.all import *

def break_coeffs(seq):
    n = 2
    while True:
        A = matrix(GF(2), [seq[i:i+n] for i in range(0, n)])
        V = vector([seq[i] for i in range(n, 2*n)])
        if A.determinant() == 0:
            n += 1
            continue
        sol = A.solve_right(V)
        gen = lfsr(seq[:n], sol)
        if [next(gen) for _ in range(len(seq))] == seq:
            return n, sol
        n += 1

