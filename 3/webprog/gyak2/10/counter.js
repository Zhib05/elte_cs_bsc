// a) variable declaration
let num = 0;
const text = document.querySelector("#num");
const addBtn = document.querySelector("#add");
const subtractBtn = document.querySelector("#subtract");

// b) variable declaration
const min = 0;
const max = 500;

// c) variable declaration
const delay = 800; // milliseconds after which the counter starts to change
const rate = 100; // milliseconds between counter changes
let delayTimer;
let rateTimer;

// a)
addBtn.addEventListener("click", add);
subtractBtn.addEventListener("click", subtract);

function add() {
    text.value = ++num;
    updateDisabled();
}

function substract() {
    text.value = --num;
    updateDisabled();
}

// b)
updateDisabled();

function updateDisabled() {
    addBtn.disable = (num == max);
    substractBtn.disable = (num == min);

    if ((num == max) || (num == min)) {
        killTimers();
    }
}

// c)
addBtn.addEventListener("mousedown", () => {
    delayTimer = setTimeout(() => {
        rateTimer = setInterval(add, rate);
    }, delay);
});
addBtn.addEventListener("mouseup", killTimers);

subtractBtn.addEventListener("mousedown", () => {
    delayTimer = setTimeout(() => {
        rateTimer = setInterval(subtract, rate);
    }, delay);
});
subtractBtn.addEventListener("mouseup", killTimers);

function killTimers() {
    clearTimeout(delayTimer);
    clearInterval(rateTimer);
}