#define _USE_MATH_DEFINES
#include <cmath>
#include "vector.h"

void Vec3::Rotate(float amount, float x, float y, float z)
{
	if(amount == 0.0)
		return;

	Vec3 u(x, y, z);

	u.normalize();

	Vec3 rotMatrixRow0, rotMatrixRow1, rotMatrixRow2;

	float sinAngle = (float)sin(M_PI * amount / 180);
	float cosAngle = (float)cos(M_PI * amount / 180);
	float oneMinusCosAngle = 1.0f - cosAngle;

	rotMatrixRow0.x=(u.x)*(u.x) + cosAngle*(1-(u.x)*(u.x));
	rotMatrixRow0.y=(u.x)*(u.y)*(oneMinusCosAngle) - sinAngle*u.z;
	rotMatrixRow0.z=(u.x)*(u.z)*(oneMinusCosAngle) + sinAngle*u.y;

	rotMatrixRow1.x=(u.x)*(u.y)*(oneMinusCosAngle) + sinAngle*u.z;
	rotMatrixRow1.y=(u.y)*(u.y) + cosAngle*(1-(u.y)*(u.y));
	rotMatrixRow1.z=(u.y)*(u.z)*(oneMinusCosAngle) - sinAngle*u.x;
	
	rotMatrixRow2.x=(u.x)*(u.z)*(oneMinusCosAngle) - sinAngle*u.y;
	rotMatrixRow2.y=(u.y)*(u.z)*(oneMinusCosAngle) + sinAngle*u.x;
	rotMatrixRow2.z=(u.z)*(u.z) + cosAngle*(1-(u.z)*(u.z));

	Vec3 new_vec  ( dot(rotMatrixRow0),
					dot(rotMatrixRow1),
					dot(rotMatrixRow2) );

	*this = new_vec;
}

Vec3 cubicInterpolate(const Vec3& p0, const Vec3& p1, const Vec3& p2, const Vec3& p3, float amount)
{
	float amount2 = amount*amount;

	Vec3 a0 = p3 - p2 - p0 + p1;
	Vec3 a1 = p0 - p1 - a0;
	Vec3 a2 = p2 - p0;
	Vec3 a3 = p1;

	return Vec3(a0 * amount * amount2 + a1 * amount2 + a2 * amount + a3);
}