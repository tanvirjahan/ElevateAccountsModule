///////////////////////////////////////////////////////
// The global jscript struct stores information
// about what the user is typing into the dropdown.  
// User keystrokes are constantly appended and used
// to search for corresponding dropdown items.
///////////////////////////////////////////////////////
var typeAheadData = 
{
	keyStrokes:"", // Stores user entered keystrokes.
	focusDDLId:"", // Id of the dropdown with focus.
	
	// Reset if DDL ID changes.
	ResetOnNewDDLRequest:function(id) 
	{
		if (this.focusDDLId != id) 
			{this.focusDDLId=id; this.keyStrokes="";}
	}
};

///////////////////////////////////////////////////////
// This method is called when a Type Ahead
// dropdown control fires the OnKeyDown event. When 
// called it stores keystrokes the user enters and 
// finds them in the corresponding dropdown.
///////////////////////////////////////////////////////
function TADD_OnKeyDown(tb)
{
	// Allow default dropdown control key behavior.
	if (event.ctrlKey || event.shiftKey || event.altKey)
		return; 
	if (event.keyCode >= 112 && event.keyCode <=123) // F1-F12 keys");
		return; 

	// Reset typeAheadData on new DDL requests.
	typeAheadData.ResetOnNewDDLRequest(tb.id);
	
	// Capture the users key stroke.
	switch (event.keyCode)
	{
		case 27:  // Esc key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 20:  // Caps lock key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 33:  // Page Up key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 34:  // Page Down key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 35:  // End key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 
			
		case 36:  // Home key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 37:  // Left arrow key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 38:  // Up arrow pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 39:  // Roght arrow key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 40:  // Down arrow pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 46:  // Delete key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 91:  // Windows key pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			return; 

		case 13:  // Return pressed.
			// Clear saved keystrokes and allow dropdown
			// default behavior.
			typeAheadData.keyStrokes = "";
			tb.fireEvent("onchange");
			return; 
			
		case 8:   // Backspace pressed.
			// Trim the last char from keyStrokes. 
			if (typeAheadData.keyStrokes.length > 0)
			{
				typeAheadData.keyStrokes = 
					typeAheadData.keyStrokes.substr(0, 
					    typeAheadData.keyStrokes.length-1);
			}
			// Cancel default dropdown behavior.
			event.cancelBubble = true;
			event.returnValue = false;
			break;

		case 9:  // Tab pressed.
            //exit();
            //alert('exit');
            return;
            break;
            
		default:
			// Convert and save users key strokes.			
			if (event.keyCode >= 96 && event.keyCode <=105)
			{
			    switch (event.keyCode)
			    {
			        case 96:
			            c='0'
			            break;
			        case 97:
			            c='1'
			            break;
			        case 98:
			            c='2'
			            break;
			        case 99:
			            c='3'
			            break;
			        case 100:
			            c='4'
			            break;
			        case 101:
			            c='5'
			            break;
			        case 102:
			            c='6'
			            break;
			        case 103:
			            c='7'
			            break;
			        case 104:
			            c='8'
			            break;
			        case 105:
			            c='9'
			            break;
			    }
			}
			else
			{
			    var c = String.fromCharCode(event.keyCode).toLowerCase();
			} 
		
			//alert(event.keyCode);
		  // 	alert(c);
		    if (c != null)
			{
				typeAheadData.keyStrokes += c;
			}

			// cancel default dropdown behavior.
			event.cancelBubble = true;
			event.returnValue = false;
			break;
	}
	
	// Find the captured keystrockes in the dropdown
	if (TADD_SelectItem(typeAheadData) == false)
	{
		// The keystrokes could not be found, reset.
		typeAheadData.keyStrokes = "";
		// provide status
		window.status="Not found"; 
	}
	else
	{
		// Fire the dropdown event OnChange		
		tb.fireEvent("onchange");
		// provide status
		window.status="KeyStrokes: " + typeAheadData.keyStrokes;
	}
}
///////////////////////////////////////////////////////
// This method iterates through all items in a dropdown
// looking for captured keystrokes.  Once found the
// item is selected otherwise none are selected.
///////////////////////////////////////////////////////
function TADD_SelectItem(typeAheadData)
{
	var ddl = document.getElementById(typeAheadData.focusDDLId);
	ddl.selectedIndex = -1;
	if (typeAheadData.keyStrokes.length > 0)
	{
		// Iterate through all dropdown items.
		for (i = 0; i < ddl.options.length; i++) 
		{
			if ((ddl.options[i].text.length >= 
				typeAheadData.keyStrokes.length)
			&&  (ddl.options[i].text.substr(0, 
			     typeAheadData.keyStrokes.length).toLowerCase() == 
			     typeAheadData.keyStrokes))
			{
				// Item was found, set the selected index.
				ddl.selectedIndex = i;
				return true;
			}
		}
	}
	// Item was not found, return false.
	return false;
}
