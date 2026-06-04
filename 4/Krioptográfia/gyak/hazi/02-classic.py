# Írjuk meg a Vigenére rejtjelezés módosított változatát a következők szerint:
#    1. Bontsuk a titkosítandó szöveget a kulcsnak megfelelő hosszúságú szakaszokra.
#    2. Egészítsük ki az utolsó szakaszt 'padding' karakterekkel úgy, hogy annak hossza megegyezik a kulcs hosszával.
#    3. Minden szakaszt titkosítsunk a Vigenére rejtjelezésnek megfelelően, de a kulcs
#        a. az első szakasz esetén az adott kulcs ('key').
#        b. a második szakasztól kezdve pedig az előző titkosított szakasz újra az eredeti kulccsal titkosítva.
#
#    Pl abc=['a','b','c', 'd', 'e'], padding='a', plaintext='edcba', key='a' esetén a titkosított szöveg: 'eceaa'
#                   a a a a
#                   + + + +
#                 a e c e a
#                 + + + + +
#                 e d c b a
#                 - - - - -
#                 e c e a a

class VigenereChain:
    def __init__(self, abc:list[str], padding:str="a"):
        self._abc = abc
        self._padding = padding

    def encrypt(self, key, plaintext) -> str:
        if not plaintext:
            return ""

        L = len(key)
        remainder = len(plaintext) % L
        if remainder > 0:
            plaintext += self._padding * (L - remainder)

        result = ""
        orig_key = key
        curr_key = key
        n = len(self._abc)
        for i in range(0, len(plaintext), L):
            block = plaintext[i : i + L]
            
            ct_block = ""
            for j in range(L):
                p_idx = self._abc.index(block[j])
                k_idx = self._abc.index(curr_key[j])
                ct_block += self._abc[(p_idx + k_idx) % n]
            
            result += ct_block

            next_key = ""
            for j in range(L):
                ct_idx = self._abc.index(ct_block[j])
                ok_idx = self._abc.index(orig_key[j])
                next_key += self._abc[(ct_idx + ok_idx) % n]
            
            curr_key = next_key

        return result
    
    def decrypt(self, key, ciphertext) -> str:
        if not ciphertext:
            return ""

        L = len(key)
        plaintext = ""
        orig_key = key
        curr_key = key
        n = len(self._abc)

        for i in range(0, len(ciphertext), L):
            block = ciphertext[i : i + L]
            
            pt_block = ""
            for j in range(L):
                c_idx = self._abc.index(block[j])
                k_idx = self._abc.index(curr_key[j])
                pt_block += self._abc[(c_idx - k_idx) % n]
            
            plaintext += pt_block

            next_key = ""
            for j in range(L):
                ct_idx = self._abc.index(block[j])
                ok_idx = self._abc.index(orig_key[j])
                next_key += self._abc[(ct_idx + ok_idx) % n]
            
            curr_key = next_key

        return plaintext

if __name__ == "__main__":
    vc = VigenereChain(["a", "b", "c", "d", "e"])
    assert vc.encrypt("a", "edcba") == "eceaa"
    assert vc.decrypt("a", "eceaa") == "edcba"
    assert vc.encrypt("ece", "abcd") == "edbbaa"
    assert vc.decrypt("ece", "edbbaa") == "abcdaa"

    cipher = VigenereChain(list("abcdefghijklmnopqrstuvwxyz"))
    key = "key"
    plaintext = "attackatdawn"
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    assert decrypted == plaintext

    key = "key"
    plaintext = "hello"  # not a multiple of key length (3)
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    # padded with 'a' by default
    expected = plaintext.ljust(len(ciphertext), "a")
    assert decrypted == expected

    key = "ab"
    plaintext = "ab"
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    assert decrypted == plaintext

    key = "abc"
    plaintext = "aaaaaaaaa"
    ciphertext = cipher.encrypt(key, plaintext)
    # chaining means repeated blocks should differ
    blocks = [ciphertext[i:i + 3] for i in range(0, len(ciphertext), 3)]
    assert len(set(blocks)) > 1

    key = "d"
    plaintext = "helloworld"
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    assert decrypted == plaintext

    key = "key"
    plaintext = ""
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    assert ciphertext == ""
    assert decrypted == ""

    abc = list("0123456789")
    cipher = VigenereChain(abc, padding="0")
    key = "12"
    plaintext = "98765"
    ciphertext = cipher.encrypt(key, plaintext)
    decrypted = cipher.decrypt(key, ciphertext)
    expected = plaintext.ljust(len(ciphertext), "0")
    assert decrypted == expected

    print('OK!')


# DO NOT DELETE
# these function helping the automatic testing on TMS (without them the tms automatic tests will fail)
def vigenere_chain_encrypt(abc, padding, key, plaintext):
    c = VigenereChain(abc, padding)
    return c.encrypt(key, plaintext)

def vigenere_chain_decrypt(abc, padding, key, ciphertext):
    c = VigenereChain(abc, padding)
    return c.decrypt(key, ciphertext)