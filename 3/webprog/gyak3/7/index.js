document.querySelector("tbody").addEventListener("click", onClick);

let x = true;
function onClick(e) {
    if (e.target.innerText == "") {
        e.target.innerText = x ? "X" : "O";
        x = !x;
    }
}