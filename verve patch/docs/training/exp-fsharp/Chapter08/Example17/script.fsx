#light

open System.Windows.Forms

let form = new Form(Text="Click Form",Visible=true,TopMost=true)

form.Click.Add(fun evArgs -> printfn "Clicked!")

form.MouseMove.Add(fun args -> printfn "Mouse, (X,Y) = (%A,%A)" args.X args.Y)

