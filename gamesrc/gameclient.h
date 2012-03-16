#ifndef _GAMECLIENT_H_
#define _GAMECLIENT_H_

class CGameClient : public CClient
{
public:
	int mouse_x;
	int mouse_y;
	int input_updown;
	int input_sides;

	CGameClient();
	int PackInput();
};

#endif