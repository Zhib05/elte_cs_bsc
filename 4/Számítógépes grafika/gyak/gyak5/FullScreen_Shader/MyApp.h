#pragma once

// GLM
#include <glm/glm.hpp>
#include <glm/gtc/type_ptr.hpp>
#include <glm/gtx/transform.hpp>

// GLEW
#include <GL/glew.h>

// SDL
#include <SDL3/SDL.h>
#include <SDL3/SDL_opengl.h>

// Utils
#include "GLUtils.hpp"

struct SUpdateInfo
{
	float ElapsedTimeInSec = 0.0f; // Program indulása óta eltelt idő 
	float DeltaTimeInSec   = 0.0f; // Előző Update óta eltelt idő 
};

class CMyApp
{
public:
	CMyApp();
	~CMyApp();

	bool Init();
	void Clean();

	void Update( const SUpdateInfo& );
	void Render();
	void RenderGUI();

	void KeyboardDown(const SDL_KeyboardEvent&);
	void KeyboardUp(const SDL_KeyboardEvent&);
	void MouseMove(const SDL_MouseMotionEvent&);
	void MouseDown(const SDL_MouseButtonEvent&);
	void MouseUp(const SDL_MouseButtonEvent&);
	void MouseWheel(const SDL_MouseWheelEvent&);
	void Resize(int, int);

	void OtherEvent( const SDL_Event& );
protected:
	void SetupDebugCallback();

	//
	// Adat változók
	//

	float m_ElapsedTimeInSec = 0.0f;
	float m_translate_X = 0.0f;
	float m_translate_Y = 0.0f;

	int m_windowWidth = 1280;
	int m_windowHeight = 720;
	glm::vec2 m_quadPositions[100];
	int m_quadCount = 0;
	//
	// OpenGL-es dolgok
	// 
	
	// shaderekhez szükséges változók 
	GLuint m_programID = 0; // shaderek programja 

	// Shaderek inicializálása, és törlése 
	void InitShaders();
	void CleanShaders();
};
