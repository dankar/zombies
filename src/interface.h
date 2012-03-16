#ifndef _INTERFACE_H_
#define _INTERFACE_H_

#include <list>
using std::list;
#include "math/vector.h"
#include <vector>
using std::vector;

struct SQVM;
typedef SQVM *HSQUIRRELVM;

struct _snapobject
{
	int id;
	char classname[256];
	int data_len;
	char data[1024];
};

struct _snapshot
{
	int tick;
	int num_objects;
	_snapobject objects[256];
};

/*
	Title: Interface

	Group: Engine
*/

/*
	Function: initEngine

	Initializes the engine. Calls all other init-funcitons. Engine uses this function.

	Returns:

		Zero on error and non-zero otherwise.
*/

int initEngine();

/*
	Function: closeEngine

	Closes and cleans up. Engine uses this function.
*/

int closeEngine();

/*
	Group: Graphics
*/

/*
	Function: gfxInit

	Initializes the graphics. Engine uses this function.

	Returns:
		Zero on error and non-zero otherwise.

*/

int gfxInit();

/*
	Function: gfxLoadTexturePNG

	Loads a png texture to be used.

	Parameters:
		filename - Filename of the texture to be loaded (will search for the file in the data subdirectory)

	Returns:
		On success <gfxLoadTexturePNG> returns the integer identifying the texture, otherwise zero.

*/

int gfxLoadTexturePNG(char* filename);

int gfxLoadLevel(const char* filename);

int gfxLevelAddFlashlight(int level, float *x, float *y, float *z, float *nx, float *ny, int id);
int gfxLevelRemoveFlashlight(int level, float *x, int id);

int gfxDrawLevel(int level);

int gfxCollideLevel(int level, float x, float y, float z, float sx, float sy, float sz, list<Vec3> *Collisions);

int gfxFindItemInLevel(int level, const char* name, vector<Vec3> *result);

/*
	Function: gfxBindTexture

	Sets a texture as the current texture used for drawing.

	Parameters:
		texture - Integer identifying the texture to be bound.
*/

int gfxBindTexture(int texture);

/*
	Function: gfxPushMatrix

	Pushes the current matrix on the matrix stack.
*/

int gfxPushMatrix();

/*
	Function: gfxPopMatrix

	Pops a matrix from the matrix stack and sets it as the current matrix.
*/

int gfxPopMatrix();

/*
	Function: gfxBlendNormal

	Sets the normal blending mode.
*/

int gfxBlendNormal();

/*
	Function: gfxBlendAdditive

	Sets additive blending mode.
*/

int gfxBlendAdditive();

/*
	Function: gfxBeginQuads

	Must be called before drawing quads. (A batch of quads must be ended with <gfxEndQuads>)
*/

int gfxBeginQuads();

/*
	Function: gfxEndQuads

	Called after <gfxBeginQuads> and a number of <gfxDrawQuad>.
*/

int gfxEndQuads();

/*
	Function: gfxSetColor

	Sets the current color used for drawing quads. The colors are floats in the range 0 - 1.

	Parameters:
		r - Red
		g - Green
		b - Blue
		a - Alpha
*/

int gfxSetColor(float r, float g, float b, float a);

int gfxDrawLine(float x1, float y1, float z1, float x2, float y2, float z2);

/*
	Function: gfxDrawQuad

	Draws a quad on the screen with the top left corner at x,y and the size w,h.

	Parameters:
		x - X coordinate for quad.
		y - Y coordinate for quad.
		w - Width for quad.
		h - Height for quad.
*/

int gfxDrawQuad(int x, int y, int w, int h);

/*
	Function gfxVertex

	Specifies a vertex.

	Parameters:
		x - x
		y - y
		z - z
*/
int gfxVertex(float x, float y, float z);

/*
	Function: gfxLoadMesh

	Loads a mesh.

	Parameters:
		filename - Filename of the mesh. (Will search for mesh in the data subdirectory)

	Returns:
		Returns the integer identifying the mesh on success, otherwise zero.

*/

int gfxLoadMesh(const char* filename);

