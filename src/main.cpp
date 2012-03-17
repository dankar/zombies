#include <GL/glew.h>
#include <GL/glfw.h>
#include <cstdio>
#include "timer.h"
#include <vector>
#include <string>
#include "video/font.h"
using namespace std;
#include "video/console.h"
#include "log.h"
#include "input.h"
#include "interface.h"
#include "socket.h"
#include "video/video.h"
#ifndef WIN32
#include <sched.h>
#endif

int main(int argc, char* argv[])
{
	bool show_console = 0;
	bool done = 0;
	int tickCounter = 0;
	int gServer = 0;
	CSocketServer server;
	CSocketClient client;
	CFont font;

	if(argc != 2)
	{
		printf("usage: mecha server|hostname\n");
		return 1;
	}

	gameInit();

	CLog::Get().Init();

	initEngine();

	font.Load("font_big.png");

	if(strcmp(argv[1], "server") == 0)
	{
		gServer = 1;
	}

	clientInit();

	if(gServer)
	{
		if(!server.Listen(4444))
		{
			CLog::Get().Write(APP_LOG, LOG_ERROR, "Listen failed");
			done = 1;
		}
		if(!client.Connect("localhost", 4444))
		{
			CLog::Get().Write(APP_LOG, LOG_ERROR, "Connection failed");
			done = 1;
		}
	}
	else
	{
		if(!client.Connect(argv[1], 4444))
		{
			CLog::Get().Write(APP_LOG, LOG_ERROR, "Connection failed");
			done = 1;
		}
	}

	while(!done)
	{
		timerUpdate();

		tickCounter += timerGetDiff();

		if(show_console)
		{
			if(inpKeyPressed(GLFW_KEY_PAGEUP)) CConsole::ScrollUp();
			if(inpKeyPressed(GLFW_KEY_PAGEDOWN)) CConsole::ScrollDown();
		}
		if(inpKeyPressed(GLFW_KEY_F1))	show_console = !show_console;
		if(inpKeyPressed(GLFW_KEY_ESC)) break;

		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT| GL_STENCIL_BUFFER_BIT);
		glLoadIdentity();

		videoSetPerspective();


		glColor3f(1.0f, 1.0f, 1.0f);

		if(gServer)
			server.Tick();

		client.Tick();

//		if(client.IsConnected())
//			clientTick();

		gfxBlendNormal();

		if(client.IsConnected())
			clientRender(client.GetPreviousSnap(), client.GetCurrentSnap(), client.GetInterpolation());

		gfxSetColor(0.5f, 0.5f, 0.5f, 1.0f);
		videoSetOrtho(800, 600);
		font.Draw(10, 10, "FPS: %f", timerGetFPS());
		font.Draw(10, 25, "Ping: %d", client.m_Ping);
		font.Draw(10, 40, "Snapshot size: %d b", client.GetSnapSize());
		font.Draw(10, 55, "Num objects: %d", client.GetSnapNumObjects());
		videoSetPerspective();

		CConsole::Draw(show_console || !client.IsConnected());

		inpUpdate();

		glfwSwapBuffers();
#ifdef WIN32
		Sleep(1);
#else
		sched_yield();
#endif
	}

	client.Disconnect();
	server.Disconnect();

	closeEngine();

	return true;
}
