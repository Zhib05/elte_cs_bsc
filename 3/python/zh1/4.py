string_hosszabb_mint_ot = lambda lista: [s for s in lista if len(s) > 5]

print(string_hosszabb_mint_ot(["alma", "banan", "korte", "szilva", "narancs"]))