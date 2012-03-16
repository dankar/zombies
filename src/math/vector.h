#ifndef _VECTOR_H_
#define _VECTOR_H_
#include <cmath>

inline float lerp(float f1, float f2, float amount)
{
	return f1 + (f2 - f1) * amount;
}

class Vec2
{
public:
	float x, y;
	Vec2(float _x, float _y) : x(_x), y(_y) {};
	Vec2(const Vec2& vec) : x(vec.x), y(vec.y) {};
	Vec2() : x(0.0f), y(0.0f) {};
	
	Vec2 operator+(const Vec2& vec)
	{ return Vec2(x + vec.x, y + vec.y); }
	Vec2 operator-(const Vec2& vec)
	{ return Vec2(x - vec.x, y - vec.y); }
	Vec2 operator/(const float z)
	{ return Vec2(x / z, y / z); }

	Vec2 normalize()
	{ float l = length(); x = x/l; y = y/l; return *this; }
	
	float length()
	{ return sqrt(x*x + y*y); }
	
	operator float*() const
	{ return (float*)this; }
};

class Vec3
{
public:
	float x, y, z;
	
	Vec3() : x(0), y(0), z(0) {};
	Vec3(float _x, float _y, float _z) : x(_x), y(_y), z(_z) {};
	Vec3(const Vec3 &vec) : x(vec.x), y(vec.y), z(vec.z) {};
	Vec3(const Vec2 &vec) : x(vec.x), y(vec.y), z(0.0f) {};
	
	Vec3 operator+(const Vec3& vec) const
	{ return Vec3(x + vec.x, y + vec.y, z + vec.z); }
	Vec3 operator-(const Vec3& vec) const
	{ return Vec3(x - vec.x, y - vec.y, z - vec.z); }
	Vec3 operator/(float f) const
	{ return Vec3(x / f, y / f, z / f); }
	Vec3 operator*(float f) const
	{ return Vec3(x * f, y * f, z * f); }

	Vec3 plus(float mul[4])
	{
		return Vec3(x + mul[0], y + mul[1], z + mul[2]);
	}

	Vec3 minus(float mul[4])
	{
		return Vec3(x - mul[0], y - mul[1], z - mul[2]);
	}

	Vec3 normal()
	{
		float l = length();

		return Vec3(x/l, y/l, z/l);
	}

	void normalize()
	{
		float l = length();

		x /= l;
		y /= l;
		z /= l;
	}

	Vec3 operator-() const
	{ return Vec3(-x, -y, -z); }

	
	float length() const
	{ return sqrt(x*x + y*y + z*z); }
	
	Vec3 interpolate(const Vec3& vec, float f) const
	{ return Vec3(	lerp(x, vec.x, f),
					lerp(y, vec.y, f),
					lerp(z, vec.z, f)); }
					
	Vec3 cross(const Vec3& vec) const
	{
		return Vec3( y * vec.z - z * vec.y,
					 z * vec.x - x * vec.z,
					 x * vec.y - y * vec.x);
	}

	void toVec4(float r[4], float w)
	{
		r[0] = x;
		r[1] = y;
		r[2] = z;
		r[3] = w;
	}


	float dot(const Vec3& vec) const
	{
		return x * vec.x + y * vec.y + z * vec.z;
	}

	void Rotate(float amount, float x, float y, float z);

	
	
	operator float*() const
	{ return (float*)this; }
};

Vec3 cubicInterpolate(const Vec3& p0, const Vec3& p1, const Vec3& p2, const Vec3& p3, float amount);

#endif
