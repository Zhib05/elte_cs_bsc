const array = [1, -2, 3, 4, -5, 6];
function findNegative(arr) {
    for (let i = 0; i < arr.length; i++) {
        if (arr[i] < 0) {
            return arr[i];
        }
    }
}
console.log(findNegative(array));

const negativeNumber = array.find(num => num < 0);
console.log(`negative number: ${negativeNumber}`);