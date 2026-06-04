###############################
###   1. Feladat (8 pont)   ###
###############################

from collections import Counter
from math import gcd
from string import ascii_uppercase

AFFINE_CT = (
    'GAFDSGXMRSPEGXTSDEAPYDGFZKDRVGAXNZDQYZXMPFDSZANJDQZADUFDCB'
    'DPZBDQGFYGXZFQYJFZZDTFZGADGRDQQXGKFSFDYKPDQPKXYPFIRFEGGAFH'
    'QGFQYFYSFRHEHFQGJXYFSQRSPEGXTSDEAPRXJKHQFZRXJENGFSZRHFQRFJ'
    'DGAFJDGHRZDQYHQMXSJDGHXQZFRNSHGPGXYFUFCXESXKNZGZPZGFJZGADG'
    'ESXGFRGESHUDGFRXJJNQHRDGHXQZDQYFQZNSFGAFZFRNSHGPDQYHQGFTSH'
    'GPXMYDGDHQAHTACPZFQZHGHUFDEECHRDGHXQZZNRADZXQCHQFKDQVHQT'
)

def break_affine_cipher(ct: str) -> tuple[int, int]:
    """Meghatározza az affin titkosító (a, b) kulcsát frekvenciaanalízissel."""
    statisztika = Counter(ct)
    leggyakoribbak = statisztika.most_common(2)
    
    c1 = ord(leggyakoribbak[0][0]) - 65
    c2 = ord(leggyakoribbak[1][0]) - 65
    
    for n1, n2 in [(4, 19), (19, 4)]:
        m = (n1 - n2) % 26
        
        if gcd(m, 26) == 1:
            m_inv = 0
            for i in range(26):
                if (m * i) % 26 == 1:
                    m_inv = i
                    break
                    
            a = ((c1 - c2) * m_inv) % 26
            b = (c1 - a * n1) % 26
            
            if gcd(a, 26) == 1:
                return (a, b)
                
    return (0, 0)


def decrypt_affine(ct: str, a: int, b: int) -> str:
    """
    Visszafejti az affin titkosított szöveget:
    D(c) = a^{-1} * (c - b) mod 26
    """
    inv_a = 0
    for i in range(26):
        if (a * i) % 26 == 1:
            inv_a = i
            break
            
    r = []
    
    for k in ct:
        if k in ascii_uppercase:
            y = ord(k) - 65
            x = (inv_a * (y - b)) % 26
            r.append(chr(x + 65))
        else:
            r.append(k)
            
    return "".join(r)


assert break_affine_cipher(AFFINE_CT) == (7, 3)
assert decrypt_affine(AFFINE_CT, 7, 3).startswith('THE')
print(f"1. feladat: OK")



################################
###   2. feladat (15 pont)   ###
################################
class Feistel:
    BLOCK_SIZE = 8

    def __init__(self, key: bytes, iv: bytes) -> None:
        self.key = key
        self.iv = iv

    def _round_fn(self, half: bytes, subkey: int) -> bytes:
        """A `half` minden bájtjához hozzáadja a `subkey` értékét (mod 256)."""
        return bytes((b+subkey) % 256 for b in half)

    def encrypt(self, block: bytes) -> bytes:
        """Titkosítja a `block`-ot négy iteráció alatt."""
        l = block[:4]
        r = block[4:]
        
        for i in range(4):
            result = self._round_fn(r, self.key[i])
            new_r = bytes(a ^ b for a, b in zip(l, result))
            
            l = r
            r = new_r
            
        return l + r

    def decrypt(self, block: bytes) -> bytes:
        """Visszafejti a `block`-ot."""
        l = block[:4]
        r = block[4:]
        
        for i in reversed(range(4)):
            result = self._round_fn(l, self.key[i])
            new_l = bytes(a ^ b for a, b in zip(r, result))
            
            r = l
            l = new_l
            
        return l + r

    def ofb(self, data: bytes) -> bytes:
        """Output Feedback módban alkalmazza az `encrypt(block)` metódust."""
        result = b""
        akt_iv = self.iv
        
        for i in range(0, len(data), self.BLOCK_SIZE):
            d = data[i:i+self.BLOCK_SIZE]
            
            akt_iv = self.encrypt(akt_iv)
            
            tit_d = bytes([a ^ b for a, b in zip(d, akt_iv)])
            result += tit_d
            
        return result

secret_key = b"\x05\x11\x2a\x7f"
public_iv  = b"\x00" * Feistel.BLOCK_SIZE
feistel = Feistel(secret_key, public_iv)
assert feistel.decrypt(feistel.encrypt(b"ABCDEFGH")) == b"ABCDEFGH"
pt = b"Titkos uzenet OFB modban!"
assert feistel.ofb(pt) != pt
assert feistel.ofb(feistel.ofb(pt)) == pt
print(f"2. feladat: OK")


################################
###   3. feladat (7 pont)    ###
################################
import math

def baby_step_giant_step(g: int, h: int, p: int) -> int:
    """Megkeresi x-et, amelyre g^x ≡ h (mod p) (p prím)."""
    m = int(p ** 0.5) + 1
    
    baby_steps = {}
    ertek = 1
    for j in range(m):
        baby_steps[ertek] = j
        ertek = (ertek * g) % p
        
    x = pow(g, p - 1 - m, p)
    
    for i in range(m):
        if h in baby_steps:
            j = baby_steps[h]
            return i * m + j
        
        h = (h * x) % p
        
    return -1



assert baby_step_giant_step(2, pow(2, 17, 29), 29) == 17
assert baby_step_giant_step(5, pow(5, 11, 23), 23) == 11
print(f"3. feladat: OK")

###############################
###   4. feladat (10 pont)  ###
###############################

import hashlib
import os

def hashcash(challenge: bytes, difficulty: int) -> bytes:
    cel = '0' * difficulty
    
    while True:
        nonce = os.urandom(8)
        
        h_result = hashlib.sha256(challenge + nonce).digest()
        
        bin_alak = ""
        for b in h_result:
            bin_alak += format(b, '08b')
        
        if bin_alak[:difficulty] == cel:
            return nonce

challenge = b"kriptografia-zh-2026"
nonce = hashcash(challenge, 16)
h = hashlib.sha256(challenge + nonce).digest()
assert h[0] == 0 and h[1] == 0
print(f"4. feladat: OK")
