
function dateformatfunction(textbox,errmsg) 
{
		var day
		var month
		var year
		str =textbox.value
		if (str.length != 0)
		{
			if (str.length ==6 )
			{
				day=str.substring(0,2)
				month=str.substring(2,4)
				year=str.substring(4,6)
				year=20+year
				str= day+ "/" + month + "/" + year 
				if (validateDate(str)==true)textbox.value=str
				else {
					alert(errmsg)
					textbox.value="";
					textbox.focus();
					}
			}
			else if (str.length ==8 )
			{
					if(validateDate(str)==false)
					{
					day=str.substring(0,2)
					month=str.substring(2,4)
					year=str.substring(4,8)
					str= day+ "/" + month + "/" + year
					if (validateDate(str)==true )
					{
						textbox.value=str
						textbox.focus();
					}
					else{
						alert(errmsg)
						textbox.value="";
						textbox.focus();
						}
						
					}
			}
			else if(str.length==10)
			{
			 if (validateDate(str)==false)
				{
				alert(errmsg)
				textbox.value="";
				textbox.focus();
				}
			 else if(validateDateFormat(str)==false)
				{
				alert(errmsg)
				textbox.value="";
				textbox.focus();
				}
			}
			else
			{
			alert(errmsg)
			textbox.value="";
			textbox.focus();
			}
		}
	}
function validateDateFormat(fld) 
{

	if (fld.length==10)
	{
	var RegExPattern ='(0[1-9]|[12][0-9]|3[01])[- /.](0[1-9]|1[012])[- /.](19|20)[0-9]{2}';
	if ((fld.match(RegExPattern)) && (fld!=''))
	{return true;}
	else
	{return false;}
	}
}

// Date Validation Javascript

function stripBlanks(fld) {var result = "";for (i=0; i<fld.length; i++) {
if (fld.charAt(i) != " " || c > 0) {result += fld.charAt(i);
if (fld.charAt(i) != " ") c = result.length;}}return result.substr(0,c);}
var numb = '0123456789';

function isValid(parm,val) {if (parm == "") return true;
for (i=0; i<parm.length; i++) {if (val.indexOf(parm.charAt(i),0) == -1)
return false;}return true;}

function isNum(parm)
	 {
		return isValid(parm,numb);
	}


function validateDate(fld) 
{
	var mth = new Array(' ','january','february','march','april','may','june','july','august','september','october','november','december');
	var day = new Array(31,28,31,30,31,30,31,31,30,31,30,31);

	var dd, mm, yy;
	var today = new Date;
	var t = new Date;
	fld = stripBlanks(fld);
		if (fld == '') return false;
	var d1 = fld.split('\/');
		if (d1.length != 3) d1 = fld.split(' ');
		if (d1.length != 3) return false;
	dd = d1[0]; mm = d1[1]; yy = d1[2];
	var n = dd.lastIndexOf('st');
		if (n > -1) dd = dd.substr(0,n);
			n = dd.lastIndexOf('nd');
		if (n > -1) dd = dd.substr(0,n);
			n = dd.lastIndexOf('rd');
		if (n > -1) dd = dd.substr(0,n);
			n = dd.lastIndexOf('th');
		if (n > -1) dd = dd.substr(0,n);
			n = dd.lastIndexOf(',');
		if (n > -1) dd = dd.substr(0,n);
			n = mm.lastIndexOf(',');
		if (n > -1) mm = mm.substr(0,n);
		if (!isNum(dd)) return false;
		if (!isNum(yy)) return false;
		if (!isNum(mm)) 
		{
			var nn = mm.toLowerCase();
				for (var i=1; i < 13; i++) 
				{
					if (nn == mth[i] ||	nn == mth[i].substr(0,3)) 
						{
							mm = i; i = 13;
						}
				}
		}
		if (!isNum(mm)) return false;
			dd = parseFloat(dd); mm = parseFloat(mm); yy = parseFloat(yy);
		if (yy < 100) yy += 2000;
		if (yy < 1582 || yy > 4881) return false;
		if (mm == 2 && (yy%400 == 0 || (yy%4 == 0 && yy%100 != 0))) 
		{
			day[mm-1]++;
		}
		if (mm < 1 || mm > 12) return false;
		if (dd < 1 || dd > day[mm-1]) return false;
		t.setDate(dd); t.setMonth(mm-1); t.setFullYear(yy);
	
	return true;
}