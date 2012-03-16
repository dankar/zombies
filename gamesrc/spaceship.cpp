#include "interface.h"
#include "global.h"

#include "gunshot.h"
#include <../include/GL/glfw.h>
#include "../src/math/vector.h"
#include <cmath>
#include "spaceship.h"


void *CPlayerFactory(){ return new CPlayer(); }

CPlayer::CPlayer() : CEntity()
{ 
	vector<Vec3> positions;

	int random = rand();

	dz = position.x = position.y = updown = sides = reload = 0.0f; position.z = 40.0f; 

	gfxFindItemInLevel(gWorld->mMap, "playerStart", &positions);

	if(positions.size() == 0)
	{
		exit(0);
	}
	
	gfxLevelAddFlashlight(gWorld->mMap, &pos.x, &pos.y, &pos.z, &aim_x, &aim_y, gClientID);

	random = random * (int)positions.size() / RAND_MAX;

	position = positions[random];
}

void CPlayer::SetupView(float interpolation, CEntity* prev)
{
	CPlayer* prevp = (CPlayer*)prev;

	Vec3 pos = prevp->position.interpolate(position, interpolation);

	gfxLookAt(pos.x, pos.y, 500+pos.z, pos.x, pos.y, 0, 0, 1, 0);	
}

int CPlayer::Render(float interpolation, CEntity* prev)
{
	CPlayer *prevp = (CPlayer*)prev;

	float mx, my;

	pos = prevp->position.interpolate(position, interpolation);

	mx = lerp((float)prevp->mouse_x, (float)mouse_x, interpolation);
	my = lerp((float)prevp->mouse_y, (float)mouse_y, interpolation);

	aim_x = mx;
	aim_y = my;

	gfxPushMatrix();

	gfxTranslate(pos.x, pos.y, pos.z);

	gfxBindTexture(0);
	gfxSetColor(1, 1, 1, 1);
	gfxBeginQuads();
	gfxDrawQuad(0, 0, 20, 20);
	gfxEndQuads();

	glColor4f(1.0f, 1.0f, 1.0f, 0.5f);

	glBlendFunc(GL_ALPHA, GL_ONE_MINUS_DST_ALPHA);

	gfxPopMatrix();

	if(gClientID == id)
	{
		gfxSetColor(0, 1, 0, 1);

		gfxPushMatrix();
		glDisable(GL_DEPTH_TEST);
		gfxTranslate(pos.x+inputGetMouseX(), pos.y-inputGetMouseY(), 0);
		
		gfxDrawLine(5,0,0,10,0,0); 
		gfxDrawLine(0,5,0,0,10,0);
		gfxDrawLine(-5,0,0,-10,0,0);
		gfxDrawLine(0,-5,0,0,-10,0);
		glEnable(GL_DEPTH_TEST);
		gfxPopMatrix();

	}

	return 1;	
}

int CPlayer::Tick()
{
	float aox, aoy, aoz;

	position.x -= sides*10;
	position.y -= updown*10;
	position.z -= dz;

	list<Vec3> Collisions;

	

	dz += 3.0f;

	if(button && (reload == 0))
	{
		CGunshot* projectile = new CGunshot();

		projectile->life = 50;

		projectile->x = position.x;
		projectile->y = position.y;
		projectile->z = position.z+5;

		Vec2 origin(0, 0);
		Vec2 target((float)mouse_x, (float)mouse_y);

		Vec2 norm = (target - origin);

		norm = norm.normalize();

		projectile->dx = norm.x;
		projectile->dy = norm.y;

		gWorld->AddEntity(projectile);

		reload = 5;
	}

	if(reload > 0)
	{
		reload-=1;
	}

	Collisions.clear();

	colliding = gfxCollideLevel(gWorld->mMap, position.x, position.y, position.z-5, 20, 20, 20, &Collisions);

	if(colliding)
	{
		list<Vec3>::iterator it;

		for(it = Collisions.begin(); it != Collisions.end(); it++)
		{
			aox = fabsf(it->x);
			aoy = fabsf(it->y);
			aoz = fabsf(it->z);
			
			if(aoz < 15)
			{
				dz=0;
				position.z+=aoz;
			}
			else
			if(aox < aoy && aox < aoz)
			{
				position.x+=it->x*1.1f;
			}
			else
			if(aoy < aox && aoy < aoz)
			{
				position.y+=it->y*1.1f;
			}
			else
			if(aoz < aox && aoz < aoy)
			{
				dz = 0;
				position.z+=it->z/2;
			}
		}
	}

	/*colliding = gfxCollideLevel(gWorld->mMap, x, y, z-5, 20, 20, 10, &ox, &oy, &oz);

	if(colliding)
		if(oz < 15)
			z+=oz;*/

	return 1;
}

int CPlayer::Snapshot(int client)
{
	netPackFloat(position.x);
	netPackFloat(position.y);
	netPackFloat(position.z);
	netPackFloat((float)mouse_x);
	netPackFloat((float)mouse_y);
	netPackInt(colliding);

	return 1;
}

int CPlayer::Unpack()
{
	position.x = netUnpackFloat();
	position.y = netUnpackFloat();
	position.z = netUnpackFloat();
	mouse_x = (int)netUnpackFloat();
	mouse_y = (int)netUnpackFloat();
	colliding = netUnpackInt();

	return 1;
}

int CPlayer::UnpackInput()
{
	updown = netUnpackInt();
	sides = netUnpackInt();
	mouse_x = netUnpackInt();
	mouse_y = netUnpackInt();
	button = netUnpackInt();

	return 1;
}