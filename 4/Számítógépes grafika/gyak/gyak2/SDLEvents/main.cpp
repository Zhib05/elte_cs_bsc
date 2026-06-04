
// SDL
#include <SDL3/SDL.h>

// standard
#include <iostream>
#include <sstream>
#include <array>

int main(int argc, char* args[])
{
	//
	// 1. lépés: inicializáljuk az SDL-t
	//

	// állítsuk be a hiba Logging függvényt 
	SDL_SetLogPriority(SDL_LOG_CATEGORY_ERROR, SDL_LOG_PRIORITY_ERROR);

	// a grafikus alrendszert kapcsoljuk csak be, ha gond van, akkor jelezzük és lépjünk ki
	if ( !SDL_Init(SDL_INIT_VIDEO) )
	{
		// írjuk ki a hibát és termináljon a program 
		SDL_LogError(SDL_LOG_CATEGORY_ERROR, "[SDL initialization] Error during the SDL initialization: %s", SDL_GetError());
		return 1;
	}

	// Miután az SDL Init lefutott, kilépésnél fusson le az alrendszerek kikapcsolása.
	// Így akkor is lefut, ha valamilyen hiba folytán lépünk ki.
	std::atexit(SDL_Quit);




	// 
	// 2. lépés: hozzunk létre egy ablakot
	// 

	const int WindowW = 1280, WindowH = 720;
	SDL_Window *win = nullptr;
	win = SDL_CreateWindow( "Hello SDL!",	// az ablak fejléce 
							WindowW,		// ablak szélessége 
							WindowH,		// és magassága 
							0);				// megjelenítési tulajdonságok 


	// ha nem sikerült létrehozni az ablakot, akkor írjuk ki a hibát, amit kaptunk és lépjünk ki
	if (win == nullptr)
	{
		SDL_LogError(SDL_LOG_CATEGORY_ERROR, "[Window creation] Error during the SDL initialization: %s", SDL_GetError());
		return 1;
	}




	//
	// 3. lépés: hozzunk létre egy renderelőt, rajzolót
	//

	SDL_Renderer *ren = nullptr;
	ren = SDL_CreateRenderer( win,		 // melyik ablakhoz rendeljük hozzá a renderert 
							  nullptr );

	if (ren == nullptr)
	{
		SDL_LogError(SDL_LOG_CATEGORY_ERROR, "[Renderer creation] Error during the creation of an SDL renderer: %s", SDL_GetError());
		SDL_DestroyWindow(win);
		return 1;
	}

	// bekapcsoljuk a V-syncet
	SDL_SetRenderVSync(ren, 1);




	//
	// 4. lépés: indítsuk el a fő üzenetfeldolgozó ciklust
	// 

	bool quit = false;	// véget kell-e érjen a program futása? 

	float mouseX = 0, mouseY = 0; // egér X és Y koordinátái 

	while (!quit)
	{
		// feldolgozandó üzenet ide kerül 
		SDL_Event ev;
		// amíg van feldolgozandó üzenet dolgozzuk fel mindet:
		while (SDL_PollEvent(&ev))
		{
			switch (ev.type)
			{
			case SDL_EVENT_QUIT:
				quit = true;
				break;
			case SDL_EVENT_KEY_DOWN:
				if (ev.key.key == SDLK_ESCAPE) quit = true;
				break;
			case SDL_EVENT_MOUSE_MOTION:
				mouseX = ev.motion.x;
				mouseY = ev.motion.y;
				break;
			case SDL_EVENT_MOUSE_BUTTON_UP:
				// egérgomb felengedésének eseménye; a felengedett gomb a ev.button.button -ban található
				// a lehetséges gombok a következőek: SDL_BUTTON_LEFT, SDL_BUTTON_MIDDLE, SDL_BUTTON_RIGHT
				// egérgomb felengedésének eseménye; a felengedett gomb a ev.button.button -ban található
				break;
			}
		}



		// töröljük a hátteret fehérre
		SDL_SetRenderDrawColor(ren, 255, 255, 255, 255);
		SDL_RenderClear(ren);

		// aktuális rajzolási szín legyen zöld és rajzoljunk ki egy vonalat
		SDL_SetRenderDrawColor(ren,	// renderer címe, aminek a rajzolási színét be akarjuk állítani 
			0,		// R -  piros 
			255,	// G -  zöld 
			0,		// B -  kék 
			255);	// A -  átlátszatlanság 

		SDL_RenderLine(ren,	// renderer címe, amivel vonalat akarunk rajzolni 
			0.0f, 0.0f,			// vonal kezdőpontjának (x,y) koordinátái 
			mouseX, mouseY);	// vonal végpontjának (x,y) koordinátái 

		// definiáljunk egy (mouseX, mouseY) középpontú, tengelyekkel párhuzamos oldalú
		// 20x20-as négyzetet:
		SDL_FRect cursor_rect;
		cursor_rect.x = mouseX - 10.0f;
		cursor_rect.y = mouseY - 10.0f;
		cursor_rect.w = 20.0f;
		cursor_rect.h = 20.0f;

		// legyen a kitöltési szín piros 
		SDL_SetRenderDrawColor(ren, 255, 0, 0, 255);
		SDL_RenderFillRect(ren, &cursor_rect);

		// 1. feladat: az eltelt idő függvényében periodikusan nőjjön és csökkenjen
		//    az egérmutató középpontjával kirajzolt négyszög

		// 2. feladat: ha a user a bal egérgombot nyomja meg akkor a téglalap színe váltson pirosra,
		//    ha a jobb egérgombot, akkor kékre

		// 3. beadható feladat: rajzolj ki egy 50 sugarú körvonalat az egérmutató köré!
		// segítség: használd a SDL_RenderLines()-t

		// jelenítsük meg a back buffer tartalmát 
		SDL_RenderPresent(ren);
	}




	//
	// 5. lépés: lépjünk ki
	// 

	SDL_DestroyRenderer(ren);
	SDL_DestroyWindow(win);

	return 0;
}