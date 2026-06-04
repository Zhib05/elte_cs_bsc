class Flower:
    def __init__(self, name : str, color : str, beauty : int):
        if beauty < 1 or beauty > 10: 
            raise ValueError("Beauty must be between 1 and 10")
        self.name = name
        self.color = color
        self.beauty = beauty

    def beauty_multiplier(self) -> float:
        return 1.0

    def beauty_score(self) -> float:
        return self.beauty * self.beauty_multiplier()

    def __lt__(self, other):
        if not isinstance(other, Flower):
            return NotImplemented
        return self.beauty_score() < other.beauty_score()

    def __eq__(self, other):
        if not isinstance(other, Flower):
            return False
        return self.beauty_score() == other.beauty_score()

class Rose(Flower):
    def beauty_multiplier(self) -> float:
        return 2.0

    def __str__(self):
        return f"{self.color} rose named {self.name} with beauty {self.beauty_score()}"

class Tulip(Flower):
    def beauty_multiplier(self) -> float:
        return 1.5

    def __str__(self):
        return f"{self.color} tulip named {self.name} with beauty {self.beauty_score()}"

class Daisy(Flower):
    def beauty_multiplier(self) -> float:
        return 1.0

    def __str__(self):
        return f"{self.color} daisy named {self.name} with beauty {self.beauty_score()}"

class Bouquet:
    total_bouquets = 0

    def __init__(self):
        self.flowers: List[Flower] = []
        Bouquet.total_bouquets += 1

    def add_flowers(self, *flowers: Flower):
        self.flowers.extend(flowers)

    def __str__(self) -> str:
        if not self.flowers:
            return "Empty bouquet"
        sorted_flowers = sorted(self.flowers)
        lines = [str(flower) for flower in sorted_flowers]
        return "Bouquet flowers:\n" + "\n".join(lines)


r1 = Rose("Rose1", "red", 8)
r2 = Rose("Rose2", "white", 6)
t1 = Tulip("Tulip1", "yellow", 7)
d1 = Daisy("Daisy1", "white", 5)
d2 = Daisy("Daisy2", "pink", 9)

bouquet1 = Bouquet()
bouquet1.add_flowers(r1, t1, d2)
bouquet2 = Bouquet()
bouquet2.add_flowers(r2, d1)

print(bouquet1)
print("\n")
print(bouquet2)

print(f"\nr1 > t1: {r1 > t1}")
print(f"\nt1 > d2: {t1 > d2}")

print(f"\nLétrehozott csokrok száma: {Bouquet.total_bouquets}")