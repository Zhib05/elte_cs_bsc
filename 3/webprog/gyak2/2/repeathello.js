const output = document.querySelector("output")
const numField = document.querySelector("#num")
const button = document.querySelector("#btn")
button.addEventListener("click", hello)

function hello() {
    for (let i = 0; i < numField.value; i++) {
        const p = document.createElement("p")
        p.textContent = "Hello World!"
        p.style.fontSize = 12 + i + "px"
        output.appendChild(p)
    }
}