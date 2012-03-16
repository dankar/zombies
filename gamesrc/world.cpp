#include <list>
using std::list;


#include "interface.h"
#include "world.h"
#include "global.h"
#include "factory.h"

CWorld::CWorld()
{
	mMap = gfxLoadLevel("asd.zml");
}

void UpdateFromSnapshot(map<int, CEntity*> *entitylist, const char* snapshot)
{
	int type, tick, numobjects, id, datalen;
	char *classname;

	netSetPackData((char*)snapshot, 1024);
	type = netUnpackInt();
	tick = netUnpackInt();
	numobjects = netUnpackInt();

	netUnpackInt();
	classname = netUnpackString();
	netUnpackInt();
	
	gClientID = netUnpackInt();

	map<int, CEntity*>::iterator it;

	for(int i = 0; i < numobjects - 1; i++)
	{
		id = netUnpackInt();
		classname = netUnpackString();
		
		datalen = netUnpackInt();

		if((*entitylist)[id])
		{
			(*entitylist)[id]->Unpack();
			(*entitylist)[id]->unpacked = tick;

		}
		else
		{
			CEntity *ent = CreateEntity(classname);
			ent->id = id;
			ent->unpacked = tick;
			ent->Unpack();
			(*entitylist)[id] = ent;
		}
	}

}

int CWorld::Tick()
{
	for(map<int, CEntity*>::iterator it = m_DeleteList.begin(); it != m_DeleteList.end(); it++)
	{
		m_Entities.erase((*it).second->id);
		delete (*it).second;
		
	}

	m_DeleteList.clear();

	for(map<int, CEntity*>::iterator it = m_Entities.begin(); it != m_Entities.end(); it++)
		(*it).second->Tick();

	for(map<int, CEntity*>::iterator it = m_DeleteList.begin(); it != m_DeleteList.end(); it++)
	{
		m_Entities.erase((*it).second->id);
		delete (*it).second;
		
	}

	m_DeleteList.clear();

	return 1;
}

int CWorld::Snapshot(int client)
{
	netBeginObjectSnap(0, "ClientID");
	netPackInt(gServer->GetClientEntity(client)->id);
	netEndObjectSnap();

	for(map<int, CEntity*>::iterator it = m_Entities.begin(); it != m_Entities.end(); it++)
	{
		netBeginObjectSnap((*it).second->id, (*it).second->GetClassname());
		(*it).second->Snapshot(client);
		netEndObjectSnap();
	}

	return 1;
}

int CWorld::AddEntity(CEntity* ent)
{
	m_Entities[ent->id] = ent;
	return 1;
}

int CWorld::RemoveEntity(CEntity* ent)
{
	m_DeleteList[ent->id] = ent;
	return 1;
}

int CWorld::Render(const char* prevsnap, const char* cursnap, float interpolation)
{
	CEntity* prev;

	if(!cursnap || !prevsnap)
	{
		gfxDrawText(150, 150, "Waiting for snapshot...");
	}
	else
	{
		
		UpdateFromSnapshot(&m_Entities, cursnap);
		UpdateFromSnapshot(&m_PrevEntities, prevsnap);

		map<int, CEntity*>::iterator it;

		if(m_Entities[gClientID])
		{
			if(m_PrevEntities[gClientID])
			{
				m_Entities[gClientID]->SetupView(interpolation, m_PrevEntities[gClientID]);
			}
			else
			{
				m_Entities[gClientID]->SetupView(interpolation, m_Entities[gClientID]);
			}

			gfxDrawLevel(mMap);
		}
		
		for(it = m_Entities.begin(); it != m_Entities.end(); it++)
		{
			prev = m_PrevEntities[(*it).second->id];

			if(prev)
				(*it).second->Render(interpolation, prev);
			else
				(*it).second->Render(interpolation, (*it).second);
		}
	}

	return 1;
}