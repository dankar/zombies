#ifndef _QUATERNION_H_
#define _QUATERNION_H_

#include <cmath>

class CQuaternion
{
public:
	float x, y, z, w;

	CQuaternion() : x(0.0f), y(0.0f), z(0.0f), w(0.0f) {};
	CQuaternion(float _x, float _y, float _z, float _w) : x(_x), y(_y), z(_z), w(_w) {};
	CQuaternion(const CQuaternion& quat) : x(quat.x), y(quat.y), z(quat.z), w(quat.w) {};

	inline float length(){ return sqrt(x * x + y * y + z * z + w * w); }
	inline void normalize()
	{
		float l = length();

		x /= l;
		y /= l;
		z /= l;
		w /= l;
	}

	inline CQuaternion conjugate(){ return CQuaternion(-x, -y, -z, w); }

	inline CQuaternion operator*(const CQuaternion& mul)
	{
		CQuaternion res;

		res.x = w*mul.x + x*mul.w + y*mul.z - z*mul.y;
		res.y = w*mul.y - x*mul.z + y*mul.w + z*mul.x;
		res.z = w*mul.z + x*mul.y - y*mul.x + z*mul.w;
		res.w = w*mul.w - x*mul.x - y*mul.y - z*mul.z;

		return res;
	}

};

#endif