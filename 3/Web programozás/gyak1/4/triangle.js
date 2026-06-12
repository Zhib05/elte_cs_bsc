const a = parseInt(prompt("Adja meg az a oldal hosszát:"));
const b = parseInt(prompt("Adja meg a b oldal hosszát:"));
const c = parseInt(prompt("Adja meg a c oldal hosszát:"));

if (a + b > c && a + c > b && b + c > a) {
    console.log("can make a triangle");
} else {
    console.log("cannot make a triangle");
}