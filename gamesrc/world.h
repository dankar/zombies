#ifndef _WORLD_H_
#define _WORLD_H_
#include <map>
using std::map;

class CEntity
{
public:
	int id;
	int unpacked;
	CEntity(){ id = netGetUniqueID(); }
	virtual int Render(float interpolation, CEntity* prev) = 0;
	virtual int Tick() = 0;
	virtual int Snapshot(int client) = 0;
	virtual int Unpack() = 0;
	virtual int UnpackInput() = 0;
	virtual char* GetClassname() = 0;
	virtual void SetupView(float interpolation, CEntity* prev){ };
};

class CWorld
{
	map<int, CEntity*> m_PrevEntities;
	map<int, CEntity*> m_Entities;
	map<int, CEntity*> m_DeleteList;
	map<int, CEntity*> m_RenderList;

	
public:
	int mMap;
	CWorld();
	int Tick();
	int Snapshot(int client);
	int AddEntity(CEntity* ent);
	int RemoveEntity(CEntity* ent);
	int Render(const char* prevsnap, const char* cursnap, float interpolation);
};

#endif