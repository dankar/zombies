#ifndef _MERSENNETWISTER_H_
#define _MERSENNETWISTER_H_

class CMersenne{
private:
	unsigned int m_State[624];
	unsigned int y;
	unsigned int m_CurrentNumber;
public:
	CMersenne(){ m_CurrentNumber = 624; }
	void initGenerator(int seed);
	void generateNumbers();
	int extractNumber(int i);
	double getDouble();
};

#endif /* _MERSENNETWISTER_H_ */