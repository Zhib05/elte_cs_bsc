const output = document.quearySelector('output');
const numField = document.quearySelector('#num');
const button = document.quearySelector('#btn');
button.addEventListener('click', circleArea);

function circleArea() {
    const r = numField.value;
    output.innerHTML = `Circle area: ${Math.PI * r * r}`;
}