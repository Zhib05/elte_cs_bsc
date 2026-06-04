#!/bin/env python3
# pylint: disable=missing-class-docstring,missing-function-docstring
# pylint: disable=consider-using-enumerate

a = ord('a')  # pylint: disable=invalid-name
A = ord('A')  # pylint: disable=invalid-name
z = ord('z')  # pylint: disable=invalid-name
Z = ord('Z')  # pylint: disable=invalid-name


# Monoalfabetikus helyettesítések

class Caesar:
    def __init__(self, key=3):
        self.__key = key

    def encrypt(self, message):
        raise NotImplementedError

    def decrypt(self, ciphertext):
        raise NotImplementedError


try:
    for k in range(26):
        e = Caesar(k)
        assert e.decrypt(e.encrypt('secretmessage')) == 'secretmessage'
    e = Caesar(7)
    assert e.encrypt('S3cr3Tm35S4gE!') == 'Z3jy3At35Z4nL!'
    assert e.decrypt('S3cr3Tm35S4gE!') == 'L3vk3Mf35L4zX!'
    print('Caesar OK')
except NotImplementedError:
    pass


# ------------------------------------------------------

class Vigenere:
    def __init__(self, key):
        self.__key = [ord(c) - ord('a') for c in key.lower() if c.isalpha()]

    def encrypt(self, message):
        raise NotImplementedError

    def decrypt(self, ciphertext):
        raise NotImplementedError


try:
    e = Vigenere('password')
    assert e.encrypt('secret message! REALLY!') == 'heujah ptskscs! GESDHM!'
    assert e.decrypt('heujah ptskscs! GESDHM!') == 'secret message! REALLY!'
    print('Vigenére OK')
except NotImplementedError:
    pass
