<%@ Language="VBScript" %>
<HTML>
<HEAD>

<!- Create the help desk object and obtain ->
<!- the current topic or title. ->
<%
   Dim strTopic
   Dim strTitle
   Dim strContents
   Dim DataQuery
   Set DataQuery=Server.CreateObject("HelpAccess.HelpAccess")

   'Determine if we're looking for a title or a topic number.
   'Use the right component method for the type of call.
   If Len(Request.QueryString("Title")) > 0 Then
      strTitle = Request.QueryString("Title")
      DataQuery.GetTitle strTopic, strTitle, strContents
   Else
      strTopic = Request.QueryString("Topic")
      ' Make sure the topic string is set correctly.
      if Len(strTopic) = 0 then
         strTopic = "00000"
      End If
      DataQuery.GetTopic strTopic, strTitle, strContents
   End If
%>

<!- Display the current topic title in the titlebar. ->
<TITLE>
   The current topic is: <% Response.Write strTitle %>
</TITLE>

</HEAD>
<BODY ID="ThisPage">

<!-Create a form to display our pushbuttons.->
<FORM NAME="MyForm">

<!- Create the appropriate headings. ->
<H1 ALIGN=center>Welcome to the Help Desk</H1>
<H2 ALIGN=center><B><% Response.Write strTitle %></B></H2>

<!- Display the content. ->
<%
    Response.Write strContents
%>

<!- Provide a place to search by topic. ->
<P><B>Search by Topic:</B><BR>
<INPUT TYPE=text NAME="SearchValue" SIZE=40><P>

<!- Allow the user to search for the topic. ->
<INPUT TYPE=button
       VALUE="Search"
       ONCLICK="window.location.href = 'HelpDesk.asp?Title=' + 
               MyForm.SearchValue.value">

<!- Make it easy for the user to get back to the main page. ->
<%
    If Not RTrim(strTopic) = "00000" Then
        Response.Write "<P>Go Back to the "
        Response.Write "<A HREF=HelpDesk.ASP>Main Page</A>"
    End If
%>

</FORM>
</BODY>
</HTML>