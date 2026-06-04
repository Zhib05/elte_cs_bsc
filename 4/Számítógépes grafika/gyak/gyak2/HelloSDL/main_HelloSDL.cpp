
// SDL
#include <SDL3/SDL.h>

// standard
#include <iostream>
#include <sstream>

int main( int argc, char* args[] )
{
	//
	// 1. lépés: inicializáljuk az SDL-t
	//

	// állítsuk be a hiba Logging függvényt 
	SDL_SetLogPriority(SDL_LOG_CATEGORY_ERROR, SDL_LOG_PRIORITY_ERROR);

	// a grafikus alrendszert kapcsoljuk csak be, ha gond van, akkor jelezzük és lépjünk ki
	if ( !SDL_Init( SDL_INIT_VIDEO ) )
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

	SDL_Window *win = nullptr;
	win = SDL_CreateWindow( "Hello SDL!",	// az ablak fejléce 
							1280,			// ablak szélessége 
							720,			// és magassága 
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
		SDL_LogError( SDL_LOG_CATEGORY_ERROR, "[Renderer creation] Error during the creation of an SDL renderer: %s", SDL_GetError() );
		SDL_DestroyWindow(win);
		return 1;
	}

	// bekapcsoljuk a V-syncet
	SDL_SetRenderVSync(ren, 1);


	//
	// 4. lépés: renderelés/rajzolás
	//

	// Töröljük az ablak háttérszínét, rajzoljunk egy vonalat és várjunk 2 másodpercet

	// aktuális rajzolási szín legyen fekete és töröljük az aktuális rajzolási színnel az ablak kliensterületét
	SDL_SetRenderDrawColor(	ren,	// melyik renderelőnek állítjuk be az aktuális rajzolási színét 
							0,		// R -  vörös intenzitás 
							0,		// G -  zöld intenzitás 
							0,		// B -  kék intenzitás 
							255);	// A -  átlátszóság 

	SDL_RenderClear(ren);


	// aktuális rajzolási szín legyen zöld és rajzoljunk ki egy vonalat
	SDL_SetRenderDrawColor(	ren,	// renderer címe, aminek a rajzolási színét be akarjuk állítani 
							0,		// R -  piros 
							255,	// G -  zöld 
							0,		// B -  kék 
							255);	// A -  átlátszóság 

	SDL_RenderLine(	ren,			// renderer címe, amivel vonalat akarunk rajzolni 
					10.0f, 10.f,	// vonal kezdőpontjának (x,y) koordinátái 
					10.0f, 60.f);	// vonal végpontjának (x,y) koordinátái 


	// jelenítsük meg a back buffer tartalmát 
	SDL_RenderPresent(ren);

	// várjunk 2 másodpercet 
	SDL_Delay(2000);




	//
	// 5. lépés: lépjünk ki
	// 

	SDL_DestroyRenderer( ren );
	SDL_DestroyWindow( win );


	return 0;
}