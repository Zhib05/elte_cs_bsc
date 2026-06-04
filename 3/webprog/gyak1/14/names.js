const array = [" Kovács János ", " Nagy István ", " Kis Lajos ", " Tóth Péter "];

function findContains(a, b) {
    const contains = []
    // foreach ciklus
    for (c of a) {
        if (c.includes(b)) {
            contains.push(c)
        }
    }
    return contains;
}

function findStartsWith(a, b) {
    const startsWith = []
    // Rendes ciklus
    for (let i = 0; i < a.length; i++) {
        if (a[i].startsWith(b)) {
            startsWith.push(a[i])
        }
    }

    return startsWith;
}

console.log("elements that contain \"er\"");
console.log(findContains(array, "er"));
// console.log(array.filter(e => e.includes("er")));

console.log("elements that start with \" K\"");
console.log(findStartsWith(array, " K"));
// console.log(array.filter(e => e.startsWith(" K")));