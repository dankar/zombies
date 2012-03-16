#include <interface.h>
#include "global.h"
#include "gameclient.h"

CGameClient::CGameClient()
{
	
}


int CGameClient::PackInput()
{
	int updown = 0, sides = 0;
	int button;

	if(inputKey('W')) updown -= 1;
	if(inputKey('S')) updown += 1;
	if(inputKey('A')) sides += 1;
	if(inputKey('D')) sides -= 1;

	mouse_x = inputGetMouseX()/2;
	mouse_y = -inputGetMouseY()/2;
	button = inputGetMouseButton(0);
		
	netPackInt(updown);
	netPackInt(sides);
	netPackInt(mouse_x);
	netPackInt(mouse_y);
	netPackInt(button);

	return 1;
}
