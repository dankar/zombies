#include "mersennetwister.h"
#include <cstdio>
#include <cstdlib>
#include "perlin.h"

#define _USE_MATH_DEFINES
#include <cmath>
#include <ctime>
#include <climits>
#include "bmp.h"

void print_usage()
{
	printf("usage: terr <x> <y> <octaves> <persistence> <scale> [seed]\n");
	exit(1);
}

int PerlinBMP(char* filename, int startx, int starty, int xsize, int ysize, double persistence, double octaves, float scale, unsigned int seed)
{
	long newsize;
	unsigned char *final = new unsigned char[xsize * ysize * 3];
	double *data = new double[xsize * ysize];

	printf("Using seed: %d\n", seed);

	printf("Generating noise...\n");

	PerlinNoise2D(startx, starty, xsize, ysize, persistence, octaves, scale, seed, data);

	printf("Converting...\n");

	

	for(int i = 0; i < (xsize * ysize); i++)
	{
		double value = (data[i] + 1.5) / 3.0;

		value = value * 255;

		if(value < 0) value = 0;
		if(value > 255) value = 255;

        	final[i*3] = value;
		final[i*3 + 1] = value;
		final[i*3 + 2] = value;
	}

	delete[] data;

	printf("Writing...\n");

	char* bmpbuffer = ConvertRGBToBMPBuffer((char*)final, xsize, ysize, &newsize);

	SaveBMP(bmpbuffer, xsize, ysize, newsize, filename);

	free(bmpbuffer);

	delete[] final;

	return 1;
}


int main(int argc, char* argv[])
{
	
	int xsize, ysize, octaves, seed, scale;
	float persistence;

	if(argc != 7 && argc != 6)
		print_usage();

	xsize = atoi(argv[1]);
	ysize = atoi(argv[2]);
	octaves = atoi(argv[3]);
	persistence = atof(argv[4]);
	scale = atoi(argv[5]);

	if(argc == 7)
		seed = atoi(argv[6]);
	else
		seed = time(NULL);

	PerlinBMP("terrain.bmp", 0, 0, xsize, ysize, persistence, octaves, scale, seed);

	return 0;
}