#include <SDL.h>
#include <stdio.h>

int main(int argc, char *argv[])
{
	if (SDL_Init(SDL_INIT_VIDEO) < 0)
	{
		fprintf(stderr, "Could not initialize SDL: %s.\n", SDL_GetError());
		exit(-1);
	}

	// query video information
	const SDL_VideoInfo *info = SDL_GetVideoInfo();
	if (!info)
	{
		fprintf(stderr, "Failed to get video info: %s.\n", SDL_GetError());
		exit(1);
	}

	// set display parameters
	int width  = 640;
	int height = 480;
	int bpp    = info->vfmt->BitsPerPixel; // bits per pixel
	int flags  = SDL_OPENGL;

	// set up our initial display
	if (SDL_SetVideoMode(width, height, bpp, flags) == 0)
	{
		// error
		fprintf(stderr, "Failed to set video mode: %s.\n", SDL_GetError());
		exit(-1);
	}

	// main loop
	bool done = false;
	while (!done)
	{
		// process SDL events
		SDL_Event event;
		while (SDL_PollEvent(&event)) switch (event.type)
		{
		case SDL_KEYDOWN:
			break;
		case SDL_QUIT:
			done = true;
			break;
		}
	}

	// finish up
	SDL_Quit();

	return 0;
}
