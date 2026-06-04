import math
import os

# 1.
def classify_by_multiplicative_order(p:int) -> dict[int, set[int]]:
    groups = {}
    for a in range(1, p):
        # Csak a mod p redukált maradékrendszer elemeit vizsgáljuk
        if math.gcd(a, p) == 1:
            order = 1
            val = a % p
            while val != 1:
                val = (val * a) % p
                order += 1
            
            if order not in groups:
                groups[order] = set()
            groups[order].add(a)
            
    return groups

# 2.
class ZigZagCipher:
    def __init__(self, key:bytes) -> None:
        self.key = key
        # A 4x4-es mátrix zig-zag bejárásának indexei (0-15)
        self.zz = [0, 1, 4, 8, 5, 2, 3, 6, 9, 12, 13, 10, 7, 11, 14, 15]
    def __F(self, block:bytes) -> bytes:
        # 1. & 2. Zig-zag átrendezés
        zz_block = bytes([block[self.zz[i]] for i in range(16)])
        # 3. Kulcs hozzáadása (XOR)
        return bytes([b ^ k for b, k in zip(zz_block, self.key)])
    def __F_inv(self, block: bytes) -> bytes:
        """Segédfüggvény a visszafejtéshez: visszavonja az __F műveleteit."""
        # 1. Kulcs levétele
        unxored = bytes([b ^ k for b, k in zip(block, self.key)])
        # 2. Zig-zag visszaállítása eredeti sorrendbe
        orig = bytearray(16)
        for i in range(16):
            orig[self.zz[i]] = unxored[i]
        return bytes(orig)

    def _pad(self, data: bytes) -> bytes:
        """PKCS#7 padding"""
        pad_len = 16 - (len(data) % 16)
        return data + bytes([pad_len] * pad_len)

    def _unpad(self, data: bytes) -> bytes:
        """PKCS#7 unpadding"""
        return data[:-data[-1]]

    def encrypt(self, message: bytes) -> bytes:
        padded = self._pad(message)
        iv = os.urandom(16)
        ciphertext = bytearray(iv) # Az IV-t a ciphertext elejére fűzzük
        prev = iv
        
        for i in range(0, len(padded), 16):
            block = padded[i:i+16]
            # CBC mód: XOR az előző titkosított blokkal (vagy az IV-vel)
            xored = bytes([b ^ p for b, p in zip(block, prev)])
            enc_block = self.__F(xored)
            ciphertext.extend(enc_block)
            prev = enc_block
            
        return bytes(ciphertext)

    def decrypt(self, message: bytes) -> bytes:
        iv = message[:16]
        ciphertext = message[16:]
        plaintext = bytearray()
        prev = iv
        
        for i in range(0, len(ciphertext), 16):
            block = ciphertext[i:i+16]
            dec_block = self.__F_inv(block)
            # CBC mód: XOR az előző titkosított blokkal
            plaintext.extend(bytes([b ^ p for b, p in zip(dec_block, prev)]))
            prev = block
            
        return self._unpad(bytes(plaintext))

# 3.
cipher_text = 'exlsnkbh gpxfsb.ntpxrsgt.gspysgksykAmrs.snkbhArasb.gtrb.gpn.AshdkTArbsb ygsTrshdkfd.bbrcsokdsgtpysg.yvusitpysbr.xysgt.gsgtrsnkbhAraskhrd.gpkxskosykAmpxfsgt.gshdkTArbsb ygsTrsdrhA.nrcsTls.snkbTpx.gpkxskosgtrsT.ypnskhrd.gpkxyskosgtrsb.ntpxru'
characters = [' ', 't', 'o', 'e', 'a', 'm', 'i', 'h', 's', 'n', 'c', 'p', 'l', 'r', 'b', 'f', 'u', 'g', 'y', 'v', 'x', 'd', '.', 'A', 'k', 'T']

def solve_substitution() -> str:
    freq = {}
    first_app = {}
    
    # Karakterek gyakoriságának és első felbukkanásának kigyűjtése
    for i, c in enumerate(cipher_text):
        freq[c] = freq.get(c, 0) + 1
        if c not in first_app:
            first_app[c] = i
            
    # Rendezés: 1. Gyakoriság (csökkenő), 2. Első felbukkanás (növekvő tie-breaker)
    sorted_cipher_chars = sorted(freq.keys(), key=lambda c: (-freq[c], first_app[c]))
    
    # Szótár felépítése az eltoláshoz
    mapping = {c: p for c, p in zip(sorted_cipher_chars, characters)}
    
    # Eredeti szöveg visszafejtése
    return "".join(mapping[c] for c in cipher_text)

# 4.

from hashlib import sha1
def collision_interval(n:int, k:int, d:int) -> int:
    counts = {}
    x = n
    while True:
        # A számot stringként, majd byte-ként kódoljuk a hash-hez
        h = sha1(str(x).encode('utf-8')).hexdigest()[:k]
        counts[h] = counts.get(h, 0) + 1
        
        # Amint megvan a kívánt ütközésszám, visszaadjuk az m (aktuális x) értékét
        if counts[h] == d:
            return x
        x += 1

# Opcionális ellenőrző futtatás a 3. feladathoz:
if __name__ == "__main__":
    print("3. Feladat megfejtése:")
    print(solve_substitution())


