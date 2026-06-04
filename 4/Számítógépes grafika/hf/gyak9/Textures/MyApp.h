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
#include "Camera.h"
#include "CameraManipulator.h"

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

	glm::mat4 m_pyramidWorldTransform;
	glm::mat4 m_fenceWorldTransform;

	// Kamera 
	Camera m_camera;
	CameraManipulator m_cameraManipulator;

	//
	// OpenGL-es dolgok
	// 

	// Shaderekhez szükséges változók 
	GLuint m_programID = 0; // shaderek programja 

	// Shaderek inicializálása, és törlése 
	void InitShaders();
	void CleanShaders();

	// Geometriával kapcsolatos változók 

	OGLObject m_pyramidGPU = {};
	OGLObject m_fenceGPU = {};

	// Geometria inicializálása, és törlése 
	void InitGeometry();
	void CleanGeometry();

	// Textúrázás, és változói 
    GLuint m_SamplerID = 0;

	// piramis 
	GLuint m_TextureID = 0;
	GLuint m_FenceTextureID = 0;
	GLuint m_FenceMaskTextureID = 0;
	GLuint m_DirtTextureID = 0; // Új textúra a sárnak/pornak

	void InitTextures();
	void CleanTextures();
};
