const textField = document.querySelcetor("#text");
const button = document.querySelector("#btn");
const img = document.querySelector("img");
button.addEventListener("click", loadImg);
function loadImg() {
    img.src = textField.value;
}