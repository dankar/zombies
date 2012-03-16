#ifndef _GUNSHOT_H_
#define _GUNSHOT_H_

#include "factory.h"

class CGunshot : public CEntity
{
public:
	float x;
	float y;
	float z;

	float dx;
	float dy;
	float dz;

	float life;

	float damage;

	int Tick();
	int Render(float interpolation, CEntity* prev);

	char* GetClassname(){ return "CGunshot"; };
	int Snapshot(int client);
	int Unpack();
	int UnpackInput();
};

void *CGunshotFactory();

#endif