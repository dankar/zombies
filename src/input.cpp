#include <gl/glfw.h>
#include "input.h"
#include <cstring>
#include <cstdio>

char Keys[KeyCount];
char OldKeys[KeyCount];
char* InputLine;
int* InputDone;
int InputMaxLen;
int InputPos;

int Mousebutton[3];
int OldMousebutton[3];
int MouseX;
int MouseY;
int MouseDX;
int MouseDY;
int MouseEnabled = 1;

bool inpKey(int key){ return Keys[key] == GLFW_PRESS; }
bool inpOldKey(int key){ return OldKeys[key] == GLFW_PRESS; }
bool inpKeyPressed(int key){ return inpKey(key) && !inpOldKey(key); }
bool inpKeyReleased(int key){ return !inpKey(key) && inpOldKey(key); }
bool inpMouse(int button){ return Mousebutton[button] == GLFW_PRESS; }
bool inpOldMouse(int button){ return OldMousebutton[button] == GLFW_PRESS; }
bool inpMousePressed(int button){ return inpMouse(button) && !inpOldMouse(button); }
bool inpMouseReleased(int button){ return !inpMouse(button) && inpOldMouse(button); }
int inpGetMouseRelX();
int inpGetMouseRelY();


bool inpInit()
{
	glfwSetKeyCallback(KeyCallback);
	glfwSetCharCallback(CharCallback);
	glfwEnable(GLFW_KEY_REPEAT);

	InputLine = 0;
	InputMaxLen = 0;
	InputPos = 0;

	inpUpdate();

	return true;
}

int inpGetMouseRelX()
{
	return MouseDX;
}

int inpGetMouseRelY()
{
	return MouseDY;
}

int inpGetMouseX()
{
	return MouseX;
}

int inpGetMouseY()
{
	return MouseY;
}


bool inpSetInput(char* buffer, int len, int *done)
{
	InputLine = buffer;
	InputMaxLen = len;
	InputPos = 0;
	if(InputLine)
		InputLine[0] = 0;
	InputDone = done;
	if(InputDone)
		*InputDone = 0;

	return true;
}

void inpUpdate()
{
	int x, y;

	memcpy(OldKeys, Keys, KeyCount);
	
	if(MouseEnabled)
	{
		glfwGetMousePos(&x, &y);

		MouseDX = x - MouseX;
		MouseDY = y - MouseY;

		MouseX = x;
		MouseY = y;
	}
	else
	{
		MouseDX = 0;
		MouseDY = 0;
	}

	//glfwSetMousePos(200, 200);

	

	OldMousebutton[0] = Mousebutton[0];
	OldMousebutton[1] = Mousebutton[1];
	OldMousebutton[2] = Mousebutton[2];

	Mousebutton[0] = glfwGetMouseButton(GLFW_MOUSE_BUTTON_LEFT);
	Mousebutton[1] = glfwGetMouseButton(GLFW_MOUSE_BUTTON_RIGHT);
	Mousebutton[2] = glfwGetMouseButton(GLFW_MOUSE_BUTTON_MIDDLE);
}

void inpDestroy()
{
	glfwSetKeyCallback(0);
}

void inpHideCursor(int hidecursor)
{
	if(hidecursor)
	{
		glfwDisable(GLFW_MOUSE_CURSOR);
	}
	else
	{
		glfwEnable(GLFW_MOUSE_CURSOR);
	}
}



void GLFWCALL KeyCallback(int key, int action)
{
	if(InputLine)
	{
		if(action == GLFW_PRESS)
		{
			if(key == GLFW_KEY_BACKSPACE)
			{
				if(InputPos > 0)
				{
					InputLine[--InputPos] = 0;
				}
			}
			if(key == GLFW_KEY_ENTER)
			{
				*InputDone = 1;
			}
		}
	}	

	Keys[key] = action;
}

void GLFWCALL CharCallback(int key, int action)
{
	if(InputLine && action == GLFW_PRESS)
	{
		if(InputPos < InputMaxLen - 1)
		{
			InputLine[InputPos++] = key;
			InputLine[InputPos] = 0;
		}
	}
}