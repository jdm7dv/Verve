#light

let powerOfFourPlusTwo n =
    let n = n * n
    let n = n * n
    let n = n + 2
    n;;

let powerOfFourPlusTwo n =
    let n1 = n * n
    let n2 = n1 * n1
    let n3 = n2 + 2
    n3;;

let powerOfFourPlusTwoTimesSix n =
    let n3 =
        let n1 = n * n
        let n2 = n1 * n1
        n2 + 2
    let n4 = n3 * 6
    n4;;

let invalidFunction n =
    let n3 =
        let n1 = n + n
        let n2 = n1 * n1
        n1 * n2
    let n4 = n1 + n2 + n3 // Error! n3 is in scope, but n1 and n2 are not!
    n4;;
