const array = [3, 5, 12, -8, 4, 1, 7, 6];

function countEvenNumbers(arr) {
  let count = 0;
  for (let c of arr) {
    if (c % 2 === 0) {
      count++;
    }
  }
  return count;
}
console.log(countEvenNumbers(array));   // 4

// megszámolás tömbfüggvénnyel
console.log(`even numbers: ${array.reduce((a, b) => a + (b % 2 === 0 ? 1 : 0), 0)}`);  // 4