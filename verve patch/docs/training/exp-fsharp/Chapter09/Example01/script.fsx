#light

let inp = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>
           <Scene>
              <Composite>
                <Circle radius='2' x='1' y='0'/>
                <Composite>
                  <Circle radius='2' x='4' y='0'/>
                  <Square side='2' left='-3' top='0'/>
                </Composite>
                <Ellipse top='2' left='-2' width='3' height='4'/>
              </Composite>
           </Scene>"

// ----------------------------
// F# Interactive

open System.Xml

let doc = new XmlDocument()

doc.LoadXml(inp)

doc.ChildNodes

fsi.AddPrinter(fun (x:XmlNode) -> x.OuterXml)

doc.ChildNodes
doc.ChildNodes.Item(1)
doc.ChildNodes.Item(1).ChildNodes.Item(0)
doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0)
doc.ChildNodes.Item(1).ChildNodes.Item(0).ChildNodes.Item(0).Attributes
