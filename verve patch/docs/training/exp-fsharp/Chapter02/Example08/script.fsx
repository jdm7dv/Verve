#light

open System.Windows.Forms;;

let form = new Form(Visible=true,TopMost=true,Text="Welcome to F#");;

let textB = new RichTextBox(Dock=DockStyle.Fill, Text="Here is some initial text");;
form.Controls.Add(textB);;

open System;;
open System.IO;;
open System.Net;;

let req = WebRequest.Create("http://www.microsoft.com");;

let resp = req.GetResponse();;

let stream = resp.GetResponseStream();;

let reader = new StreamReader(stream);;

let html = reader.ReadToEnd();;

textB.Text <- html;;
