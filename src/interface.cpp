#include <GL/glew.h>
#include <GL/glfw.h>
#include <map>
#include <string>
#include <vector>
#include "math/vector.h"
#include "video/model.h"
#include "interface.h"
#include "video/texture.h"
#include "resourcehandler.h"
#include "video/video.h"
#include "input.h"
#ifdef WIN32
#include <winsock2.h>
#endif
#include "log.h"
#include "timer.h"
#include "video/font.h"
#include "video/console.h"


int unique_id = 1;

struct _snapshot snapshot;
struct _snapobject *snapobject;
char* packData;
int packPosition;
int packMaxLen;

const char* currentSnapshot;
const char* previousSnapshot;
float snapshotInterpolation;

char tempData[512];
char tempString[512];

CFont EngineFont;

#pragma warning(disable : 4312)

int initEngine()
{
	

	CLog::Get().Write(APP_LOG, LOG_INFO, "Initializing graphics");
	
	if(!gfxInit())
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Graphics failed");
		return 0;
	}

	CLog::Get().Write(APP_LOG, LOG_INFO, "Initializing input");

	if(!inpInit())
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Input failed");
		return 0;
	}

	CLog::Get().Write(APP_LOG, LOG_INFO, "Initializing network");

	if(!netInit())
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Network failed");
		return 0;
	}

	EngineFont.Load("font_big.png");

	timerStart();

	return 1;
}

int closeEngine()
{
	timerStop();

	gfxClose();

	inpDestroy();

	netStop();

	return 1;
}

// MISC

int miscGetTime()
{
	return int(glfwGetTime() * 1000);
}

// INPUT

int inputKey(char key)
{
	return inpKey(key);
}

int inputGetMouseRelX()
{
	return inpGetMouseRelX();
}

int inputGetMouseRelY()
{
	return inpGetMouseRelY();
}

int inputGetMouseX()
{
	return inpGetMouseX();
}

int inputGetMouseY()
{
	return inpGetMouseY();
}

int inputGetMouseButton(int button)
{
	return inpMouse(button);
}

int inputShowMouse(int hide)
{
	if(hide)
		glfwDisable(GLFW_MOUSE_CURSOR);
	else
		glfwEnable(GLFW_MOUSE_CURSOR);

	return 1;
}

// NETWORK

int netGetSnapshotTick()
{
	return snapshot.tick;
}

int netGetSnapshotNumObjects()
{
	return snapshot.num_objects;
}

_snapobject* netGetSnapshotObject(int index)
{
	return &snapshot.objects[index];
}

int netSetPackData(char* data, int maxlen)
{
	packData = data;
	packPosition = 0;
	packMaxLen = maxlen;

	return 1;
}

int netGetPackPosition()
{
	return packPosition;
}

int netInit()
{
#ifdef WIN32
	WSADATA wsaData;

	if (WSAStartup(MAKEWORD(1, 1), &wsaData) != 0)
		return false;
#endif
	snapshot.tick = 0;
	return 1;
}

int netStop()
{
#ifdef WIN32
	WSACleanup();
#endif
	return 1;
}

int netNextSnapshot()
{
	snapshot.tick++;
	return 1;
}
int netGetUniqueID()
{
	return unique_id++;
}

int netBeginSnapshot()
{
	snapshot.num_objects = 0;
	return 1;
}

int netBeginObjectSnap(int id, const char* classname)
{
	snapobject = &(snapshot.objects[snapshot.num_objects]);
	snapobject->id = id;
	strcpy(snapobject->classname, classname);
	//snapobject->data_len = 0;
	netSetPackData(snapshot.objects[snapshot.num_objects].data, 1024);
	return 1;
}

int netPackFloat(float f)
{
	float* dst = (float*)&packData[packPosition];

	if(packPosition + sizeof(float) > packMaxLen)
		return 0;

	*dst = f;
	packPosition += sizeof(float);

	return 1;
}

int netPackInt(int i)
{
	int* dst = (int*)&packData[packPosition];

	if(packPosition + sizeof(int) > packMaxLen)
		return 0;

	*dst = i;
	packPosition += sizeof(int);
	return 1;
}

int netPackString(const char* str)
{
	netPackInt(strlen(str));

	if(packPosition + strlen(str) > packMaxLen)
		return 0;

	memcpy(packData + packPosition, str, strlen(str));

	packPosition += strlen(str);

	return 1;
}

float netUnpackFloat()
{
	float f;
	
	
	if(packPosition + sizeof(float) > packMaxLen)
		return 0;
	
	f = *(float*)&packData[packPosition];

	packPosition += sizeof(float);

	return f;
}

int netUnpackInt()
{
	int i;

	if(packPosition + sizeof(int) > packMaxLen)
		return 0;
	
	i = *(int*)&packData[packPosition];

	packPosition += sizeof(int);

	return i;
}

char* netUnpackString()
{
	int len = netUnpackInt();

	if(packPosition + len > packMaxLen)
		return 0;

	memcpy(tempString, &packData[packPosition], len);

	tempString[len] = 0;

	packPosition += len;

	return tempString;
}

int netEndObjectSnap()
{
	snapshot.num_objects++;
	snapobject->data_len = packPosition;

	return 1;
}

int netEndSnapshot()
{
	return 1;
}


// GRAPHICS


int gfxInit()
{
	if(!glfwInit())
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not init glfw");
		return 0;
	}

	if(!glewInit())
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not init glew");
		return 0;
	}

	if(!videoOpenWindow(800, 600, 32, 0))
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not open a window, %dx%d, %d", 800, 600, 32);
		return 0;
	}

	if(!GL_ARB_texture_non_power_of_two)
	{
		CLog::Get().Write(APP_LOG, LOG_WARNING, "No support for NPOT textures.");
	}

	CConsole::Init();

	return 1;
}

