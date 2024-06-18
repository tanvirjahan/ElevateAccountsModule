function RemoveAll(ListDropDown)
{
	if (ListDropDown == null)
		return;
	ListDropDown.selectedIndex = -1;
	var iListBoxLength = ListDropDown.options.length;
	for (var i = 0; i < iListBoxLength; i++)
		ListDropDown.options.remove(0);
}

function FillDropDownList(arr,ListBox,objdoc)
{
    var str1;
    var str2;
    var ary1;
    var ary2;
    var sel ;
    sel=0
    ary1=arr.split("|");

    for (var i = 0; i < ary1.length; i++)
	    {
		    var objOption = objdoc.createElement("option");
		    str2=ary1[i];
		    sel=i;
		    ary2=str2.split("@");
	        objOption.text = ary2[0];
	        objOption.value = ary2[1];
		    ListBox.add(objOption);
	    }
	    
	   //ListBox.Value = "[Select]";
//	    if (sel == null)
//    		ListBox.selectedIndex= 0;
//		else
//	    	ListBox.selectedIndex= sel;
	return;	
}

//function FillDropDownListN(arr,ListBox,objdoc)
//{
//    var str1;
//    var str2;
//    var ary1;
//    var ary2;
//    ary1=arr.split("|");

//    for (var i = 0; i < ary1.length; i++)
//	    {
//		    var objOption = objdoc.createElement("option");
//		    str2=ary1[i];
//		    ary2=str2.split("@");
//	        objOption.text = ary2[0];
//	        objOption.value = ary2[1];
//		    ListBox.add(objOption);
//	    }
//	return;	
//}

