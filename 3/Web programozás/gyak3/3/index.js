const ul = document.querySelector("ul");
ul.addEventListener("click", onClick);

let x = false;
let prev;

function onClick(e) {
    if (e.target.matches("ul > li")) {
        if (!x) {
            prev = e.target;
            x = true;
        }
        else {
            let temp = prev.innerText;
            prev.innerText = e.target.innerText;
            e.target.innerText = temp;
            x = false;
        }
    }
}