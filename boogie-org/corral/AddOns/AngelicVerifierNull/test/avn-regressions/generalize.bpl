//generalize from a given field

const NULL: int;

var a: [int]int;
var b: [int]int;


var g1: int;
var {:scalar} g2: int;
var g3: int;
var {:scalar} g4: int;
var g5: int;


axiom (NULL == 0);

procedure Foo(c:int) returns (d:int) {

   assert (b[g1+1] != NULL);
   assert (b[g1+2] != NULL);

   assert (b[g1] != NULL);
   assert (b[g2] != NULL);
   assert (b[g3] != NULL);
   assert (b[g4] != NULL);

}

procedure {:allocator} malloc(a:int) returns (b:int);
procedure {:allocator "full"} malloc_full(a:int) returns (b:int);
