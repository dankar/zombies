#include "perlin.h"
#include "mersennetwister.h"
#include <cmath>
#include <cstdio>

unsigned int gWidth;
unsigned int gHeight;
double *gNumbers;

double Noise(int x, int y)
{
	return gNumbers[(y % gHeight) * gWidth + (x % gWidth)];
}

double CosInterpolate(double a, double b, double x)
{	
	double ft = x * 3.1415927;
	double f = (1.0 - cos(ft)) * 0.5;

	return  a*(1.0 - f) + b * f;
}

double CubeInterpolate(double v0, double v1, double v2, double v3, double x)
{
    double P = (v3 - v2) - (v0 - v1);
	double Q = (v0 - v1) - P;
	double R = v2 - v0;
	double S = v1;

	return P*pow(x, 3) + Q*pow(x,2) + R*x + S;
}

double SmoothNoise(float x, float y){
    double corners = double(Noise(x - 1, y - 1) + Noise(x + 1, y - 1) + Noise(x - 1, y + 1) + Noise(x + 1, y + 1)) / 16.0;
    double sides   = double(Noise(x - 1, y) + Noise(x + 1, y) + Noise(x, y - 1) + Noise(x, y + 1)) / 8.0;
    double center  = double(Noise(x, y)) / 4.0;
    return corners + sides + center;
}

double CosInterpolatedNoise(float x, float y)
{
	int intX = int(x);
	double fracX = x - intX;
	
	int intY = int(y);
	double fracY = y - intY;

	double v1 = Noise(intX, intY);
	double v2 = Noise(intX + 1, intY);
	double v3 = Noise(intX, intY + 1);
	double v4 = Noise(intX + 1, intY + 1);

	double i1 = CosInterpolate(v1, v2, fracX);
	double i2 = CosInterpolate(v3, v4, fracX);

	return CosInterpolate(i1, i2, fracY);
}

double CubeInterpolatedNoise(float x, float y)
{
    int intX = int(x);
    double fracX = x - intX;

	int intY = int(y);
	double fracY = y - intY;

	double a0 = SmoothNoise(intX - 1, intY - 1);
	double a1 = SmoothNoise(intX,     intY - 1);
	double a2 = SmoothNoise(intX + 1, intY - 1);
	double a3 = SmoothNoise(intX + 2, intY - 1);

	double b0 = SmoothNoise(intX - 1, intY);
	double b1 = SmoothNoise(intX,     intY);
	double b2 = SmoothNoise(intX + 1, intY);
	double b3 = SmoothNoise(intX + 2, intY);

	double c0 = SmoothNoise(intX - 1, intY + 1);
	double c1 = SmoothNoise(intX,     intY + 1);
	double c2 = SmoothNoise(intX + 1, intY + 1);
	double c3 = SmoothNoise(intX + 2, intY + 1);

	double d0 = SmoothNoise(intX - 1, intY + 2);
	double d1 = SmoothNoise(intX,     intY + 2);
	double d2 = SmoothNoise(intX + 1, intY + 2);
	double d3 = SmoothNoise(intX + 2, intY + 2);


	double i0 = CubeInterpolate(a0, a1, a2, a3, fracX);
	double i1 = CubeInterpolate(b0, b1, b2, b3, fracX);
	double i2 = CubeInterpolate(c0, c1, c2, c3, fracX);
	double i3 = CubeInterpolate(d0, d1, d2, d3, fracX);
	
	return CubeInterpolate(i0, i1, i2, i3, fracY);
}

double PerlinNoise(float x, float y, double p, double n)
{
	double total = 0.0;

	for(int i = 0; i < n; i++){
		double frequency = pow(2, i);
		double amplitude = pow(p, i);

		total = total + CosInterpolatedNoise(x * frequency, y * frequency) * amplitude;
	}

	return total;
}

void PerlinNoise2D(int startx, int starty, int width, int height, double p, double n, float scale, unsigned int seed, double* data)
{
	gWidth = width;
	gHeight = height;
	gNumbers = new double[width * height];
	CMersenne rNum;
	rNum.initGenerator(seed);

	for(int i = 0; i < (width * height); i++)
		gNumbers[i] = rNum.getDouble();

	for(int x = 0; x < width; x++){
		if(x % 100 == 0) printf("Column %d\n", x);
		for(int y = 0; y < height; y++)
			data[y * width + x] = PerlinNoise(double(startx + x)/scale, double(starty + y)/scale, p, n);
	}

	delete[] gNumbers;
}
