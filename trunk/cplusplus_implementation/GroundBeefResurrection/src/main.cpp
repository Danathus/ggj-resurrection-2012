#include <SDL.h>
#include <stdio.h>

int main(int argc, char *argv[])
{
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) == -1)
	{
		printf("Could not initialize SDL: %s.\n", SDL_GetError());
		exit(-1);
	}

	printf("hello world!\n");

	SDL_Quit();

	return 0;
}
