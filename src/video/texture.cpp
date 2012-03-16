#include <png.h>
#include <GL/glew.h>
#include <GL/glfw.h>

#include "png.h"
#include "texture.h"
#include "../log.h"

unsigned char* GetData(const char* filename, int *x, int *y, int *alpha, int invert)
{
	PNG png;

	if(!(*alpha = png.open(filename))){
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not open '%s'.\n", filename);
		return false;
	}

	*x = png.width;
	*y = png.height;

	unsigned char *data = new unsigned char[png.width * png.height * 4];

	int counter = 0;
	
	for(int i = 0; i < png.height; i++){
		if(*alpha == 2){ // *alpha är 2 om bilden har en alfa-kanal
			for(int j = 0; j < png.width * 4; j++){
				if(invert)
					if(j > 0 && j % 4 == 3) // invertera inte om värdet anger pixelns alfa-kanal
						data[counter] = png.row_pointers[i][j];
					else
						data[counter] = 255 - png.row_pointers[i][j];
						
				else
					data[counter] = png.row_pointers[i][j];

				counter++;
			}
		}else{
			for(int j = 0; j < png.width * 3; j++){
				if(invert)
                    data[counter] = 255 - png.row_pointers[i][j];
				else
					data[counter] = png.row_pointers[i][j];

				counter++;
			}
		}
	}

	return data;
}

int LoadTexturePNG(const char* _pFilename, int _mipmap, int repeat, int linear, int invert)
{
	//glEnable(GL_TEXTURE_2D);
	int alpha = 0;
	unsigned int texture;
	int width;
	int height;
	
	//printf("Loading texture:\t%s\n", _pFilename);
	
	//glEnable(GL_TEXTURE_2D);

	unsigned char *data = GetData(_pFilename, &width, &height, &alpha, invert);

	if(!data)
		return 0;

	glGenTextures(1,&texture);
	glBindTexture(GL_TEXTURE_2D,texture);
	
	if(linear)
	{
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_LINEAR);
	}
	else
	{
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MIN_FILTER,GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D,GL_TEXTURE_MAG_FILTER,GL_NEAREST);
	}


	if (repeat)
	{
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	}
	else
	{
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	}

	if(_mipmap)
		if(alpha == 2)
			gluBuild2DMipmaps(GL_TEXTURE_2D,4,width,
   					height,GL_RGBA,GL_UNSIGNED_BYTE,data);
		else
			gluBuild2DMipmaps(GL_TEXTURE_2D,4,width,
   					height,GL_RGB,GL_UNSIGNED_BYTE,data);

	else
		if(alpha == 2)
			glTexImage2D(GL_TEXTURE_2D,0,GL_RGBA,width,
   					height,0,GL_RGBA,GL_UNSIGNED_BYTE,data);
		else
			glTexImage2D(GL_TEXTURE_2D,0,GL_RGB,width,
   					height,0,GL_RGB,GL_UNSIGNED_BYTE,data);

	
	delete[] data;

	//glDisable(GL_TEXTURE_2D);

	return texture;
}

/*bool CTexture::loadPNG(const char* filename1,
					   const char* filename2,
					   const char* filename3,
					   const char* filename4,
					   const char* filename5,
					   const char* filename6,
					   bool _mipmap, bool repeat)
{
	glEnable(GL_TEXTURE_CUBE_MAP_EXT);

	int alpha = 0;

	char *tex[6];
	
	tex[0] = GetData(filename1, &m_Width, &m_Height, &alpha);
	tex[1] = GetData(filename2, &m_Width, &m_Height, &alpha);
	tex[2] = GetData(filename3, &m_Width, &m_Height, &alpha);
	tex[3] = GetData(filename4, &m_Width, &m_Height, &alpha);
	tex[4] = GetData(filename5, &m_Width, &m_Height, &alpha);
	tex[5] = GetData(filename6, &m_Width, &m_Height, &alpha);

	if(!tex[0] ||
		!tex[1] ||
		!tex[2] ||
		!tex[3] ||
		!tex[4] ||
		!tex[5])
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not open some file...\n");
		return false;
	}


	glGenTextures(1, &m_Texture);
	glBindTexture(GL_TEXTURE_CUBE_MAP_EXT, m_Texture);
	
	glTexParameteri(GL_TEXTURE_CUBE_MAP_EXT, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_CUBE_MAP_EXT, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	if (repeat)
	{
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	}
	else
	{
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP);
	}

	glTexGeni(GL_S, GL_TEXTURE_GEN_MODE, GL_REFLECTION_MAP_EXT);
	glTexGeni(GL_T, GL_TEXTURE_GEN_MODE, GL_REFLECTION_MAP_EXT);
	glTexGeni(GL_R, GL_TEXTURE_GEN_MODE, GL_REFLECTION_MAP_EXT);

	for(int i = 0; i < 6; i++)
	{
		if(alpha == 2)
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X_EXT + i,0,4,m_Width,
				m_Height,0,GL_RGBA,GL_UNSIGNED_BYTE,tex[i]);
		else
			glTexImage2D(GL_TEXTURE_CUBE_MAP_POSITIVE_X_EXT + i,0,4,m_Width,
				m_Height,0,GL_RGB,GL_UNSIGNED_BYTE,tex[i]);
		delete[] tex[i];
	}

	glDisable(GL_TEXTURE_CUBE_MAP_EXT);
	return true;
}


CTexture::~CTexture()
{
	glDeleteTextures(1, &m_Texture);
}

unsigned int CTexture::GetTexture()
{
	return m_Texture;
}

unsigned int CTexture::GetWidth()
{
	return m_Width;
}

unsigned int CTexture::GetHeight()
{
	return m_Height;
}*/
