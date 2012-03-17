#include <GL/glfw.h>
#include <vector>
#include <string>
using namespace std;
#include "font.h"
#include "console.h"
#include "../timer.h"
#include "../input.h"
#include "../interface.h"
#include "video.h"



vector<string> CConsole::m_Console;
CFont CConsole::m_Font;
float CConsole::m_DropHeight = 0;
float CConsole::m_DropDelta = 0;
int CConsole::m_Scroll;
char CConsole::m_Input[256];
bool CConsole::m_LastShow = 0;
int CConsole::m_InputDone = 0;

void CConsole::AddLine(string line)
{
	m_Console.push_back(line);
}

void CConsole::Init()
{
	m_Font.Load("font_big.png");
}

void CConsole::ScrollUp()
{
	if(m_Scroll < m_Console.size() - 1)
	{
		m_Scroll += 1;
	}
}

void CConsole::ScrollDown()
{
	if(m_Scroll > 0)
	{
		m_Scroll -= 1;
	}
}

void CConsole::Draw(bool show)
{
	if(show)
	{
		m_DropHeight += 500.0 * timerGetDiff();

		if(!m_LastShow)
		{
			m_LastShow = show;
			inpSetInput(m_Input, 256, &m_InputDone);
		}

		if(m_InputDone)
		{
			std::string line(m_Input);
			line.insert(0, "> ");
			AddLine(line);
			AddLine("Syntax error");
		}
	}

	if(!show)
	{
		m_DropHeight -= 500.0 * timerGetDiff();
		if(m_LastShow)
		{
			m_LastShow = show;
			inpSetInput(0, 0, 0);
		}
	}

	if(m_DropHeight > 300.0f)
		m_DropHeight = 300.0f;
	if(m_DropHeight < 0.0f)
		m_DropHeight = 0.0f;

	if(m_DropHeight > 3)
	{
		float y = m_DropHeight;
		videoSetOrtho(800,600);

		glDisable(GL_DEPTH_TEST);
		gfxBindTexture(0);

		glColor4f(0.3f, 0.0f, 0.0f, 0.5f);

		gfxBlendNormal();

		glBegin(GL_QUADS);
			glVertex2f(0.0f, 0.0f);
			glVertex2f(800.0f, 0.0f);
			glVertex2f(800.0f, m_DropHeight);
			glVertex2f(0.0f, m_DropHeight);
		glEnd();

		glColor4f(0.9f, 0.0f, 0.0f, 1.0f);

		glBegin(GL_LINE_STRIP);
			glVertex2f(0.0f, 0.0f);
			glVertex2f(798.0f, 0.0f);
			glVertex2f(798.0f, m_DropHeight);
			glVertex2f(0.0f, m_DropHeight);
			glVertex2f(0.0f, 0.0f);
		glEnd();

		glColor4f(1.0f, 1.0f, 1.0f, 1.0f);

		y -= 128.0f/8;

		for(int count = 0, i = m_Console.size() - 1 - m_Scroll; i >= 0 && count < 35; i--, count++)
		{
			m_Font.Draw(10, int(y - 21), m_Console[i].c_str());
			y -= 128.0f/8;
		}

		if(m_InputDone)
		{
			inpSetInput(m_Input, 256, &m_InputDone);
		}

		m_Font.Draw(1, int(m_DropHeight - 128.0f/6), ">");
		m_Font.Draw(10, int(m_DropHeight - 128.0f/6), m_Input);

		videoSetPerspective();

		glEnable(GL_DEPTH_TEST);
	}
}
