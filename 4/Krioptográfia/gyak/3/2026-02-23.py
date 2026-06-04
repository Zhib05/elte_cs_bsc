from string import ascii_lowercase, ascii_uppercase
import random

# ELTOLÁSOS TITKOSÍTÁS

def shift_cipher_gen() -> int:
    return random.randint(0, 25)


def shift_cipher_enc(key: int, pt: str) -> str:
    ct=""
    for c in pt:
        ct += ascii_uppercase[(ascii_lowercase.find(c) + key) % 26]
    return ct

def shift_cipher_dec(key: int, ct: str) -> str:
    pt=""
    for c in ct:
        pt += ascii_lowercase[(ascii_uppercase.find(c) - key) % 26]
    return pt

# k = shift_cipher_gen()
# print(shift_cipher_dec(k, ct=shift_cipher_enc(k, pt="helloworld")) == "helloworld")

##############################################################################################################

# HELYETTESÍTÉSES TITKOSÍTÁS

def subst_gen() -> dict[str, str]:
    perm = random.sample(ascii_lowercase, k=26)
    return {k : v for k, v in zip(ascii_lowercase, perm)}

# print(subst_gen())

def subst_enc(key: dict[str, str], pt: str) -> str:
    return ''.join(key[c] for c in pt)


def subst_dec(key: dict[str, str], ct: str) -> str:
    reverse_key = {v: k for k, v in key.items()}
    return subst_enc(reverse_key, ct)

# k = subst_gen()
# print(subst_dec(k, subst_enc(k, pt="helloworld")) == "helloworld")


# forrás: http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html
freq = {'a': .0812, 'b': .0149, 'c': .0271, 'd': .0432, 'e': .1202, 'f': .0230, 'g': .0203,
        'h': .0592, 'i': .0731, 'j': .0010, 'k': .0069, 'l': .0398, 'm': .0261, 'n': .0695,
        'o': .0768, 'p': .0182, 'q': .0011, 'r': .0602, 's': .0628, 't': .0910, 'u': .0288,
        'v': .0111, 'w': .0209, 'x': .0017, 'y': .0211, 'z': .0007}

subst_ct = 'LOJUMGEPJSESNEPCEPYQLJQLOYBJQDYLJMEMDIPD' \
           'CGYSNGSGLDDLCNLLLOJVPDYVJNEJSKGUJMJMLPYN' \
           'YPLOJLOJTVQXTDVDNJTJDGYNKIUSCPTPJDMYIOLU' \
           'SVJNLLSKDLDPPYXQOYGMNJQLOJPPESDMTJZOSVDV' \
           'PJDGGJGCPLJWMSENGYQJWMJGGDNGYNSVDNTGDWWJ' \
           'JGGJGMOYVDYSNDLVSGLLWMSWSDMLSLXTUOYGDNJE' \
           'UYLEOOSCIOGYLYSOJCNYEOOJOMSVDNSKDVCLGOYG' \
           'NGSKJNYLYDDQDMMEJM'


##############################################################################################################

# VIGENERE-FÉLE TITKOSÍTÁS
def repeat_to_length(s: str, length: int) -> str:
    return (s * length)[:length]

print(repeat_to_length(s="kripto", length=8))

def vigenere_gen(n: int) -> str:
    return ''.join(random.choices(ascii_lowercase, n))


def vigenere_enc(key: str, pt: str) -> str:
    ct=""
    # for k, c in zip(key, pt):
    #     ct += ascii_uppercase[(ascii_lowercase.find(c) + ascii_lowercase.find(k)) % 26]
    for i, c in enumerate(pt):
        ct += ascii_uppercase[(ascii_lowercase.find(c) + ascii_lowercase.find(key[i])) % 26]
    return ct


def vigenere_dec(key: str, ct: str) -> str:
    pt=""
    # for k, c in zip(key, ct):
    #     pt += ascii_lowercase[(ascii_uppercase.find(c) - ascii_lowercase.find(k)) % 26]
    for i, c in enumerate(ct):
        pt += ascii_lowercase[(ascii_uppercase.find(c) - ascii_lowercase.find(key[i])) % 26]
    return pt

# pt = "hellotesztszoveghello"
# k = repeat_to_length(s="kripto", length=len(pt))
# print(vigenere_dec(k, vigenere_enc(k, pt)) == pt)

##############################################################################################################

# VIGENERE FELTÖRÉSE KASISKI MÓDSZERÉVEL
vigenere_ct = 'CHREEVOAHMAERATBIAXXWTNXBEEOPHBSBQMQEQERBW' \
              'RVXUOAKXAOSXXWEAHBWGJMMQMNKGRFVGXWTRZXWIAK' \
              'LXFPSKAUTEMNDCMGTSXMXBTUIADNGMGPSRELXNJELX' \
              'VRVPRTULHDNQWTWDTYGBPHXTFALJHASVBFXNGLLCHR' \
              'ZBWELEKMSJIKNBHWRJGNMGJSGLXFEYPHAGNRBIEQJT' \
              'AMRVLCRREMNDGLXRRIMGNSNRWCHRQHAEYEVTAQEBBI' \
              'PEEWEVKAKOEWADREMXMTBHHCHRTKDNVRZCHRCLQOHP' \
              'WQAIIWXNRMGWOIIFKEE'


kasiski = []
import re
from math import gcd

# matches = re.finditer("CHR", vigenere_ct, re.MULTILINE)
# for match in matches:
#     print(match.start())
#     kasiski.append(match.start())
# print(gcd(*kasiski))

from math import comb
from collections import Counter

def index_of_coincidence(text: str) -> float:
    return sum(comb(fi, 2) for fi in Counter(text).values()) / comb(len(text), 2)

# print(index_of_coincidence(vigenere_ct))
# print(index_of_coincidence(''.join(random.choices(ascii_uppercase, k=len(vigenere_ct)))))

# k = 5
# c_k = [vigenere_ct[i::k] for i in range(k)]
# print(c_k)

# k = 1
# while True:
#     c_k = [index_of_coincidence(vigenere_ct[i::k]) for i in range(k)]
#     if all(.055 <= c <= .075 for c in c_k):
#         break
#     k += 1

# print("A kulcs hossza valoszinuleg:", k)