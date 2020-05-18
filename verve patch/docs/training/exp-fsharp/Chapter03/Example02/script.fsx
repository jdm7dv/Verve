#light

"MAGIC"B;;

let dir = @"c:\Program Files";;

let s = "All the kings horses
and all the kings men";;

let s = "Couldn't put Humpty";;

s.Length;;

s.[13];;

let s = "Couldn't put Humpty";;

s.[13] <- 'h';; /// Error!

"Couldn't put Humpty" + " " + "together again";;

let buf = new System.Text.StringBuilder();;

buf.Append("Humpty Dumpty");;

buf.Append(" sat on the wall");;

buf.ToString();;

