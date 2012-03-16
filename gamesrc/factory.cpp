#include <list>
using std::list;
#include "interface.h"
#include "world.h"
#include "factory.h"
#include "spaceship.h"
#include "gunshot.h"

CEntity* CreateEntity(const char* classname)
{
	if(strcmp(classname, "CPlayer") == 0)
		return (CEntity*)CPlayerFactory();
	else if(strcmp(classname, "CGunshot") == 0)
		return (CEntity*)CGunshotFactory();
	else
		return 0;
}