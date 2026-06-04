class Animal: 
    def __init__(self, name):
        self.name = name
        self._age = 0

    def speak(self):
        print(f"{self.name} makes a sound.")

    def __bool__(self):
        return self._age > 0

    def __lt__(self, other):
        return self.name < other.name

    @staticmethod
    def eat():
        return "Eating food."
    
animal = Animal("cica")
animal.speak()
animal.eat()

print(Animal.eat())
# print(Animal.PI)

kutya = Animal("kutya")
cica = Animal("cica")
print(kutya < cica)