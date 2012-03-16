#include <gl/glew.h>
#include <gl/glfw.h>
#include <gl/glu.h>
#include "video.h"
#include "../log.h"


int videoOpenWindow(int x, int y, int depth, int fullscreen)
{
	if(!glfwOpenWindow(x, y, 8 ,8, 8, 8, 8, 8, fullscreen?GLFW_FULLSCREEN:GLFW_WINDOW))
	{
		return 0;
	}

	videoSetPerspective();

	//glfwSwapInterval(1);

	CLog::Get().Write(APP_LOG, LOG_INFO, "GL_VENDOR:\t%s", glGetString(GL_VENDOR));
	CLog::Get().Write(APP_LOG, LOG_INFO, "GL_RENDERER:\t%s", glGetString(GL_RENDERER));
	CLog::Get().Write(APP_LOG, LOG_INFO, "GL_VERSION:\t%s", glGetString(GL_VERSION));

	glShadeModel(GL_SMOOTH);
	glEnable(GL_DEPTH_TEST);
//	glEnable(GL_DEPTH_FUNC);
	glClearDepth(100.0f);
	glEnable(GL_BLEND);
	glEnable(GL_TEXTURE_2D);
	//glDepthFunc(GL_LEQUAL);

	glewInit();

	return 1;
}

int videoSetOrtho(int x, int y)
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	gluOrtho2D(0, x, y, 0);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	return 1;
}


int videoSetPerspective()
{
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	gluPerspective(45.0f, 800.0f/600.0f, 0.1, 1000.0f);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	return 1;
}