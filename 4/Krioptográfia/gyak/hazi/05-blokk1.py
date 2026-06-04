import random
import secrets


def xor_strings(a: bytes, b: bytes) -> bytes:
    """Két bytestring-et XOR-oz össze."""
    return b"".join((i ^ j).to_bytes(1) for i, j in zip(a, b))


class BlockCipher:
    """
    Egy blokktitkosító Cipher Feedback (CFB) módban.

    A blokkok, a kulcs és az inicializációs vektor (iv) mérete 8 bájt hosszú.

    Az inicializációs vektor (iv) opcionális: ha nem adjuk meg (None), akkor az osztály
    generáljon egy 8 bájt hosszú véletlen bytestring-et.
    """
    BLOCK_SIZE: int = 8

    def __init__(self, key: bytes, iv: bytes | None = None) -> None:
        self.key = key
        if iv is None:
            self.iv = secrets.token_bytes(self.BLOCK_SIZE)
        else:
            self.iv = iv

    def block_cipher(self, v: bytes) -> bytes:
        """
        A paraméterül kapott `v`, blokkméret hosszú bytestring-et
        bitenként megfordítja, majd hozzáadja a kulcsot modulo 2.
        Tehát az eredményt úgy kapjuk, hogy a megfordított `v` bytestring
        és kulcs között XOR műveletet végzünk.
        """
        bin_str = "".join(f"{b:08b}" for b in v)

        reversed_bin_str = bin_str[::-1]

        reversed_bytes_list = [int(reversed_bin_str[i:i+8], 2) for i in range(0, 64, 8)]
        reversed_v = bytes(reversed_bytes_list)

        return bytes([b1 ^ b2 for b1, b2 in zip(reversed_v, self.key)])

    def encrypt(self, plaintext: bytes) -> bytes:
        """
        CFB módban titkosító algoritmus.
        Az inicializációs vektor **nem** része a ciphertext-nek.
        """
        ciphertext = b""
        feedback = self.iv
        
        for i in range(0, len(plaintext), self.BLOCK_SIZE):
            block = plaintext[i:i+self.BLOCK_SIZE]
            
            stream_block = self.block_cipher(feedback)
            
            c_block = bytes([p ^ s for p, s in zip(block, stream_block)])
            ciphertext += c_block

            feedback = c_block
            
        return ciphertext

    def decrypt(self, ciphertext: bytes) -> bytes:
        """
        CFB módban visszafejtő algoritmus.
        Az inicializációs vektor **nem** része a ciphertext-nek.
        """
        plaintext = b""
        feedback = self.iv
        
        for i in range(0, len(ciphertext), self.BLOCK_SIZE):
            block = ciphertext[i:i+self.BLOCK_SIZE]
            
            stream_block = self.block_cipher(feedback)

            p_block = bytes([c ^ s for c, s in zip(block, stream_block)])
            plaintext += p_block

            feedback = block
            
        return plaintext

    def __repr__(self) -> str:
        return f"BlockCipher(key={self.key}, iv={self.iv})"


if __name__ == "__main__":
    cipher = BlockCipher(bytes.fromhex("0123456789abcdef"))
    pt = b""
    assert cipher.encrypt(pt) == b""

    cipher = BlockCipher(bytes.fromhex("0123456789abcdef"), bytes.fromhex("abcdef0123456789"))
    pt = b""
    assert cipher.encrypt(pt) == b""

    pt = bytes.fromhex("0123456789abcdef" * 2)
    assert len(cipher.encrypt(pt)) == 16

    pt = b"\x00\x00\x00\x00\x00\x00\x00\x00"
    assert cipher.encrypt(pt) == b'\x90\xc5\xe7\xa3\t\\~:'

    pt = b"kutyahaz"
    assert cipher.encrypt(pt) == b'\xfb\xb0\x93\xdah4\x1f@'

    for _ in range(50):
        cipher = BlockCipher(secrets.token_bytes(8))
        pt = secrets.token_bytes(cipher.BLOCK_SIZE * random.randint(0, 10))
        ct = cipher.encrypt(pt)
        assert cipher.decrypt(ct) == pt
    for _ in range(50):
        cipher = BlockCipher(secrets.token_bytes(8), secrets.token_bytes(8))
        pt = secrets.token_bytes(cipher.BLOCK_SIZE * random.randint(0, 10))
        ct = cipher.encrypt(pt)
        assert cipher.decrypt(ct) == pt
    print("OK")


# DO NOT DELETE
# these function helping the automatic testing on TMS (without them the tms automatic tests will fail)
def block_encrypt(plaintext: bytes, key: bytes, iv: bytes | None = None) -> bytes:
    c = BlockCipher(key, iv)
    return c.encrypt(plaintext)


def block_decrypt(ciphertext: bytes, key: bytes, iv: bytes | None = None) -> bytes:
    c = BlockCipher(key, iv)
    return c.decrypt(ciphertext)
