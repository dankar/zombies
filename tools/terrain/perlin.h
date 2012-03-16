#ifndef _PERLIN_H_
#define _PERLIN_H_

double Noise(int x, int y);
double PerlinNoise(float x, float y, double p, double n);
void PerlinNoise2D(int startx, int starty, int width, int height, double p, double n, float scale, unsigned int seed, double* data);

#endif