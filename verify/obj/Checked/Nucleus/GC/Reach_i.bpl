#line 12
type Time;
var $Time:Time;





function reached(a:int,t:Time)
  returns(bool)
;
#line 21
procedure newTime();
modifies $Time;

procedure reachStackRoot($s:int,$frame:int,$j:int,$t:Time);
requires (($FrameSlices)[($s)])[(($FrameAddrs)[($s)])[($frame)]+4*($j)]==($frame);
requires frameHasPtr((($FrameLayouts)[($s)])[($frame)],($j));
ensures reached(((($FrameAbss)[($s)])[($frame)])[($j)],($t));

procedure reachStaticRoot($s:int,$j:int,$t:Time);
requires sectionHasPtr(($s),($j));
ensures reached((($SectionAbs)[($s)])[($j)],($t));


procedure reach($a:int,$j:int,$t:Time);
requires reached(($a),($t));
requires (($AbsMem)[($a)])[($j)]!=NO_ABS;
ensures reached((($AbsMem)[($a)])[($j)],($t));
