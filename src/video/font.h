#ifndef _FONT_H_
#define _FONT_H_

class CFont
{
public:
	int m_Font;

	bool Load(char* filename);
	CFont();
	CFont(char* filename);
	void Draw(int x, int y, const char* string, ...);
};

#endif /* _FONT_H_ */