#include "interface.h"
#include "global.h"
#include "gunshot.h"
#include "../src/math/vector.h"

void *CGunshotFactory() { return new CGunshot(); }

int CGunshot::Tick()
{
	x+=dx * 20.0f;
	y+=dy * 20.0f;

	list<Vec3> Collisions;

	life -= 1;

	if(gfxCollideLevel(gWorld->mMap, x, y, z, 5, 5, 1, &Collisions))
		life = 0;



	if(life == 0)
		gWorld->RemoveEntity(this);

	return 1;
}

int CGunshot::Render(float interpolation, CEntity* prev)
{
	CGunshot *prevG = (CGunshot*)prev;

	float ix, iy;

	ix = lerp(prevG->x, x, interpolation);
	iy = lerp(prevG->y, y, interpolation);

	gfxSetColor(1, 0, 0, 1);
	gfxPushMatrix();
	gfxTranslate(0.0f, 0.0f, 25.0f);
	gfxBeginQuads();
	gfxDrawQuad((int)ix, (int)iy, 2, 2);
	gfxEndQuads();
	gfxPopMatrix();

	return 1;
}

int CGunshot::Snapshot(int client)
{
	netPackFloat(x);
	netPackFloat(y);
	netPackFloat(z);
	netPackFloat(dx);
	netPackFloat(dy);

	return 1;
}

int CGunshot::Unpack()
{
	x = netUnpackFloat();
	y = netUnpackFloat();
	z = netUnpackFloat();
	dx = netUnpackFloat();
	dy = netUnpackFloat();

	return 1;
}

int CGunshot::UnpackInput()
{
	return 1;
}