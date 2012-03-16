#ifndef _TEXTURE_H_
#define _TEXTURE_H_

#define _CRTDBG_MAP_ALLOC 
#include <stdlib.h>  
//#include <crtdbg.h>

int LoadTexturePNG(const char *_pFilename, int _mipmap = 1, int repeat = 0, int linear = 1, int invert = 0);
/*int loadPNG(const char* _pFilename1,
				   const char* _pFilename2,
				   const char* _pFilename3,
				   const char* _pFilename4,
				   const char* _pFilename5,
				   const char* _pFilename6,
				   bool _mipmap, bool repeat);*/

char* GetData(const char* filename, int *x, int *y, int *alpha);

#endif /* _TEXTURE_H_ */
