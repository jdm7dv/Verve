<%@ Page Language="F#" %>
<html>
<head runat="server">
    <title>Category Listing</title>
</head>
<body>
    <form runat="server">
        <!-- Unordered list of products using ASP.NET Repeater -->
        <ul>
        <asp:Repeater runat="server" id="rptProducts"
                      DataSourceID="nwindProducts">
            <ItemTemplate>
                <li><%# this.Eval("ProductName") %>
                     (price: <%# this.Eval("Price") %>)</li>
            </ItemTemplate>
        </asp:Repeater>
        </ul>
    
        <!-- ASP.NET DataSource control for loading the data -->
        <asp:ObjectDataSource id="nwindProducts" runat="server" 
            TypeName="FSharpWeb.DataSource" SelectMethod="GetProducts">
            <SelectParameters>
                <asp:QueryStringParameter Name="categoryId" Type="Int32" 
                     QueryStringField="id" DefaultValue="0"/>
            </SelectParameters>
        </asp:ObjectDataSource>
    </form>
</body>
</html>