/*
	Function: gfxDrawMesh

	Draws a mesh.

	Parameters:
		object - Integer identifying the mesh.
		texture - Integer identifying the texture to be used when drawing the mesh.
*/

int gfxDrawMesh(int object, int texture);

/*
	Function: gfxTranslate

	Changes translation of the current matrix.

	Parameters:
		x - Translation in x coordinates.
		y - Translation in y coordinates.
		z - Translation in z coordinates.
*/

int gfxTranslate(float x, float y, float z);

/*
	Function: gfxRotate

	Changes the rotation of the current matrix.

	Parameters:
		a - Numbers of degrees to rotate.
		x - x
		y - y
		z - z
*/

int gfxRotate(float a, float x, float y, float z);

/*
	Function: gfxLookAt

	Bla bla bla

	Parameters:
		x - Eye x
		y - Eye y
		z - Eye z
		lx - Lookat x
		ly - Lookat y
		lz - Lookat z
		ux - Up x
		uy - Up y
		uz - Up z
*/

int gfxLookAt(float x, float y, float z, float lx, float ly, float lz, float ux, float uy, float uz);

/*
	Function: gfxDrawText

	Draws the text on the screen.

	Parameters:
		x - x coordinate
		y - y coordinate
		z - z coordinate
*/
int gfxDrawText(int x, int y, const char* text);

/*
	Function gfxDrawSprite3D

	Draws a 3D sprite on the screen. Must be within <gfxBeginQuads>/<gfxEndQuads>

	Parameters:
		x - x coordinate
		y - y coordinate
		z - z coordinate
		size - size of sprite
*/
int gfxDrawSprite3D(float x, float y, float z, float size);
/*
	Function: gfxClose

	Closes and cleans up. Engine uses this function.
*/

int gfxClose();

/*
	Group: Network
*/

/*
	Function: netGetSnapshotTick

	Retrieves the current tick of the active snapshot.

	Returns:
		The current tick of the active snapshot.
*/
int netGetSnapshotTick();

/*
	Function: netGetSnapshotNumObjects

	Retrieves the number of objects in the active snapshot.

	Returns:
		Number of objects in the active snapshot.
*/
int netGetSnapshotNumObjects();

/*
	Function: netGetSnapshotTick

	Retrieves the object at <index> in the snapshot.

	Parameters:
		index - Index

	Returns:
		The object.
*/
_snapobject *netGetSnapshotObject(int index);

/*
	Function: netSetPackData

	Sets a pointer to be filled with data from pack-functions.

	Parameters:
		data - Pointer to buffer to be filled.
*/

int netSetPackData(char* data, int maxlen);
/*
	Function: netGetPackPosition

	Returns the number of bytes packed/unpacked since the call to netSetPackData

	Parameters:
		data - Pointer to buffer
		maxlen - Max length of buffer

	Returns:
		The number of packed/unpacked bytes.
*/

int netGetPackPosition();
/*
	Function: netGetUniqueID

	Returns a unique id that can be used to identify objects on the network. Only used on server.

	Returns:
		A unique integer.
*/
int netGetUniqueID();
/*
	Function: netInit

	Initializes the network. Used by the engine.

	Returns:
		Non-zero on success, otherwise zero.
*/
int netInit();
/*
	Function: netStop

	Stops and cleans up the network. Used by the engine.
*/
int netStop();
/*
	Function: netNextSnapshot

	Prepare for a new snapshot. Used by the engine.
*/
int netNextSnapshot();
/*
	Function: netBeginSnapshot

	Start a snapshot. Used by the engine.
*/
int netBeginSnapshot();
/*
	Function: netBeginObjectSnap

	Creates snapshot for an object specified by <id> and <classname>.

	Parameters:
		id - Object id
		classname - Type of object
*/
int netBeginObjectSnap(int id, const char* classname);
/*
	Function: netPackFloat

	Packs a float.

	Parameters:
		f - Float to be packed.

*/
int netPackFloat(float f);
/*
	Function: netPackInt

	Packs an integer.

	Parameters:
		i - Integer to be packed.
*/
int netPackInt(int i);
/*
	Function: netPackString

	Packs a string.

	Parameters:
		str - String to be packed.
*/
int netPackString(const char* str);
/*
	Function: netUnpackFloat

	Unpacks a float.

	Returns:
		The float unpacked.
*/
float netUnpackFloat();
/*
	Function: netUnpackInt

	Unpacks an integer.

	Returns:
		The integer unpacked.
*/
int netUnpackInt();
/*
	Function: netUnpackString

	Unpacks a string.

	Returns:
		The unpacked string.
*/

