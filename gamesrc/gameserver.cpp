#include <interface.h>
#include "global.h"
#include "gameserver.h"
#include "spaceship.h"


int CGameServer::Init()
{
	gServer->SetClientClass(CPlayerFactory);

	return 1;
}