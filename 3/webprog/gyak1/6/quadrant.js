function quadrant(x, y) {
    if (x == 0 || y == 0)
        return "none";
    if (x > 0)
        return y > 0 ? 1 : 4;
    else
        return y > 0 ? 2 : 3;
}

console.log(`(1, 1) lies in: quadrant ${quadrant(1, 1)}`); // 1
console.log(`(-1, 1) lies in: quadrant ${quadrant(-1, 1)}`); // 2
console.log(`(-1, -1) lies in: quadrant ${quadrant(-1, -1)}`); // 3
console.log(`(1, -1) lies in: quadrant ${quadrant(1, -1)}`); // 4
console.log(`(0, 1) lies in: quadrant ${quadrant(0, 1)}`); // none