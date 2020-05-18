#light

let http(url: string) =
    let req = System.Net.WebRequest.Create(url)
    use resp = req.GetResponse()
    use stream = resp.GetResponseStream()
    use reader = new System.IO.StreamReader(stream)
    let html = reader.ReadToEnd()
    html

