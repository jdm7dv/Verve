function SelCode(i){
	var numberOfTabs = 9;
	if (document.getElementById){
		for (f=1;f<=numberOfTabs;f++){
			document.getElementById('Code'+f).style.display='none';
			document.getElementById('link'+f).className ="backtab";
		}
		document.getElementById('Code'+i).style.display='block';
		document.getElementById('link'+i).className="tab";
	}
}


