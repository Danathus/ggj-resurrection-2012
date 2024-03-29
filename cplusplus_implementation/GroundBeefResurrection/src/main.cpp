#include <SDL.h>
#include <SDL_opengl.h>
#include <stdio.h>

////////////////////////////////////////////////////////////////////////////////

void SetupGraphicsDisplay(int width, int height)
{
	// set some parameters for OpenGL
	glShadeModel(GL_SMOOTH);  // smooth shading
	glClearColor(0, 0, 0, 0); // clear color: black
	glViewport(0, 0, width, height); // viewport

	// orthographic projection
	glMatrixMode(GL_PROJECTION);
    glLoadIdentity();
	glOrtho(-width/2, width/2, -height/2, height/2, +1000, -1000); // left, right, bottom, top, near, far
}

////////////////////////////////////////////////////////////////////////////////

void Startup()
{
}

////////////////////////////////////////////////////////////////////////////////

void Shutdown()
{
}

////////////////////////////////////////////////////////////////////////////////

float rotZ = 0.0f;
float posX = 0, posY = 0;

void Update(const float deltaTime)
{
	// rotate at a speed of 100 degrees per second
	rotZ += 100 * deltaTime;

	// keyboard controls
	unsigned char *keys = SDL_GetKeyState(NULL);
	posX += (keys['d'] - keys['a']) * 100 * deltaTime; // direction * speed * time
	posY += (keys['w'] - keys['s']) * 100 * deltaTime; // direction * speed * time
}

////////////////////////////////////////////////////////////////////////////////

void Draw()
{
	// clear screen to start
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	// set up transformation
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	// transform into place
	glTranslatef(posX, posY, 0);
	glRotatef(rotZ, 0, 0, 1);

	// draw a white square in the center
	glColor3f(1, 1, 1);
	glBegin(GL_QUADS);
	glVertex2f(-100, -100);
	glVertex2f( 100, -100);
	glVertex2f( 100,  100);
	glVertex2f(-100,  100);
	glEnd();

	// finish up
	SDL_GL_SwapBuffers();
}

////////////////////////////////////////////////////////////////////////////////

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

	SetupGraphicsDisplay(width, height);
	Startup();

	// main loop
	bool done = false;
	unsigned int nowTicks = SDL_GetTicks(), thenTicks = SDL_GetTicks(); // in tickets (milliseconds)
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

		// update time
		nowTicks = SDL_GetTicks();
		const float deltaTime = float(nowTicks - thenTicks) / 1000; // in seconds
		thenTicks = nowTicks; // set up for next time

		// update and draw
		Update(deltaTime);
		Draw();
	}

	// finish up
	Shutdown();
	SDL_Quit();

	return 0;
}
