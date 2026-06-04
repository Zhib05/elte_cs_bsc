const p = document.querySelector("p");
p.addEventListener("click", onClick);

function onClick(e) {
    // a)
    console.log(this)
    // b)
    console.log(e.type)
    // c)
    console.log(e.button)
    // d)
    console.log(`${e.screenX}, ${e.screenY}`)
    // e)
    console.log(e.target)
    // f)
    if (e.target.matches("p span")) {
        console.log(e.target.innerText);
    }
    // g)
    if (e.targett.matches("p a")) {
        if (e.target.innerText == "libero") {
            e.preventDefault();
        }
    }
}