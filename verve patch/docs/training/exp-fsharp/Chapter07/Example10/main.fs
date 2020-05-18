open System.Windows.Forms

let form = new Form(Width=400, Height=300, Visible=true, Text="F# Forms Sample")

#if COMPILED
// Run the main code
do System.Windows.Forms.Application.Run(form)
#endif
