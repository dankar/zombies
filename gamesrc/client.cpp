#include "interface.h"
#include "global.h"
#include "client.h"


int CClient::Init()
{
	inputShowMouse(1);

	
	return 1;
}

int CClient::Render(const char* prevsnap, const char* cursnap, float interpolation)
{
	gWorld->Render(prevsnap, cursnap, interpolation);


	return 1;
}