#include "mersennetwister.h"

void CMersenne::initGenerator(int seed)
{
	m_State[0] = seed;
	
	for(int i = 1; i < 624; i++)
		m_State[i] = int(((long long)(69069) * m_State[i - 1]) + 1);
}

void CMersenne::generateNumbers()
{
	for(int i = 0; i < 623; i++){
		y = (m_State[i] >> 32) & 0x01 + m_State[i+1] & 0x3FFFFFFF;

		if(y % 2 == 0)
			m_State[i] = m_State[(i + 397) % 624] ^ (y >> 1);
		else
			m_State[i] = m_State[(i + 397) % 624] ^ (y >> 1) ^ (2567483615);
	}

	y = (m_State[623] >> 32) & 0x01 + m_State[0] & 0x3FFFFFFF;

	if(y % 2 == 0)
		m_State[623] = m_State[396] ^ (y >> 1);
	else
		m_State[623] = m_State[396] ^ (y >> 1) ^ (2567483615);

	m_CurrentNumber = 0;
}

int CMersenne::extractNumber(int i)
{
	y = m_State[i];
	y = y ^ (y >> 11);
	y = y ^ (y << 7) & (2636928640);
	y = y ^ (y << 15) & (4022730752);
	y = y ^ (y >> 18);

	return y;
}

double CMersenne::getDouble()
{
	if(m_CurrentNumber == 624)
		generateNumbers();

	unsigned int temp = extractNumber(m_CurrentNumber++);

	double ret = double(temp) / (4294967295 / 2) - 1.0;

	return ret;
}