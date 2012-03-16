#include <GL/glfw.h>
#include "timer.h"
#include "interface.h"

int g_FrameDiff;
int g_TimeLastFrame;
int g_TimeThisFrame;
int g_FramesCounter;
float g_LastFPS;
float g_FPS;

int timerStart()
{
	g_TimeThisFrame = miscGetTime();
	g_TimeLastFrame = g_TimeThisFrame;
	g_LastFPS = float(g_TimeThisFrame);
	g_FrameDiff = 0;
	return true;
}

void timerUpdate()
{
	g_TimeLastFrame = g_TimeThisFrame;
	g_TimeThisFrame = miscGetTime();
	g_FrameDiff = (g_TimeThisFrame - g_TimeLastFrame);

	g_FramesCounter++;

	if(g_TimeThisFrame - g_LastFPS > 1000)
	{
		g_FPS = float(g_FramesCounter);
		g_FramesCounter = 0;
		g_LastFPS = float(g_TimeThisFrame);
	}
}

int timerGetDiff()
{
	return g_FrameDiff;
}

void timerStop()
{

}

float timerGetFPS()
{
	return g_FPS;
}

int timerGetTick()
{
	return g_TimeThisFrame;
}