#include <GL/glfw.h>
#include "../math/vector.h"
#include "../input.h"
#include "../timer.h"
#include "camera.h"

void CCamera::SetFromMouse()
{

}

void CCamera::SetClickDragFromMouse()
{
	Vec3 side = m_View.cross(m_Up);

	float delta = float(timerGetDiff()) / 1000.0f;

	side.normalize();

	if(inpMouse(0))
	{
		
		m_View.Rotate((float)-inpGetMouseRelY(), side.x, side.y, side.z);
		m_View.Rotate((float)-inpGetMouseRelX(), 0, 1, 0); // m_Up.y, m_Up.z);
		
		inpHideCursor(1);
	}
	else
	{
		inpHideCursor(0);
	}

	if(inpKey('W'))
		m_Pos = m_Pos + m_View * delta * 20;

	if(inpKey('S'))
		m_Pos = m_Pos - m_View * delta * 20;

	if(inpKey('A'))
		m_Pos = m_Pos - side * delta * 20;
	
	if(inpKey('A'))
		m_Pos = m_Pos + side * delta * 20;
}

void CCamera::Setup()
{
	gluLookAt(m_Pos.x, m_Pos.y, m_Pos.z, m_Pos.x+m_View.x, m_Pos.y+m_View.y, m_Pos.z+m_View.z, m_Up.x, m_Up.y, m_Up.z);
}