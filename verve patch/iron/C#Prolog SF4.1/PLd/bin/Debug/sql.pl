:- stacktrace(on).

go1( S) :-
  sql_connect( msaccess, 'SQL\\Northwind.mdb', CI),
  sql_command( CI, "UPDATE Customers SET PostalCode=12208 WHERE CustomerID='ALFKI'"),
  sql_connection( CI, Pr, Co),
  writelnf( "Pr = {0}", [Pr]),
  writelnf( "Co = {0}", [Co]),
  sql_select2( CI, 'SELECT * FROM Customers', S).

go2( S1, S2) :-
  sql_connect( excel, ['SQL\\Names.xls', 'Yes'], CI),
  sql_select( CI, "SELECT * FROM Range1 WHERE Length > 190", S1),
  %sql_disconnect( CI),
  sql_select( CI, "SELECT * FROM Range1", S2).

cd :-
  sql_connect( excel, ['SQL\\Names.xls', 'Yes'], CI),
  sql_disconnect( CI).
  
pc( P, C) :-
  sql_connect( msaccess, 'SQL\\Northwind.mdb', CI),
  sql_connection( CI, P, C).