int gfxClose()
{
	glfwTerminate();

	return 1;
}

int gfxLoadLevel(const char* filename)
{
	return CResourceHandler::LoadLevel(filename);
}

int gfxDrawLevel(int level)
{
	CLevel *levelclass = (CLevel*)level;

	if(!levelclass)
		return 0;

	levelclass->Render();

	return 1;
}

int gfxFindItemInLevel(int level, const char* name, vector<Vec3> *result)
{
	CLevel *levelclass = (CLevel*)level;

	if(!levelclass)
		return 0;

	*result = levelclass->FindItemByName(name);

	return 1;
}

int gfxLevelAddFlashlight(int level, float *x, float *y, float *z, float *nx, float *ny, int id)
{
	CLevel *levelclass = (CLevel*)level;

	if(!levelclass)
		return 0;

	levelclass->AddFlashlight (x, y, z, nx, ny, id);
}

int gfxLevelRemoveFlashlight(int level, float *x, int id)
{
	CLevel *levelclass = (CLevel*)level;

	if(!levelclass)
		return 0;

	levelclass->RemoveFlashlight(id);
}

int gfxCollideLevel(int level, float x, float y, float z, float sx, float sy, float sz, list<Vec3> *Collisions)
{
	CLevel *levelclass = (CLevel*)level;

	if(!levelclass)
		return 0;

	Vec3 position(x, y, z);
	Vec3 size(sx, sy, sz);

	return levelclass->Collide(position, size, Collisions);
}

int gfxLoadTexturePNG(const char* filename)
{
	return CResourceHandler::LoadTexture(filename, 1, 0, 1);
}

int gfxBindTexture(int texture)
{
	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, texture);

	return 1;
}

int gfxPushMatrix()
{
	glPushMatrix();

	return 1;
}

int gfxPopMatrix()
{
	glPopMatrix();

	return 1;
}

int gfxBlendNormal()
{
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	return 1;
}

int gfxDrawLine(float x1, float y1, float z1, float x2, float y2, float z2)
{
	glBegin(GL_LINES);
	glVertex3f(x1, y1, z1);
	glVertex3f(x2, y2, z2);
	glEnd();

	return 1;
}

int gfxBlendAdditive()
{
	glBlendFunc(GL_SRC_ALPHA, GL_ONE);

	return 1;
}

int gfxBeginQuads()
{
	glPushMatrix();
	glBegin(GL_QUADS);

	return 1;
}

int gfxEndQuads()
{
	glEnd();
	glPopMatrix();
	return 1;
}

int gfxSetColor(float r, float g, float b, float a)
{
	glColor4f(r, g, b, a);

	return 1;
}

int gfxDrawQuad(int x, int y, int w, int h)
{
	glTexCoord2f(0, 0); glVertex2i(x,	y);
	glTexCoord2f(1, 0); glVertex2i(x+w, y);
	glTexCoord2f(1, 1); glVertex2i(x+w,	y+h);
	glTexCoord2f(0, 1); glVertex2i(x,	y+h);

	return 1;
}


int gfxLoadMesh(const char* filename)
{
	int mesh = CResourceHandler::LoadModel(filename, 1.0, 0.0, 0.0, 0.0, 0.0);

	return mesh;
}

int gfxDrawMesh(int object, int texture)
{
	gfxBindTexture(texture);
	
	CModel* model = (CModel*)object;

	if(!model)
		return 0;

	model->Draw();

	gfxBindTexture(0);

	return 1;
}

int gfxTranslate(float x, float y, float z)
{
	glTranslatef(x, y, z);

	return 1;
}

int gfxLookAt(float x, float y, float z, float lx, float ly, float lz, float ux, float uy, float uz)
{
	gluLookAt(x, y, z, lx, ly, lz, ux, uy, uz);

	return 1;
}

int gfxRotate(float a, float x, float y, float z)
{
	glRotatef(a, x, y, z);

	return 1;
}

int gfxDrawText(int x, int y, const char* text)
{
	gfxPushMatrix();
	videoSetOrtho(800, 600);
	EngineFont.Draw(x, y, text);
	videoSetPerspective();
	gfxPopMatrix();

	return 1;
}

int gfxVertex(float x, float y, float z)
{
	glVertex3f(x, y, z);

	return 1;
}

int gfxDrawSprite3D(float x, float y, float z, float size)
{
	glBegin(GL_QUADS);
	glTexCoord2f(0.0f, 0.0f); gfxVertex(x+-size, y+size, z+0.0);
	glTexCoord2f(1.0f, 0.0f); gfxVertex(x+size, y+size, z+0.0);
	glTexCoord2f(1.0f, 1.0f); gfxVertex(x+size, y+-size, z+0.0);
	glTexCoord2f(0.0f, 1.0f); gfxVertex(x+-size, y+-size, z+0.0);
			
	glTexCoord2f(0.0f, 0.0f); gfxVertex(x+0.0, y+size, z+-size);
	glTexCoord2f(1.0f, 0.0f); gfxVertex(x+0.0, y+size, z+size);
	glTexCoord2f(1.0f, 1.0f); gfxVertex(x+0.0, y+-size, z+size);
	glTexCoord2f(0.0f, 1.0f); gfxVertex(x+0.0, y+-size, z+-size);
	glEnd();

	return 1;
}
