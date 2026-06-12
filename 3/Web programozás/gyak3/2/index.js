const px = document.querySelector("body > p:nth-of-type(1) > span")
const time = document.querySelector("body > p:nth-of-type(2) > span")

let prevX = 0;
let prevY = 0;
let distSum = 0;
let clicks = 0;

let prevT = 0;
let tSum = 0;
document.addEventListener("click", onClick);

function onClick(e) {
    clicks++;
    calcAvgDist(e);
    calcAvgTime(e);
}

function calcAvgDist(e) {
    console.log(`e.screenX - prevX: ${e.screenX} - ${prevX}, e.screenY - prevY: ${e.screenY} - ${prevY}`);

    distSum += Math.sqrt(Math.pow(e.screenX - prevX, 2) + Math.pow(e.screenY - prevY, 2));
    px.innerText = distSum / clicks;
    prevX = e.screenX;
    prevY = e.screenY;
}

function calcAvgTime(e) {
    if (prevT == 0) {
        prevT == Date.now();
    }
    tSum += Date.now() - prevT;
    time.innerText = tSum / clicks / 1000;
    prevT = Date.now();
}