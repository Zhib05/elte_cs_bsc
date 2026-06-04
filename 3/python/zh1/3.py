def osszeg_nagyobb_mint_ot(*szamok):
    return sum(s for s in szamok if s > 5)

osszeg_nagyobb_mint_ot_lambda = lambda *szamok: sum(s for s in szamok if s > 5)

print(osszeg_nagyobb_mint_ot(1, 2, 3, 4, 5, 6))
print(osszeg_nagyobb_mint_ot_lambda(1, 2, 3, 4, 5, 6))