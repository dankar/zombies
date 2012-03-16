#include "texture.h"
#include "font.h"
#include <cstdio>
#include <cstdarg>
#include <gl/glfw.h>
#include <string.h>
#include <map>
#include <string>
#include "../math/vector.h"
#include "model.h"
#include "../resourcehandler.h"
#include "../log.h"
#include "../timer.h"
#include "video.h"

CFont::CFont()
{

}

CFont::CFont(char* filename)
{
	Load(filename);
}

bool CFont::Load(char* filename)
{
	if(!(m_Font = CResourceHandler::LoadTexture(filename, 0, 1, 1, 1)))
	{
		CLog::Get().Write(APP_LOG, LOG_ERROR, "Could not open font \"%s\"\n", filename);
		return false;
	}

	return true;
}

void CFont::Draw(int x, int y, const char* string, ...)
{
	va_list ap;
	va_start(ap, string);
	char buffer[1024];
	vsprintf(buffer, string, ap);
	va_end(ap);

	float FontX = 16.0f;
	float FontY = 14.0f * 1.14f;
	float XTexStep = 1.0/FontX;
	float YTexStep = 1.0/FontY;
	float CharWidth = 64.0f/4;
	float CharHeight = 128.0f/4;
	float screenStep = 64.0f/4 / 2;

	float xPos = 0;
	float yPos = 0;

	glDisable(GL_DEPTH_TEST);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	glBindTexture(GL_TEXTURE_2D, m_Font);

	glPushMatrix();
	glTranslatef(float(x), float(y), 0.0f);


	for(int i = 0; i < strlen(buffer); i++)
	{
		float XTexPos = 0.0f;
		float YTexPos = 0.0f;
		int charNum = buffer[i] - ' ';

		XTexPos = float(charNum % 16) * XTexStep;
		YTexPos = float(charNum / 16) * YTexStep;

		if(buffer[i] != ' ' && buffer[i] != '\n' && buffer[i] != '\t')
		{
			glBegin(GL_QUADS);
				glTexCoord2d(XTexPos, YTexPos); glVertex2f(xPos, 0.0f);
				glTexCoord2d(XTexPos + XTexStep, YTexPos); glVertex2f(xPos + CharWidth, 0.0f);
				glTexCoord2d(XTexPos + XTexStep, YTexPos + YTexStep); glVertex2f(xPos + CharWidth, CharHeight);
				glTexCoord2d(XTexPos, YTexPos + YTexStep); glVertex2f(xPos, CharHeight);
			glEnd();
		}

		xPos += screenStep;

	}

	glDisable(GL_BLEND);
	glEnable(GL_DEPTH_TEST);
	glPopMatrix();

}