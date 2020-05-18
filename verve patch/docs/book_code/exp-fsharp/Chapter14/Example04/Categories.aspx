<%@ Page Language="F#" %>
<html>
<head runat="server">
    <title>Categories</title>
</head>
<body>
    <form runat="server">
        <!-- Unordered list of products using ASP.NET Repeater -->
        <ul>
        <asp:Repeater runat="server" id="rptCategories"
                      DataSourceID="nwindCategories">
            <ItemTemplate>
                <li><a href="Category.aspx?id=<%# this.Eval("CategoryID") %>">
                     Category: <%# this.Eval("Name") %></a></li>
            </ItemTemplate>
        </asp:Repeater>
        </ul>
    
        <!-- ASP.NET DataSource control for loading the data -->
        <asp:ObjectDataSource id="nwindCategories" runat="server" 
            TypeName="FSharpWeb.DataSource" SelectMethod="GetCategories" />
    </form>
</body>
</html>
