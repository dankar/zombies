#ifndef _CAMERA_H_
#define _CAMERA_H_

class CCamera
{
	Vec3 m_View;
	Vec3 m_Pos;
	Vec3 m_Up;
public:

	CCamera(float x, float y, float z, float lx, float ly, float lz, float ux, float uy, float uz)
		: m_Pos(x, y, z), m_View(lx-x, ly-y, lz-z), m_Up(ux, uy, uz)
	{
		m_View.normalize();
	}
	void SetFromMouse();
	void SetClickDragFromMouse();
	void Setup();
};

#endif