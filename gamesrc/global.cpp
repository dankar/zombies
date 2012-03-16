#include "interface.h"
#include "global.h"
#include "gameclient.h"
#include "gameserver.h"

CWorld* gWorld;
CServer* gServer;
CClient* gClient;
int gClientID = 0;

int gameInit()
{
	gWorld = new CWorld();
	gServer = new CGameServer();
	gClient = new CGameClient();

	return 1;
}

int clientInit(){ return gClient->Init(); }
int clientRender(const char* prevsnap, const char* cursnap, float interpolation){ return gClient->Render(prevsnap, cursnap, interpolation); }
int clientPackInput(){ return gClient->PackInput(); }

int serverInit(){ return gServer->Init(); }
int serverClientJoined(int client){ return gServer->ClientJoined(client); }
int serverClientLeft(int client){ return gServer->ClientLeft(client); }
int serverUnpackInput(int client){ return gServer->UnpackInput(client); }
int serverSnapshot(int client){ return gServer->Snapshot(client); }
int serverTick(int tick){ return gServer->Tick(tick); }