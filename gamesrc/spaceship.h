#ifndef _SPACESHIP_H_
#define _SPACESHIP_H_

class CPlayer : public CEntity
{
public:
	Vec3 position;
	Vec3 pos;
	float dz;
	int updown, sides;
	int mouse_x, mouse_y;
	float aim_x, aim_y;
	int button;
	int reload;
	int colliding;
	char* GetClassname(){ return "CPlayer"; };

	CPlayer();

	int Render(float interpolation, CEntity* prev);
	int Tick();
	int Snapshot(int client);
	int Unpack();
	int UnpackInput();

	void SetupView(float interpolation, CEntity* prev);
};

void *CPlayerFactory();

#endif