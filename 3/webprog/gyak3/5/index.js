document.addEventListener("click", onClick);

function onClick(e) {
    if (e.target.matches("a")) {
        if (!e.target.href.includes("elte")) {
            e.preventDefault();
        }
    }
}