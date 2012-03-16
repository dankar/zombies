#ifndef _VIDEO_H_
#define _VIDEO_H_

int videoOpenWindow(int x, int y, int depth, int fullscreen);
int videoSetOrtho(int x, int y);
int videoSetPerspective();
int videoGenFramebuffer();

#endif