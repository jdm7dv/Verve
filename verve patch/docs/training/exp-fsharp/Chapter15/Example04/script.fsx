open System.Xml

let xml1() =
    let doc = new XmlDocument()
    let rootNode = doc.CreateElement "contacts"
    doc.AppendChild rootNode |> ignore
    let contactNode = doc.CreateElement "contact"
    let nameNode = doc.CreateElement "name"
    let nameText = doc.CreateTextNode "John Smith"
    let phoneNode = doc.CreateElement "phone"
    phoneNode.SetAttribute("type", "home")
    let phoneText = doc.CreateTextNode "+1 626-123-4321"
    nameNode.AppendChild nameText |> ignore
    contactNode.AppendChild nameNode |> ignore
    contactNode.AppendChild phoneNode |> ignore
    phoneNode.AppendChild phoneText |> ignore
    rootNode.AppendChild contactNode |> ignore

let xml2() =
    let doc = new XmlDocument()
    let writer = doc.CreateNavigator().AppendChild()
    writer.WriteStartElement "contacts"
    writer.WriteStartElement "contact"
    writer.WriteElementString ("name", "John Smith")
    writer.WriteStartElement "phone"
    writer.WriteAttributeString ("type", "home")
    writer.WriteString "+1 626-123-4321"
    writer.WriteEndElement()
    writer.Close()

