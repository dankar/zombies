#ifndef _CONSOLE_H_
#define _CONSOLE_H_

class CConsole
{
	static int m_Scroll;
	static float m_DropHeight;
	static float m_DropDelta;
	static vector<string> m_Console;
	static CFont m_Font;
	static char m_Input[256];
	static bool m_LastShow;
	static int m_InputDone;

public:
	static void Init();
	static void AddLine(string line);
	static void ScrollUp();
	static void ScrollDown();
	static void Draw(bool show);
};

#endif