char* netUnpackString();
/*
	Function: netEndObjectSnap

	Ends a snapshot for an object.
*/
int netEndObjectSnap();
/*
	Function: netEndSnapshot

	Ends the creation of a snapshot.
*/
int netEndSnapshot();


/*
	Group: Input
*/
/*
	Function: inputKey

	Returns wether or not the <key>-key is down.

	Parameters:
		key - Key to check.

	Returns:
		One if the key is being pressed down and zero otherwise.
*/
int inputKey(char key);

/*
	Funciton: inputGetMouseRelX

	Gets the relative movement of the mouse.

	Returns:
		Returns the difference in the X position of the mouse since the last frame.
*/

int inputGetMouseRelX();

/*
	Funciton: inputGetMouseRelY

	Gets the relative movement of the mouse.

	Returns:
		Returns the difference in the Y position of the mouse since the last frame.
*/
int inputGetMouseRelY();


/*
	Function: inputGetMouseX

	Gets the mouse cursor x position.

	Returns:
		X coordinate of the mouse cursor.
*/
int inputGetMouseX();


/*
	Function: inputGetMouseY

	Gets the mouse cursor y position.

	Returns:
		Y coordinate of the mouse cursor.
*/
int inputGetMouseY();

/*
	Function: inputGetMouseButton

	Get mouse button.

	Parameters:
		button - Which button to check.

	Returns:
		One if the mouse button is down, otherwise zero.

	Description:
		The button parameter can be 0, 1 or 2.
		0 - Left mouse button
		1 - Middle mouse button
		2 - Right mouse button
*/

int inputGetMouseButton(int button);

/*
	Function: inputShowMouse

	Shows or hides mouse cursor.

	Parameters:
		hide - Whether to hide the mouse cursor.

	Description:
		Parameter can be 0 or 1

*/

int inputShowMouse(int hide);

/*
	Group: Misc
*/
/*
	Function: miscGetTime

	Gets the current time.

	Returns:
		Integer with the current time.
*/
int miscGetTime();

/*
	Group: Callbacks
*/

/*
	Function: gameInit

	Called when the program starts.

	Returns:
		Returns zero on failure, one otherwise.
*/

int gameInit();

/*
	Function: serverInit

	Called when the server should be initialized.

	Returns:
		Return zero on failure, one otherwise.
*/
int serverInit();

/*
	Function: serverClientJoined

	Called when a client joins the server.

	Parameters:
		client_id - Id of the joining client.
*/

int serverClientJoined(int client_id);

/*
	Function: serverClientLeft

	Called when a client leaves the server.

	Parameters:
		client_id - Id of the leaving client.
*/
int serverClientLeft(int client_id);

/*
	Function: serverUnpackInput

	Called when the server should unpack input from a client.

	Parameters:
		client_id - Id of the client owning the input.
*/
int serverUnpackInput(int client_id);

/*
	Function: serverSnapshot

	Called when the server should generate a snapshot.

	Parameters:
		client_id - Id of the client intended for the snapshot.
*/
int serverSnapshot(int client_id);

/*
	Function: serverTick

	Called when the server should progress the gamestate one tick.

	Parameters:
		tick - Current tick of the engine.
*/
int serverTick(int tick);

int clientInit();
int clientPackInput();
int clientRender(const char* prevsnap, const char* cursnap, float interpolation);
int clientUnpack(int entity_id, const char* classname);
//int clientTick();


// SQUIRREL BINDING
int AddBinding(HSQUIRRELVM vm);

#endif