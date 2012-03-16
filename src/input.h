#ifndef _INPUT_H_
#define _INPUT_H_

const int KeyCount = 512;

bool inpKey(int key);
bool inpOldKey(int key);
bool inpKeyPressed(int key);
bool inpKeyReleased(int key);
bool inpMouse(int button);
bool inpOldMouse(int button);
bool inpMousePressed(int button);
bool inpMouseReleased(int button);
bool inpSetInput(char *buffer, int len, int *done);

int inpGetMouseRelX();
int inpGetMouseRelY();
int inpGetMouseX();
int inpGetMouseY();

bool inpInit();
void inpHideCursor(int hidecursor);
void inpUpdate();
void inpDestroy();

void GLFWCALL KeyCallback(int key, int action);
void GLFWCALL CharCallback(int key, int action);


#endif /* _INPUT_H_ */
