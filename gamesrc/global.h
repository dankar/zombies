#include <list>
using std::list;
#include "world.h"
#include <map>
using std::map;
#include "server.h"
#include "client.h"

extern CWorld* gWorld;
extern CServer* gServer;
extern CClient* gClient;
extern int gClientID;

int gameInit();