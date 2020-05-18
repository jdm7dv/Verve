let (|MulThree|_|) inp = if inp % 3 = 0 then Some(inp/3) else None
let (|MulSeven|_|) inp = if inp % 7 = 0 then Some(inp/7) else None

let (|MulN|_|) n inp = if inp % n = 0 then Some(inp/n) else None
